using System;
using UnityEngine;

namespace Player
{
    public class PlayerCameraManager : MonoBehaviour
    {
        [SerializeField] private Transform cameraHolder;

        private void Awake()
        {
            cameraHolder.parent = null;
        }
    }
}
