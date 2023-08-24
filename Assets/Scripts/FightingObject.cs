using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Mathematics;

public class FightingObject : AnimatableObject
{
    public Player player;
    public GameObject blood;
    private float playerDistance;
    public float fightingDistance;
    private float agroDistance;
    public float health;
    public float damage;
    public GameObject enemyObject;
    [SerializeField] private bool isFlankingLeft;
    [SerializeField] private bool isFlankingRight;
    public int level;
    protected static SoundManager soundManager;
    [SerializeField] private int dropCount = 3;
    [SerializeField] protected GameObject coinPrefab;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = SoundManager.instance;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        agroDistance = 15;
        fightingDistance = 2.5f;
        enemyObject = null;
        health = 10;
        damage = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {   
        if (player.level == level)
        {
            walk();
            flankRight();
            flankLeft();
            slash();
            UpdatePolygonCollider2D();
            die();
        }
        

    }


    void slash()
    {
        if (Mathf.Abs(playerDistance) < fightingDistance && isFlankingLeft == false && isFlankingRight == false)
        {
            animator.SetTrigger("slash");
        }
    }

    void walk()
    {
        playerDistance = gameObject.transform.position.x - player.transform.position.x;

        if (Mathf.Abs(playerDistance) < agroDistance && Mathf.Abs(playerDistance) > fightingDistance && !isFlankingLeft && !isFlankingRight)
        {
            if (playerDistance < 0 && isRightClear())
            {
                gameObject.transform.position += new Vector3(0.08f, 0, 0);
                spriteRenderer.flipX = false;
                animator.SetBool("walking", true);

            }
            else if (playerDistance > 0 && isLeftClear())
            {
                gameObject.transform.position -= new Vector3(0.08f, 0, 0);
                spriteRenderer.flipX = true;
                animator.SetBool("walking", true);

            }
            else if (playerDistance < 0 && !isRightClear())
            {
                animator.SetBool("walking", false);
            }
            else if (playerDistance > 0 && !isLeftClear())
            {
                animator.SetBool("walking", false);
            }

        }
        else
        {
            animator.SetBool("walking", false);
        }
    }

    private void LateUpdate()
    {
     
    }

    public void OnTriggerStay2D(Collider2D collision)
    {   
        if (collision.gameObject.tag == "Player")
            enemyObject = collision.gameObject;
        

    }

    public virtual void hitEnemy()
    {
        if (enemyObject != null && enemyObject.tag == "Player")
        {
            Debug.Log("Player has been hit by" + gameObject.name);
            enemyObject.GetComponent<FightingObject>().health -= damage;
            enemyObject.GetComponent<Player>().bleed();
            

        }
    }

    private bool isLeftClear()
    {

        List<FightingObject> allFightingObjects = GameObject.FindObjectsOfType<FightingObject>().ToList();
        List<FightingObject> near = (from fo in allFightingObjects where fo != this && fo != player && Mathf.Abs(getDistance(fo)) < 2 && getDistance(fo) < 0 && fo.isFlankingLeft == false && fo.isFlankingRight == false select fo).ToList();

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

    private bool isRightClear()
    {

        List<FightingObject> allFightingObjects = GameObject.FindObjectsOfType<FightingObject>().ToList();
        List<FightingObject> near = (from fo in allFightingObjects where fo != this && fo != player && Mathf.Abs(getDistance(fo)) < 2 && getDistance(fo) > 0 && fo.isFlankingLeft == false && fo.isFlankingRight == false select fo).ToList();

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

    private void flankLeft()
    {
        if (!isLeftClear() && Mathf.Abs(getDistance(player)) > fightingDistance && !isFlankingLeft && !isFlankingRight && getDistance(player) < 0 && !isFlankingRight || isFlankingLeft)
        {
            spriteRenderer.flipX = true;
            if (isFlankingLeft == false)
            {
                isFlankingLeft = true;
                StartCoroutine("stopFlanking");
            }
            gameObject.transform.position += new Vector3(-0.08f, 0, 0);
            animator.SetBool("walking", true);
            
            if (Mathf.Abs(getDistance(player)) > 4)
            {
                isFlankingLeft = false;
                if (getDistance(player) > 0)
                {
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }
            }
            
        }
        else if (isFlankingLeft && Mathf.Abs(getDistance(player)) < fightingDistance)
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

    private void flankRight()
    {
        if (!isRightClear() && Mathf.Abs(getDistance(player)) > fightingDistance && !isFlankingLeft && !isFlankingRight && getDistance(player) > 0 && !isFlankingLeft || isFlankingRight)
        {
            spriteRenderer.flipX = false;
            if (isFlankingRight == false)
            {
                isFlankingRight = true;
                StartCoroutine("stopFlanking");
            }
            gameObject.transform.position += new Vector3(0.08f, 0, 0);
            animator.SetBool("walking", true);

            if (Mathf.Abs(getDistance(player)) > 4)
            {
                isFlankingRight = false;
                if (getDistance(player) > 0)
                {
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }
            }

        }
        else if (isFlankingRight && Mathf.Abs(getDistance(player)) > fightingDistance)
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

    IEnumerator stopFlanking()
    {
        
        float seconds = Mathf.Abs(getDistance(player)) * 2 / (0.08f * 50);
        Debug.Log(seconds);
        yield return new WaitForSeconds(seconds);
        isFlankingLeft = false;
        isFlankingRight = false;
        if (getDistance(player) > 0)
        {
            spriteRenderer.flipX = false;
        } else
        {
            spriteRenderer.flipX = true;
        }
    }

    public virtual void bleed()
    {
        Debug.Log("Enemy bleeds");
        // music[0].GetComponent<AudioSource>().Play();
        soundManager.playSound(Sound.BASIC_ATTACK);
        blood.GetComponent<BloodObject>().startBleed(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.2f));
    }

    public float getDistance(FightingObject target)
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

    public virtual void die()
    {
        if (health <= 0)
        {
            DropLoot();
            Destroy(gameObject);
        }
    }
}
