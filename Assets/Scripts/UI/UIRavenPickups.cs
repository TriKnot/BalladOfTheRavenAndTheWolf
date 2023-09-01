using Player;
using Player.PlayerAbilities.Raven;
using UnityEngine;

public class UIRavenPickups : MonoBehaviour
{
    [SerializeField] private GameObject _logPrompt;
    [SerializeField] private GameObject _rotationPrompt;
    [SerializeField] private GameObject _dropPrompt;
    [SerializeField] private RavenPickupAbility _ability;
    [SerializeField] private ObservablePlayerHolder _holder;
    [SerializeField] private RavenPickupRotation[] _rotation;

    private float _rotateTrigger;
    void Start()
    {
        _ability.OnPickupEvent += OnFirstPickup;
        _ability.OnDropEvent += OnFirstDrop;
        foreach (var rotate in _rotation)
        {
            rotate.OnRotationEvent += OnRotation;
        }
    }

    private void OnFirstPickup()
    {
        if (_ability.pickup != _holder.WolfPlayerManager.transform)
        {
            if(_logPrompt != null) _logPrompt.SetActive(false);
            if(_rotationPrompt != null) _rotationPrompt.SetActive(true);
            _ability.OnPickupEvent -= OnFirstPickup;
        }
    }

    private void OnFirstDrop()
    {
        if (_ability.pickup != _holder.WolfPlayerManager.transform)
        {
            if(_dropPrompt != null) _dropPrompt.SetActive(false);
            if (_rotationPrompt != null) _rotationPrompt.SetActive(false);
            _ability.OnDropEvent -= OnFirstDrop;
        }
    }

    private void OnRotation()
    {
        _rotateTrigger += Time.deltaTime;
        if (_rotateTrigger >= 1f)
        {
            if (_rotationPrompt != null) _rotationPrompt.SetActive(false);
            foreach (var rotate in _rotation)
            {
                rotate.OnRotationEvent -= OnRotation;
            }
            if(_dropPrompt != null) _dropPrompt.SetActive(true);
        }
    }
}
