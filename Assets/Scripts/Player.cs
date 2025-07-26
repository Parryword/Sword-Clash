using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using static GameManager;


public class Player : FightingObject
{
    public int goldAmount;
    public PlayerState playerState;
    public TextMeshProUGUI textFieldGold;
    public GameObject targetIndicator;
    public GameObject healthBar;
    public InputManager inputManager;
    public BandageScript bandage;
    public FightingObject[] enemies;
    public FightingObject lockedEnemy;
    
    private float lockedDistance;
    private int enemyIndex;
    private GameObject enemyObject;
    private StatsTextManager statsTextManager;
    private RectTransform healthBarRect;

    void Start()
    {
        keyDisabled = false;
        enemyObject = null;
        statsTextManager = GameObject.Find("GameManager").GetComponent<StatsTextManager>();
        healthBarRect = healthBar.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!keyDisabled)
            HandleKeys();

        if (lockedEnemy != null)
            lockedDistance = GetDistance(lockedEnemy);
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
            targetIndicator.transform.position = new Vector3(lockedEnemy.transform.position.x,
                lockedEnemy.transform.position.y + 3 + Mathf.Sin(Time.realtimeSinceStartup * 3) / 3,
                lockedEnemy.transform.position.z);
        }

        // RESIZES HEALTH BAR
        int width = 380 * health / maxHealth;
        healthBarRect.sizeDelta = new Vector2(width, 35);
    }

    private void LateUpdate()
    {
        textFieldGold.text = goldAmount.ToString();
        statsTextManager.updateText(health.ToString(),
            maxHealth.ToString(), damage.ToString(), defense.ToString(), level.ToString());
    }

    private void ScanEnemy()
    {
        int prev = 0;
        if (enemies != null)
            prev = enemies.Length;
        enemies = FindObjectsOfType<Enemy>();

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
        }
        else if (enemies.Length == 0)
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
            if (IsFighting())
            {
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
        if (IsFighting())
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
            if (!IsFighting())
            {
                spriteRenderer.flipX = false;
            }

            gameObject.transform.position += new Vector3(speed, 0, 0);
        }
    }

    private void WalkLeft()
    {
        animator.SetBool("walking", true);
        if (IsFighting())
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
            if (!IsFighting())
            {
                spriteRenderer.flipX = true;
            }
            
            gameObject.transform.position += new Vector3(-speed, 0, 0);
        }
    }

    private void RunRight()
    {
        animator.SetBool("running", true);
        spriteRenderer.flipX = false;
        gameObject.transform.position += new Vector3(speed * 1.5f, 0, 0);
    }

    private void RunLeft()
    {
        animator.SetBool("running", true);
        spriteRenderer.flipX = true;

        gameObject.transform.position += new Vector3(-speed * 1.5f, 0, 0);
    }

    private void Dash()
    {
        animationDisabled = true;
        //colliderBox.isTrigger = false;
        animator.SetTrigger("dashing");
    }

    public override void HitEnemy()
    {
        if (enemyObject != null)
        {
            enemyObject.GetComponent<FightingObject>().TakeDamage(damage);
            enemyObject = null;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (IsNotTooClose(collision))
            {
                enemyObject = collision.gameObject;
            }
            else
            {
                enemyObject = null;
            }
        }
    }

    private bool IsNotTooClose(Collider2D collision)
    {
        return spriteRenderer.flipX == false &&
            collision.gameObject.transform.position.x - gameObject.transform.position.x > 1 || spriteRenderer.flipX &&
            collision.gameObject.transform.position.x - gameObject.transform.position.x < -1;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemyObject = null;
    }

    private bool IsFighting()
    {
        if (Mathf.Abs(lockedDistance) < 10 && lockedEnemy != null)
        {
            animator.SetBool("fightingfoward", true);
            return true;
        }
        
        animator.SetBool("fightingfoward", false);
        return false;
    }

    public override void Bleed()
    {
        blood.GetComponent<Blood>()
            .StartBleed(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.2f));
    }

    protected override void Die()
    {
        if (health > 0) return;
        Destroy(gameObject);
        Time.timeScale = 0;
    }
}


public enum PlayerState
{
    IDLE,
    WALK_LEFT,
    WALK_RIGHT,
    DASH,
    RUN_LEFT,
    RUN_RIGHT,
    STUNNED
}