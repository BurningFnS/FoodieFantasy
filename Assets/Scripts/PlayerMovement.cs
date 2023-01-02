using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{    
    public Rigidbody2D rb;

    public float moveSpeed = 7f;
    public float gravityPowerup;

    private float moveX;
    private bool isTimerRunning = false;

    float boostTimer = 3.0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gravityPowerup = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxis("Horizontal") * moveSpeed;

        if (isTimerRunning)
        {
            boostTimer -= Time.smoothDeltaTime;
            rb.gravityScale = 0.5f;
        }
        if (boostTimer <= 0f)
        {
            rb.gravityScale = gravityPowerup;
            boostTimer = 3.0f;
            isTimerRunning = false;
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = moveX;
        rb.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if (isTimerRunning)
            {
                boostTimer += 3.0f;
                collision.gameObject.SetActive(false);
            }
            else
            {
                isTimerRunning = true;
                collision.gameObject.SetActive(false);
            }

        }

        if(collision.gameObject.tag == "Food")
        {
            collision.gameObject.SetActive(false);

        }
    }
}
