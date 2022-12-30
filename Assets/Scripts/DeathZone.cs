using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{

    private List<GameObject> wastedFoodList;
    // Start is called before the first frame update
    void Start()
    {
        wastedFoodList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Food")
        {

            wastedFoodList.Add(other.gameObject);

            Debug.Log(string.Join(", ", wastedFoodList));
            Debug.Log("Food Wasted Count: " + wastedFoodList.Count);

            Destroy(other.gameObject);

        }
    }
}
