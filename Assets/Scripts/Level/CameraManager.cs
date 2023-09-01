using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using SharedBaseClasses;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Access")]
    [SerializeField] private ObservablePlayerHolder _holder;
    [SerializeField] private ScriptableHealthSystem _health;
    
    [Header("Cameras")]
    [SerializeField] private Camera _cam;

    private void OnEnable()
    {
        _health.OnWinEvent += OnWin;
        _health.OnLoseEvent += OnLose;
    }

    private void OnDisable()
    {
        _health.OnWinEvent -= OnWin;
        _health.OnLoseEvent -= OnLose;
    }

    private void OnWin()
    {
        EnableCameras();
    }

    private void OnLose()
    {
        EnableCameras();
    }

    private void EnableCameras()
    {
        if(_cam != null)
            _cam.gameObject.SetActive(true);
        if (_holder.WolfPlayerManager != null)
        {
            _holder.RavenPlayerManager.Camera.gameObject.SetActive(false);
            _holder.WolfPlayerManager.Camera.gameObject.SetActive(false);
        }
    }
}
