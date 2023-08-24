using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class ObjectiveManager : MonoBehaviour
{
    public GameObject[] enemies;
    [SerializeField]
    private TextMeshProUGUI textArea;
    public TextMeshProUGUI victoryText;
    public GameObject[] music;
    [SerializeField] GateScript[] doors;
    
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
            //  textArea.text = "Objectives\n•Reach the castle.";
            //textArea = "Objectives\n•Reach the castle";
            setObjective("Reach the castle");
        }

        if (enemies[0] == null && enemies[1] == null && enemies[2] == null && level1Completed == false)
        {
            level1Completed = true;
            doors[0].unlock();
            setObjective("Enter the castle");
            Debug.Log("Will stop");
            music[0].GetComponent<AudioSource>().Stop();
            Debug.Log("Will play");
            music[1].GetComponent<AudioSource>().Play();

        }

        if (enemies[3] == null && enemies[4] == null && level2Completed == false)
        {
            level2Completed = true;
            doors[1].unlock();
            setObjective("Enter the dungeon.");
            Debug.Log("Will stop");
            music[0].GetComponent<AudioSource>().Stop();
            Debug.Log("Will play");
            music[1].GetComponent<AudioSource>().Play();
        }

        if (level1Completed && !level2Completed && GameObject.FindObjectOfType<Player>().stage == 2)
        {
            //   textArea.text = "Objective\nKill the guards.";
            setObjective("Kill the guards.");
        }

        if (level2Completed && !level3Completed && GameObject.FindObjectOfType<Player>().stage == 3)
        {
         //   textArea.text = "Objective\nKill the lord.";
            setObjective("Kill the lord.");
        }

        if (enemies[5] == null && level3Completed == false)
        {
            level3Completed = true;
            setObjective();
            Debug.Log("Will stop");
            music[0].GetComponent<AudioSource>().Stop();
            Debug.Log("Will play");
            music[1].GetComponent<AudioSource>().Play();
            victoryText.gameObject.SetActive(true);
        }
    }

    public void setObjective (params string[] objectives)
    {
        if (objectives.Length == 0)
        {
            textArea.text = "Objectives\nAll completed.";
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.Append("Objectives\n");
        foreach (var item in objectives)
        {
            sb.Append("• " + item + "\n");
        }

        textArea.text = sb.ToString();
    }
}
