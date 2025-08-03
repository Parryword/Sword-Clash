using System;
using System.Collections.Generic;
using UnityEngine;

public class ComplexSpawner : MonoBehaviour
{
    public ComplexData complexData;
    public Transform complexParent;
    public GameObject buttonPrefab;

    private GameObject upgradePanel;
    private readonly List<GameObject> buildings = new();

    private void Start()
    {
        SpawnComplex();
        SpawnUpgradePanel();
    }

    private void SpawnComplex()
    {
        if (complexData == null)
        {
            Debug.LogError("No ComplexData assigned to ComplexSpawner.");
            return;
        }

        foreach (var entry in complexData.buildings)
        {
            if (entry.building == null || entry.building.prefab == null)
            {
                Debug.LogWarning($"BuildingData or prefab missing for an entry in {complexData.complexName}");
                continue;
            }

            // Instantiate the building
            var buildingInstance = Instantiate(
                entry.building.prefab,
                complexParent
            );

            buildingInstance.transform.localPosition = entry.relativePosition;
            buildingInstance.name = entry.building.buildingName;
            
            buildings.Add(buildingInstance);
        }

        Debug.Log($"{complexData.complexName} spawned successfully.");
    }

    private void SpawnUpgradePanel()
    {
        var panel = new GameObject();
        panel.transform.SetParent(complexParent);
        panel.transform.localPosition = new Vector3(0, 2, 0);

        foreach (var child in complexData.buildings)
        {
            var button = Instantiate(buttonPrefab, panel.transform);
            button.transform.localPosition = child.buttonPosition;
            
            var buttonScript = button.GetComponent<SlotButton>();
            buttonScript.SetIcon(child.building.buildingLevelData[0].icon);
            buttonScript.SetPrice(child.building.buildingLevelData[0].price);
            
            var building = buildings.Find(x => x.name == child.building.buildingName);
            var buildingScript = building.GetComponent<IUpgradeable>();

            if (buildingScript == null)
            {
                Debug.LogWarning($"Building {child.building.buildingName} does not implement IUpgradeable.");
                continue;
            }
            
            buttonScript.onClick.AddListener(() => buildingScript.Upgrade(buttonScript));
        }

        panel.SetActive(false);
        upgradePanel = panel;
    }

    public void ShowUpgradePanel(bool show)
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(show);
    }
}