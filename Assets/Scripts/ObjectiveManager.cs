using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public GameObject[] enemies;
    public TextMeshProUGUI textArea;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            music[1].GetComponent<AudioSource>().Play();
        }
    }

    private void FixedUpdate()
    {
        if (enemies[0] == null && enemies[1] != null && enemies[2] != null)
        {
            textArea.text = "Objectives\n•Reach the castle.";
            //textArea = "Objectives\n•Reach the castle";
        }

        if (enemies[0] == null && enemies[1] == null && enemies[2] == null && level1Completed == false)
        {
            level1Completed = true;
            doors[0].unlock();
            textArea.text = "Objectives\nEnter the castle.";
            Debug.Log("Will stop");
            music[0].GetComponent<AudioSource>().Stop();
            Debug.Log("Will play");
            music[1].GetComponent<AudioSource>().Play();

        }

        if (enemies[3] == null && enemies[4] == null && level2Completed == false)
        {
            level2Completed = true;
            doors[1].unlock();
            textArea.text = "Objectives\nEnter the dungeon.";
            Debug.Log("Will stop");
            music[0].GetComponent<AudioSource>().Stop();
            Debug.Log("Will play");
            music[1].GetComponent<AudioSource>().Play();
        }

        if (level1Completed && !level2Completed && GameObject.FindObjectOfType<Player>().stage == 2)
        {
            textArea.text = "Objective\nKill the guards.";
        }

        if (level2Completed && !level3Completed && GameObject.FindObjectOfType<Player>().stage == 3)
        {
            textArea.text = "Objective\nKill the lord.";
        }

        if (enemies[5] == null && level3Completed == false)
        {
            level3Completed = true;
            textArea.text = "Objectives\nAll completed.";
            Debug.Log("Will stop");
            music[0].GetComponent<AudioSource>().Stop();
            Debug.Log("Will play");
            music[1].GetComponent<AudioSource>().Play();
            victoryText.gameObject.SetActive(true);
        }


    }

    
}
