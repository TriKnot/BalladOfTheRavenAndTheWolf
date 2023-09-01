using UnityEngine;

namespace Enemy.AI.Decision
{
    public abstract class EnemyDecision : ScriptableObject
    {
        public abstract bool Decide(EnemyStateController controller);
    }
}