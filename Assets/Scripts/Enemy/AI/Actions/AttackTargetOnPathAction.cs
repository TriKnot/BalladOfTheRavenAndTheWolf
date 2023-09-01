using UnityEngine;

namespace Enemy.AI.Actions
{
    [CreateAssetMenu(fileName = "SetTargetToObstacleOnPathAction", menuName = "Enemy/AI/Actions/SetTargetToObstacleOnPathAction")]
    public class AttackTargetOnPathAction : EnemyAction
    {
        public override void Act(EnemyStateController controller)
        {
            SetTarget(controller);
        }

        private void SetTarget(EnemyStateController controller)
        {
            var targetLocation = controller.Agent.destination;
            var target = controller.EnemyTargetList.GetClosestObject(targetLocation);
            if(target == null) return;
            controller.AttackTarget = target;
            controller.MoveTarget = target;
        }
    }
}
