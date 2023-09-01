using Player.State.Wolf;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player.PlayerAbilities.Raven
{
    [CreateAssetMenu(fileName = "RavenPickupRotation", menuName = "Player/Raven/Attacks/RavenPickupRotation",
        order = 0)]
    public class RavenPickupRotation : PlayerAbilityBase
    {
        [SerializeField,Range(0f,100f)] float _rotationSpeed;
        [SerializeField] private RotateDirection _rotateDirection;
        [FormerlySerializedAs("_ravenPickup")] [SerializeField] private RavenPickupAbility _ravenPickupAbility;
        
        private Transform _target;
        private bool _holdingButtonDown;
        private Transform _transform;
        private RavenPickupTarget _ravenPickupTarget;

        public delegate void OnRotation();
        public event OnRotation OnRotationEvent;

        public override void InitAbility(PlayerAbilityManager playerManager)
        {
            _transform = playerManager.transform;
        }

        public override void MakeUpdate()
        {
            if (_holdingButtonDown && _ravenPickupAbility.holdingTarget)
            {
                _target = _ravenPickupAbility.pickup;
                if (!_target.TryGetComponent(out WolfPlayerManager wolfPlayerManager))
                {
                    var rotSpd = _rotationSpeed * 2 * Time.deltaTime;
                    if(_rotateDirection == RotateDirection.Right)
                        _target.Rotate(0,0,rotSpd);
                    else if (_rotateDirection == RotateDirection.Left)
                        _target.Rotate(0,0,-rotSpd);
                    OnRotationEvent?.Invoke();
                }
            }
        }

        public override void Act(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _holdingButtonDown = true;
            }
            else if (context.canceled)
            {
                _holdingButtonDown = false;
            }
        }

        public override bool IsOnCooldown()
        {
            return false;
        }

        enum RotateDirection
        {
            Right,
            Left
        }
    }
}