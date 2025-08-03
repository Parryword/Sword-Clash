using System;
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

    public void SetIcon(Sprite icon)
    {
        slotIcon.GetComponent<SpriteRenderer>().sprite = icon;
    }

    public void SetPrice(int price)
    {
        priceText.text = price.ToString();
    }

    public void SetLevel(int level, bool isMax = false)
    {
        if (isMax)
        {
            levelText.text = "MAX";
            return;
        }
        levelText.text = ToRoman(level);
    }

    public void DisablePrice()
    {
        priceText.gameObject.SetActive(false);
        priceIcon.SetActive(false);
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
}
