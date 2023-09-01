using System.Collections;
using SharedBaseClasses;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIHealthBarImage : MonoBehaviour
    {   
        [Header("References (Required)")]
        [SerializeField] Image _fill;
        [SerializeField] Image _infectionFill;
        [SerializeField] ScriptableHealthSystem _healthSystem;

        [Header("Settings")] 
        [SerializeField] private Color _damageFlashColor;
        [SerializeField] private Color _healFlashColor;
        [SerializeField] private float _delayBeforeFlash = 0.1f;
        [SerializeField] private float _fadeDuration = 2f;
        
        private Coroutine _lerpToOriginalColorCoroutineFill;
        private Coroutine _lerpToOriginalColorCoroutineInfection;
        private Color _originalColorFill;
        private Color _originalColorInfection;

        private void Start()
        {
            _originalColorFill = _fill.color;
            _originalColorInfection = _infectionFill.color;
            _healthSystem.OnHealthChanged += SetHealth;
            _fill.fillAmount = _healthSystem.GetHealthPercent();
        }

        private void OnDisable()
        {
            _healthSystem.OnHealthChanged -= SetHealth;
        }

        private void SetHealth()
        {
            var healthPercent = _healthSystem.GetHealthPercent();
            if (healthPercent > _fill.fillAmount)
            {
                _fill.color = _healFlashColor;

                if (_lerpToOriginalColorCoroutineFill == null)
                    _lerpToOriginalColorCoroutineFill = StartCoroutine(LerpToOriginalColor(_fill, _originalColorFill));
                else
                {
                    StopCoroutine(_lerpToOriginalColorCoroutineFill);
                    _lerpToOriginalColorCoroutineFill = StartCoroutine(LerpToOriginalColor(_fill, _originalColorFill));
                }
            }
            else
            {
                _infectionFill.color = _damageFlashColor;
                
                if (_lerpToOriginalColorCoroutineInfection == null)
                    _lerpToOriginalColorCoroutineInfection = StartCoroutine(LerpToOriginalColor2(_infectionFill, _originalColorInfection));
                else
                {
                    StopCoroutine(_lerpToOriginalColorCoroutineInfection);
                    _lerpToOriginalColorCoroutineInfection = StartCoroutine(LerpToOriginalColor2(_infectionFill, _originalColorInfection));
                }
            }
            
            _fill.fillAmount = healthPercent;

        }
        
        private IEnumerator LerpToOriginalColor(Image targetedImage, Color color)
        {
            yield return new WaitForSeconds(_delayBeforeFlash);
            float t = 0;
            while (_fill.color != _originalColorFill)
            {
                if (t < 1){ 
                    t += Time.deltaTime/_fadeDuration;
                }
                targetedImage.color = Color.Lerp(targetedImage.color, color,t);
                yield return null;
            }
            _lerpToOriginalColorCoroutineFill = null;
        }
        
        private IEnumerator LerpToOriginalColor2(Image targetedImage, Color color)
        {
            yield return new WaitForSeconds(_delayBeforeFlash);
            float t = 0;
            while (_infectionFill.color != _originalColorInfection)
            {
                if (t < 1){ 
                    t += Time.deltaTime/_fadeDuration;
                }
                targetedImage.color = Color.Lerp(targetedImage.color, color, t);
                yield return null;
            }
            _lerpToOriginalColorCoroutineInfection = null;
        }
    }
}
