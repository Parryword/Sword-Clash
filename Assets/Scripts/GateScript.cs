using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        Debug.Log("Clicked");
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(-30,-23,0);
        GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(-30, -23, -10);
    }

}
