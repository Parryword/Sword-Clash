using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : FightingObject
{
    private float playerDistance;
    public float attackDistance;
    private float agroDistance;
    public int dropCount = 3;
    private bool isFlankingLeft, isFlankingRight;
    public GameObject enemyObject;
    [SerializeField] protected GameObject coinPrefab;
    private Player player;

    void Start()
    {
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        Walk();
        FlankRight();
        FlankLeft();
        Slash();
        UpdatePolygonCollider2D();
        Die();
    }

    void Slash()
    {
        if (Mathf.Abs(playerDistance) < attackDistance && isFlankingLeft == false && isFlankingRight == false)
        {
            animator.SetTrigger("slash");
        }
    }

    void Walk()
    {
        playerDistance = gameObject.transform.position.x - player.transform.position.x;

        if (Mathf.Abs(playerDistance) < agroDistance && Mathf.Abs(playerDistance) > attackDistance && !isFlankingLeft &&
            !isFlankingRight)
        {
            if (playerDistance < 0 && IsRightClear())
            {
                gameObject.transform.position += new Vector3(0.08f, 0, 0);
                spriteRenderer.flipX = false;
                animator.SetBool("walking", true);
            }
            else if (playerDistance > 0 && IsLeftClear())
            {
                gameObject.transform.position -= new Vector3(0.08f, 0, 0);
                spriteRenderer.flipX = true;
                animator.SetBool("walking", true);
            }
            else if (playerDistance < 0 && !IsRightClear())
            {
                animator.SetBool("walking", false);
            }
            else if (playerDistance > 0 && !IsLeftClear())
            {
                animator.SetBool("walking", false);
            }
        }
        else
        {
            animator.SetBool("walking", false);
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            enemyObject = collision.gameObject;
    }

    public override void HitEnemy()
    {
        if (enemyObject != null && enemyObject.CompareTag("Player"))
        {
            Debug.Log("Player has been hit by" + gameObject.name);
            int dmgAmount = damage - player.defense;
            if (dmgAmount < 1)
            {
                dmgAmount = 1;
            }

            enemyObject.GetComponent<FightingObject>().health -= dmgAmount;
            enemyObject.GetComponent<Player>().Bleed();
        }
    }
    
    private bool IsLeftClear()
    {
        var hits = Physics2D.RaycastAll(transform.position, Vector2.left, 2);
        var enemies = hits
            .Where(hit => hit.transform.CompareTag("Enemy"))
            .Select(hit => hit.collider.gameObject).ToList();
        return enemies.Count > 0;
    }
    
    private bool IsRightClear()
    {
        var hits = Physics2D.RaycastAll(transform.position, Vector2.right, 2);
        var enemies = hits
            .Where(hit => hit.transform.CompareTag("Enemy"))
            .Select(hit => hit.collider.gameObject).ToList();
        return enemies.Count > 0;
    }

    private void FlankLeft()
    {
        if (!IsLeftClear() && Mathf.Abs(GetDistance(player)) > attackDistance && !isFlankingLeft && !isFlankingRight &&
            GetDistance(player) < 0 && !isFlankingRight || isFlankingLeft)
        {
            spriteRenderer.flipX = true;
            if (isFlankingLeft == false)
            {
                isFlankingLeft = true;
                StartCoroutine("stopFlanking");
            }

            gameObject.transform.position += new Vector3(-0.08f, 0, 0);
            animator.SetBool("walking", true);

            if (Mathf.Abs(GetDistance(player)) > 4)
            {
                isFlankingLeft = false;
                if (GetDistance(player) > 0)
                {
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }
            }
        }
        else if (isFlankingLeft && Mathf.Abs(GetDistance(player)) < attackDistance)
        {
            gameObject.transform.position += new Vector3(-0.08f, 0, 0);
            animator.SetBool("walking", true);
        }
    }

    private void FlankRight()
    {
        if (!IsRightClear() && Mathf.Abs(GetDistance(player)) > attackDistance && !isFlankingLeft && !isFlankingRight &&
            GetDistance(player) > 0 && !isFlankingLeft || isFlankingRight)
        {
            spriteRenderer.flipX = false;
            if (isFlankingRight == false)
            {
                isFlankingRight = true;
                StartCoroutine("stopFlanking");
            }

            gameObject.transform.position += new Vector3(0.08f, 0, 0);
            animator.SetBool("walking", true);

            if (Mathf.Abs(GetDistance(player)) > 4)
            {
                isFlankingRight = false;
                if (GetDistance(player) > 0)
                {
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }
            }
        }
        else if (isFlankingRight && Mathf.Abs(GetDistance(player)) > attackDistance)
        {
            gameObject.transform.position += new Vector3(0.08f, 0, 0);
            animator.SetBool("walking", true);
        }
    }

    private bool IsDirectionRight()
    {
        playerDistance = gameObject.transform.position.x - player.transform.position.x;
        return playerDistance > 0;
    }

    IEnumerator StopFlanking()
    {
        float seconds = Mathf.Abs(GetDistance(player)) * 2 / (0.08f * 50);
        Debug.Log(seconds);
        yield return new WaitForSeconds(seconds);
        isFlankingLeft = false;
        isFlankingRight = false;
        spriteRenderer.flipX = GetDistance(player) <= 0;
    }
    
    private void DropLoot()
    {
        for (int i = 0; i < dropCount; i++)
        {
            Instantiate(coinPrefab, gameObject.transform.position, Quaternion.identity);
        }
    }
    
    protected override void Die()
    {
        if (health > 0) return;
        DropLoot();
        Destroy(gameObject);
    }
}