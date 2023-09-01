using System.Collections.Generic;
using Enemy;
using Player.State.Wolf;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.PlayerAbilities.Wolf
{
    [CreateAssetMenu(fileName = "WolfBasicAttackAbility", menuName = "Player/Wolf/Attacks/WolfBasicAttackAbility", order = 0)]
    public class WolfBasicAttackAbility : PlayerAbilityBase
    {
        [SerializeField] private float _attackRangeMultiplier;
        [SerializeField] private float _attackAngle;
        [SerializeField] private float _attackDamageMultiplier;
        [SerializeField] private float _attackCooldown;
        // [SerializeField] private GameObject _attackVFXPrefab;
        [SerializeField] private string _animationTriggerName;
        private int AnimationHash => Animator.StringToHash(_animationTriggerName);
        [SerializeField] private string _soundPath;
        [SerializeField, Tooltip("Time during which the wolf is immobilized during the attack. Will never be longer than cooldown.")] 
        private float _immobilizeTime = .3f;
        private float _immobilizeTimer;
        
        [Header("Animation")]
        [SerializeField] private GameObject _attackVFXPrefab;
        private GameObject _attackVFXObject;

      

        private bool _isOnCoolDown;
        private float _attackTimer;
        private HashSet<Collider> _hitColliders;
        private bool _isAttacking;
        private WolfPlayerManager _playerManager;
        // private Animator _attackAnimationAnimator;
        // private GameObject _attackVFXObject;

        public override void InitAbility(PlayerAbilityManager abilityManager)
        {
            manager = abilityManager;
            _playerManager = (WolfPlayerManager) abilityManager.PlayerManager;
            _hitColliders = new HashSet<Collider>();
            _attackVFXObject = Instantiate(_attackVFXPrefab, _playerManager.PlayerModelObject.transform);
            _attackVFXObject.transform.rotation = _playerManager.PlayerModelObject.transform.rotation;
            _attackVFXObject.transform.localPosition = Vector3.zero;
            _attackVFXObject.SetActive(false);
        }
        
        public override void MakeUpdate()
        {
            if(_isOnCoolDown)
                CountDownTimer();
            
        }
        
        private void CountDownTimer()
        {
            _attackTimer -= Time.deltaTime;
            _immobilizeTimer -= Time.deltaTime;

            if (manager.PlayerManager.State == PlayerState.Locked && _immobilizeTimer <= 0)
            {
                _playerManager.SetPlayerState(PlayerState.Grounded);
            }
            
            if (_attackTimer > 0) return;
            _attackTimer = 0;
            _isOnCoolDown = false;
            _hitColliders.Clear();
            _attackVFXObject.SetActive(false);
        }
    
        public override void Act(InputAction.CallbackContext context)
        {
            if (_isOnCoolDown) return;
            _isOnCoolDown = true;
            _isAttacking = true;
            _attackTimer = _attackCooldown;
            _immobilizeTimer = _immobilizeTime;
            _playerManager.SetPlayerState(PlayerState.Locked);
            Attack();
            AnimateAttack();
        }

        public override bool IsOnCooldown()
        {
            return _isOnCoolDown;
        }

        private void AnimateAttack()
        {
            manager.PlayerManager.Animator.Play(AnimationHash);
            _attackVFXObject.SetActive(true);
        }

        private void Attack()
        {
            Collider[] hitColliders = Physics.OverlapSphere(manager.transform.position, _attackRangeMultiplier);
            
            FindTargets(hitColliders);
            DamageTargets();
            FMODUnity.RuntimeManager.PlayOneShot(_soundPath);
        }
        
        private void DamageTargets()
        {
            foreach (var col in _hitColliders)
            {
                if (!col.TryGetComponent(out EnemyManager enemy)) continue;
                var damage = Mathf.FloorToInt(_attackDamageMultiplier * _playerManager.AttackDamage);
                enemy.HealthSystem.Damage(damage);
                enemy.ApplyEffect(_statusEffect);
                
                if(damage >= _minFloatingTextDamage)
                    ShowFloatingText(damage, enemy.transform.position);
            }
        }

        private void FindTargets(Collider[] hitColliders)
        {
            foreach (var col in hitColliders)
            {
                if(IsTargetInCone(col.transform, _attackAngle))
                {
                    _hitColliders.Add(col);
                }
            }
        }
       
        private bool IsTargetInCone(Transform target, float angle)
        {
            var direction = (target.position - manager.BodyTransform.position).normalized;
            return Vector3.Angle(manager.BodyTransform.forward, direction) < angle / 2;
        }
        
        
        
    }
}
