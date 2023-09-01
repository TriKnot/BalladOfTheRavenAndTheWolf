using UnityEngine;

namespace StatusEffects
{
    [CreateAssetMenu (fileName = "AttackDamageModifyEffect", menuName = "StatusEffects/AttackDamageModifyEffect")]
    public class AttackDamageModifyEffect : StatusEffect
    {
        public override void TickEffect(IEffectable effectable)
        {
            effectable.AttackDamageMultiplier = Modifier;
        }

        public override void ReverseEffect(IEffectable effectable)
        {
            effectable.AttackDamageMultiplier = 1f;
        }
    }
}