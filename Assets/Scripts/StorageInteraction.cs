using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageInteraction : MonoBehaviour
{
    bool isUsed;
    // Start is called before the first frame update
    void Start()
    {
        isUsed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {   
        var PlayerRef = GameObject.Find("Player").GetComponent<Player>();
        if (PlayerRef.health >= 25 && PlayerRef.health < 30 && !isUsed)
        {
            PlayerRef.health = 30;
            isUsed = true;
            Debug.Log("Player healed with bandages found in house.");
        }
        else if (PlayerRef.health < 25 && !isUsed)
        {
            PlayerRef.health += 5;
            isUsed = true;
            Debug.Log("Player healed with bandages found in house.");
        }
    }
}
