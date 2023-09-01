using SharedBaseClasses;
using UnityEngine;

namespace Level.Lanes
{
    public class EnvironmentLifeManager : MonoBehaviour
    {
        [SerializeField] private ScriptableHealthSystem _heartScriptableHealth;
    
        [Header("Health Sections")]
        [SerializeField] private HealthSection[] _healthSections;
        

        private void Awake()
        {
            UpdateForceForm();
        }

        private void OnEnable()
        {
            _heartScriptableHealth.OnHealthChanged += UpdateForceForm;
        }
        private void OnDisable()
        {
            _heartScriptableHealth.OnHealthChanged -= UpdateForceForm;
        }

        private void UpdateForceForm()
        {
            foreach (var section in _healthSections)
            {
                if (_heartScriptableHealth.GetHealthPercent() >= section.maxHealthPercentage) //Set to essence form if health is above asked percentage
                {
                    foreach (var target in section.effectedObjects)
                    {
                            target.SetEssenceForm();
                    }
                }
                else if (_heartScriptableHealth.GetHealthPercent() < section.maxHealthPercentage) //Set to blight form if health is below asked percentage
                {
                    foreach (var target in section.effectedObjects)
                    {
                        target.SetBlightForm();
                    }
                }
            }
        }
    
        [System.Serializable]
        private class HealthSection
        {
            [Range(0,1)] public float maxHealthPercentage;
            public EnvironmentLifeTarget[] effectedObjects;
        }
    }
}
