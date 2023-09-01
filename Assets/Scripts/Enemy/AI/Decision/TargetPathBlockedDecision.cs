using UnityEngine;
using UnityEngine.AI;

namespace Enemy.AI.Decision
{
    [CreateAssetMenu(fileName = "new TargetPathBlockedDecision", menuName = "Enemy/AI/Decision/TargetPathBlocked")]
    public class TargetPathBlockedDecision : EnemyDecision
    {

        public override bool Decide(EnemyStateController controller)
        {
            return IsPathBlocked(controller);
        }

        private bool IsPathBlocked(EnemyStateController controller)
        {
            if (controller.MoveTarget == null)
                return false;

            if (controller.Path == null)
                return false;
            
            return controller.Path.status 
                is NavMeshPathStatus.PathPartial 
                or NavMeshPathStatus.PathInvalid 
                or not NavMeshPathStatus.PathComplete;
        }
    }
}
