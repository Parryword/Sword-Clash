namespace Building
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Character;
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
                Globals.player.transform.position = new Vector3(teleportX, teleportY, 0);
                GameObject.FindObjectOfType<Player>().level++;
                Globals.camera.transform.position = new Vector3(teleportX, teleportY, -10);
            }
        }

        public void Unlock()
        {
            unlocked = true;
        }
    }
}