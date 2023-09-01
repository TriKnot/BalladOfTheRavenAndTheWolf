using UnityEngine;

namespace Enemy.AI.Decision
{
[CreateAssetMenu (fileName = "new AttackTargetIsNotSetDecision", menuName = "Enemy/AI/Decision/AttackTargetIsNotSetDecision")]
public class AttackTargetIsNotSetDecision : EnemyDecision
    {
       
        public override bool Decide(EnemyStateController controller)
        {
            return IsWaypointInRange(controller);
        }

        private bool IsWaypointInRange(EnemyStateController controller)
        {
            return controller.AttackTarget == null;
        }
    }
}