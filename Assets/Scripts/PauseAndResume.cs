using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAndResume : MonoBehaviour
{
    bool gamePaused;
    bool pauseButtonPressed;
    // Start is called before the first frame update
    void Start()
    {
        gamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gamePaused == false && pauseButtonPressed == true)
        {
            PauseGame();
        }
        else if(gamePaused == true && pauseButtonPressed == false)
        {
            ResumeGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
