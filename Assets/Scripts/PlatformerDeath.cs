using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerDeath : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Death") //if player collides with an object with a tag called Death,
        {
            //PlayerManager.GameOver = true; 

            gameObject.SetActive(false);  // after which, then the player will die/ not be able to move when the Gameover screen is shown

        }
    }
}
