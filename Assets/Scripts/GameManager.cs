using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Vector3 startingLine;
    IEnumerator isGameRunning; //This variable can change name. It is necessary to "pause" the loop because if u directly pause it, it will start from initial.
    [SerializeField]
    private GameObject[] groceriesList;

    [HideInInspector]
    public bool endGame = false;

    public Button pauseButton;
    public Button resumeButton;
    public float timeBetweenFood = 1.0f;


    void Awake()
    {
        startingLine = new Vector3(Random.Range(-2.2f, 2.2f), 5.7f, 0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        isGameRunning = GameLoop();
        StartCoroutine(isGameRunning);

        Button pausebtn = pauseButton.GetComponent<Button>();
        pausebtn.onClick.AddListener(pausing);

        Button resumebtn = resumeButton.GetComponent<Button>();
        resumebtn.onClick.AddListener(resume);
    }

    // Update is called once per frame
    void Update()
    {
        startingLine = new Vector3(Random.Range(-2.2f, 2.2f), 5.7f, 0f);


        /*------------------------------OLD PAUSE BUTTON----------------------------*/
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    Debug.Log("Game Loop Has been resumed");
        //    StartCoroutine(isGameRunning);
        //}

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    Debug.Log("Game Loop Has been paused");
        //    StopCoroutine(isGameRunning);
        //}   
    }

    IEnumerator GameLoop()
    {
        while (endGame == false)
        {
            Instantiate(groceriesList[Random.Range(0, groceriesList.Length)], startingLine, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenFood); // DELAY FOR EACH INSTANTIATE 

            Debug.Log("Game is running");
            yield return new WaitForSeconds(1f);
        }
    }

    void pausing()
    {
        Debug.Log("Game is paused!");
        StopCoroutine(isGameRunning);
    }
    void resume()
    {
        Debug.Log("Game is resumed!");
        StartCoroutine(isGameRunning);

    }

}
