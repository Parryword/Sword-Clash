using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class StatsTextManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textField;

    // Start is called before the first frame update
    void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateText(string HP, string maxHP, string dmg, string def, string lv)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Stats\n");
        sb.Append("HP: " + HP + " / " + maxHP + "\n");
        sb.Append("DMG: " + dmg + "\n");
        sb.Append("DEF: " +  def + "\n");
        sb.Append("LV: " + lv + "\n");
        
        textField.text = sb.ToString();
    }
}
