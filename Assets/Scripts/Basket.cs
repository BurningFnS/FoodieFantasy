using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basket : MonoBehaviour
{
    public GameManager gamemanager;
    private GameObject[] platformSpawn;
    public int maxPlatforms;
    public List<Vector2> platformList;

    public float speed = 10f;
    bool isMoving = false;

    float cap = 100;

    Rigidbody2D rb;
    Rigidbody2D groceryrb2d;

    Groceries groceries;

    //private List<string> ItemList = new List<string>();
    public List<GameObject> groceryList;

    public Text capacityText;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groceryList = new List<GameObject>();
        platformSpawn = gamemanager.platformSpawn;
        maxPlatforms = gamemanager.maxPlatforms;
        platformList = gamemanager.platformList;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        BasketMovement();
    }

    void BasketMovement()
    {
        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if(isMoving == false)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            rb.MovePosition(transform.position + horizontal * Time.deltaTime * speed);
        }
        
        
        //transform.position += horizontal * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Food")
        {
            //ItemList.Add(gameObject.name);
            //Debug.Log("ItemList Count: " + ItemList.Count);

            groceryList.Add(other.gameObject);

            groceries = other.gameObject.GetComponent<Groceries>();
            groceryrb2d = other.gameObject.GetComponent<Rigidbody2D>();

            float s = groceries.size;


            cap -= s;

            capacityText.text = "Basket Capacity: " + (cap).ToString() + "%";

            Debug.Log(string.Join(", ", groceryList));
            Debug.Log("Grocery Count: " + groceryList.Count);

            //platformSpawn = new GameObject[maxPlatforms];
            for (int i = 0; i < platformList.Count; i++)
            {

                //Debug.Log(platformSpawn[i].transform.position);

                for (int j = 0; j < groceryList.Count; j++)
                {
                    if(i == j)
                    {
                        other.gameObject.transform.position = new Vector2(platformList[i].x, platformList[i].y + 1f);
                        groceries.enabled = false;
                        other.gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    }
                }

            }
            
        }
    }
}
