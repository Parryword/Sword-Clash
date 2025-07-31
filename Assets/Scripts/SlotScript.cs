using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotScript : MonoBehaviour
{
    public Color hoverColor = new(0.5f, 0.5f, 0.5f, 1);
    public Color pressedColor = new(0.3f, 0.3f, 0.3f, 1);
    public Castle upgrade;
    public BuildingType buildingType;
    
    private Color originalColor;
    private bool isMouseOver;
    private SpriteRenderer sr;
    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseEnter()
    {
        // Called when the mouse starts hovering over the collider
        sr.color = hoverColor;
        isMouseOver = true;
    }

    void OnMouseExit()
    {
        // Called when the mouse stops hovering
        sr.color = originalColor;
        isMouseOver = false;
    }


    private void OnMouseDown()
    {
        sr.color = pressedColor;
        upgrade.Upgrade(buildingType);
    }

    private void OnMouseUp()
    {
        sr.color = isMouseOver ? hoverColor : originalColor;
    }
}
