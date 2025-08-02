using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface ICastle
{
    public void UpgradeGateHouse();
    public void UpgradeLeftTower();
    public void UpgradeRightTower();
} 

public class Castle : MonoBehaviour, ICastle
{
    public GameObject upgradePanel;
    public SlotButton gateHouseButton;
    public SlotButton leftTowerButton;
    public SlotButton rightTowerButton;
    public TowerScript gateHouse;
    public TowerScript leftTower;
    public TowerScript rightTower;
    public GameObject door;
    public GameObject ruins;

    private Player player;
    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        upgradePanel.SetActive(false);
        leftTowerButton.gameObject.SetActive(false);
        rightTowerButton.gameObject.SetActive(false);
        gateHouse.gameObject.SetActive(false);
        leftTower.gameObject.SetActive(false);
        rightTower.gameObject.SetActive(false);
        door.SetActive(false);
        
        player = GameObject.Find("Player").GetComponent<Player>();
        soundManager = GameObject.Find("GameManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gateHouse.level > 0)
        {
            leftTowerButton.gameObject.SetActive(true);
            rightTowerButton.gameObject.SetActive(true);
            gateHouse.gameObject.SetActive(true);
            door.SetActive(true);
            ruins.SetActive(false);
        }

        if (leftTower.level > 0)
        {
            leftTower.gameObject.SetActive(true);
        }

        if (rightTower.level > 0)
        {
            rightTower.gameObject.SetActive(true);
        }
    }

    public void UpgradeGateHouse()
    {
        var currentLevel = gateHouse.level;
        var prices = gateHouse.price;
        var levelText  = gateHouseButton.levelText;
        var priceText = gateHouseButton.priceText;
        var gold = gateHouseButton.priceIcon;
        
        // Max level check
        if (currentLevel >= prices.Length)
        {
            Debug.Log($"{gateHouse} is already at max level.");
            return;
        }

        int price = prices[currentLevel];

        // Check gold
        if (player.goldAmount < price)
        {
            Debug.Log("Not enough gold.");
            return;
        }

        // Deduct gold and upgrade
        player.goldAmount -= price;
        currentLevel++;
        
        gateHouse.level = currentLevel;

        // Update UI texts
        levelText.text = currentLevel < prices.Length ? ToRoman(currentLevel + 1) : "MAX";
        priceText.text = currentLevel < prices.Length ? prices[currentLevel].ToString() : "";
        if (currentLevel == prices.Length)
        {
            gold.SetActive(false);
        }
        soundManager.PlaySoundEffect(Sound.Construction);
        gateHouse.Upgrade();
    }
    
    public void UpgradeRightTower()
    {
        var currentLevel = rightTower.level;
        var prices = rightTower.price;
        var levelText  = rightTowerButton.levelText;
        var priceText = rightTowerButton.priceText;
        var gold = rightTowerButton.priceIcon;
        
        // Max level check
        if (currentLevel >= prices.Length)
        {
            Debug.Log($"{rightTower} is already at max level.");
            return;
        }

        int price = prices[currentLevel];

        // Check gold
        if (player.goldAmount < price)
        {
            Debug.Log("Not enough gold.");
            return;
        }

        // Deduct gold and upgrade
        player.goldAmount -= price;
        currentLevel++;
        
        rightTower.level = currentLevel;

        // Update UI texts
        levelText.text = currentLevel < prices.Length ? ToRoman(currentLevel + 1) : "MAX";
        priceText.text = currentLevel < prices.Length ? prices[currentLevel].ToString() : "";
        if (currentLevel == prices.Length)
        {
            gold.SetActive(false);
        }
        soundManager.PlaySoundEffect(Sound.Construction);
        rightTower.Upgrade();
    }
    
    public void UpgradeLeftTower()
    {
        var currentLevel = leftTower.level;
        var prices = leftTower.price;
        var levelText  = leftTowerButton.levelText;
        var priceText = leftTowerButton.priceText;
        var gold = leftTowerButton.priceIcon;
        
        // Max level check
        if (currentLevel >= prices.Length)
        {
            Debug.Log($"{leftTower} is already at max level.");
            return;
        }

        int price = prices[currentLevel];

        // Check gold
        if (player.goldAmount < price)
        {
            Debug.Log("Not enough gold.");
            return;
        }

        // Deduct gold and upgrade
        player.goldAmount -= price;
        currentLevel++;
        
        leftTower.level = currentLevel;

        // Update UI texts
        levelText.text = currentLevel < prices.Length ? ToRoman(currentLevel + 1) : "MAX";
        priceText.text = currentLevel < prices.Length ? prices[currentLevel].ToString() : "";
        if (currentLevel == prices.Length)
        {
            gold.SetActive(false);
        }
        soundManager.PlaySoundEffect(Sound.Construction);
        leftTower.Upgrade();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            upgradePanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            upgradePanel.SetActive(false);
        }
    }
    
    private static string ToRoman(int number)
    {
        if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException(nameof(number), "insert value between 1 and 3999");
        if (number < 1) return string.Empty;            
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900); 
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);            
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        throw new InvalidOperationException("Impossible state reached");
    }
}
