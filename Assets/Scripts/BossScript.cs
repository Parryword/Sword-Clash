using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : FightingObject
{
    private bool isSlashing;
    // Start is called before the first frame update
    void Start()
    {
        fightingDistance = 2.5f;
        horizontalSpeed = 0.08f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {   
        if (player.stage == level)
        {
            UpdatePolygonCollider2D();
            slash();
            walk();
            Die();
        }
        
    }

    private void slash()
    {
        if (isSlashing == true)
        {
            animator.SetBool("attack", true);
        }
    }

    private void walk()
    {
        float playerDistance = gameObject.transform.position.x - player.transform.position.x;

        if (Mathf.Abs(playerDistance) > fightingDistance && !isSlashing)
        {
            if (playerDistance < 0)
            {
                gameObject.transform.position += new Vector3(horizontalSpeed, 0, 0);
                spriteRenderer.flipX = true;
                animator.SetBool("walk", true);

            }
            else if (playerDistance > 0)
            {
                gameObject.transform.position -= new Vector3(horizontalSpeed, 0, 0);
                spriteRenderer.flipX = false;
                animator.SetBool("walk", true);

            }

        }
        else
        {
            animator.SetBool("walk", false);
        }
    }

    private new void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemyObject = collision.gameObject;
            isSlashing = true;
        }
            

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isSlashing = false;
    }
    /*
    public new void hitEnemy()
    {
        if(enemyObject != null && enemyObject.tag == "Player")
        {
            Debug.Log("Player has been hit by" + gameObject.name);
            enemyObject.GetComponent<FightingObject>().health -= damage;
            enemyObject.GetComponent<Player>().bleed();
        }
    }
    */
}
