using UnityEngine;

namespace Enemy.AI.States
{
    [CreateAssetMenu(fileName = "State", menuName = "Enemy/AI/State")]
    public class EnemyState : ScriptableObject
    {
        public EnemyAction[] Actions;
        public EnemyTransition[] Transitions;
        
        public void UpdateState(EnemyStateController controller)
        {
            ExecuteActions(controller);
            CheckForTransitions(controller);
        }
        
        private void ExecuteActions(EnemyStateController controller)
        {
            foreach (var action in Actions)
            {
                action.Act(controller);
            }
        }
        
        private void CheckForTransitions(EnemyStateController controller)
        {
            foreach (var transition in Transitions)
            {
                if (transition.Decision.Decide(controller))
                {
                    controller.TransitionToState(transition.TrueState);
                }
                else
                {
                    controller.TransitionToState(transition.FalseState);
                }
            }
        }
    }
}