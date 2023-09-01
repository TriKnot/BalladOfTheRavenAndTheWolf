using UnityEngine;

namespace Enemy.AI.Decision
{
    [CreateAssetMenu(fileName = "new InRangeDecision", menuName = "Enemy/AI/Decision/InRange")]
    public class TargetInRangeDecision : EnemyDecision
    {
        
        [SerializeField] private float range = 10f;
        
        public override bool Decide(EnemyStateController controller)
        {
            return IsTargetInRange(controller);
        }

        private bool IsTargetInRange(EnemyStateController controller)
        {
            if(controller.MoveTarget is null)
                return false;

            return Vector3.Distance(controller.Agent.destination, controller.transform.position) < range;
        }
    }
}