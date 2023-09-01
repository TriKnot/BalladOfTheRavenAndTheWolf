using System.Collections;
using Level.Tree;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Variables;
using System.Collections.Generic;
using Enemy;
using Player.State.Wolf;
using UnityEngine.Serialization;
using Utils;

namespace Player.PlayerAbilities.Wolf
{
    [CreateAssetMenu(fileName = "WolfSiphonAbility", menuName = "Player/Wolf/Attacks/WolfSiphonAbility", order = 0)]
    public class WolfSiphonAbility : PlayerAbilityBase
    {
        
        [SerializeField] private float _range;
        [SerializeField, Tooltip("How often we should check for nearby object in distance not time")]
        private float _distanceCheckInterval;
        private HashSet<SiphonTarget> _inRangeTargets;
        [SerializeField ] private ScriptableGameObjectList _targetList;
        private Vector3 _lastUpdatePosition;
        private Transform _transform;
        private Vector3 _currentPos;
        private WolfPlayerManager _playerManager;
        
        [SerializeField] private float _duration = 3f;
        
        [Header("Speed Increase")]
        [SerializeField] private float _speedDuration = 5f;
        [SerializeField] private float _speedMultiplier = 1f;
        
        [FormerlySerializedAs("_animationTriggerName")]
        [Header("Animation")]
        [SerializeField] private string _SyphonBoolName;
        private int AnimationHash => Animator.StringToHash(_SyphonBoolName);
        [SerializeField] private ScriptableGameObjectPool _siphonVFXPool;
        [SerializeField] private int HealthToTickAnimation = 5;
            
        public override void InitAbility(PlayerAbilityManager abilityManager)
        {
            _inRangeTargets = new HashSet<SiphonTarget>();
            _transform = abilityManager.transform;
            _lastUpdatePosition = _transform.position;
            _playerManager = (WolfPlayerManager) abilityManager.PlayerManager;
            manager = abilityManager;
        }

        public override void MakeUpdate()
        {
            _currentPos = _transform.position;
            var distance = Vector3.Distance(_currentPos, _lastUpdatePosition);
            if (distance >= _distanceCheckInterval)
            {
                _lastUpdatePosition = _currentPos;
                FindTargetsInRange();
            }
        }


        public override void Act(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if(_playerManager.BlightScriptableHealthSystem.CurrentHealth == 0) return;
            if (_playerManager.State == PlayerState.Locked) return;
            if (_inRangeTargets.Count == 0) return;
            
            SiphonTarget target = GetClosestTarget();
            if (target == null) return;
            
            var coroutine = manager.TriggerCoroutine(Siphon(target));
        }

        public override bool IsOnCooldown()
        {
            return false;
        }

        private IEnumerator Siphon(SiphonTarget target)
        {
            manager.PlayerManager.SetPlayerState(PlayerState.Locked);
            manager.PlayerManager.Animator.SetBool(AnimationHash, true);
            target.ToggleSyphonAnimation(true);
            var maxHealth = _playerManager.BlightScriptableHealthSystem.MaxHealth;
            float healAmount = 0;
            var tick = 0;
            while (_playerManager.BlightScriptableHealthSystem.CurrentHealth > 0)
            {
                 healAmount += maxHealth / _duration * Time.deltaTime;
                 if(healAmount > 1)
                 {
                     var heal = Mathf.FloorToInt(healAmount);
                     _playerManager.BlightScriptableHealthSystem.Damage(heal);
                     target.HeartScriptableHealth.Heal(heal);
                     healAmount -= heal;
                     tick += heal;
                 } 
                 if(tick >= HealthToTickAnimation)
                 {
                     tick -= HealthToTickAnimation;
                     var vfx = _siphonVFXPool.GetPooledObject();
                     vfx.GetComponent<EnemyKillOrb>().Init(_transform.position, target.transform);
                     vfx.SetActive(true);
                 }
                 yield return null;
            }
            _playerManager.SetPlayerState(PlayerState.Grounded);
            target.ToggleSyphonAnimation(false);
            FindTargetsInRange();
            manager.TriggerCoroutine(IncreaseSpeed());
            manager.PlayerManager.Animator.SetBool(AnimationHash, false);
            yield return null;
        }

        private IEnumerator IncreaseSpeed()
        {
            _playerManager.PlayerMovement.MoveSpeedMultiplier += _speedMultiplier;
            yield return new WaitForSeconds(_speedDuration);
            _playerManager.PlayerMovement.MoveSpeedMultiplier -= _speedMultiplier;
        }

        private SiphonTarget GetClosestTarget()
        {
            SiphonTarget closestTarget = null;
            var closestDistance = float.MaxValue;
            foreach (var target in _inRangeTargets)
            {
                var distance = Vector3.Distance(target.transform.position, _currentPos);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }
            return closestTarget;
        }
        
        private void FindTargetsInRange()
        {
            foreach (var target in _targetList.Value)
            {
                var distance = Vector3.Distance(target.transform.position, _currentPos);
                if (_playerManager.BlightScriptableHealthSystem.CurrentHealth > 0 && distance <= _range)
                {
                    AddTargetToList(target);
                }
                else
                {
                    if (target.TryGetComponent(out SiphonTarget pickupTarget) && _inRangeTargets.Contains(pickupTarget))
                    {
                        RemoveTargetFromList(pickupTarget);
                    }
                }
            }        
        }

        private void AddTargetToList(GameObject target)
        {
            if (!target.TryGetComponent(out SiphonTarget siphonTarget)) return;
            siphonTarget.ToggleTargetIndicator(true);
            _inRangeTargets.Add(siphonTarget);
        }
        private void RemoveTargetFromList(SiphonTarget pickupTarget)
        {
            pickupTarget.ToggleTargetIndicator(false);
            _inRangeTargets.Remove(pickupTarget);
        }

    }
}
