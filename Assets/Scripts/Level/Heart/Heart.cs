using System;
using System.Collections;
using SharedBaseClasses;
using UnityEngine;
using Utils;
using Utils.Variables;

namespace Level.Heart
{
    public class Heart : MonoBehaviour
    {
        [SerializeField] private ScriptableHealthSystem _scriptableHealthSystem;
        public ScriptableHealthSystem ScriptableHealthSystem => _scriptableHealthSystem;
        [SerializeField] private ScriptableGameObjectList _enemyAttackTargets;
        [Header("VFX")]
        [SerializeField] private GameObject _loseVFXGameObject;
        [SerializeField, Range(0,1)] private float _loseVFXMaxPercent = 0.2f;
        
        [SerializeField] private GameObject _normalVFXGameObject;
        [SerializeField, Range(0,1)] private float _normalVFXMaxPercent = 0.8f;

        [SerializeField] private GameObject _winVFXGameObject;
        
        
        [SerializeField] private ScriptableGameObjectPool _damageVFXPool;
        [SerializeField] private float _effectTime = 0.5f;
        private float lastHealth;

        private void OnEnable()
        {
            _scriptableHealthSystem.ResetHealth();
            _scriptableHealthSystem.OnHealthChanged += CheckHealth;
            _enemyAttackTargets.Add(gameObject);
            _loseVFXGameObject.SetActive(false);
            lastHealth = _scriptableHealthSystem.GetHealthPercent();
            
            CheckHealth();
        }
        
        private void OnDisable()
        {
            _scriptableHealthSystem.OnHealthChanged -= CheckHealth;
            _enemyAttackTargets.Remove(gameObject);
        }

        private void CheckHealth()
        {
            _loseVFXGameObject.SetActive(false);
            _normalVFXGameObject.SetActive(false);
            _winVFXGameObject.SetActive(false);
            
            switch (_scriptableHealthSystem.GetHealthPercent())
            {
                case var n when n <= _loseVFXMaxPercent:
                    _loseVFXGameObject.SetActive(true);
                    break;
                case var n when n <= _normalVFXMaxPercent:
                    _normalVFXGameObject.SetActive(true);
                    break;
                case var n when n > _normalVFXMaxPercent:
                    _winVFXGameObject.SetActive(true);
                    break;
            }
            
            if(_scriptableHealthSystem.GetHealthPercent() < lastHealth)
            {
                StartCoroutine(PlayDamageEffect());
                lastHealth = _scriptableHealthSystem.GetHealthPercent();
            }            
            
        }

        private IEnumerator PlayDamageEffect()
        {
            var vfx = _damageVFXPool.GetPooledObject();
            vfx.transform.position = transform.position;
            vfx.SetActive(true);
            yield return new WaitForSeconds(_effectTime);
            _damageVFXPool.ReturnToPool(vfx);
        }
    }
}
