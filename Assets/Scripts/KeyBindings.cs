using UnityEngine;

[CreateAssetMenu(fileName = "Keybindings", menuName = "Keybindings")]
public class KeyBindings : ScriptableObject
{
    [System.Serializable]
    public class KeyBindingCheck
    {
        public KeyBindingActions keyBindingAction;
        public KeyCode keyCode;
    }
    public KeyBindingCheck[] keyBindingChecks; 
}

