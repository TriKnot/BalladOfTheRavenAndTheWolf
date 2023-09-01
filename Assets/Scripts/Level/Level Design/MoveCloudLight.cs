using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCloudLight : MonoBehaviour
{

    [SerializeField] private float cloudSpeed = 7.5f;

    private int direction = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(cloudSpeed * Time.deltaTime * direction, 0, 0);
        
        if(transform.position.x > 1000)
        {
            direction = -1;
        }
        else if(transform.position.x < -1000)
        {
            direction = 1;
        }
    }
}
