using Player;
using UnityEngine;

public class UIRavenArrow : MonoBehaviour
{

    private Transform target;
    private Collider objCollider;
    private Plane[] planes;
    
    private float timer;
    private int counter;

    [SerializeField] private float speed = 20f;
    [SerializeField] private Camera cam;
    [SerializeField] private Renderer[] rend;
    [SerializeField] private float arrowDecayDelay;
    [SerializeField] private ObservablePlayerHolder observablePlayerHolder;
    private Camera _camera;

   
    public void Awake()
    {
        Setup();
        if (target != null) return;
        enabled = false;
        print($"No target found for {gameObject.name}");

        _camera ??= observablePlayerHolder.WolfPlayerManager.Camera;
        if(_camera == null) print("Wolf camera not set for Raven Arrow");
    }
     
    private void Setup()
    {
        var wolfManager = observablePlayerHolder.WolfPlayerManager;
        if (wolfManager == null) return;
        target = wolfManager.PlayerModelObject.transform;
        objCollider = target.GetComponent<Collider>();
        _camera = GameObject.Find("Camera Two").GetComponent<Camera>();
    }
    
    
    
    void LateUpdate()
    {
        //Follow Target
        RotateTowardsTarget();
        
        //Disable arrow if Wolf is on screen
        var isTargetOnScreen = IsTargetOnScreen();
        SetActiveArrowRenderer(!isTargetOnScreen);
    }
    
    private void SetActiveArrowRenderer(bool active)
    {
        foreach (var r in rend)
        {
            r.enabled = active;
        }
    }

    private bool IsTargetOnScreen()
    {
        var screenPos = _camera.WorldToScreenPoint(target.position);
        var onScreen = screenPos.x > Screen.width / 2 &&
                       screenPos.x < Screen.width &&
                       screenPos.y > 0f &&
                       screenPos.y < Screen.height;
        if (onScreen)
        {
            timer = 0;
            return true;
        }
        timer += Time.deltaTime;
        return !(timer > arrowDecayDelay);
    }

    private void RotateTowardsTarget()
    {
        Vector3 targetDirection = (new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position);
        float singleStep = speed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
