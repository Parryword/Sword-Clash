using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SlotButton : MonoBehaviour
{
    public GameObject slotIcon;
    public GameObject priceIcon;
    public TextMeshPro levelText;
    public TextMeshPro priceText;
    public Color hoverColor = new(0.5f, 0.5f, 0.5f, 1);
    public Color pressedColor = new(0.3f, 0.3f, 0.3f, 1);
    public UnityEvent onClick;
    
    private Color originalColor;
    private bool isMouseOver;
    private SpriteRenderer sr;

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

    public void UpdateUI(string level, string price, bool isMaxLevel)
    {
        levelText.text = level;
        priceText.text = price;
        priceIcon.SetActive(!isMaxLevel);
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
        onClick.Invoke();
    }

    private void OnMouseUp()
    {
        sr.color = isMouseOver ? hoverColor : originalColor;
    }
}
