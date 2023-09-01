using Player;
using Player.State;
using UnityEngine;

public class UILookAtRavenCamera : MonoBehaviour
{
    [SerializeField] private ObservablePlayerHolder _holder;
    [SerializeField] private RotationTarget _rotationTarget;
    private Transform _target;
    private bool targetSet;
    
    private enum RotationTarget
    {
        Raven, Wolf
    }
    
    private void OnEnable()
    {
        _holder.OnPlayerManagerAdded += SetTarget;
        SetTarget();
    }

    private void OnDisable()
    {
        _holder.OnPlayerManagerAdded -= SetTarget;
    }

    private void SetTarget(PlayerManager manager)
    {
       SetTarget();
    }

    private void LateUpdate()
    {
        if(targetSet)
            transform.LookAt(_target.transform);
    }
    private void SetTarget()

    {
        if(_rotationTarget == RotationTarget.Raven && _holder.RavenPlayerManager == null)
            return;
        if(_rotationTarget == RotationTarget.Wolf && _holder.WolfPlayerManager == null)
            return;

        _target = _rotationTarget == RotationTarget.Raven ? _holder.RavenPlayerManager.Camera.transform : _holder.WolfPlayerManager.Camera.transform;
        targetSet = true;
    }

}
