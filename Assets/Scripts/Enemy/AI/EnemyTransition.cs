using Enemy.AI.Decision;
using Enemy.AI.States;

namespace Enemy.AI
{
    [System.Serializable]
    public class EnemyTransition
    {
        public EnemyDecision Decision;
        public EnemyState TrueState;
        public EnemyState FalseState;
    }
}