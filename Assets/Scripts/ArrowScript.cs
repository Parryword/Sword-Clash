using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public Target target = Target.Enemy;
    public int forceMultiplier = 50;
    
    private bool isGrounded;
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

        GameObject targetObject = null;

        if (target == Target.Player)
        {
            targetObject = GameObject.FindGameObjectWithTag("Player");
        }
        if (target == Target.Enemy)
        {
            targetObject = GameObject.FindGameObjectsWithTag("Enemy")
                .OrderBy(e => Mathf.Abs(e.transform.position.x - transform.position.x))
                .FirstOrDefault();;
        }

        if (targetObject != null)
        {
            deltaX = targetObject.transform.position.x - transform.position.x;
        }
        else
        {
            Debug.Log("No target found");
            Destroy(gameObject);
        }
        
        if (deltaX > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * forceMultiplier * deltaX);
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
