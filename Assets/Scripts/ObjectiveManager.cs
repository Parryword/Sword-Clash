using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public GameObject[] enemies;
    public TextMeshProUGUI textArea;
    public GameObject[] music;
    
    public bool allEnemiesDead;
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

        if (enemies[0] == null && enemies[1] == null && enemies[2] == null && allEnemiesDead == false)
        {
                allEnemiesDead = true;
                textArea.text = "Objectives\nEnter the castle.";
                Debug.Log("Will stop");
                music[0].GetComponent<AudioSource>().Stop();
                Debug.Log("Will play");
                music[1].GetComponent<AudioSource>().Play();

        }
        
        
    }

    
}
