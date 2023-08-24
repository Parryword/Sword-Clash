using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField]
    private float coef = 0.1f;
    [SerializeField]
    private float period = 1f;
    private float yPos;
    // Start is called before the first frame update
    void Start()
    {
        yPos = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        var v3 = gameObject.transform.position;
        v3.y = yPos + coef * Mathf.Sin(period * Time.realtimeSinceStartup);
        gameObject.transform.position = v3;
    }
}
