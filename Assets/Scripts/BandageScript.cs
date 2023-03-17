using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BandageScript : MonoBehaviour
{
    private int count = 0;
    [SerializeField] private TextMeshProUGUI text;
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void increaseBandage()
    {
        count++;
        updateText();
    }

    public void decreaseBandage()
    {
        if (count > 0)
        {
            count--;
            updateText();
        } else
        {
            Debug.Log("No bandages left.");
        }
    }

    public void useBandage(Player player)
    {
        if (player.health >= 22 && player.health < 30 && count > 0)
        {
            player.health = 30;
            decreaseBandage();

            Debug.Log("Player healed with bandages found in house.");
        }
        else if (player.health < 22 && count > 0)
        {
            player.health += 8;
            decreaseBandage();

            Debug.Log("Player healed with bandages found in house.");
        }
    }

    public void updateText()
    {
        text.text = count.ToString();
    }
}
