using System.Collections.Generic;
using Player.State.Wolf;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Variables;

namespace Player.PlayerAbilities.Raven
{
    [CreateAssetMenu(fileName = "RavenPickupAbility", menuName = "Player/Raven/Attacks/RavenPickupAbility", order = 0)]
    public class RavenPickupAbility : PlayerAbilityBase
    {
        [Header("Stats")]
        [SerializeField] private float _carrySpeedMultiplier;
        [SerializeField] private float _carryAccelerationSpeed;
        [SerializeField] private float _carryDecelerationSpeed;
        [SerializeField] private float _pickupRange;
        [SerializeField] private int _dropSpeed;

        [Header("Check for object")]
        [SerializeField, Tooltip("How often we should check for nearby object in distance not time")]
        private float _distanceToCheck;

        [SerializeField] private HashSet<RavenPickupTarget> _foundObjects;
        public ScriptableGameObjectList pickupList;
        [SerializeField] private LayerMask _dropZoneLayer;
        private Vector3 _updatePosition;
        private float _dis;
        private Transform _transform;
        private Vector3 _hitPoint;

        public bool holdingTarget = false;
        public Transform pickup;
        
        public delegate void OnPickup();
        public event OnPickup OnPickupEvent;
        public delegate void OnDrop();
        public event OnDrop OnDropEvent;
        public delegate void OnMoved();
        public event OnMoved OnMovedEvent;
        
        public override void InitAbility(PlayerAbilityManager playerManager)
        {

            holdingTarget = false;
            _foundObjects = new();
            _transform = playerManager.transform;
            _updatePosition = _transform.position;
            
        }

        public override void MakeUpdate()
        {
            _dis = Vector3.Distance(_transform.position, _updatePosition); //Update Distance
            if (_dis >= _distanceToCheck) //Check distance
            {
                _updatePosition = _transform.position; //Update new position
                CheckForObject(); //Check for targeted objects

                if (holdingTarget)
                {
                    pickup.GetComponentInChildren<PickupTargetCookieLight>().UpdateColor();
                }
                OnMovedEvent?.Invoke();
            }
        }

        public override void Act(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (holdingTarget == false)
                    PickUp();
                else if (holdingTarget == true)
                    Drop();
            }
        }

        public override bool IsOnCooldown()
        {
            return false;
        }

        private void CheckForObject()
        {
            if (Physics.Raycast(_transform.position, Vector3.down, out var hit))
            {
                _hitPoint = hit.point;
                foreach (var target in pickupList.Value)
                {
                    _dis = Vector3.Distance(target.transform.position, _hitPoint);
                    if (_dis <= _pickupRange)
                    {
                        AddTargetToList(target);
                    }
                    else
                    {
                        if (target.TryGetComponent(out RavenPickupTarget pickupTarget) &&
                            _foundObjects.Contains(pickupTarget))
                        {
                            RemoveTargetFromList(pickupTarget);
                        }
                    }

                    HighlightClosestTarget();
                }
            }
        }

        private RavenPickupTarget GetClosestTarget()
        {
            RavenPickupTarget bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = _hitPoint;

            foreach (RavenPickupTarget potentialTarget in _foundObjects)
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }

            return bestTarget;
        }

        private void AddTargetToList(GameObject obj)
        {
            if (obj.TryGetComponent(out RavenPickupTarget pickupTarget))
            {
                _foundObjects.Add(pickupTarget);
            }
        }

        private void RemoveTargetFromList(RavenPickupTarget pickupTarget)
        {
            pickupTarget.ToggleTargetIndicator(false);
            _foundObjects.Remove(pickupTarget);
        }

        private void HighlightClosestTarget()
        {
            var bestTarget = GetClosestTarget();
            if (_foundObjects.Contains(bestTarget))
            {
                foreach (var pickupTargets in _foundObjects)
                {
                    pickupTargets.ToggleTargetIndicator(false);
                }

                bestTarget.ToggleTargetIndicator(true);
            }
        }

        private void PickUp()
        {
            var bestTarget = GetClosestTarget();
            if (bestTarget != null)
            {
                //If attacking don't pickup
                if (bestTarget.TryGetComponent(out WolfPlayerManager playerManager))
                {
                    if (playerManager.State == PlayerState.Locked) return;
                    
                    playerManager.SetPlayerState(PlayerState.Aerial);
                }
                
                bestTarget.Pickup(_transform, this);
                pickup = bestTarget.transform;

                pickupList.Remove(bestTarget.gameObject);
                RemoveTargetFromList(bestTarget);

                var ravenPickupTarget = pickup.GetComponent<RavenPickupTarget>();
                
                OnPickupEvent?.Invoke();
            }
        }

        public void Drop()
        {
            var ravenPickupTarget = pickup.GetComponent<RavenPickupTarget>();
            
            if (Physics.Raycast(_transform.position, Vector3.down, Mathf.Infinity, _dropZoneLayer))
            {
                ravenPickupTarget.dropSpeed = _dropSpeed;
                ravenPickupTarget.Drop(pickup);
                ravenPickupTarget.pickup = pickup;
            }
            OnDropEvent?.Invoke();
        }

        public void UpdateFlyingSpeed()
        {
            if (holdingTarget)
            {
                if (_transform.TryGetComponent(out PlayerMovement _playerMovement))
                {
                    _playerMovement.MoveSpeedMultiplier += _carrySpeedMultiplier;
                    _playerMovement.AccelerationMultiplier += _carryAccelerationSpeed;
                    _playerMovement.DecelerationMultiplier += _carryDecelerationSpeed;
                }
            }
            else
            {
                if (_transform.TryGetComponent(out PlayerMovement _playerMovement))
                {
                    _playerMovement.MoveSpeedMultiplier -= _carrySpeedMultiplier;
                    _playerMovement.AccelerationMultiplier -= _carryAccelerationSpeed;
                    _playerMovement.DecelerationMultiplier -= _carryDecelerationSpeed;
                }
            }
        }
    }
}