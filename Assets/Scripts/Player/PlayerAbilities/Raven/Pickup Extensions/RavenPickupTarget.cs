using System.Collections;
using Player.PlayerAbilities.Raven.Pickup_Extensions;
using Player.State.Wolf;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Utils.Variables;

namespace Player.PlayerAbilities.Raven
{
    [RequireComponent(typeof(Rigidbody))]
    public class RavenPickupTarget : MonoBehaviour
    {
        [SerializeField] private ScriptableGameObjectList _pickupList;
        [SerializeField] private bool _constraint = true;
        public Rigidbody rb;
        public float dropSpeed;
        public Transform pickup;

        private RavenPickupAbility ravenPickupAbility;

        [Header("Reset pickup")]
        [SerializeField] private bool _resetPositionOnTimer;
        [SerializeField] private float _waitSeconds;
        private Vector3 _startPosition;
        private Quaternion _startRotation;

        [Header("Outline & Cookie Light")]
        [SerializeField] private GameObject _outlineGameObject;
        public GameObject _cookieLight;

        private Light _light; 
        
        [SerializeField] private RespawnManager _respawnManager;
        public RespawnManager RespawnManager => _respawnManager;


        private void Awake()
        {
            
            _cookieLight.SetActive(false);
            
            _startPosition = transform.position;
            _startRotation = transform.rotation;
        }

        private void OnEnable()
        {
            AddToList();
            if (_constraint)
            {
                rb = GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX |
                                 RigidbodyConstraints.FreezePositionZ;
            }
        }

        private void OnDisable()
        {
            RemoveFromList();
        }

        public void ToggleTargetIndicator(bool active)
        {
            _outlineGameObject.SetActive(active);
        }

        public void AddToList()
        {
            _pickupList.Add(gameObject);
        }

        public void RemoveFromList()
        {
            _pickupList.Remove(gameObject);
        }

        public void Pickup(Transform target, RavenPickupAbility pickupAbility)
        {
            transform.position = target.position - new Vector3(0, 4, 0);
            rb.isKinematic = true;
            transform.parent = target;
            
            ravenPickupAbility = pickupAbility;
            ravenPickupAbility.holdingTarget = true;
            ravenPickupAbility.pickupList.Remove(gameObject);
            ravenPickupAbility.UpdateFlyingSpeed(); //Update flying speed to carry speed
            
            _cookieLight.SetActive(true);
        }

        public void Drop(Transform target)
        {
            //Check if dropping wolf
            if (transform.TryGetComponent(out WolfPlayerManager playerManager))
            {
                playerManager.SetPlayerState(PlayerState.Grounded);
            }

            transform.parent = null;
            rb.isKinematic = false;
            rb.velocity = new Vector3(0, -dropSpeed, 0);
            
            ravenPickupAbility.holdingTarget = false;
            ravenPickupAbility.UpdateFlyingSpeed(); //Update flying speed to default speed
            ravenPickupAbility.pickupList.Add(gameObject);

            _cookieLight.SetActive(false);
            
            CheckIfResetPosition();
        }

        private void CheckIfResetPosition()
        {
            if (_resetPositionOnTimer)
            {
                StartCoroutine(ResetPositionTimer(_waitSeconds));
            }
        }

        private IEnumerator ResetPositionTimer(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            ResetPosition();
        }

        public void ResetPosition()
        {
            if (ravenPickupAbility != null && ravenPickupAbility.pickup == transform)
            {
                ravenPickupAbility.holdingTarget = false;
            }

            transform.parent = null;
            transform.position = _startPosition;
            transform.rotation = _startRotation;
        }
    }
}