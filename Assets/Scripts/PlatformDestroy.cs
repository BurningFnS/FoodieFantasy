using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDestroy : MonoBehaviour
{
    public GameObject player;
    public GameObject Platform_Blue;
    private GameObject myPlat;
    public float increaseJump = 0;

    private void OnTriggerEnter2D(Collider2D other) //when the platform colliders with the trigger...
    {

        myPlat = (GameObject)Instantiate(Platform_Blue, new Vector2(Random.Range(20f, 30.5f), player.transform.position.y + (2 + Random.Range(-9.5f, 10.5f))), Quaternion.identity);
        // It will instantiate new blue platforms prefabs, and the first set of numbers is to let it spawn on the X-axis while the next Random.Range is to let it spawn at the y-axis and will be aligned to the player

        //myPlat.GetComponent<Platforms>().jumpForce += increaseJump;   // the myplat gameobject will get the variable of jumpforce from the Platform Script and it will add 1 to the increaseJump float number.
        Debug.Log("awjughau");
        Destroy(other.gameObject);  // This will destroy the blue platforms when it collides with the platformdestroyer 

    }
}
