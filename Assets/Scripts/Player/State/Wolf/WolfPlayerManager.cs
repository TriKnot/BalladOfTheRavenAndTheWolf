using SharedBaseClasses;
using UnityEngine;

namespace Player.State.Wolf
{
    public class WolfPlayerManager : PlayerManager
    {
        public ScriptableHealthSystem BlightScriptableHealthSystem;
        [SerializeField] private AnimationCurve _blightDamageIncreaseCurve;
        [SerializeField] private Material _blightMaterial;
        [SerializeField, GradientUsage(true)] private Gradient _blightColorGradient;
        [HideInInspector] public PlayerMovement PlayerMovement;
        [SerializeField] private ParticleSystem _attackParticles;
        
        public ParticleSystem AttackParticles => _attackParticles;
        public int AttackDamage => 
            Mathf.FloorToInt(Stats.AttackDamage + Stats.AttackDamage * _blightDamageIncreaseCurve.Evaluate(BlightScriptableHealthSystem.GetHealthPercent()));
        
        protected override void Init()
        {
            base.Init();
            BlightScriptableHealthSystem.ResetHealth();
            BlightScriptableHealthSystem.OnHealthChanged += OnBlightHealthChanged;
            OnBlightHealthChanged();
            this.PlayerMovement = GetComponent<PlayerMovement>();
        }

        private void OnBlightHealthChanged()
        {
            var color = _blightColorGradient.Evaluate(BlightScriptableHealthSystem.GetHealthPercent());
            _blightMaterial.SetColor("_EmissionColor", color);
        }

        // private void Awake()
        // {
        //     print($"100%: {_blightDamageIncreaseCurve.Evaluate(1f) } | { Mathf.FloorToInt(Stats.AttackDamage + _blightDamageIncreaseMultiplier * _blightDamageIncreaseCurve.Evaluate(1f) * BlightScriptableHealthSystem.MaxHealth)}");
        //     print($"70%: {_blightDamageIncreaseCurve.Evaluate(.7f) } | { Mathf.FloorToInt(Stats.AttackDamage + _blightDamageIncreaseMultiplier * _blightDamageIncreaseCurve.Evaluate(.7f) * BlightScriptableHealthSystem.MaxHealth)}");
        //     print($"50%: {_blightDamageIncreaseCurve.Evaluate(.5f) } | { Mathf.FloorToInt(Stats.AttackDamage + _blightDamageIncreaseMultiplier * _blightDamageIncreaseCurve.Evaluate(.5f) * BlightScriptableHealthSystem.MaxHealth)}");
        //     print($"30%: {_blightDamageIncreaseCurve.Evaluate(.3f) } | { Mathf.FloorToInt(Stats.AttackDamage + _blightDamageIncreaseMultiplier * _blightDamageIncreaseCurve.Evaluate(.3f) * BlightScriptableHealthSystem.MaxHealth)}");
        //     print($"10%: {_blightDamageIncreaseCurve.Evaluate(.1f) } | { Mathf.FloorToInt(Stats.AttackDamage + _blightDamageIncreaseMultiplier * _blightDamageIncreaseCurve.Evaluate(.1f) * BlightScriptableHealthSystem.MaxHealth)}");
        // }
    }

}
