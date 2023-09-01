using System.Collections;
using Enemy;
using SharedBaseClasses;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils.Variables;

namespace UI
{
    public class UIHealthBarSlider : MonoBehaviour
    {
        [Header("References (Required)")]
        [SerializeField] Slider _slider;
        [SerializeField] Gradient _gradient;
        [SerializeField] Image _fill;
        [SerializeField] ScriptableGameObject _rotateToThis;
        private Transform _lookAtTransform;
        
        [Header("HealthSystem (One Required)")]
        [SerializeField] private ScriptableHealthSystem _scriptableHealthSystem;
        [SerializeField] private HealthSystemComponentBase _enemyHealth;
        
        [Header("Settings")]
        [SerializeField] private bool _showPersistentHealthBar;
        [SerializeField] private float _showHealthBarDuration = 2f;
        
        private float _showCountDownTimer;
        private Coroutine _hideHealthBarCoroutine;
        private bool _isHealthSystemNotNull;

        private void OnEnable()
        {
            _slider.maxValue = 1;

            // If we have a HealthSystem, subscribe to its OnHealthChanged event
            if(_scriptableHealthSystem != null)
            {
                _scriptableHealthSystem.OnHealthChanged += SetScriptableHealth;
            }            
            // If we have an EnemyHealth, subscribe to its OnHealthChanged event
            if (_enemyHealth != null)
            {
                _enemyHealth.OnHealthChanged += SetScriptableHealth;
            }
            // If we don't have either, start complaining
            if(_scriptableHealthSystem == null && _enemyHealth == null)
            {
                Debug.LogError("No HealthSystem or EnemyHealth found on " + gameObject.name);
            }
            _isHealthSystemNotNull = _scriptableHealthSystem != null;
            SetScriptableHealth();
            
        }
        
        private void OnDisable()
        {
            // If we have a HealthSystem, unsubscribe from its OnHealthChanged event
            if(_scriptableHealthSystem != null)
            {
                _scriptableHealthSystem.OnHealthChanged -= SetScriptableHealth;
            }
            // If we have an EnemyHealth, unsubscribe from its OnHealthChanged event
            else if (_enemyHealth != null)
            {
                _enemyHealth.OnHealthChanged -= SetScriptableHealth;
            }
        }

        private void Start()
        {
            SetScriptableHealth();
            if(_rotateToThis!=null)
            _lookAtTransform = _rotateToThis.Value.transform;
        }

        // Set the health bar to the correct value and color, and start the timer to hide it
        private void SetScriptableHealth()
        {
            var healthPercent = _isHealthSystemNotNull ? _scriptableHealthSystem.GetHealthPercent() : _enemyHealth.GetHealthPercent();
            
            _slider.value = healthPercent;
            _fill.color = _gradient.Evaluate(_slider.normalizedValue);
            if(healthPercent <= 0)
            {
                _slider.gameObject.SetActive(false);
            }
            if(healthPercent > 0)
            {
                _slider.gameObject.SetActive(true);
            }
            
            if (_showPersistentHealthBar) return;
            
            var activeHealthBar = healthPercent < 1;
            ToggleHealthBar(activeHealthBar);
        }
        
        // If the health bar is already in the correct state, only reset the timer
        private void ToggleHealthBar(bool showBar)
        {
            if(_slider.gameObject.activeSelf != showBar)
                _slider.gameObject.SetActive(showBar);

            if (!showBar) return;
           
            _showCountDownTimer = _showHealthBarDuration;
            _hideHealthBarCoroutine ??= StartCoroutine(UpdateHealthBarTimer());
        }
        
        private IEnumerator UpdateHealthBarTimer()
        {
            while (_showCountDownTimer > 0)
            {
                _showCountDownTimer -= Time.deltaTime;
                yield return null;
            }
            _hideHealthBarCoroutine = null;
            ToggleHealthBar(false);
        }
        
        
    private void LateUpdate()
    {
        if(_lookAtTransform!=null)
        {
            transform.LookAt(transform.position + _lookAtTransform.forward);  
        }   
    }

    }
}
