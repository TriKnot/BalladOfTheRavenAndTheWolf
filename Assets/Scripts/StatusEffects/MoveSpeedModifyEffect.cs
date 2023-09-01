using UnityEngine;

namespace StatusEffects
{
    [CreateAssetMenu(fileName = "New MoveSpeedModifyEffect", menuName = "StatusEffects/MoveSpeedModifyEffect", order = 0)]
    public class MoveSpeedModifyEffect : StatusEffect
    {
        public override void TickEffect(IEffectable effectable)
        {
            effectable.MoveSpeedMultiplier = Modifier;
        }

        public override void ReverseEffect(IEffectable effectable)
        {
            effectable.MoveSpeedMultiplier = 1f;
        }
    }
}
