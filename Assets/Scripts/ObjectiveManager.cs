using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class ObjectiveManager : MonoBehaviour
{
    public GameObject[] enemies;
    [SerializeField] private TextMeshProUGUI textArea;
    public TextMeshProUGUI victoryText;
    [SerializeField] GateScript[] doors;
    public SoundManager soundManager;
    public Player player;
    
    public bool level1Completed;
    public bool level2Completed;
    public bool level3Completed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (enemies[0] == null && enemies[1] != null && enemies[2] != null)
        {
            //  textArea.text = "Objectives\n�Reach the castle.";
            //textArea = "Objectives\n�Reach the castle";
            setObjective("Reach the castle");
        }

        if (enemies[0] == null && enemies[1] == null && enemies[2] == null && level1Completed == false)
        {
            level1Completed = true;
            doors[0].Unlock();
            setObjective("Enter the castle");
            soundManager.PlaySoundEffect(Sound.SUCCESS, true);

        }

        if (enemies[3] == null && enemies[4] == null && level2Completed == false)
        {
            level2Completed = true;
            doors[1].Unlock();
            setObjective("Enter the dungeon");
            soundManager.PlaySoundEffect(Sound.SUCCESS, true);
        }

        if (level1Completed && !level2Completed)
        {
            //   textArea.text = "Objective\nKill the guards.";
            setObjective("Kill the guards");
        }

        if (level2Completed && !level3Completed)
        {
         //   textArea.text = "Objective\nKill the lord.";
            setObjective("Kill the lord");
        }

        if (enemies[5] == null && level3Completed == false)
        {
            level3Completed = true;
            setObjective();
            soundManager.PlaySoundEffect(Sound.SUCCESS, true);
            victoryText.gameObject.SetActive(true);
        }
    }

    public void setObjective (params string[] objectives)
    {
        if (objectives.Length == 0)
        {
            setObjective("All completed");
            return;
        }

        StringBuilder sb = new();
        sb.Append("Objectives\n");
        foreach (var item in objectives)
        {
            sb.Append("• " + item + "\n");
        }

        textArea.text = sb.ToString();
    }
}
