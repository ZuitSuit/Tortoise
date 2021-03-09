using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    Vector3 positionBuffer;
    
    private void Awake()
    {
        positionBuffer = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null) 
        {
            positionBuffer.x = target.position.x;
            positionBuffer.y = target.position.y;
            transform.position = positionBuffer;
        }
        
    }
}
