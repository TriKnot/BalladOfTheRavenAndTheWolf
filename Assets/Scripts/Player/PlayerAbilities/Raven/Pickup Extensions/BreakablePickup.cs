using System;
using SharedBaseClasses;
using UnityEngine;
using Utils.Variables;

namespace Player.PlayerAbilities.Raven.Pickup_Extensions
{
    [RequireComponent(typeof(RavenPickupTarget))]
    public class BreakablePickup : HealthSystemComponentBase
    {

        private RavenPickupTarget _ravenPickupTarget;
        [SerializeField] private ScriptableGameObjectList _enemyTargetList;
        [SerializeField] private float _respawnTime;

        private bool active;

        protected override void OnEnable()
        {
            base.OnEnable();
            _enemyTargetList.Add(gameObject);
            _ravenPickupTarget = GetComponent<RavenPickupTarget>();
            if(_ravenPickupTarget.RespawnManager == null)
                Debug.LogError("RespawnManager is null on " + gameObject.name);

            if (!active) return;
                Reset();
        }

        private void Start()
        {
            active = true;
        }

        private void OnDisable()
        {
            _enemyTargetList.Remove(gameObject);
        }

        private void Reset()
        {
            _currentHealth = _stats.MaxHealth;
            _ravenPickupTarget.ResetPosition();
        }

        protected override void Die()
        {
            if(_deathSoundPath != "")
                FMODUnity.RuntimeManager.PlayOneShot(_deathSoundPath);
            _ravenPickupTarget.RespawnManager.Respawn(gameObject, _respawnTime);
        }
        

    }
}
