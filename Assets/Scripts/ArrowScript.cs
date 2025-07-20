using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [SerializeField] private bool isGrounded = false;
    private bool flippedX = false;
    [SerializeField] private int forceMultiplier = 50;

    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.Find("Player").GetComponent<Player>();  
        var deltaX = player.transform.position.x - transform.position.x;
        if (deltaX > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            flippedX = true;
        }
        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * forceMultiplier * deltaX);
    }

    // Update is called once per frame
    void Update()
    {
        if (flippedX)
        {
            if (!isGrounded && transform.eulerAngles.z > -125)
            {
                transform.Rotate(0, 0, -0.5f);
            }
        }
        else
        {
            if (!isGrounded && transform.eulerAngles.z < 125)
            {           
                transform.Rotate(0, 0, 0.5f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            var player = collision.gameObject.GetComponent<Player>();
            player.Bleed();
            player.health -= 1;
        }

        isGrounded = true;
        Destroy(gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
