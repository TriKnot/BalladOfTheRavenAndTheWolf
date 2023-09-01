using System;
using System.Collections;
using System.Collections.Generic;
using Player.PlayerAbilities.Raven;
using UnityEngine;
using UnityEngine.Serialization;

public class PickupTargetCookieLight : MonoBehaviour
{
    [Header("Color Change")]
    [SerializeField] private Color CanDropColor;
    [SerializeField] private Color CantDropColor;
    [SerializeField] private LayerMask _dropZoneLayer;
    
    [Header("Height Controlls")]
    [SerializeField] private LayerMask _layerMask;
    [FormerlySerializedAs("_ravenPickup")] [SerializeField] private RavenPickupAbility _ravenPickupAbility;

    private Light _light;

    private void Awake()
    {
        _light = GetComponent<Light>();
    }

    void Update()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, _layerMask);
                
        var cookieHeight = hit.point.y + 11;
        
        Vector3 eulerRotation = new Vector3(90, _ravenPickupAbility.pickup.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Euler(eulerRotation);
        transform.position = new Vector3(transform.position.x, cookieHeight, transform.position.z);
    }
    
    public void UpdateColor()
    {
        if (Physics.Raycast(transform.position, Vector3.down, Mathf.Infinity, _dropZoneLayer))
        {
            _light.color = CanDropColor;
        }
        else
        {
            _light.color = CantDropColor;
        }
    }
}
