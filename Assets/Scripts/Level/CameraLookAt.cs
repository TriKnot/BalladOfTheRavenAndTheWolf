using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Variables;

public class CameraLookAt : MonoBehaviour
{
    [SerializeField] private ScriptableGameObject _lookAtHolder;
    
    private void Awake()
    {
        _lookAtHolder.SetValue(gameObject);
    }
}
