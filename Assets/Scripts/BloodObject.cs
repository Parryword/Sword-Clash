using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodObject : AnimatableObject
{
    
    public void startBleed(Vector2 target)
    {
        animator.SetTrigger("exit");
        gameObject.transform.position = new Vector2(target.x, target.y + 0.5f);


    }
    


}
