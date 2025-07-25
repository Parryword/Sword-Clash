using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using static GameManager;


public class Player : FightingObject
{
    // Player stats
    [SerializeField]
    public PlayerState playerState;
    FightingObject[] enemies;
    public FightingObject lockedEnemy;
    private float lockedDistance;
    public int enemyIndex;
    public int stage;
    public BandageScript bandage;
    public int goldAmount;

    // GUI
    [SerializeField]
    private TextMeshProUGUI textFieldGold;

    [SerializeField]
    private GameObject targetIndicator;
    [SerializeField]
    private GameObject healthBar;
    private InputManager inputManager;

    // Managers


    void Start()
    {
        maxHealth = 30;
        health = maxHealth;
        damage = 4;
        defense = 0;
        level = 1;
        stage = 1;
        enemyIndex = 0;

        keyDisabled = false;
        enemyObject = null;

        inputManager = GameObject.Find("GameManager").GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (!keyDisabled)
            HandleKeys();
        
        if (lockedEnemy != null)
            lockedDistance = lockedEnemy.transform.position.x - gameObject.transform.position.x;
    }

    private void FixedUpdate()
    {
        ScanEnemy();

        if (!animationDisabled)
            switch (playerState)
            {
                case PlayerState.IDLE: Idle(); break;
                case PlayerState.WALK_LEFT: WalkLeft(); break;
                case PlayerState.WALK_RIGHT: WalkRight(); break;
                case PlayerState.DASH: Dash(); break;
                case PlayerState.RUN_LEFT: RunLeft(); break;
                case PlayerState.RUN_RIGHT: RunRight(); break;
            }

        UpdatePolygonCollider2D();
        Die();

        // MOVES TARGET INDICATOR
        if (lockedEnemy != null)
        {
            targetIndicator.transform.position = new Vector3(lockedEnemy.transform.position.x, lockedEnemy.transform.position.y + 3 + Mathf.Sin(Time.realtimeSinceStartup * 3) / 3, lockedEnemy.transform.position.z);
        }

        // RESIZES HEALTH BAR
        int width = 380 * health / maxHealth;
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 35);
    }

    private void LateUpdate()
    {
        textFieldGold.text = goldAmount.ToString();
        GameObject.Find("GameManager").GetComponent<StatsTextManager>().updateText(health.ToString(), maxHealth.ToString(), damage.ToString(), defense.ToString(), level.ToString());
    }

    private void ScanEnemy()
    {
        int prev = 0;
        if (enemies != null)
            prev = enemies.Length;
        enemies = GameObject.FindObjectsOfType<FightingObject>();
        enemies = (from e in enemies where e.level == level && e.tag == "Enemy" select e).ToArray();

        int curr = enemies.Length;

        if (prev > curr && enemyIndex == curr && enemies.Length > 0)
        {
            targetIndicator.SetActive(true);
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

    private void ChangeFocus()
    {
        if (enemies.Length == 0) return;
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

    private void HandleKeys()
    {   
        if (inputManager.GetKeyDown(KeyBindingActions.Dash))
        {   
            if (IsFighting("left") || IsFighting("right")) {
                playerState = PlayerState.DASH;
                keyDisabled = true;
            }
        }
        else if (inputManager.GetKey(KeyBindingActions.WalkRight)) 
        {
            if (inputManager.GetKey(KeyBindingActions.Run))
            {
                playerState = PlayerState.RUN_RIGHT;
                return;
            }
            if (inputManager.GetKeyUp(KeyBindingActions.Run))
            {
                animator.SetBool("running", false);
            }

            playerState = PlayerState.WALK_RIGHT;
        }
        else if (inputManager.GetKey(KeyBindingActions.WalkLeft))
        {
            if (inputManager.GetKey(KeyBindingActions.Run))
            {
                playerState = PlayerState.RUN_LEFT;
                return;
            }
            if (inputManager.GetKeyUp(KeyBindingActions.Run))
            {
                animator.SetBool("running", false);
            }
            playerState = PlayerState.WALK_LEFT;
        }
        /*
        else if (playerState == PlayerState.WALK_LEFT || playerState == PlayerState.WALK_RIGHT && inputManager.GetKeyDown(KeyBindingActions.Run))
        {

        }*/
        /*
        else if (inputManager.GetKeyUp(KeyBindingActions.WalkRight) && playerState == PlayerState.WALK_RIGHT)
        {
            playerState = PlayerState.IDLE;
        }
        else if (inputManager.GetKeyUp(KeyBindingActions.WalkLeft) && playerState == PlayerState.WALK_LEFT)
        {
            playerState = PlayerState.IDLE;
        }*/
        else
        {
            playerState = PlayerState.IDLE;
        }

        if (inputManager.GetKeyDown(KeyBindingActions.Focus))
        {
            ChangeFocus();
        }
    }

    private void Idle()
    {
        animator.SetBool("walking", false);
        animator.SetBool("fighting", false);
        animator.SetBool("fightingfoward", false);
        animator.SetBool("running", false);
    }

    private void WalkRight()
    {
        animator.SetBool("walking", true);
        if (IsFighting("right"))
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
            if (!IsFighting("right"))
            {
                spriteRenderer.flipX = false;
            }
            gameObject.transform.position += new Vector3(horizontalSpeed, verticalSpeed, 0);
        }
    }

    private void WalkLeft()
    {
        animator.SetBool("walking", true);
        if (IsFighting("left"))
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
            if (!IsFighting("left"))
            {
                spriteRenderer.flipX = true;
            }
            gameObject.transform.position += new Vector3(-horizontalSpeed, verticalSpeed, 0);
        }
    }

    private void RunRight()
    {
        animator.SetBool("running", true);
        spriteRenderer.flipX = false;
            gameObject.transform.position += new Vector3(horizontalSpeed * 1.5f, verticalSpeed, 0);
        
    }

    private void RunLeft()
    {
        animator.SetBool("running", true);
        spriteRenderer.flipX = true;

            gameObject.transform.position += new Vector3(-horizontalSpeed * 1.5f, verticalSpeed, 0);
        
    }

    private void Dash()
    {
        animationDisabled = true;
        //colliderBox.isTrigger = false;
        animator.SetTrigger("dashing");
    }

    public override void HitEnemy ()
    {
        if (enemyObject != null)
        {
            Debug.Log(enemyObject.name);
            enemyObject.GetComponent<FightingObject>().Bleed();
            enemyObject.GetComponent<FightingObject>().health -= damage;
            enemyObject = null;
        } else
        {
            Debug.Log("There are no enemies nearby.");
        }
    }

    public new void OnTriggerStay2D(Collider2D collision)
    {   
        if (collision.tag == "Enemy")
        {
            if (spriteRenderer.flipX == false && collision.gameObject.transform.position.x - gameObject.transform.position.x > 1)
            {
                enemyObject = collision.gameObject;
            }
            else if (spriteRenderer.flipX == true && collision.gameObject.transform.position.x - gameObject.transform.position.x < 1)
            {
                enemyObject = collision.gameObject;
            }
            else
            {
                enemyObject = null;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemyObject = null;
    }

    private bool IsFighting(string direction)
    {
        if (Mathf.Abs(lockedDistance) < 10 && lockedEnemy != null)
        {
            animator.SetBool("fighting", false);
            animator.SetBool("fightingfoward", true);

            return true;
        }
        else
        {
            animator.SetBool("fighting", false);
            animator.SetBool("fightingfoward", false);
            return false;
        }          
    }

    public override void Bleed()
    {
        if (true)
        {
            blood.GetComponent<Blood>().StartBleed(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.2f));
        }
    }

    public override void Die()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            Time.timeScale = 0;
        }
    }
}


public enum PlayerState
{
    IDLE, WALK_LEFT, WALK_RIGHT, DASH, RUN_LEFT, RUN_RIGHT,
    STUNNED
}