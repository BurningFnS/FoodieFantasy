using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basket : MonoBehaviour
{
    public GameManager gamemanager;

    public float speed = 10f;

    private List<string> ItemList = new List<string>();
    private List<string> JunkList = new List<string>();
    private List<string> BadItemsList = new List<string>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        BasketMovement();
    }

    void BasketMovement()
    {
        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
        transform.position += horizontal * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Food")
        {
            ItemList.Add(gameObject.name);
            Debug.Log("ItemList Count: " + ItemList.Count);
            Destroy(other.gameObject);

        }
    }
}
