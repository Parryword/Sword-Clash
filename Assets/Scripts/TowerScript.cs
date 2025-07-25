using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{   
    public Player player;
    public int deltaTime = 5;
    public GameObject arrowPrefab;
    public int range = 15;
    public Target target;

    private float timer = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= deltaTime)
        {
            var spawnPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 5);
            if (Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) < range)
            {
                var arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
                var arrowScript = arrow.GetComponent<ArrowScript>();
                arrowScript.target = target;
            }
            timer = 0f;
        }
    }
}

public enum Target
{
    Player,
    Enemy
}
