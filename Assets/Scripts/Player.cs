using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : FightingObject
{
    public InputManager inputManager;
    public int playerState;
    private int IDLE = 0, WALKRIGHT = 1, WALKLEFT = 2, DASH = 3;
    // Start is called before the first frame update
    GameObject[] enemies;
    public GameObject lockedEnemy;
    private float lockedDistance;
    public int enemyIndex;
    public GameObject targetIndicator;
    public GameObject healthBar;
    public int maxHealth;


    void Start()
    {
        inputManager = InputManager.instance;
        isBusy = false;
        enemyObject = null;
        maxHealth = 30;
        maxHealth = 30;
        health = maxHealth;

        damage = 4;
        enemyIndex = 0;
        
     
    }

    // Update is called once per frame
    void Update()
    {   
        if (!isBusy)
            handleKeys();
        
        if (lockedEnemy != null)
            lockedDistance = lockedEnemy.transform.position.x - gameObject.transform.position.x;
        
    }

    private void scanEnemy()
    {
        int prev = 0;
        if (enemies != null)
            prev = enemies.Length;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int curr = enemies.Length;

        if (prev > curr && enemyIndex == curr && enemies.Length > 0)
        {   

            enemyIndex = 0;
            lockedEnemy = enemies[enemyIndex];
        }
        else if (enemies.Length > 0)
        {
            targetIndicator.SetActive(true);
            lockedEnemy = enemies[enemyIndex];
        } else if (enemies.Length == 0)
        {   
       
            lockedEnemy = null;
            targetIndicator.SetActive(false);
        }

    }

    private void changeFocus()
    {
        if (enemies.Length - 1 > enemyIndex)
        {
            lockedEnemy = enemies[++enemyIndex];
        }
        else
        {
            enemyIndex = 0;
            lockedEnemy = enemies[enemyIndex];
        }
    }

   

    private void FixedUpdate()
    {
        scanEnemy();

        if (!isBusyFixed)
            switch (playerState)
            {
                case 0: idle(); break;
                case 1: walkright(); break;
                case 2: walkleft(); break;
                case 3: dash(); break;
            }

        UpdatePolygonCollider2D();
        die();

   

        // MOVES TARGET INDICATOR
        if (lockedEnemy != null)
        {
            targetIndicator.transform.position = new Vector3(lockedEnemy.transform.position.x, lockedEnemy.transform.position.y + 3 + Mathf.Sin(Time.realtimeSinceStartup * 3) / 3, lockedEnemy.transform.position.z);
        }


        // RESIZES HEALTH BAR
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(400 * ((300 / maxHealth) * health / 300), 40);
    
    }

    private void handleKeys()
    {   
        if (inputManager.GetKeyDown(KeyBindingActions.Dash))
        {   
            playerState = DASH;
            isBusy = true;
        }
        else if (inputManager.GetKey(KeyBindingActions.WalkRight)) {
            playerState =  WALKRIGHT;
        }
        else if (inputManager.GetKey(KeyBindingActions.WalkLeft))
        {
            playerState = WALKLEFT;
        }
        else if (inputManager.GetKeyUp(KeyBindingActions.WalkRight) && playerState == WALKRIGHT)
        {
            playerState = IDLE;
        }
        else if (inputManager.GetKeyUp(KeyBindingActions.WalkLeft) && playerState == WALKLEFT)
        {
            playerState = IDLE;
        }
        else
        {
            playerState = IDLE;
        }

        if (inputManager.GetKeyDown(KeyBindingActions.Focus))
        {
            changeFocus();
        }
    }

    private void idle()
    {
        animator.SetBool("walking", false);
        animator.SetBool("fighting", false);
        animator.SetBool("fightingfoward", false);
    }

    private void walkright ()
    {
        animator.SetBool("walking", true);
        if (isFighting("right"))
        {
            if (lockedDistance > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (lockedDistance < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
        if (inputManager.GetKey(KeyBindingActions.WalkRight))
        {
            if (!isFighting("right"))
            {
                spriteRenderer.flipX = false;
            }
            verticalSpeed = 0.1f;
            gameObject.transform.position += new Vector3(verticalSpeed, horizontalSpeed, 0);

        }
        
    }

    private void walkleft ()
    {
        animator.SetBool("walking", true);
        if (isFighting("left"))
        {
            if (lockedDistance > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (lockedDistance < 0)
            {
                spriteRenderer.flipX = true;
              
            }
        }
        if (inputManager.GetKey(KeyBindingActions.WalkLeft))
        {
            if (!isFighting("left"))
            {
                spriteRenderer.flipX = true;
            }
            verticalSpeed = -0.1f;
            gameObject.transform.position += new Vector3(verticalSpeed, horizontalSpeed, 0);
        }
    }

    private void dash()
    {
        isBusyFixed = true;
        //colliderBox.isTrigger = false;
        animator.SetTrigger("dashing");
       

    }

    public override void hitEnemy ()
    {
        //Debug.Log("Animation Event worked");
        //Debug.Log("Player:" + gameObject.name + " " + enemyObject.gameObject.name);
        if (enemyObject != null)
        {
            enemyObject.GetComponent<FightingObject>().bleed();
            enemyObject.GetComponent<FightingObject>().health -= damage;
            enemyObject = null;
        } else
        {
            Debug.Log("There are no enemies nearby.");
        }
        
    }

    public new void OnCollisionStay2D(Collision2D collision)
    {   
        if (spriteRenderer.flipX == false && collision.gameObject.transform.position.x - gameObject.transform.position.x > 1 )
        {
            enemyObject = collision.gameObject;
        }
        else if (spriteRenderer.flipX == true && collision.gameObject.transform.position.x - gameObject.transform.position.x < 1)
        {
            enemyObject = collision.gameObject;
        } else
        {
            enemyObject = null;
        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        enemyObject = null;
    }

    private bool isFighting(string direction)
    {
        if (Mathf.Abs(lockedDistance) < 10 && lockedEnemy != null)
        {
            if (direction == "right" && lockedDistance < 0)
            {
                animator.SetBool("fighting", true);
                animator.SetBool("fightingfoward", false);
            }
                
            else if (direction == "left" && lockedDistance > 0)
            {
                animator.SetBool("fighting", true);
                animator.SetBool("fightingfoward", false);
            }
                
            else if (direction == "right" && lockedDistance > 0)
            {
                animator.SetBool("fighting", false);
                animator.SetBool("fightingfoward", true);
            }
                
            else if (direction == "left" && lockedDistance < 0)
            {
                animator.SetBool("fighting", false);
                animator.SetBool("fightingfoward", true);
            }
                
            return true;
        }
        else
        {
            animator.SetBool("fighting", false);
            return false;
        }
            
    }

    public override void bleed()
    {
        if (true)
        {
            blood.GetComponent<BloodObject>().startBleed(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.2f));
        }
    }

    public override void die()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            Time.timeScale = 0;
            Debug.Log("YOU DIED!");
        }
    }


}
