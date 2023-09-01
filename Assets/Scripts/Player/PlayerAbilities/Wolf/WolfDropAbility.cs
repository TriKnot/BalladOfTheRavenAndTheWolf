using System.Collections;
using System.Collections.Generic;
using Enemy;
using Player.PlayerAbilities.Raven;
using Player.State;
using Player.State.Wolf;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Variables;

namespace Player.PlayerAbilities.Wolf
{
    
    [CreateAssetMenu(fileName = "WolfDropAbility", menuName = "Player/Wolf/Attacks/WolfDropAbility", order = 0)]
    public class WolfDropAbility : PlayerAbilityBase
    {
        [SerializeField] private float _groundCheckDistance;
        [SerializeField] private LayerMask _DropZoneLayer;
        [SerializeField] private ScriptableGameObjectList _activeEnemies;
        
        [Header("Cooldown Times")]
        [Tooltip("In Seconds")]
        [SerializeField] private float _floatTime;
        [Tooltip("In Seconds")]
        [SerializeField] private float _cooldownTime;
        [Tooltip("In Seconds")]
        [SerializeField] private float _recoveryTime;

        [Header("Stats")]
        [SerializeField] private int _slamStrength;
        [SerializeField] private int _attackRange;
        [SerializeField] private int _attackDamageMultiplier;
        
        private bool _groundCheck;
        private Transform _transform;
        private Rigidbody _rb;
        private WolfPlayerManager _wolfPlayerManager;
        private PlayerManager _playerManager;
        private PlayerMovement _playerMovement;
        private PlayerStats _playerStats;
        private bool _isSlamming = false;
        private bool _isAttacking = false;
        [SerializeField] private List<EnemyManager> _inRangeTargets;
        private int _attackDamage;
        
        private float _cooldown;
        private float _float;
        private float _recovery;
        
        [Header("Animation")]
        [SerializeField] private string _animationTriggerName;
        private int AnimationHash => Animator.StringToHash(_animationTriggerName);
        [SerializeField] private GameObject _aoeSplashPrefab;
        private GameObject _aoeSplash;

        
        public override void InitAbility(PlayerAbilityManager playerManager)
        {
            //OnEnable event
            _transform = playerManager.transform;
            _rb = _transform.GetComponent<Rigidbody>();
            _wolfPlayerManager = _transform.GetComponent<WolfPlayerManager>();
            _playerManager = _transform.GetComponent<PlayerManager>();
            _playerMovement = _transform.GetComponent<PlayerMovement>();
            _playerStats = _playerManager.Stats;
            _isAttacking = false;
            manager = playerManager;
            _attackDamage = Mathf.FloorToInt(_playerStats.AttackDamage * _attackDamageMultiplier);
            _inRangeTargets = new List<EnemyManager>();
            _aoeSplash = Instantiate(_aoeSplashPrefab, _transform);
            _aoeSplash.transform.rotation = _playerManager.PlayerModelObject.transform.rotation;
            _aoeSplash.transform.localPosition = Vector3.zero;
        }
        
        public override void MakeUpdate()
        {
            UpdateTimers();
            _groundCheck = Physics.Raycast(_transform.position, Vector3.down, _groundCheckDistance);
            if (_isAttacking)
            {
                if (_float > 0)
                {
                    var rbVel = _rb.velocity;
                    _rb.velocity = new Vector3(rbVel.x, 0f, rbVel.z);  
                }
                else if (_float <= 0 && !_isSlamming && _playerMovement.isGravityOn == false)
                {
                    _isSlamming = true;
                    _playerMovement.isGravityOn = true;
                    _rb.AddForce(Vector3.down * _slamStrength);
                }
                WolfSlam();
            }
        }
            
        public override void Act(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (_cooldown <= 0 && !_groundCheck && !_isAttacking
                    && Physics.Raycast(_transform.position,Vector3.down,Mathf.Infinity,_DropZoneLayer))
                {
                    if (_transform.TryGetComponent( out RavenPickupTarget ravenPickupTarget))
                    {
                        ravenPickupTarget.Drop(_transform);
                    }
                    _isSlamming = false;
                    _isAttacking = true;
                    _cooldown = _cooldownTime;
                    _float = _floatTime;
                    _playerMovement.isGravityOn = false;
                    _playerManager.SetPlayerState(PlayerState.Locked);
                }
            }
        }

        public override bool IsOnCooldown()
        {
            return _cooldown > 0;
        }

        private void UpdateTimers()
        {
            //Reduce cooldowns
            if (_float > 0) _float -= Time.deltaTime;
            if (_cooldown > 0) _cooldown -= Time.deltaTime;
            if (_recovery > 0) _recovery -= Time.deltaTime;
        }

        private void WolfSlam()
        {
            if (_groundCheck && _isSlamming)
            {
                Attack();
                manager.TriggerCoroutine(AnimateAttack());
                _isSlamming = false;
                _isAttacking = false;
                _playerManager.SetPlayerState(PlayerState.Grounded);
                _recovery = _recoveryTime;
            }
        }

        private void RecoveryFrames()
        {
            if (_recovery <= 0)
            {
                _isAttacking = false;
            }
        }
        
        private IEnumerator AnimateAttack()
        {
            manager.PlayerManager.Animator.Play(AnimationHash);
            _aoeSplash.SetActive(true);
            yield return new WaitForSeconds(1f);
            _aoeSplash.SetActive(false);
            // _attackVFXObject.SetActive(true);
            // _attackDurationTimer = _attackAnimationAnimator.GetCurrentAnimatorStateInfo(0).length;
        }

        private void Attack()
        {
            if(_activeEnemies.Count == 0) return;
  
            //Find enemies with an enemyManager in distance and add them to inRangeList.
            foreach (var act in _activeEnemies.Value)
            {
                if (act.TryGetComponent(out EnemyManager enemyManager) && _inRangeTargets.Contains(enemyManager))
                {
                    _inRangeTargets.Remove(enemyManager);
                }
                
                var distance = Vector3.Distance(_transform.position, act.transform.position);
                if (distance <= _attackRange)
                {
                    _inRangeTargets.Add(enemyManager);
                }
            }

            if(_inRangeTargets.Count == 0) return;
            //Go through inRangeList to see if they have a health script and deal damage to them.
            for (var i = _inRangeTargets.Count - 1; i >= 0; i--)
            {
                var target = _inRangeTargets[i];
                if(target)
                {
                    if(target.TryGetComponent(out EnemyHealth enemy))
                    {
                        var damage = Mathf.FloorToInt(_attackDamageMultiplier * _wolfPlayerManager.AttackDamage);
                        enemy.Damage(damage);
                        if(enemy.GetHealthPercent() <= 0)
                            _inRangeTargets.Remove(target);
                        
                        if(damage > _minFloatingTextDamage)
                            ShowFloatingText(damage, target.transform.position);
                    }
                }

            }
        }
    }
}
