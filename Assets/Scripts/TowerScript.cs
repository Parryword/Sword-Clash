using System.Linq;
using UnityEngine;

public interface IUpgradeable
{
    public void Upgrade(SlotButton button);
}

public class TowerScript : Building, IUpgradeable
{
    public int deltaTime = 5;
    public int range = 15;
    public Target target;
    public ArrowScript arrowPrefab;

    private float timer = 0f;
    private 

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Instantiate()
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
            withinRange = Mathf.Abs(Globals.player.transform.position.x - transform.position.x) < range;
        }

        if (target == Target.Enemy && closestEnemy is not null) // check if we found any enemy
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

    public override void Upgrade(SlotButton button)
    {
        base.Upgrade(button);
                
        deltaTime -= 1;
        Debug.Log("Tower upgraded");
    }
}

public enum Target
{
    Player,
    Enemy
}