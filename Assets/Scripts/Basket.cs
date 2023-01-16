using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Basket : MonoBehaviour
{
    public GameManager gamemanager;
    private GameObject[] platformSpawn;
    public int maxPlatforms;
    public List<Vector2> platformList;

    public List<Vector2> groceryPlatformedList;
    private Vector2 lastPlatform;

    public AudioSource basketAudio;


    public float speed = 10f;
    bool isMoving = false;

    float cap = 100;

    [HideInInspector]
    public bool isBasketFull = false;

    Rigidbody2D rb;
    Rigidbody2D groceryrb2d;

    Groceries groceries;
    private int rows;
    private int foodPlatform;

    //private List<string> ItemList = new List<string>();
    public List<GameObject> groceryList;
    public List<object> foodList;

    public Text capacityText;
    public Text groceryListUI;

    //movement variables//
    private bool moveLeft = false;
    private bool moveRight = false;
    private float horizontalMove;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groceryList = new List<GameObject>();
        foodList = new List<object>();
        platformSpawn = gamemanager.platformSpawn;
        maxPlatforms = gamemanager.maxPlatforms;
        platformList = gamemanager.platformList;
        rows = gamemanager.rowsOfPlatforms;

        for (int i = 0; i < rows; i++)
        {
            foodPlatform = Random.Range(0, 3);

            for (int j = 0; j < 3; j++)
            {

                if (j == foodPlatform)
                {
                    groceryPlatformedList.Add(platformList[3 * i + foodPlatform]);
                }
            }

        }
    }

    void FixedUpdate()
    {
        BasketMovement();
        keyboardBasketMovement();
        rb.velocity = new Vector2(horizontalMove, rb.velocity.y);

    }

    void Update()
    {
        if (cap <= 0)
        {
            isBasketFull = true;
        }
        else
        {
            isBasketFull = false;
        }
    }

    void keyboardBasketMovement()
    {
        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (isMoving == false)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            rb.MovePosition(transform.position + horizontal * Time.deltaTime * speed);
        }


        //transform.position += horizontal * speed * Time.deltaTime;
    }
    public void BasketMovement()
    {
        if (moveLeft)
        {
            isMoving = true;
            horizontalMove = -speed;
        }
        else if (moveRight)
        {
            isMoving = true;
            horizontalMove = speed;
        }
        else
        {
            isMoving = false;
            horizontalMove = 0;
        }
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Food")
        {
            //ItemList.Add(gameObject.name);
            //Debug.Log("ItemList Count: " + ItemList.Count);

            groceryList.Add(other.gameObject);
            foodList.Add(other.gameObject.name);

            groceries = other.gameObject.GetComponent<Groceries>();
            groceryrb2d = other.gameObject.GetComponent<Rigidbody2D>();

            float s = groceries.size;


            cap -= s;

            capacityText.text = "Basket Capacity: " + (cap).ToString() + "%";
            groceryListUI.text = "Groceries: " + string.Join(", ", foodList);

            basketAudio.Play();

            Debug.Log(string.Join(", ", groceryList));
            Debug.Log("Grocery Count: " + groceryList.Count);


            for (int j = 0; j < groceryList.Count; j++)
            {
                lastPlatform = groceryPlatformedList[j];
                other.gameObject.transform.position = new Vector2(lastPlatform.x, lastPlatform.y + 1f);
                groceries.enabled = false;
                other.gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                
            }



            ////platformSpawn = new GameObject[maxPlatforms];
            //for (int i = 0; i < platformList.Count; i++)
            //{
            //    //lastPlatform = platformList[i];
            //    for (int k = 0; k < rows; k++)
            //    {
            //        foodPlatform = Random.Range(0, 3);
            //        if (i % 3 == 1)
            //        {
            //            if (foodPlatform == 0)
            //            {
            //                Debug.Log("platform no: " + (k * 3) + foodPlatform);
            //                lastPlatform = platformList[(k * 3) + foodPlatform];
            //                other.gameObject.transform.position = new Vector2(lastPlatform.x, lastPlatform.y + 1f);
            //                groceries.enabled = false;
            //                other.gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            //            }
            //        }
            //        else if (i % 3 == 2)
            //        {
            //            if (foodPlatform == 1)
            //            {
            //                Debug.Log("platform no: " + (k * 3) + foodPlatform);
            //                lastPlatform = platformList[(k * 3) + foodPlatform];
            //                other.gameObject.transform.position = new Vector2(lastPlatform.x, lastPlatform.y + 1f);
            //                groceries.enabled = false;
            //                other.gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            //            }
            //        }
            //        else if (i % 3 == 0)
            //        {
            //            if (foodPlatform == 2)
            //            {
            //                Debug.Log("platform no: " + (k * 3) + foodPlatform);
            //                lastPlatform = platformList[(k * 3) + foodPlatform];
            //                other.gameObject.transform.position = new Vector2(lastPlatform.x, lastPlatform.y + 1f);
            //                groceries.enabled = false;
            //                other.gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            //            }
            //        }
            //    }

            //Debug.Log(platformSpawn[i].transform.position);


            //for (int j = 0; j < groceryList.Count; j++)
            //{
            //    if(i == j)
            //    {
            //        other.gameObject.transform.position = new Vector2(lastPlatform.x, lastPlatform.y + 1f);
            //        groceries.enabled = false;
            //        other.gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            //    }
            //}

            //}

        }
    }
}
