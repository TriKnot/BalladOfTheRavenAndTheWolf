using UnityEngine;

namespace Enemy.AI.Decision
{
    [CreateAssetMenu(fileName = "new WaypointInRangeDecision", menuName = "Enemy/AI/Decision/WaypointInRange")]
    public class WaypointInRangeDecision : EnemyDecision
    {
        [Tooltip("The distance at which the enemy will switch pathing target to the next waypoint.")]
        [SerializeField] private float range = 1f;
       
        public override bool Decide(EnemyStateController controller)
        {
            return IsWaypointInRange(controller);
        }

        private bool IsWaypointInRange(EnemyStateController controller)
        {
            if(controller.EnemyManager.CurrentWaypoint >= controller.EnemyManager.WaveManager.Lane.Waypoints.Length)
                return false;
            
            var currentWaypointLocation = controller.EnemyManager.WaveManager.Lane.Waypoints[controller.EnemyManager.CurrentWaypoint].position;
            var position = controller.transform.position;
            position.y = currentWaypointLocation.y;
            var distance = Vector3.Distance(currentWaypointLocation, position);
            
            return distance < range;
        }
    }
}