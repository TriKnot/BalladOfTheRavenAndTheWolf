using System;
using System.Collections;
using StatusEffects;
using UnityEngine;
using UnityEngine.Events;
using Utils.Variables;

namespace Player.PlayerAbilities.Raven.Pickup_Extensions
{
    public class PickupTargetDropEffect : MonoBehaviour
    {
        [SerializeField] private LayerMask _triggeredOnLayerMask;
        [SerializeField] private ScriptableGameObjectList _targetList;
        [SerializeField] private float _radius;
        [SerializeField] private float _cooldown;
        [SerializeField] private StatusEffect[] _statusEffects;
        [SerializeField] private UnityEvent _onTrigger;
        
        private bool active;

        private void Awake()
        {
            if (_targetList == null)
                throw new Exception($"Target list is null in {name}");
        }
        
        private void OnEnable()
        {
            active = false;
            Invoke(nameof(Activate), 0.5f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!active) return;
            if ((_triggeredOnLayerMask.value & (1 << other.gameObject.layer)) > 0)
                TriggerEffect();
        }

        private void TriggerEffect()
        {
            var inRange = _targetList.GetObjectsInRange(transform.position, _radius);
            foreach (var target in inRange)
            {
                var effectable = target.GetComponent<IEffectable>();
                effectable?.ApplyEffect(_statusEffects);
            }
            _onTrigger?.Invoke();
            if(gameObject.activeInHierarchy)
                StartCoroutine(EffectCoolDown());
        }
        
        private IEnumerator EffectCoolDown()
        {
            active = false;
            yield return new WaitForSeconds(_cooldown);
            active = true;
        }

        private void Activate()
        {
            active = true;
        }
    }
}
