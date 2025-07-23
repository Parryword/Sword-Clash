using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScript : MonoBehaviour
{
    public GameObject upgradePanel;

    // Start is called before the first frame update
    void Start()
    {
        upgradePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        upgradePanel.SetActive(true);
    }

    private void OnMouseExit()
    {
        upgradePanel.SetActive(false);
    }
}
