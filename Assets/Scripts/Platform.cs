using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float jumpForce = 10f;
    void OnCollisionEnter2D(Collision2D collision) //it is a callback method that gets triggered whenever an object collides with another object  
    {
        if (collision.relativeVelocity.y <= 0f) //if player is coming from the top then the below codes will occur else, nth will happen
        {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>(); //gets the rigidbody component so we can modify it 

            if (rb != null) //if there is rigidbody on the thing we collided with, 
            {
                Vector2 velocity = rb.velocity; // gets velocity of the rigidbody in the current state
                velocity.y = jumpForce; //controls the amount of force the platform will give to the player after it hits the platform
                rb.velocity = velocity;  // rb then becomes the velocity vector
            };
        }

    }
}
