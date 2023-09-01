using System;
using System.Collections;
using System.Linq;
using Enemy;
using Player.State;
using SharedBaseClasses;
using StatusEffects;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using Utils.Variables;

namespace Player.PlayerAbilities.Raven
{
    [CreateAssetMenu(fileName = "RavenBasicAttack", menuName = "Player/Raven/Attacks/RavenBasicAttack", order = 0)]
    public class RavenBasicAttack : PlayerAbilityBase
    {
        [SerializeField] private float _rangeMultiplier = 1f;
        [SerializeField] private ScriptableGameObjectList _spawnedEnemyList;
        [SerializeField] private ScriptableGameObjectPool _projectilePool;
        private Transform _transform;
        private Vector3 _currentPos;
        private PlayerManager _playerManager;
        
        [SerializeField] private float _attackSpeedMultiplier = 1f;
        private float _coolDownTimer;

        [SerializeField]  private GameObject _projectilePrefab;
        [SerializeField] private float _damageMultiplier = 1f;
        [SerializeField] private float _projectileSpeed = 10f;
        [SerializeField] private LayerMask _targetLayerMask;
        
        [SerializeField, Tooltip("Time before each attack where input is buffered and still counts as attack input.")] 
        private float _inputBufferTime = 1f;
        private float _inputBufferTimer;
        InputAction.CallbackContext _bufferedContext;
        private bool _shouldAttack;
        
        [Header("Sound")]
        [SerializeField] private string _soundPath;

        public override void InitAbility(PlayerAbilityManager abilityManager)
        {
            _transform = abilityManager.transform;
            _playerManager = abilityManager.PlayerManager;
            manager = abilityManager;
            _coolDownTimer = 0;
        }

        public override void MakeUpdate()
        {
            _currentPos = _transform.position;
            CountDownInputBufferTimer();
            CountDownTimer();
            if(IsOnCooldown()) return;
            if(_shouldAttack)
            {
                Shoot();
                _shouldAttack = false;
            }        
        }

        public override void Act(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            _shouldAttack = true;
            _inputBufferTimer = _inputBufferTime;
        }

        public override bool IsOnCooldown()
        {
            return _coolDownTimer > 0;
        }
        
        private void CountDownInputBufferTimer()
        {
            if(_inputBufferTimer <= 0)
            {
                _shouldAttack = false;
                return;
            }
            _inputBufferTimer = Mathf.Max(_inputBufferTimer - Time.deltaTime, 0);
        }

        private void Shoot()
        {
            EnemyManager target = GetClosestUnAffectedTarget();
            if (target == null) return;
            
            _coolDownTimer = _playerManager.Stats.AttackSpeed * _attackSpeedMultiplier;
            
            int damage = Mathf.FloorToInt(_playerManager.Stats.AttackDamage * _damageMultiplier);
            var projectile = _projectilePool.GetPooledObject();
            projectile.transform.position = _transform.position;
            projectile.GetComponent<Projectile>().Init(target.transform, damage, _projectileSpeed,_projectilePool, _statusEffect);
            if(!string.IsNullOrEmpty(_soundPath))
                FMODUnity.RuntimeManager.PlayOneShot(_soundPath);
        }

        private void CountDownTimer()
        {
            if(IsOnCooldown())
                _coolDownTimer = Mathf.Max(_coolDownTimer - Time.deltaTime, 0);
        }

        private EnemyManager GetClosestUnAffectedTarget()
        {
            Physics.Raycast(_transform.position, -_transform.up, out var hit,Mathf.Infinity, _targetLayerMask);
            var inRangeTargets =
                _spawnedEnemyList.GetObjectsInRange(hit.point, _playerManager.Stats.AttackRange * _rangeMultiplier);
            Array.Sort(inRangeTargets, (a, b) =>
            {
                var aDistance = Vector3.Distance(a.transform.position, _currentPos);
                var bDistance = Vector3.Distance(b.transform.position, _currentPos);
                return aDistance.CompareTo(bDistance);
            });
            var target = (from t in inRangeTargets where t.GetComponent<IEffectable>().StatusEffects.Count == 0 select t.GetComponent<EnemyManager>()).FirstOrDefault();
            if (target == null)
            {
                target = inRangeTargets.FirstOrDefault()?.GetComponent<EnemyManager>();
            }
            return target;
        }


    }
}
