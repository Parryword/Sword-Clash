using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseScript : MonoBehaviour, IObserver
{
    private bool isUsed;

    // Start is called before the first frame update
    void Start()
    {
        isUsed = false;
        Globals.objectiveManager.Attach(this);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDown()
    {
        if (isUsed) return;

        Globals.player.bandage.increaseBandage();
        isUsed = true;
    }

    public void Refresh()
    {
        isUsed = false;
    }
}