using System;
using System.Collections;
using Enemy.Spawner;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIIndicator : MonoBehaviour
    {
        [Header("Indicator")]
        [SerializeField,Tooltip("How many seconds indicator shows on screen")] private float _seconds;
        public Image[] images;
        public Lane[] lanes;


        public void ShowSpawnIndicator(Lane lane)
        {
            for (int i = 0; i < lanes.Length; i++)
            {
                if (lanes[i] == lane)
                {
                    StartCoroutine(TriggerIndicator(images[i]));
                }
            }
        }
            
        private IEnumerator TriggerIndicator(Image target)
        {
            target.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(_seconds);
            
            target.gameObject.SetActive(false);
        }
    }
}
