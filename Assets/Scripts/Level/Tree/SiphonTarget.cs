using SharedBaseClasses;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;
using Utils.Variables;

namespace Level.Tree
{
    public class SiphonTarget : MonoBehaviour
    {
        [SerializeField] private ScriptableGameObjectList _targetGameObjectList;
        [FormerlySerializedAs("_heartHealth")] [SerializeField] private ScriptableHealthSystem _heartScriptableHealth;
        [SerializeField] private VisualEffect _syphonAnimation;
        [SerializeField] private float _syphonAnimationSpawnRate = 10f;
        [SerializeField] private Canvas canvas;
        public ScriptableHealthSystem HeartScriptableHealth => _heartScriptableHealth;

        private void OnEnable()
        {
            _targetGameObjectList.Add(gameObject);
        }
        
        private void OnDisable()
        {
            _targetGameObjectList.Remove(gameObject);
        }

        public void ToggleTargetIndicator(bool toggle)
        {
            if(canvas != null)
                canvas.enabled = toggle;
        }
        
        public void ToggleSyphonAnimation(bool toggle)
        {
            var rate = toggle ? _syphonAnimationSpawnRate : 0f;
            _syphonAnimation.SetFloat("Rate", rate);
        }
    }
}
