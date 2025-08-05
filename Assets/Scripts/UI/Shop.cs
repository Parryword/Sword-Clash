namespace UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;


    public class Shop : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI defenseText;
        [SerializeField]
        private TextMeshProUGUI attackText;
        [SerializeField]
        private int defenseLevel = 0;
        [SerializeField]
        private int attackLevel = 0;
        [SerializeField]
        private int[] defensePrices = {2, 3, 5 ,8};
        [SerializeField] 
        private int[] attackPrices = {2, 3, 5, 8};
        [SerializeField]
        private Button upgradeDefenseButton;
        [SerializeField]
        private Button upgradeAttackButton;

        // Start is called before the first frame update
        void Start()
        {
            upgradeDefenseButton.onClick.AddListener(UpgradeDefense);
            upgradeAttackButton.onClick.AddListener(UpgradeAttack);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void UpgradeDefense()
        {
            if (defenseLevel == defensePrices.Length)
                return;

            if (Globals.player.goldAmount < attackPrices[attackLevel])
                return;
        
            Globals.player.goldAmount -= defensePrices[defenseLevel++];
            Globals.player.defense++;
        
        }

        private void UpgradeAttack()
        {
            if (attackLevel == attackPrices.Length)
                return;

            if (Globals.player.goldAmount < attackPrices[attackLevel])
                return;
        
            Globals.player.goldAmount -= attackPrices[attackLevel++];
            Globals.player.damage+=2;
        
        }

        private void LateUpdate()
        {
            defenseText.text = defenseLevel.ToString();
            attackText.text = attackLevel.ToString();
        }
    }
}