using Enemy.AI.States;
using Enemy.Attacks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Utils.Variables;

namespace Enemy.AI
{
    public class EnemyStateController : MonoBehaviour
    {
        // Constant References
        [FormerlySerializedAs("Enemy")] [HideInInspector] public EnemyManager EnemyManager;
        [HideInInspector] public EnemyStats Stats;
        [HideInInspector] public EnemyAttack Attack;
        [HideInInspector] public NavMeshAgent Agent;
        
        // Runtime Variables
        [HideInInspector] public bool StateBoolVariable;
        [HideInInspector] public float StateFloatVariable;
        [HideInInspector] public Vector3 StateVector3Variable;
        [HideInInspector] public GameObject MoveTarget;
        [HideInInspector] public GameObject AttackTarget;
        [HideInInspector] public NavMeshPath Path;
        private float _updateTimer;

        // Serialized Variables/References
        public EnemyState CurrentState;
        [SerializeField] private EnemyState _remainInCurrentState;
        [SerializeField] private float _updateFrequency = 0.1f;
        [SerializeField] private bool _isActive;
        [SerializeField] private ScriptableGameObjectList _enemyTargetList;
        public ScriptableGameObjectList EnemyTargetList => _enemyTargetList;
        
        [Header("Debug: Shows gizmos for targets")]
        [SerializeField] private bool _debug;

        private void Start()
        {
            EnemyManager = GetComponent<EnemyManager>();
            Stats = EnemyManager.Stats;
            Attack = EnemyManager.Attack;
            Agent = EnemyManager.Agent;
            _updateTimer = 0;
        }

        private void Update()
        {
            if(!_isActive) return;
            
            _updateTimer += Time.deltaTime;
            if (!(_updateTimer >= _updateFrequency)) return;
            
            _updateTimer = 0;
            CurrentState.UpdateState(this);

        }
        
        public void Activate(bool active, Transform target)
        {
            _isActive = active;
        }
        
        public void TransitionToState(EnemyState nextState)
        {
            if(nextState != _remainInCurrentState)
            {
                CurrentState = nextState;
                OnExitState();
            }        
        }
        
        public bool HasTimeElapsed(float duration)
        {
            StateFloatVariable += Time.deltaTime;
            if(StateFloatVariable >= duration)
            {
                StateFloatVariable = 0;
                return true;
            }
            return false;
        }
        
        
        private void OnExitState()
        {
            StateBoolVariable = false;
            StateFloatVariable = 0;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            if(!_debug) return;
            if (MoveTarget != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(Agent.destination, 1f);
            }
            if (AttackTarget != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(AttackTarget.transform.position, 2f);
            }
        }
    }
}