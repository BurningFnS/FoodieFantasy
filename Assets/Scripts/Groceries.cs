using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groceries : MonoBehaviour
{
    [Range(0.5f, 1f)]
    public float mass;
    float height;
    float speed = 7f;
    public float size;
    public float filling;

    // Start is called before the first frame update
    void Start()
    {
        height = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GroceriesFalling();
 
    }

    void GroceriesFalling()
    {
        height -= mass * Time.deltaTime * speed;
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
    }
}
