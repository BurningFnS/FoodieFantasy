using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        if (target.position.y > transform.position.y)  // if player moves above middle of camera,
        {
            Vector3 newPos = new Vector3(transform.position.x, target.position.y, transform.position.z); //gives the camera the position of the player's y position
            transform.position = newPos; //the new position of the camera
        }
    }
}
