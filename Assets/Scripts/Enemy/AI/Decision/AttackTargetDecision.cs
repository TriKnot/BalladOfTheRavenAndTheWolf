using UnityEngine;

namespace Enemy.AI.Decision
{
    [CreateAssetMenu(fileName = "new InAttackRangeDecision", menuName = "Enemy/AI/Decision/InAttackRange")]
    public class AttackTargetDecision : EnemyDecision
    {
            public override bool Decide(EnemyStateController controller)
            {
                return CanAttack(controller);
            }

            private bool CanAttack(EnemyStateController controller)
            {
                if(controller.EnemyTargetList.Count == 0)
                    return false;
                
                if(controller.AttackTarget == null)
                    return false;

                var distanceToTarget = Vector3.Distance(controller.AttackTarget.transform.position, controller.transform.position);
                return distanceToTarget < controller.Stats.AttackRange;
            }

    }
}