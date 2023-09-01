using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Enemy
{
    public class EnemyKillOrb : MonoBehaviour
    {
        [SerializeField] private float _upSpeed = 10f;
        [SerializeField] private float _toTargetSpeed = 10f;
        [SerializeField] private float upDistance = 10f;
        [SerializeField] private float _sideDistance = 10f;
        [SerializeField] private float _transitionDelayTime = 0.5f;
        [SerializeField] private Vector2 _minMaxSize;
        [SerializeField] private ScriptableGameObjectPool _orbPool;

        private Transform _target;

        public void Init(Vector3 startPos, Transform target)
        {
            _target = target;
            transform.position = startPos;
            transform.localScale = Vector3.one * _minMaxSize.x;
            StartCoroutine(ShootUp());
        }

        private IEnumerator ShootUp()
        {
            var randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            var targetPos = transform.position + Vector3.up * upDistance + randomDir * _sideDistance;
            while (transform.position != targetPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, _upSpeed * Time.deltaTime);
                transform.localScale = Vector3.one * Mathf.Lerp(_minMaxSize.x, _minMaxSize.y, Vector3.Distance(transform.position, targetPos));
                yield return null;
            }

            yield return new WaitForSeconds(_transitionDelayTime);
            StartCoroutine(LerpToPlayer());
        }
        

        private IEnumerator LerpToPlayer()
        {
            while (Vector3.Distance(transform.position, _target.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _toTargetSpeed * Time.deltaTime);
                transform.localScale = Vector3.one * Mathf.Lerp(_minMaxSize.x, _minMaxSize.y, Vector3.Distance(transform.position, _target.transform.position));
                yield return null;
            }

            _orbPool.ReturnToPool(gameObject);
        }
        
    }
}
