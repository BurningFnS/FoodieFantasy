using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class DeathPlatformer : MonoBehaviour
{
    public PlayerController player;
    public PlayerController playerController;

    [SerializeField]
    Text _FoodConsumed, _FoodWasted, _TotalScore, _GoodMessage, _BadMessage;
    [SerializeField]
    RectTransform _PanelEnd, _PanelStart;
    [SerializeField]
    Button _Continue;

    // Update is called once per frame
    void Update()
    {
        if(player.grounded)
        {
            transform.position = new Vector2(player.transform.position.x, player.transform.position.y - 8f);
        }
        else if(player.jump)
        {
            transform.position = player.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.SetActive(false);

            Time.timeScale = 0f;

            _PanelEnd.gameObject.SetActive(true);
            _PanelStart.gameObject.SetActive(false);
            _FoodConsumed.text = "Food Consumed: " + playerController.fullnessPercentage * 5;
            _FoodWasted.text = "Food Wasted: -" + 100;
            _TotalScore.text = "Total Score : " + ((playerController.fullnessPercentage * 5) - 100);
            if (((playerController.fullnessPercentage * 5) - 100) >= 100)
            {
                _GoodMessage.gameObject.SetActive(true);
                _BadMessage.gameObject.SetActive(false);
            }
            else
            {
                _BadMessage.gameObject.SetActive(true);
                _GoodMessage.gameObject.SetActive(false);
            }
        }
    }
}
