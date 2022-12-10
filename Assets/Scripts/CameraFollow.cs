using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float speed;
    /// <summary>
    /// Move camera follow Player, freeze y Axis of camera
    /// </summary>
    void FixedUpdate()
    {
        Vector3 temp = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, new Vector3(temp.x, transform.position.y, temp.z), Time.fixedDeltaTime * speed);
    }
}
