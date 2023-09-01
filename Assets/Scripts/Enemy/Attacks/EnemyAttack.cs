using Enemy.AI;
using UnityEngine;

namespace Enemy.Attacks
{
    public abstract class EnemyAttack : ScriptableObject
    {
        
        [SerializeField] protected GameObject AttackAnimation;
        public abstract void Attack(int damage, EnemyStateController controller);
        
    }
}