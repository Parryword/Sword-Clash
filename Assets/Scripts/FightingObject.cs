using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Mathematics;
using static GameManager;

public class FightingObject : AnimatableObject
{
    // Stats
    private float playerDistance;
    public float fightingDistance;
    private float agroDistance;
    private bool active;
    [SerializeField] private int dropCount = 3;
    [SerializeField] public int maxHealth = 10, health, damage = 3, defense = 0, level;
    [SerializeField] private bool isFlankingLeft, isFlankingRight;

    // Other
    public GameObject blood;
    public GameObject enemyObject;
    protected static SoundManager soundManager;
    [SerializeField] protected GameObject coinPrefab;


    // Start is called before the first frame update
    void Start()
    {
        soundManager = SoundManager.instance;
        agroDistance = 15;
        fightingDistance = 2.5f;
        health = maxHealth;
        enemyObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.level == level)
            active = true;
        else
            active = false;
    }

    private void FixedUpdate()
    {   
        if (active)
        {
            Walk();
            FlankRight();
            FlankLeft();
            Slash();
            UpdatePolygonCollider2D();
            Die();
        }
    }

    void Slash()
    {
        if (Mathf.Abs(playerDistance) < fightingDistance && isFlankingLeft == false && isFlankingRight == false)
        {
            animator.SetTrigger("slash");
        }
    }

    void Walk()
    {
        playerDistance = gameObject.transform.position.x - player.transform.position.x;

        if (Mathf.Abs(playerDistance) < agroDistance && Mathf.Abs(playerDistance) > fightingDistance && !isFlankingLeft && !isFlankingRight)
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
        if (collision.gameObject.tag == "Player")
            enemyObject = collision.gameObject;
    }

    public virtual void HitEnemy()
    {
        if (enemyObject != null && enemyObject.tag == "Player")
        {
            Debug.Log("Player has been hit by" + gameObject.name);
            int dmgAmount = damage - player.defense;
            if (dmgAmount < 1) { 
                dmgAmount = 1;
            }
            enemyObject.GetComponent<FightingObject>().health -= dmgAmount;
            enemyObject.GetComponent<Player>().Bleed();
        }
    }

    private bool IsLeftClear()
    {
        List<FightingObject> allFightingObjects = GameObject.FindObjectsOfType<FightingObject>().ToList();
        List<FightingObject> near = (from fo in allFightingObjects where fo != this && fo != player && Mathf.Abs(GetDistance(fo)) < 2 && GetDistance(fo) < 0 && fo.isFlankingLeft == false && fo.isFlankingRight == false select fo).ToList();

        foreach (FightingObject f in near)
        {
            Debug.Log("On left of " + gameObject.name + " " + f.name);
        }

        if (near.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }   
    }

    private bool IsRightClear()
    {
        List<FightingObject> allFightingObjects = GameObject.FindObjectsOfType<FightingObject>().ToList();
        List<FightingObject> near = (from fo in allFightingObjects where fo != this && fo != player && Mathf.Abs(GetDistance(fo)) < 2 && GetDistance(fo) > 0 && fo.isFlankingLeft == false && fo.isFlankingRight == false select fo).ToList();

        foreach (FightingObject f in near)
        {
            Debug.Log("On right of " + gameObject.name + " " + f.name);
        }
        if (near.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FlankLeft()
    {
        if (!IsLeftClear() && Mathf.Abs(GetDistance(player)) > fightingDistance && !isFlankingLeft && !isFlankingRight && GetDistance(player) < 0 && !isFlankingRight || isFlankingLeft)
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
        else if (isFlankingLeft && Mathf.Abs(GetDistance(player)) < fightingDistance)
        {
            gameObject.transform.position += new Vector3(-0.08f, 0, 0);
            animator.SetBool("walking", true);
        }
        /*else if (isFlankingLeft && (isLeftClear() || getDistance(player) >= fightingDistance))
        {
            isFlankingLeft = false;
            animator.SetBool("walking", false);
        }*/
    }

    private void FlankRight()
    {
        if (!IsRightClear() && Mathf.Abs(GetDistance(player)) > fightingDistance && !isFlankingLeft && !isFlankingRight && GetDistance(player) > 0 && !isFlankingLeft || isFlankingRight)
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
        else if (isFlankingRight && Mathf.Abs(GetDistance(player)) > fightingDistance)
        {
            gameObject.transform.position += new Vector3(0.08f, 0, 0);
            animator.SetBool("walking", true);
        }
        /*else if (isFlankingRight && (isLeftClear() || getDistance(player) <= fightingDistance))
        {
            isFlankingRight = false;
            animator.SetBool("walking", false);
        }*/
    }

    IEnumerator StopFlanking()
    {
        float seconds = Mathf.Abs(GetDistance(player)) * 2 / (0.08f * 50);
        Debug.Log(seconds);
        yield return new WaitForSeconds(seconds);
        isFlankingLeft = false;
        isFlankingRight = false;
        if (GetDistance(player) > 0)
        {
            spriteRenderer.flipX = false;
        } else
        {
            spriteRenderer.flipX = true;
        }
    }

    public virtual void Bleed()
    {
        Debug.Log("Enemy bleeds");
        // music[0].GetComponent<AudioSource>().Play();
        soundManager.playSound(Sound.BASIC_ATTACK);
        blood.GetComponent<BloodObject>().startBleed(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.2f));
    }

    public float GetDistance(FightingObject target)
    {
        return target.transform.position.x - gameObject.transform.position.x;
    }

    private void DropLoot()
    {
        for (int i = 0; i < dropCount; i++)
        {
            Instantiate(coinPrefab, gameObject.transform.position, Quaternion.identity);
        }
    }

    public virtual void Die()
    {
        if (health <= 0)
        {
            DropLoot();
            Destroy(gameObject);
        }
    }
}

public enum FightingObjectState {
    WALK, ATTACK, FLANK
}

interface IArtificialIntelligence
{
    bool isStunned { get; set; }
    bool isEnraged { get; set; }
    void Walk();
    void FlankRight();
    void FlankLeft();
    void Slash();
    void UpdatePolygonCollider2D();
    void Die();
}