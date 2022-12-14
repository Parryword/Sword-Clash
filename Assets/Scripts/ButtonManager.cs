using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button spawnGuard;
    public GameObject guardPrefab;
    

    // Start is called before the first frame update
    void Start()
    {
        spawnGuard.onClick.AddListener(spawnGuardFunc);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnGuardFunc()
    {
        Instantiate(guardPrefab);
    }
}
