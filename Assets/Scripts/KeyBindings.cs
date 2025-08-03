using UnityEngine;

[CreateAssetMenu(fileName = "Keybindings", menuName = "Scriptable/Keybindings")]
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

