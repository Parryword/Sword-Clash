using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField]
    private float coef = 0.1f;
    [SerializeField]
    private float period = 1f;
    [SerializeField]
    private bool disableAnimation = false;
    private float yPos;
    private float playerDistance;

    // Start is called before the first frame update
    void Start()
    {
        yPos = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    private void FixedUpdate()
    {
        playerDistance = Globals.player.transform.position.x - gameObject.transform.position.x;

        if (!disableAnimation)
        {
            var v3 = gameObject.transform.position;
            v3.y = yPos + coef * Mathf.Sin(period * Time.realtimeSinceStartup);
            gameObject.transform.position = v3;
        }

        if (Mathf.Abs(playerDistance) < 1)
        {
            Globals.player.goldAmount++;
            Globals.soundManager.PlaySoundEffect(Sound.Coin);
            Destroy(gameObject);
        }

        if (Mathf.Abs(playerDistance) < 4)
        {
            if (playerDistance > 0)
            {
                gameObject.transform.position += new Vector3(0.2f, 0, 0);
            }

            if (playerDistance < 0)
            {
                gameObject.transform.position += new Vector3(-0.2f, 0, 0);
            }
        }
    }
}
