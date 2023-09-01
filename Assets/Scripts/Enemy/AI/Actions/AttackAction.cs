using UnityEngine;

namespace Enemy.AI.Actions
{
    
    [CreateAssetMenu (fileName = "AttackAction", menuName = "Enemy/AI/Actions/AttackAction")]
    public class AttackAction : EnemyAction
    {
        public override void Act(EnemyStateController controller)
        {
            Attack(controller);
        }

        private void Attack(EnemyStateController controller)
        {
            if (controller.AttackTarget == null) return;

            if (!controller.StateBoolVariable)
            {
                controller.StateBoolVariable = true;
                controller.StateFloatVariable = controller.Stats.AttackSpeed;
            }
            
            if (controller.HasTimeElapsed(controller.Stats.AttackSpeed))
            {
                controller.Attack.Attack(controller.EnemyManager.AttackDamage, controller);
            }

        }
    }
}