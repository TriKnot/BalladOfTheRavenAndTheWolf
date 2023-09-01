using System.Collections.Generic;
using System.Linq;
using Enemy.Attacks;
using Enemy.Spawner;
using StatusEffects;
using UnityEngine;
using UnityEngine.AI;
using Utils.Variables;

namespace Enemy
{
    public class EnemyManager : MonoBehaviour, IEffectable
    {
        [SerializeField] private EnemyStats stats; 
        [SerializeField] private ScriptableGameObjectList _enemyList;
        [SerializeField] private EnemyAttack attack;
        public EnemyAttack Attack => attack;
        public EnemyStats Stats => stats;
        public EnemyHealth HealthSystem { get; private set; }

        [HideInInspector] public EnemyWaveManager WaveManager;

        private int _currentWaypoint;
        
        private List<EffectHolder> _statusEffects; 
        public List<EffectHolder> StatusEffects => _statusEffects;
        [SerializeField] private GameObject _statusVFXParticlesParent;
        [SerializeField] private GameObject _statusMaterialParent;

        public float MoveSpeedMultiplier { get; set; } = 1f;
        public float AttackDamageMultiplier { get; set; } = 1f;
        public float DamageTakenMultiplier { get; set; } = 1f;        
        public float OnDeathHealthMultiplier { get; set; } = 1f;
        public float MoveSpeed => stats.MoveSpeed * MoveSpeedMultiplier;
        public int AttackDamage => Mathf.FloorToInt(stats.AttackDamage * AttackDamageMultiplier);

        
        public int CurrentWaypoint
        {
            get => _currentWaypoint;
            set => _currentWaypoint = Mathf.Min(value, WaveManager.Lane.Waypoints.Length - 1);
        }

        public NavMeshAgent Agent { get; private set; }

        private void Awake()
        {
            _statusEffects = new List<EffectHolder>();
            HealthSystem = gameObject.GetComponent<EnemyHealth>();
            // Navigation setup
            SetupNavigation();
        }

        private void OnEnable()
        {
            _enemyList.Add(gameObject);
        }

        private void OnDisable()
        {
            _enemyList.Remove(gameObject);
        }
        
        private void SetupNavigation()
        {
            Agent = GetComponent<NavMeshAgent>();
            Agent.speed = MoveSpeed;
            Agent.acceleration = stats.Acceleration;
            Agent.angularSpeed = stats.TurnSpeed;
            Agent.stoppingDistance = stats.StoppingDistance;
            Agent.radius = stats.AvoidanceRadius;
            Agent.height = stats.AvoidanceHeight;
            Agent.avoidancePriority = stats.PushPriority;
        }

        public void InitEnemy(EnemyWaveManager waveManager)
        {
            Agent = GetComponent<NavMeshAgent>();
            WaveManager = waveManager;
        }

        private void Update()
        {
            if(_statusEffects.Count > 0) UpdateEffects();
        }



        public void ApplyEffect(StatusEffect[] statusEffect)
        {
            foreach (var effect in statusEffect)
            {
                for (var i = _statusEffects.Count - 1; i >= 0; i--)
                {
                    var holder = _statusEffects[i];
                    if(holder.Effect != effect) continue;
                    _statusEffects.Remove(holder);
                }

                var effectHolder = new EffectHolder
                {
                    Effect = effect,
                    Timer = 0,
                    TickTimer = 0
                };
                _statusEffects.Add(effectHolder);
                effect.TickEffect(this);
            }
            foreach (var effect in statusEffect)
            {
                _statusMaterialParent.SetActive(true);
                if (effect is not DamageTakenModifyEffect) continue;
                _statusVFXParticlesParent.SetActive(true);
            }
        }

        public void UpdateEffects()
        {

            for (var i = _statusEffects.Count - 1; i >= 0; i--)
            {
                var effect = _statusEffects[i];
                effect.Timer += Time.deltaTime;
                if (effect.Timer >= effect.Effect.Duration)
                {
                    RemoveEffect(effect);
                    effect.Effect.ReverseEffect(this);
                }
                else
                {
                    effect.TickTimer += Time.deltaTime;
                    if (!(effect.TickTimer >= effect.Effect.TickRate)) continue;
                    effect.TickTimer = 0;
                    effect.Effect.TickEffect(this);
                }
                
            }
        }

        public void RemoveEffect(EffectHolder statusEffect)
        {
            _statusEffects.Remove(statusEffect);
            if (_statusEffects.Count != 0) return;
            _statusMaterialParent.SetActive(false);
            _statusVFXParticlesParent.SetActive(false);
        }

    }
    
}