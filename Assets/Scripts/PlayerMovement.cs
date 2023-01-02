using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public TextMeshProUGUI TimingText, FullnessText;

    public Rigidbody2D rb;

    [HideInInspector]
    public int fullnessPercentage;
    public float timeRemaining;
    public float moveSpeed = 7f;
    public float gravityPowerup;

    private float moveX;
    private bool isTimerRunning = false;

    float boostTimer = 3.0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gravityPowerup = rb.gravityScale;
        timeRemaining = 60f;
        fullnessPercentage = 0;
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
        TimingText.text = "Time Remaining: " + Mathf.RoundToInt(timeRemaining -= Time.smoothDeltaTime);
        FullnessText.text = "Fullness: " + fullnessPercentage;
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
            fullnessPercentage += 5;
            collision.gameObject.SetActive(false);
        }
    }
}
