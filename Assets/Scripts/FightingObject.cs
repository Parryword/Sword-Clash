using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Mathematics;

/// <summary>
/// Extract some of this to Enemy.cs
/// </summary>
public abstract class FightingObject : AnimatableObject
{
    public int maxHealth, health, damage, defense, level;
    public float crit;
    public float speed;
    public GameObject blood;
    
    // Start is called before the first frame update
    protected void Start()
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
        blood.GetComponent<Blood>().StartBleed(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.2f));
    }

    public void TakeDamage(int value, bool isCrit = false)
    {   
        if (isCrit)
        {
            Bleed();
            Globals.soundManager.PlaySoundEffect(Sound.Bleed);
        }
        else
        {
            Globals.soundManager.PlaySoundEffect(Sound.BasicAttack);
        }
        health -= value;
        StartCoroutine(nameof(Colorize));
    }

    private IEnumerator Colorize()
    {
        spriteRenderer.color = new Color(1, 0.6f, 0.6f);
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = new Color(1, 1, 1);
    }

    protected float GetDistance(FightingObject target)
    {
        return target.transform.position.x - gameObject.transform.position.x;
    }

    protected float GetAbsoluteDistance(FightingObject target)
    {
        return Mathf.Abs(GetDistance(target));
    }
}