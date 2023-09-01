using System.Collections;
using Cinemachine;
using UnityEngine;

namespace VFX
{
    public class AoeCamera : MonoBehaviour
    {
        [SerializeField] private float _shakeDuration = 0.3f;
        [SerializeField] private float _amplitude = 1f;
        [SerializeField] private float _frequency = 5f;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        private CinemachineBasicMultiChannelPerlin _noise;


        private void Start()
        {
            _noise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _noise.m_AmplitudeGain = 0;
            _noise.m_FrequencyGain = 0;
        }
        
        public void StartExplosion()
        {
            StartCoroutine(ScreenShake());
        }

        private IEnumerator ScreenShake()
        {
            _noise.m_AmplitudeGain = _amplitude;
            _noise.m_FrequencyGain = _frequency;
            yield return new WaitForSeconds(_shakeDuration);
            _noise.m_AmplitudeGain = 0;
            _noise.m_FrequencyGain = 0;
        }
    }
}