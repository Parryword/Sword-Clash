using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    public float teleportX;
    public float teleportY;
    public int stage;
    public bool unlocked;

    // Start is called before the first frame update
    void Start()
    {
        unlocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if(unlocked == true)
        {
            Debug.Log("Clicked");
            GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(teleportX, teleportY, 0);
            GameObject.FindObjectOfType<Player>().level++;
            GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(teleportX, teleportY, -10);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().stage = stage;
        }
    }

    public void unlock()
    {
        unlocked = true;
    }
}
