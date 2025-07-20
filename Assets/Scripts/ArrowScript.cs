using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private int forceMultiplier = 50;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Vector2 originalOffset;
    private float deltaX = 0;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider != null)
            originalOffset = boxCollider.offset;

        var player = GameObject.Find("Player").GetComponent<Player>();  
        deltaX = player.transform.position.x - transform.position.x;
        if (deltaX > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * forceMultiplier * deltaX);

        // Time.timeScale = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRenderer.flipX)
        {
            if (!isGrounded && (transform.eulerAngles.z > 235 || transform.eulerAngles.z == 0))
            {
                transform.Rotate(0, 0, -3f / Mathf.Abs(deltaX));
            }
        }
        else
        {
            if (!isGrounded && transform.eulerAngles.z < 125)
            {           
                transform.Rotate(0, 0, 3f / Mathf.Abs(deltaX));
            }
        }

        if (boxCollider != null)
        {
            if (spriteRenderer.flipX)
            {
                // Flip the collider offset on X axis
                boxCollider.offset = new Vector2(-originalOffset.x, originalOffset.y);
            }
            else
            {
                // Reset collider offset when not flipped
                boxCollider.offset = originalOffset;
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
