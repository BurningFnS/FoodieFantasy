using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;

    public float moveForce = 365f;
    public float maxSpeed = 3f;
    public float jumpForce = 1000f;
    public AudioSource jumpAudio;
    public AudioSource eatingAudio;

    public float fullnessPercentage;
    public float timeRemaining;


    //These variables are for power ups
    //Jump boost
    private float jumpBoostTimer = 3.0f;
    private bool isBoostTimer = false;

    //Double Jump
    private float doubleJumpTimer = 3.0f;
    private bool isDoubleJumpTrue = false;
    private bool hasDoubleJumped = false;

    // Blinded Debuff
    private float blindedTimer = 3.0f;
    private bool isBlinded = false;
    public GameObject _BlindnessOverlay;
    //---------------------------------//
    public Transform groundCheck;

    Groceries groceries;
    [HideInInspector]
    public bool grounded = false;

    //private Animator anim;
    private Rigidbody2D rb2d;
    private Animator anim;

    [SerializeField]
    Text TimingText, FullnessText, _FoodConsumed, _FoodWasted, _TotalScore, _GoodMessage, _BadMessage, _Score;
    [SerializeField]
    RectTransform _PanelEnd, _PanelStart, _Transparent, _PanelPaused;
    [SerializeField] Button _Tutorial1, _Tutorial2, _PauseBtn;
    [SerializeField] Slider slider;


    //Variables used for movement//
    private bool moveLeft = false;
    private bool moveRight = false;
    private float horizontalMove;
    private float speed = 1f;

    //Variables used for tutorial//
    bool hasTutorial1Ran = false;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        timeRemaining = 20f;
        fullnessPercentage = 0;

        _PanelEnd.gameObject.SetActive(false);
        _Transparent.gameObject.SetActive(false);
        _PanelStart.gameObject.SetActive(true);
        _PanelPaused.gameObject.SetActive(false);

    }

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = 50f;

        anim = GetComponent<Animator>();
        anim.SetTrigger("idle");
        _BlindnessOverlay.SetActive(false);
        _Tutorial1.gameObject.SetActive(true);
        Time.timeScale = 0f;

        
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
            jumpForce = 600f;
        }
        if (jumpBoostTimer <= 0f)
        {
            jumpForce = 410f;
            jumpBoostTimer = 3.0f;
            isBoostTimer = false;
        }
        //-----------------------//

        //-------Blindness Debuff------//
        if (isBlinded)
        {
            _BlindnessOverlay.SetActive(true);
            blindedTimer -= Time.smoothDeltaTime;
        }
        if (blindedTimer <= 0f)
        {
            isBlinded = false;
            _BlindnessOverlay.SetActive(false);
            blindedTimer = 3.0f;
        }
        //-----------------------//


        FullnessText.text = "Fullness: " + fullnessPercentage + "%";

        if(timeRemaining > 0)
        {
            TimingText.text = "Time Remaining: " + Mathf.RoundToInt(timeRemaining -= Time.smoothDeltaTime);
            _Score.text = "Score : " + ((fullnessPercentage * 5));
        }
        else
        {
            Time.timeScale = 0f;

            _PanelEnd.gameObject.SetActive(true);
            _Transparent.gameObject.SetActive(true);
            _PanelStart.gameObject.SetActive(false);
            _FoodConsumed.text = "Food Consumed: " + fullnessPercentage * 5;
            _FoodWasted.text = "Food Wasted: -" + Basket.spoiledfoodamount * 2;
            _TotalScore.text = "Total Score : " + ((fullnessPercentage * 5) - Basket.spoiledfoodamount * 2);
            if(((fullnessPercentage * 5) - Basket.spoiledfoodamount * 2) >= 50)
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

        Collider2D hit = Physics2D.OverlapCircle(this.gameObject.transform.position, 3f);
        if (hit.gameObject.layer == 12 && !hasTutorial1Ran) 
        {
                hasTutorial1Ran = true;
                _Tutorial2.gameObject.SetActive(true);
                Time.timeScale = 0f;

        }

    }
    void FixedUpdate()
    {

        if (horizontalMove != 0 && !anim.GetBool("isJump"))
        {
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }
        PlayerMovement();

        //anim.SetFloat("Speed", Mathf.Abs(h));

        if (horizontalMove > 0 && !facingRight)
            Flip();
        else if (horizontalMove < 0 && facingRight)
            Flip();

        //------------------------------------THIS IS KEYBOARD SETTINGS--------------------------------//
        float h = Input.GetAxis("Horizontal");

        if (horizontalMove != 0 && !anim.GetBool("isJump"))
        {
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }
        if (speed * rb2d.velocity.x < maxSpeed)
            rb2d.AddForce(Vector2.right * h * moveForce);

        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();
        // -----------------------------------------------------------------------------------------//

        if (jump)
        {
            //anim.SetTrigger("Jump");
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }

        //This is to reset the double jump powerup
        if (hasDoubleJumped && grounded)
        {
            hasDoubleJumped = false;
        }

    }
    public void PlayerMovement()
    {
        if (moveLeft)
        {
            horizontalMove = -speed;
            if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
                rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
            if (horizontalMove * rb2d.velocity.x < maxSpeed)
                rb2d.AddForce(Vector2.right * horizontalMove * moveForce);
        }
        else if (moveRight)
        {
            horizontalMove = speed;
            if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
                rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
            if (horizontalMove * rb2d.velocity.x < maxSpeed)
                rb2d.AddForce(Vector2.right * horizontalMove * moveForce);

        }
        else
        {
            horizontalMove = 0;
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
            groceries = collider.gameObject.GetComponent<Groceries>();
            float filling = groceries.filling;

            timeRemaining += 5;
            fullnessPercentage += filling;
            slider.value += groceries.filling;
			collider.gameObject.SetActive(false);
        }

        // General Spoiled Food collection
        if (collider.gameObject.tag == "Spoiled Food")
        {
            if (isBlinded)
            {
                blindedTimer += 3.0f;
                Debug.Log("You have consumed a spoiled food, debuff time extention.");

            }
            else
            {
                isBlinded = true;            
                fullnessPercentage -= 5;
                timeRemaining -= 2;
                collider.gameObject.SetActive(false);
                slider.value -= 5;
                Debug.Log("You have consumed a spoiled food and received a debuff.");

            }
        }
    }

    public void ReturnMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
        Debug.Log("The game will not be continued as it has not been implemented further.");
    }

    public void TouchDownLeft()
    {
        moveLeft = true;
    }
    public void TouchUpLeft()
    {
        moveLeft = false;
    }
    public void TouchDownRight()
    {
        moveRight = true;
    }
    public void TouchUpRight()
    {
        moveRight = false;
    }
    public void TouchDownJump()
    {
        //-------Double Jump------//

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

    public void TouchUpJump()
    {

    }

    public void runTutorial1()
    {
        _Tutorial1.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    public void runTutorial2()
    {
        _Tutorial2.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void pausedGame()
    {

    }
}
