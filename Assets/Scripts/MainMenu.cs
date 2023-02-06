using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public RectTransform DropperPanel;
    public RectTransform FirstPanel;
    public void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
    public void HowToPlay()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void Return()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OnClickButton()
    {
      DropperPanel.gameObject.SetActive(false);
      FirstPanel.gameObject.SetActive(true);
        
    }

}
