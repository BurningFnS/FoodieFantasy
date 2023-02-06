using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseAndResume : MonoBehaviour
{
    [SerializeField]
    RectTransform _PanelPaused, _PanelPaused2;
    [SerializeField]
    Button _btnPaused, _btnPaused2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        Time.timeScale = 0;
        _PanelPaused.gameObject.SetActive(true);
        _PanelPaused2.gameObject.SetActive(true);

    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        _PanelPaused.gameObject.SetActive(false);
        _btnPaused.gameObject.SetActive(true);
        _PanelPaused2.gameObject.SetActive(false);
        _btnPaused2.gameObject.SetActive(true);
    }
}
