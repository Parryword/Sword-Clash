using UnityEngine;

[CreateAssetMenu(fileName = "Complex", menuName = "Scriptable/Complex")]
public class ComplexData : ScriptableObject
{
    [System.Serializable]
    public struct ComplexBuildingEntry
    {
        public BuildingData building;
        public Vector3 relativePosition;
        public Vector3 buttonPosition;
    }
    
    public string complexName;
    public ComplexBuildingEntry[] buildings;
}