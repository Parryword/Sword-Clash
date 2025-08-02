using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class AnimatableObject : MonoBehaviour
{
    public string entityName;
    public bool keyDisabled;
    public bool animationDisabled;
    public Animator animator;
    public Rigidbody2D rigidBody;
    public PolygonCollider2D colliderBox;
    public SpriteRenderer spriteRenderer;
    
    private readonly List<Vector2> points = new();
    private readonly List<Vector2> simplifiedPoints = new();
    
    protected void UpdatePolygonCollider2D(float tolerance = 0.05f)
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
