using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseScript : MonoBehaviour, IObserver
{
    private bool isUsed;
    private Player player;
    private ObjectiveManager objectiveManager;

    // Start is called before the first frame update
    void Start()
    {
        isUsed = false;
        player = GameObject.Find("Player").GetComponent<Player>();
        objectiveManager = GameObject.Find("GameManager").GetComponent<ObjectiveManager>();
        objectiveManager.Attach(this);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Resupply()
    {
        isUsed = false;
    }

    private void OnMouseDown()
    {
        if (isUsed) return;

        player.bandage.increaseBandage();
        isUsed = true;
    }

    public void Refresh()
    {
        Resupply();
    }
}