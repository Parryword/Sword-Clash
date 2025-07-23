using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEventDispatcher2D : MonoBehaviour
{
    private HashSet<Collider2D> previousHits = new HashSet<Collider2D>();

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

        HashSet<Collider2D> currentHits = new HashSet<Collider2D>();
        foreach (var hit in hits)
        {
            currentHits.Add(hit.collider);
            hit.collider.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver);
        }

        // MouseEnter
        foreach (var collider in currentHits)
        {
            if (!previousHits.Contains(collider))
                collider.SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
        }

        // MouseExit
        foreach (var collider in previousHits)
        {
            if (!currentHits.Contains(collider))
                collider.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
        }

        // MouseDown & MouseUp
        if (Input.GetMouseButtonDown(0))
        {
            foreach (var collider in currentHits)
                collider.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
        }
        if (Input.GetMouseButtonUp(0))
        {
            foreach (var collider in currentHits)
                collider.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
        }

        previousHits = currentHits;
    }
}
