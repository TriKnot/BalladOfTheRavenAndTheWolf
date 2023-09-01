using Enemy;
using StatusEffects;
using UnityEngine;
using Utils;

namespace SharedBaseClasses
{
    [RequireComponent(typeof(Collider))]
    public class Projectile : MonoBehaviour
    {
        private Transform _target;
        private int _damage;
        private float _speed;
        private StatusEffect[] _statusEffect;
        private ScriptableGameObjectPool _pool;
        
        public void Init(Transform target, int damage, float speed, ScriptableGameObjectPool pool, StatusEffect[] statusEffect = null)
        {
            _target = target;
            _damage = damage;
            _speed = speed;
            _pool = pool;
            _statusEffect = statusEffect;
        }
        
        private void Update()
        {
            if (!_target)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                float spd = _speed * Time.deltaTime;
                transform.LookAt(_target);
                transform.position = Vector3.MoveTowards(transform.position, _target.position, spd);

                if (!(Vector3.Distance(transform.position, _target.position) < 0.1f)) return;
                Hit();
            }
           
        }

        private void Hit()
        {
            var hit = false;
            if(_target.TryGetComponent(out EnemyHealth enemy))
            {
                enemy.Damage(_damage);
                hit = true;
            }

            if (_target.TryGetComponent(out IEffectable target))
            {
                target.ApplyEffect(_statusEffect);
                hit = true;
            }

            if (!hit) return;
                _pool.ReturnToPool(gameObject);
        }
        
        private void OnDestroy()
        {
            _target = null;
        }
        
        // private void OnTriggerEnter(Collider other)
        // {
        //     Debug.Log($"Hit: {other.name}");
        //     if (other.TryGetComponent(out EnemyHealth enemy))
        //     {
        //         print($"Target: {_target.name}, other: {other.name}");
        //         enemy.Damage(_damage);
        //         Destroy(gameObject);
        //     }
        // }
    }
}
