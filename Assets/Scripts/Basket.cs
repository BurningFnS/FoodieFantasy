using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basket : MonoBehaviour
{
    public GameManager gamemanager;

    public float speed = 10f;
    bool isMoving = false;

    Rigidbody2D rb;

    Groceries groceries;

    //private List<string> ItemList = new List<string>();
    private List<GameObject> groceryList;

    public Text capacityText;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groceryList = new List<GameObject>();
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

            float s = groceries.size;

            float cap = 100;

            cap -= s;

            capacityText.text = "Basket Capacity: " + (cap).ToString() + "%";

            Debug.Log(string.Join(", ", groceryList));
            Debug.Log("Grocery Count: " + groceryList.Count);

            Destroy(other.gameObject);

        }
    }
}
