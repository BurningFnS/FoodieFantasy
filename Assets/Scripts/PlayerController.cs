using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;

    public TextMeshProUGUI TimingText, FullnessText;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;

    public int fullnessPercentage;
    public float timeRemaining;

    //These variables are for power ups
    //Jump boost
    private float jumpBoostTimer = 3.0f;
    private bool isBoostTimer = false;

    //Double Jump
    private float doubleJumpTimer = 3.0f;
    private bool isDoubleJumpTrue = false;
    private bool hasDoubleJumped = false;

    //---------------------------------//
    public Transform groundCheck;

    private bool grounded = false;
    //private Animator anim;
    private Rigidbody2D rb2d;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        timeRemaining = 60f;
        fullnessPercentage = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));



        //-------Double Jump------//

        if (Input.GetButtonDown("Jump"))
        {
            if (isDoubleJumpTrue && (hasDoubleJumped == false))
            {
                jump = true;
                if(Input.GetButtonDown("Jump") && (jump == false))
                {
                    hasDoubleJumped = true;
                    jump = true;
                }
            }
            else if (grounded)
            {
                jump = true;
            }
        }
        if (isDoubleJumpTrue)
        {
            doubleJumpTimer -= Time.smoothDeltaTime;
        }
        if (doubleJumpTimer <= 0f)
        {
            isDoubleJumpTrue = false;
        }
        //-----------------------//


        //-------Jump boost------//
        if (isBoostTimer)
        {
            jumpBoostTimer -= Time.smoothDeltaTime;
            jumpForce = 550f;
        }
        if (jumpBoostTimer <= 0f)
        {
            jumpForce = 350f;
            jumpBoostTimer = 3.0f;
            isBoostTimer = false;
        }
        //-----------------------//

        TimingText.text = "Time Remaining: " + Mathf.RoundToInt(timeRemaining -= Time.smoothDeltaTime);
        FullnessText.text = "Fullness: " + fullnessPercentage;
    }
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        //anim.SetFloat("Speed", Mathf.Abs(h));

        if (h * rb2d.velocity.x < maxSpeed)
            rb2d.AddForce(Vector2.right * h * moveForce);

        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();

        if (jump)
        {
            //anim.SetTrigger("Jump");
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
            //This is to reset the double jump powerup
            if (hasDoubleJumped && grounded)
            {
                hasDoubleJumped = false;
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D collider)
        //This colliderEnter checks for power ups and food triggers
    {
        // Layer 10 is jump boost food
        if(collider.gameObject.layer == 10)
        {
            if (isBoostTimer)
            {
                jumpBoostTimer += 3.0f;
                collider.gameObject.SetActive(false);
            }
            else
            {
                isBoostTimer = true;
                collider.gameObject.SetActive(false);
            }
        }

        // Layer 11 is Double Jump food
        if (collider.gameObject.layer == 11)
        {
            if (isDoubleJumpTrue)
            {
                doubleJumpTimer += 3.0f;
            }
            else
            {
                isDoubleJumpTrue = true;
            }
        }

        // General food collection
        if (collider.gameObject.tag == "Food")
        {
            fullnessPercentage += 5;
            collider.gameObject.SetActive(false);
        }
    }
}
