using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject upgradePanel;
    public GameObject gateHouseSlot;
    public GameObject leftTowerSlot;
    public GameObject rightTowerSlot;
    public TextMeshPro gateHouseLevelText;
    public TextMeshPro leftTowerLevelText;
    public TextMeshPro rightTowerLevelText;
    public TextMeshPro gateHousePriceText;
    public TextMeshPro leftTowerPriceText;
    public TextMeshPro rightTowerPriceText;
    public GameObject gateHouseGold;
    public GameObject leftTowerGold;
    public GameObject rightTowerGold;
    public GameObject gateHouse;
    public GameObject leftTower;
    public GameObject rightTower;
    public GameObject door;
    public GameObject ruins;
    public Player player;
    public int leftTowerLevel;
    public int rightTowerLevel;
    public int gateHouseLevel;
    private readonly int[] leftTowerPrice = {3, 7, 15};
    private readonly int[] rightTowerPrice = {3, 7, 15};
    private readonly int[] gateHousePrice = {5, 12, 25};
    private SoundManager soundManager;


    // Start is called before the first frame update
    void Start()
    {
        upgradePanel.SetActive(false);
        leftTowerSlot.SetActive(false);
        rightTowerSlot.SetActive(false);
        gateHouse.SetActive(false);
        leftTower.SetActive(false);
        rightTower.SetActive(false);
        door.SetActive(false);
        soundManager = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gateHouseLevel > 0)
        {
            leftTowerSlot.SetActive(true);
            rightTowerSlot.SetActive(true);
            gateHouse.SetActive(true);
            door.SetActive(true);
            ruins.SetActive(false);
        }

        if (leftTowerLevel > 0)
        {
            leftTower.SetActive(true);
        }

        if (rightTowerLevel > 0)
        {
            rightTower.SetActive(true);
        }
    }

    public void Upgrade(BuildingType building)
    {
        int currentLevel;
        int[] prices;
        TextMeshPro levelText;
        TextMeshPro priceText;
        GameObject gold;

        // Select building data
        switch (building)
        {
            case BuildingType.GateHouse:
                currentLevel = gateHouseLevel;
                prices = gateHousePrice;
                levelText = gateHouseLevelText;
                priceText = gateHousePriceText;
                gold = gateHouseGold;
                break;
            case BuildingType.LeftTower:
                currentLevel = leftTowerLevel;
                prices = leftTowerPrice;
                levelText = leftTowerLevelText;
                priceText = leftTowerPriceText;
                gold = leftTowerGold;
                break;
            case BuildingType.RightTower:
                currentLevel = rightTowerLevel;
                prices = rightTowerPrice;
                levelText = rightTowerLevelText;
                priceText = rightTowerPriceText;
                gold = rightTowerGold;
                break;
            default:
                throw new InvalidOperationException("Unknown building type");
        }

        // Max level check
        if (currentLevel >= prices.Length)
        {
            Debug.Log($"{building} is already at max level.");
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

        // Update correct level variable
        switch (building)
        {
            case BuildingType.GateHouse: gateHouseLevel = currentLevel; break;
            case BuildingType.LeftTower: leftTowerLevel = currentLevel; break;
            case BuildingType.RightTower: rightTowerLevel = currentLevel; break;
        }

        // Update UI texts
        levelText.text = currentLevel < prices.Length ? ToRoman(currentLevel + 1) : "MAX";
        priceText.text = currentLevel < prices.Length ? prices[currentLevel].ToString() : "";
        if (currentLevel == prices.Length)
        {
            gold.SetActive(false);
        }
        soundManager.PlaySoundEffect(Sound.Construction);
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
}

public enum BuildingType
{
    GateHouse,
    LeftTower,
    RightTower
}
