using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Mathematics;
using static GameManager;

/// <summary>
/// Extract some of this to Enemy.cs
/// </summary>
public abstract class FightingObject : AnimatableObject
{
    public int maxHealth, health, damage, defense, level;
    public GameObject blood;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public abstract void HitEnemy();

    protected abstract void Die();
    
    public virtual void Bleed()
    {
        Debug.Log("Enemy bleeds");
        GameObject.Find("GameManager").GetComponent<SoundManager>().PlaySoundEffect(Sound.BASIC_ATTACK);
        blood.GetComponent<Blood>().StartBleed(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.2f));
    }

    protected float GetDistance(FightingObject target)
    {
        return target.transform.position.x - gameObject.transform.position.x;
    }
}