using UnityEngine;

namespace StatusEffects
{
    [CreateAssetMenu(fileName = "KnockBackStatusEffect", menuName = "StatusEffects/KnockBackStatusEffect", order = 0)]
    public class KnockBackStatusEffect : StatusEffect
    {
        public override void TickEffect(IEffectable effectable)
        {
            var direction = effectable.Agent.transform.position - effectable.Agent.destination;
            effectable.Agent.velocity = direction.normalized * Modifier;
            effectable.MoveSpeedMultiplier = 0f;
        }

        public override void ReverseEffect(IEffectable effectable)
        {
            effectable.MoveSpeedMultiplier = 1f;
            effectable.Agent.velocity = Vector3.zero;
        }
    }
}
