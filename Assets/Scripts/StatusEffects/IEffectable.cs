using System.Collections.Generic;
using UnityEngine.AI;

namespace StatusEffects
{
    public interface IEffectable
    {
        public float MoveSpeedMultiplier { get; set; } 
        public float AttackDamageMultiplier { get; set; } 
        public float DamageTakenMultiplier { get; set; } 
        public float OnDeathHealthMultiplier { get; set; } 

        public List<EffectHolder> StatusEffects { get; }
        
        public NavMeshAgent Agent { get; }



        public void ApplyEffect(StatusEffect[] statusEffect);
        public void UpdateEffects();
        public void RemoveEffect(EffectHolder statusEffect);
        
    }
    public class EffectHolder
    {
        public StatusEffect Effect;
        public float Timer;
        public float TickTimer;
    }
}
