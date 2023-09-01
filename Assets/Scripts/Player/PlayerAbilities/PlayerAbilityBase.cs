using StatusEffects;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Player.PlayerAbilities
{
    public abstract class PlayerAbilityBase : ScriptableObject
    {
        
        protected PlayerAbilityManager manager;
        protected PlayerStats playerStats;
        [SerializeField] protected StatusEffect[] _statusEffect;

        [Header("Floating Text")] 
        [SerializeField] private ScriptableGameObjectPool _floatingTextPool;
        [SerializeField] public int _minFloatingTextDamage;
        public abstract void InitAbility(PlayerAbilityManager abilityManager);
        
        public abstract void MakeUpdate();
        
        public abstract void Act(InputAction.CallbackContext context);

        public abstract bool IsOnCooldown();
        
        protected void ShowFloatingText(int Damage, Vector3 pos)
        {
            var go = _floatingTextPool.GetPooledObject().GetComponent<FloatingText>();
            go.transform.position = pos;
            go.Innit(Damage, _floatingTextPool);
        }

    }
}
