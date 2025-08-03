using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public int level;
    public int[] prices;
    public Sprite sprite;
    
    public virtual void Upgrade(SlotButton button)
    {
        if (level >= prices.Length)
        {
            Debug.Log($"{gameObject.name} is already at max level.");
            return;
        }

        int price = prices[level];

        if (Globals.player.goldAmount < price)
        {
            Debug.Log("Not enough gold.");
            return;
        }

        Globals.player.goldAmount -= price;
        level++;
        
        var isMaxLevel = level == prices.Length;
        button.SetPrice(price);
        button.SetLevel(level + 1, isMaxLevel);

        Globals.soundManager.PlaySoundEffect(Sound.Construction);
    }
}