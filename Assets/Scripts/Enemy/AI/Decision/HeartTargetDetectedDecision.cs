using UnityEngine;

namespace Enemy.AI.Decision
{
    [CreateAssetMenu(fileName = "New TargetDetectedDecision", menuName = "Enemy/AI/Decision/TargetDetectedDecision")]
    public class HeartTargetDetectedDecision : EnemyDecision
    {
        public override bool Decide(EnemyStateController controller)
        {
            return DetectTarget(controller);
        }

        private bool DetectTarget(EnemyStateController controller)
        {
            var target = controller.EnemyManager.WaveManager.Lane.Heart.gameObject;
            var distance = Vector3.Distance(controller.transform.position, target.transform.position);
            if (!(distance <= controller.Stats.DetectionRadius)) return false;
            controller.MoveTarget = target;
            controller.AttackTarget = target;
            return true;
        }
    }
}
