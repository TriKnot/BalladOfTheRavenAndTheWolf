using UnityEngine;
using UnityEngine.VFX;

namespace StatusEffects
{
    public abstract class StatusEffect : ScriptableObject
    {
        public float Duration;
        public float TickRate;
        public float Modifier;
        public GameObject EffectVFXObject;
        [HideInInspector] public VisualEffect EffectVFX;
        public Material EffectMaterial;
        

        public abstract void TickEffect(IEffectable effectable);
        
        public abstract void ReverseEffect(IEffectable effectable);

    }
}
