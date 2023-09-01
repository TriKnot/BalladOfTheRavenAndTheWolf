using UnityEngine;

namespace Enemy.AI.Decision
{
[CreateAssetMenu (fileName = "new FindNewMoveTargetDecision", menuName = "Enemy/AI/Decision/FindNewMoveTarget")]
public class MoveTargetIsNotSetDecision : EnemyDecision
    {
       
        public override bool Decide(EnemyStateController controller)
        {
            return IsWaypointInRange(controller);
        }

        private bool IsWaypointInRange(EnemyStateController controller)
        {
            return controller.MoveTarget == null;
        }
    }
}