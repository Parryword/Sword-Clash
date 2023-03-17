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
        if(!isUsed)
        {
            var PlayerRef = GameObject.Find("Player").GetComponent<Player>();
            PlayerRef.bandage.increaseBandage();
            isUsed = true;
        }
        
    }
}
