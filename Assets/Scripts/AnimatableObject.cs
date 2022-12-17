using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AnimatableObject : MonoBehaviour
{
    public string entityName;
    protected float horizontalSpeed;
    protected float verticalSpeed;
    public Animator animator;
    public Rigidbody2D rigidBody;
    public PolygonCollider2D colliderBox;
    public SpriteRenderer spriteRenderer;
    public bool isBusy;
    public bool isBusyFixed;
   
    protected void notBusy()
    {
        isBusy = false;
    }

    private List<Vector2> points = new List<Vector2>();
    private List<Vector2> simplifiedPoints = new List<Vector2>();
    public void UpdatePolygonCollider2D(float tolerance = 0.05f)
    {
        colliderBox.pathCount = spriteRenderer.sprite.GetPhysicsShapeCount();
        for (int i = 0; i < colliderBox.pathCount; i++)
        {
            spriteRenderer.sprite.GetPhysicsShape(i, points);
            LineUtility.Simplify(points, tolerance, simplifiedPoints);
            if (spriteRenderer.flipX == true)
            {
                colliderBox.SetPath(i, simplifiedPoints.Select((Vector2 point) => new Vector2(-point.x, point.y)).ToList());
            }
            else
            {
                colliderBox.SetPath(i, simplifiedPoints);
            }

        }

    }
}
