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

    public float boostTimer = 3.0f;
    public bool isBoostTimer = false;
    public int fullnessPercentage;
    public float timeRemaining;
    public bool canJump = true;
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

        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
        }

        if (isBoostTimer)
        {
            boostTimer -= Time.smoothDeltaTime;
            jumpForce = 550f;
        }
        if (boostTimer <= 0f)
        {
            jumpForce = 350f;
            boostTimer = 3.0f;
            isBoostTimer = false;
        }
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

        if (jump &&canJump)
        {
            //anim.SetTrigger("Jump");
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
            canJump = false;
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
    {
        if(collider.gameObject.layer == 10)
        {
            if (isBoostTimer)
            {
                boostTimer += 3.0f;
                collider.gameObject.SetActive(false);
            }
            else
            {
                isBoostTimer = true;
                collider.gameObject.SetActive(false);
            }
        }
        if (collider.gameObject.tag == "Food")
        {
            fullnessPercentage += 5;
            collider.gameObject.SetActive(false);
        }
    }
}
