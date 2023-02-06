using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject TutPanel;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 0;

        StartCoroutine(PanelAppear());
        Time.timeScale = 1;
       

    }
    void OnClickButton()
    {
        SceneManager.LoadScene("BasketCapacity");
    }
    IEnumerator PanelAppear()
    {
        TutPanel.SetActive(true);
        yield return new WaitForSeconds(5f);
        TutPanel.SetActive(false);
        
    }
}
