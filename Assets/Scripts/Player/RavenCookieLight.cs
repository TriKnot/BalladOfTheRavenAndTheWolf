using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenCookieLight : MonoBehaviour
{

    [SerializeField] private LayerMask _layerMask;
    void Update()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, _layerMask);
                
        var cookieHeight = hit.point.y + 11;
        
        transform.position = new Vector3(transform.position.x, cookieHeight, transform.position.z);
    }
}
