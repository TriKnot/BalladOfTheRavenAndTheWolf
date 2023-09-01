using UnityEngine;

namespace Enemy.AI.Actions
{
    [CreateAssetMenu (fileName = "New Move Action", menuName = "Enemy/AI/Actions/MoveTowardsTargetAction")]
    public class MoveToTargetAction : EnemyAction
    {
        [SerializeField] private float _moveSpeedMultiplier = 1f;
        [SerializeField] private float _accelerationMultiplier = 1f;
        public override void Act(EnemyStateController controller)
        {
            MoveToTarget(controller);
        }
        private void MoveToTarget(EnemyStateController controller)
        {
            controller.Agent.speed = controller.EnemyManager.MoveSpeed * _moveSpeedMultiplier;
            controller.Agent.acceleration = controller.Stats.Acceleration * _accelerationMultiplier;
            if(controller.Agent.hasPath) return;
                controller.Agent.SetPath(controller.Path);
            
        }
    }
    
    
}