using Enemy.AI;
using Level.Heart;
using SharedBaseClasses;
using UnityEngine;
using UnityEngine.VFX;

namespace Enemy.Attacks
{
    [CreateAssetMenu(fileName = "new Expode", menuName = "Enemy/Expode")]
    public class EnemySelfDestructAttack : EnemyAttack
    {

        public override void Attack(int damage, EnemyStateController controller)
        {
            var colliders = Physics.OverlapSphere(controller.transform.position, controller.Stats.AttackRange * 1.1f);
            
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out EnemyStateController enemyStateController))
                    continue;
                if (collider.TryGetComponent(out Heart heart))
                {
                    heart.ScriptableHealthSystem.Damage(damage);
                }
                if(collider.TryGetComponent(out HealthSystemComponentBase targetHealthSystem))
                    targetHealthSystem.Damage(damage);

            }
            PlayDamageVFX(controller.transform.position);
            Destroy(controller.gameObject);
        }

        private void PlayDamageVFX(Vector3 position)
        {
            if(AttackAnimation == null)
                return;
            var animation = Instantiate(AttackAnimation, position, Quaternion.identity).GetComponent<VisualEffect>();
            Destroy(animation.gameObject, 1);
            animation.Play();
        }
        
    }
}
