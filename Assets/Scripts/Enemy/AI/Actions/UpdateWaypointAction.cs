using UnityEngine;

namespace Enemy.AI.Actions
{
    [CreateAssetMenu (fileName = "UpdateWaypointAction", menuName = "Enemy/AI/Actions/UpdateWaypointAction")]
    public class UpdateWaypointAction : EnemyAction
    {
        public override void Act(EnemyStateController controller)
        {
            UpdateWaypoint(controller);
        }

        private void UpdateWaypoint(EnemyStateController controller)
        {
            controller.EnemyManager.CurrentWaypoint++;
        }
    }
}
