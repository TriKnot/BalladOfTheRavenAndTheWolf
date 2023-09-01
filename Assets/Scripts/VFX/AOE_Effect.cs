using Player;
using Player.State;
using UnityEngine;
using UnityEngine.VFX;

namespace VFX
{
    public class AOE_Effect : MonoBehaviour
    {
        [SerializeField] private ObservablePlayerHolder playerHolder;
        [SerializeField] private VisualEffect VFX_AOE;
        private AoeCamera aoeCamera;
    
        private void Start()
        {
            playerHolder.OnPlayerManagerAdded += SetupCamera;
            aoeCamera = playerHolder.WolfPlayerManager.Camera.GetComponent<AoeCamera>();
            VFX_AOE.Stop();
            gameObject.SetActive(false);
        }

        private void SetupCamera(PlayerManager manager)
        {
            aoeCamera = playerHolder.WolfPlayerManager.Camera.GetComponent<AoeCamera>();
            playerHolder.OnPlayerManagerAdded -= SetupCamera;
        }

        private void StartExplosion()
        {
            VFX_AOE.Play();
            aoeCamera.StartExplosion();
        }
    }
}