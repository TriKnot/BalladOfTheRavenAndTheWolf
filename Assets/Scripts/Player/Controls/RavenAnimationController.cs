using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Player.PlayerAbilities.Raven;
using UnityEngine;

public class RavenAnimationController : MonoBehaviour
{
    [Header("Animation speeds")]
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;
    
    [Header("Animator")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _speedFloat;
    
    [Header("Get access to check if moved")]
    [SerializeField] private RavenPickupAbility _ravenPickupAbility;
    
    private PlayerMovement _playerMovement;
    private float _desiredSpeed;

    private void Awake()
    {
        _playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void OnEnable()
    {
        _ravenPickupAbility.OnMovedEvent += GetDesiredSpeed;
    }

    private void OnDisable()
    {
        _ravenPickupAbility.OnMovedEvent -= GetDesiredSpeed;
    }

    private void GetDesiredSpeed()
    {
        float a1 = _playerMovement.MinSpeed;
        float b1 = _playerMovement.TargetSpeed;
        float a2 = _minSpeed;
        float b2 = _maxSpeed;
        float value = _playerMovement.CurrentSpeed;

        _desiredSpeed = InverseRemap(a1, b1, a2, b2, value);
        
        _animator.SetFloat(_speedFloat,_desiredSpeed);
    }

    private float InverseRemap(float iMin, float iMax, float oMin, float oMax, float v)
    {
        float t = Mathf.InverseLerp(iMin, iMax, v);
        return Mathf.Lerp(oMax, oMin, t);
    }
}
