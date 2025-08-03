using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "Scriptable/BuildingData")]
public class BuildingData : ScriptableObject
{
    [System.Serializable]
    public struct BuildingLevelData
    {
        public Sprite sprite;
        public Sprite icon;
        public string description;
        public int price;
    }
    
    public string buildingName;
    public BuildingLevelData[] buildingLevelData;
    [Header("Prefab")]
    public GameObject prefab;
    [Header("Unlock Requirements")]
    public BuildingData requiredBuilding;
    public int requiredBuildingLevel; 
}
