using Player;
using Player.PlayerAbilities.Wolf;
using SharedBaseClasses;
using UnityEngine;
using Utils;
namespace Enemy
{
    [RequireComponent(typeof(EnemyManager))]
    public class EnemyHealth : HealthSystemComponentBase
    {
        [SerializeField] private ScriptableHealthSystem _wolfBlightScriptableHealthSystem;
        [SerializeField] ScriptableGameObjectPool _floatingTextPrefabLow;
        [SerializeField] ScriptableGameObjectPool _floatingTextPrefabMed;
        [SerializeField] ScriptableGameObjectPool _floatingTextPrefabHigh;

        private EnemyStats _enemyStats => _stats as EnemyStats;
        private EnemyManager _enemyManager => GetComponent<EnemyManager>();
        [SerializeField] private ScriptableGameObjectPool _orbPool;
        [SerializeField] private ObservablePlayerHolder _playerHolder;


        protected override void OnEnable()
        {
            _stats = _enemyManager.Stats;
            base.OnEnable();
        }

        public override void Damage(int damage)
        {
            var damageTaken = Mathf.FloorToInt(damage * _enemyManager.DamageTakenMultiplier);
            base.Damage(damageTaken);
                        
            if(damage > 0)
                PlayDamageVFX();

        }

        protected override void Die()
        {
            var heal = Mathf.FloorToInt(_enemyStats.BlightHealAmount * _enemyManager.OnDeathHealthMultiplier);
            _wolfBlightScriptableHealthSystem?.Heal(heal);
            _orbPool.GetPooledObject().GetComponent<EnemyKillOrb>().Init(transform.position, _playerHolder.WolfPlayerManager.transform);
            base.Die(); 
        }
        

    }
}