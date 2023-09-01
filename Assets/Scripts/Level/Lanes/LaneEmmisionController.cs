using System;
using UnityEngine;

namespace Level
{
    public class LaneEmmisionController : MonoBehaviour
    {
        [SerializeField] private Material mat;
        [SerializeField] private Color col;
        [SerializeField] private Color col2;
        [SerializeField] private float _emessiveStrength = 1.5f;
        private Material newMat;

        private void Awake()
        {
            newMat = new Material(mat);
            col = mat.color;
            Color newColor = Color.Lerp(col, col2, 1f);
            newMat.SetColor("_EmissionColor", new Vector4(newColor.r,newColor.g,newColor.b) * _emessiveStrength);
            GetComponent<Renderer>().material = newMat;
        }

        private void Update()
        {

        }
    }
}
