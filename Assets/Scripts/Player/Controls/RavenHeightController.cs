using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controls
{
    [RequireComponent(typeof(Rigidbody))]
    public class RavenHeightController : MonoBehaviour
    {
        [Header("Flight Controller")]
        [SerializeField,Tooltip("How fast you move up and down")] private int _heightSpeed;
        [SerializeField,Tooltip("How fast you move up when too close to the ground")] private int _forceHeightSpeed;
        [SerializeField] private float _minHeight;
        [SerializeField] private float _maxHeight;
        [SerializeField] private LayerMask _layerMask;

        private float _newMinHeight;
        private Vector2 _heightInput = Vector2.zero;
        private Vector3 _direction = Vector3.zero;
        private Rigidbody _rb;

        [Header("Cookie Light")]
        public GameObject _cookieLight;
    
        // Start is called before the first frame update
        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            HeightControl();
            RestrainHeight();
        }

        public void OnHeight(InputAction.CallbackContext context)
        {
            _heightInput = context.ReadValue<Vector2>();
        }

        private void HeightControl()
        {
            _direction = new Vector3(0, _heightInput.normalized.y, 0);
            _direction = _direction.normalized;
            if (transform.position.y <= _newMinHeight)
            {
                if(_direction.y < 0f)
                    _direction = new Vector3(0, 0, 0);
            }
            else if (transform.position.y >= _maxHeight)
            {
                if(_direction.y > 0f)
                    _direction = new Vector3(0, 0, 0);
            }
            _rb.velocity = new Vector3(_rb.velocity.x,_heightSpeed*_direction.y, _rb.velocity.z);
            
        }

        private void RestrainHeight()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down,out hit, Mathf.Infinity, _layerMask))
            {
                _newMinHeight = hit.point.y + _minHeight;
                
                if (transform.position.y < _newMinHeight-1f)
                {
                    _rb.velocity = new Vector3(_rb.velocity.x,_forceHeightSpeed, _rb.velocity.z);
                }
                
            }
        }
    }
}
