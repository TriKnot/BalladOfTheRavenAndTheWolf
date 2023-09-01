using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using Utils;

namespace SharedBaseClasses
{
    public class HealthSystemComponentBase : MonoBehaviour
    {
        protected int _currentHealth;
        [SerializeField] protected StatsBase _stats;
        [SerializeField] protected string _healSoundPath;
        [SerializeField] protected string _damageSoundPath;
        [SerializeField] protected string _deathSoundPath;
        [SerializeField] protected ScriptableGameObjectPool _damageEffectObjectPool;
        
        public delegate void HealthChanged();
        public event HealthChanged OnHealthChanged;

        protected virtual void OnEnable()
        {
            ResetHealth();
        }
        
        protected virtual void ResetHealth()
        {
            _currentHealth = _stats.MaxHealth * _stats.StartingHealthPercent / 100;
            OnHealthChanged?.Invoke();
        }

        public virtual void Damage(int damage)
        {
            _currentHealth = Mathf.Max(_currentHealth - damage, 0);
            OnHealthChanged?.Invoke();
            if (_currentHealth <= 0)
            {
                Die();
            }        
            
            if(!string.IsNullOrWhiteSpace(_damageSoundPath))
                FMODUnity.RuntimeManager.PlayOneShot(_damageSoundPath);
            
        }

        protected void PlayDamageVFX()
        {
            
            var obj = _damageEffectObjectPool.GetPooledObject();
            obj.transform.position = transform.position;
            // Rotate the damage effect away from the player
            var randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            obj.transform.rotation = Quaternion.LookRotation(randomRotation * Vector3.forward, Vector3.up);
            StartCoroutine(ReturnToPool(obj, 1f));
            // Play the damage effect
        }
        
        private IEnumerator ReturnToPool(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            _damageEffectObjectPool.ReturnToPool(obj);
        }


        public virtual void Heal(int value)
        {
            _currentHealth = Mathf.Min(_currentHealth + value, _stats.MaxHealth);
            OnHealthChanged?.Invoke();
            
            if(_healSoundPath != "")
                FMODUnity.RuntimeManager.PlayOneShot(_healSoundPath);

        }
        
        public virtual float GetHealthPercent()
        {
            return (float)_currentHealth / _stats.MaxHealth;
        }

        protected virtual void Die()
        {
            if(_deathSoundPath != "")
                FMODUnity.RuntimeManager.PlayOneShot(_deathSoundPath);
            Destroy(gameObject);
        }
        
        
    }
}