using UnityEngine;

namespace Enemy.AI.Actions
{
    [CreateAssetMenu (fileName = "SetClosestTargetToAttackTargetAction", menuName = "Enemy/AI/Actions/SetClosestTargetToAttackTargetAction")]
    public class FocusClosestTargetAction : EnemyAction
    {
        public override void Act(EnemyStateController controller)
        {
            SetTarget(controller);
        }

        private void SetTarget(EnemyStateController controller)
        {
            controller.AttackTarget = controller.EnemyTargetList.GetClosestObject(controller.transform.position);
            var distanceToTarget = Vector3.Distance(controller.AttackTarget.transform.position, controller.transform.position);
        }
    }
}
