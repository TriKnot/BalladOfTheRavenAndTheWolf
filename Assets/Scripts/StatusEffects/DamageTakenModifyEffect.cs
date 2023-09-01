using UnityEngine;

namespace StatusEffects
{
    [CreateAssetMenu (fileName = "new DamageTakenModifyEffect", menuName = "StatusEffects/DamageTakenModifyEffect")]
    public class DamageTakenModifyEffect : StatusEffect
    {
        public override void TickEffect(IEffectable effectable)
        {
            effectable.DamageTakenMultiplier = Modifier;
        }

        public override void ReverseEffect(IEffectable effectable)
        {
            effectable.DamageTakenMultiplier = 1f;
        }
    }
}