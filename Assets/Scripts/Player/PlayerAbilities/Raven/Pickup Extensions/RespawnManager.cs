using System.Collections;
using UnityEngine;

namespace Player.PlayerAbilities.Raven.Pickup_Extensions
{
    public class RespawnManager : MonoBehaviour
    {
        public void Respawn(GameObject obj, float respawnTime)
        {
            StartCoroutine(RespawnAfterDelay(obj, respawnTime));
        }
        
        private IEnumerator RespawnAfterDelay(GameObject obj, float respawnTime)
        {
            obj.SetActive(false);
            yield return new WaitForSeconds(respawnTime);
            obj.SetActive(true);
        }
   
    }
}
