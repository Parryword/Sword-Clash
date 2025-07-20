using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private KeyBindings keyBindings;

    public KeyCode GetKeyForAction(KeyBindingActions keyBindingAction)
    {
        foreach (KeyBindings.KeyBindingCheck keyBindingCheck in keyBindings.keyBindingChecks)
        {
            if (keyBindingCheck.keyBindingAction == keyBindingAction)
            {
                return keyBindingCheck.keyCode;
            }
        }
        return KeyCode.None;
    }

    public bool GetKeyDown(KeyBindingActions key)
    {
        foreach (KeyBindings.KeyBindingCheck keyBindingCheck in keyBindings.keyBindingChecks)
        {
            if (keyBindingCheck.keyBindingAction == key)
            {
                return Input.GetKeyDown(keyBindingCheck.keyCode);
            }
        }
        return false;
    }

    public bool GetKey(KeyBindingActions key)
    {
        foreach (KeyBindings.KeyBindingCheck keyBindingCheck in keyBindings.keyBindingChecks)
        {
            if (keyBindingCheck.keyBindingAction == key)
            {
                return Input.GetKey(keyBindingCheck.keyCode);
            }
        }
        return false;
    }

    public bool GetKeyUp(KeyBindingActions key)
    {
        foreach (KeyBindings.KeyBindingCheck keyBindingCheck in keyBindings.keyBindingChecks)
        {
            if (keyBindingCheck.keyBindingAction == key)
            {
                return Input.GetKeyUp(keyBindingCheck.keyCode);
            }
        }
        return false;
    }
}
