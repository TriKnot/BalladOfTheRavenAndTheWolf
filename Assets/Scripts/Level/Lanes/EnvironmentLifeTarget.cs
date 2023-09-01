using UnityEngine;

namespace Level.Lanes
{
    public class EnvironmentLifeTarget : MonoBehaviour
    {
        [SerializeField] private GameObject essenceForm;
        [SerializeField] private GameObject blightForm;
        public void SetEssenceForm()
        {
            essenceForm.SetActive(true);
            blightForm.SetActive(false);
            // Debug.Log(transform + "Essence");
        }
    
        public void SetBlightForm()
        {
            essenceForm.SetActive(false);
            blightForm.SetActive(true);
            // Debug.Log(transform + "Blight");
        }
    }
}
