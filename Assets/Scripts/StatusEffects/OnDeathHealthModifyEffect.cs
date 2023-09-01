using UnityEngine;

namespace StatusEffects
{
    [CreateAssetMenu (fileName = "OnDeathHealthModifyEffect", menuName = "StatusEffects/OnDeathHealthModifyEffect")]
    public class OnDeathHealthModifyEffect : StatusEffect
    {
        public override void TickEffect(IEffectable effectable)
        {
            effectable.OnDeathHealthMultiplier = Modifier;
        }

        public override void ReverseEffect(IEffectable effectable)
        {
            effectable.OnDeathHealthMultiplier = Modifier;
        }
    }
}