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

    public void IncreaseBandage()
    {
        count++;
        UpdateText();
    }

    private void DecreaseBandage()
    {
        if (count > 0)
        {
            count--;
            UpdateText();
        } else
        {
            Debug.Log("No bandages left.");
        }
    }

    public void UseBandage()
    {
        var player = Globals.player;
        
        if (count < 1) return;
        
        player.health = Mathf.Clamp(player.health + 8, 0, player.maxHealth);
        DecreaseBandage();
    }

    private void UpdateText()
    {
        text.text = count.ToString();
    }
}
