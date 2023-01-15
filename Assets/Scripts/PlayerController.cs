using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;

    public TextMeshProUGUI TimingText, FullnessText;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public AudioSource jumpAudio;
    public AudioSource eatingAudio;

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

    [HideInInspector]
    public bool grounded = false;

    //private Animator anim;
    private Rigidbody2D rb2d;
    private Animator anim;

    [SerializeField]
    Text _FoodConsumed, _FoodWasted, _TotalScore, _GoodMessage, _BadMessage;
    [SerializeField]
    RectTransform _PanelEnd, _PanelStart;
    [SerializeField]
    Button _Continue;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        timeRemaining = 10f;
        fullnessPercentage = 0;

        _PanelEnd.gameObject.SetActive(false);
        _PanelStart.gameObject.SetActive(true);


    }

    // Start is called before the first frame update
    void Start()
    {
        Button continueBtn = _Continue.GetComponent<Button>();
        continueBtn.onClick.AddListener(Resume);

        anim = GetComponent<Animator>();
        anim.SetTrigger("idle");
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        //checks 3 variables to properly implement her idle form after jumping
        if (grounded && rb2d.velocity.y == 0 && jump == false)
        {
            anim.SetBool("isJump", false);
        }

        //-------Double Jump------//

        if (Input.GetButtonDown("Jump"))
        {
            if (isDoubleJumpTrue && (hasDoubleJumped == false))
            {
                jump = true;
                anim.SetBool("isJump", true);
                if (Input.GetButtonDown("Jump"))
                {
                    hasDoubleJumped = true;
                    jump = true;
                    anim.SetBool("isJump", true);
                }
            }
            else if (grounded)
            {
                jump = true;
                anim.SetBool("isJump", true);
            }
        }
        if (isDoubleJumpTrue)
        {
            doubleJumpTimer -= Time.smoothDeltaTime;
        }
        if (doubleJumpTimer <= 0f)
        {
            isDoubleJumpTrue = false;
            doubleJumpTimer = 3.0f;
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

        FullnessText.text = "Fullness: " + fullnessPercentage;

        if(timeRemaining > 0)
        {
            TimingText.text = "Time Remaining: " + Mathf.RoundToInt(timeRemaining -= Time.smoothDeltaTime);
        }
        else
        {
            Time.timeScale = 0f;

            _PanelEnd.gameObject.SetActive(true);
            _PanelStart.gameObject.SetActive(false);
            _FoodConsumed.text = "Food Consumed: " + fullnessPercentage * 5;
            _FoodWasted.text = "Food Wasted: -" + 100;
            _TotalScore.text = "Total Score : " + ((fullnessPercentage * 5) - 100);
            if(((fullnessPercentage * 5) - 100) >= 100)
            {
                _GoodMessage.gameObject.SetActive(true);
                _BadMessage.gameObject.SetActive(false);
            }
            else
            {
                _BadMessage.gameObject.SetActive(true);
                _GoodMessage.gameObject.SetActive(false);
            }
        }

    }
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        if (h != 0 && !anim.GetBool("isJump"))
        {
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }
        //anim.SetFloat("Speed", Mathf.Abs(h));

        if (h * rb2d.velocity.x < maxSpeed)
            rb2d.AddForce(Vector2.right * h * moveForce);

        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();

        if (jump )
        {
            //anim.SetTrigger("Jump");
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jumpAudio.Play();
            jump = false;
        }

        //This is to reset the double jump powerup
        if (hasDoubleJumped && grounded)
        {
            hasDoubleJumped = false;
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
        eatingAudio.Play();
        // Layer 10 is jump boost food
        if (collider.gameObject.layer == 10)
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
                Debug.Log("You have consumed a double jump boost, time extended");

            }
            else
            {
                isDoubleJumpTrue = true;
                Debug.Log("You have consumed a double jump boost");

            }
        }

        // General food collection
        if (collider.gameObject.tag == "Food")
        {
            fullnessPercentage += 5;
            collider.gameObject.SetActive(false);
        }
    }

    void Resume()
    {
        Time.timeScale = 1f;
        timeRemaining += 999f;
        _PanelEnd.gameObject.SetActive(false);
        _PanelStart.gameObject.SetActive(true);
        Debug.Log("The game will not be continued as it has not been implemented further.");
    }
}
