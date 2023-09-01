using UnityEngine;
using UnityEngine.AI;

namespace Enemy.AI.Actions
{
    [CreateAssetMenu (fileName = "MoveToWaypointAction", menuName = "Enemy/AI/Actions/MoveToWaypointAction")]
    public class TargetWaypointAction : EnemyAction
    {

        public override void Act(EnemyStateController controller)
        {
            SetTargetToWaypoint(controller);
        }

        private void SetTargetToWaypoint(EnemyStateController controller)
        {
            var waypoint = controller.EnemyManager.WaveManager.Lane.Waypoints[controller.EnemyManager.CurrentWaypoint];
            controller.MoveTarget = waypoint.gameObject;
        }

    }
}
