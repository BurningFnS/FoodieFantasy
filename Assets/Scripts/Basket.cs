using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

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
    public static int spoiledfoodamount = 0;

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

    //Spoiled food variables//
    public List<GameObject> _spoiledFood;
    public int _randomrange;
    public List<GameObject> _FinalGroceryList;

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
            foodPlatform = UnityEngine.Random.Range(0, 3);

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
        if (Input.GetKey(KeyCode.E))
        {
            for (int j = 0; j < groceryList.Count; j++)
            {
                Debug.Log(groceryList[j]);

            }
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


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Food")
        {            
            groceryList.Add(other.gameObject);
            foodList.Add(other.gameObject.name);

            groceries = other.gameObject.GetComponent<Groceries>();
            groceryrb2d = other.gameObject.GetComponent<Rigidbody2D>();

            float s = groceries.size;

            cap -= s;
            //This is to spawn spoiled food and get values.
            if ((cap - s) < 0)
            {
                for (int i = (int)cap; i < 0; i = i + 2)
                {
                    Debug.Log("this is how much spoiled food you have " + Mathf.Abs(i));
                    spoiledfoodamount++;
                    _randomrange = UnityEngine.Random.Range(0, _spoiledFood.Count);
                    GameObject _CreateSpoiledFood = Instantiate(_spoiledFood[_randomrange]);
                    _CreateSpoiledFood.transform.position = new Vector2(UnityEngine.Random.Range(1000, 5000), UnityEngine.Random.Range(1000, 5000));
                    groceryList.Add(_CreateSpoiledFood);
                    Debug.Log("Spawn this amount of spoiled food = " + spoiledfoodamount);
                }

            }
            //ItemList.Add(gameObject.name);
            //Debug.Log("ItemList Count: " + ItemList.Count);

            

            capacityText.text = "Basket Capacity: " + (cap).ToString() + "%";
            groceryListUI.text = "Groceries: " + string.Join(", ", foodList);

            basketAudio.Play();

            Debug.Log(string.Join(", ", groceryList));
            Debug.Log("Grocery Count: " + groceryList.Count);

            // Start this when collect food AFTER that spawn these mf else where.
            groceries.enabled = false;
            other.gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            other.gameObject.transform.position = new Vector2(UnityEngine.Random.Range(1000, 5000), UnityEngine.Random.Range(1000, 5000));

            if(cap <= 0)
            {
                // Randomly Order it by Guid..
                groceryList = groceryList.OrderBy(i => Guid.NewGuid()).ToList();
                // It's now shuffled.
                SpawnOnEnd();
            }
        }
    }    

    void SpawnOnEnd()
    {
        //Put this into another method and call it once If(cap <= 0)
        for (int j = 0; j < groceryList.Count; j++)
        {
            // This with the for loop will be in another method
            lastPlatform = groceryPlatformedList[j];
            groceryList[j].transform.position = new Vector2(lastPlatform.x, lastPlatform.y + 1f);

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
}
