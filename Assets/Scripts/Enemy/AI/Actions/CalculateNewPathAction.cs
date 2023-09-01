using UnityEngine;
using UnityEngine.AI;

namespace Enemy.AI.Actions
{
    [CreateAssetMenu (fileName = "CalculateNewPathAction", menuName = "Enemy/AI/Actions/CalculateNewPathAction")]
    public class CalculateNewPathAction : EnemyAction
    {
        public override void Act(EnemyStateController controller)
        {
            CalculateNewPath(controller);
        }

        private void CalculateNewPath(EnemyStateController controller)
        {
            bool onNavMesh;
            var maxDistance = 0.1f;
            var samplePosition = controller.MoveTarget.transform.position;
            NavMeshHit hit;
            do            
            {
                onNavMesh = NavMesh.SamplePosition(samplePosition, out hit, maxDistance,
                    NavMesh.AllAreas);
                maxDistance += 0.1f;
                Vector3.MoveTowards(samplePosition, controller.transform.position, 0.1f);
            } while (!onNavMesh);

            controller.Path = new NavMeshPath();
            controller.Agent.CalculatePath(hit.position, controller.Path);
            controller.Agent.SetPath(controller.Path);
        }
    }
}
