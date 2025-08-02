using System.Linq;
using UnityEngine;


public class TowerScript : MonoBehaviour
{
    public Player player;
    public int deltaTime = 5;
    public ArrowScript arrowPrefab;
    public int range = 15;
    public Target target;
    public int level;
    public int[] price;

    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer < deltaTime) return;
        
        var closestEnemy = GameObject.FindGameObjectsWithTag("Enemy")
            .OrderBy(e => Mathf.Abs(e.transform.position.x - transform.position.x))
            .FirstOrDefault();

        var spawnPosition = new Vector2(transform.position.x, transform.position.y + 5);

        var withinRange = false;

        if (target == Target.Player)
        {
            withinRange = Mathf.Abs(player.transform.position.x - transform.position.x) < range;
        }

        if (target == Target.Enemy && closestEnemy != null) // check if we found any enemy
        {
            withinRange = Mathf.Abs(closestEnemy.transform.position.x - transform.position.x) < range;
        }

        if (withinRange)
        {
            var arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
            arrow.target = target;
        }

        timer = 0f;
    }
    
    public void Upgrade()
    {
        if (deltaTime <= 1)
        {
            Debug.Log("Cannot be upgraded further.");
            return;
        }
        
        deltaTime -= 1;
        Debug.Log("Tower upgraded");
    }
}

public enum Target
{
    Player,
    Enemy
}