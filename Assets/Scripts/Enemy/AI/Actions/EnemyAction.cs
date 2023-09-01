using UnityEngine;

namespace Enemy.AI
{
    public abstract class EnemyAction : ScriptableObject
    {
        public abstract void Act(EnemyStateController controller);
    }
}