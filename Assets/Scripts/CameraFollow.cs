using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float speed;
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        Vector3 tg = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, new Vector3(tg.x, transform.position.y, tg.z), Time.fixedDeltaTime * speed);
    }
}
