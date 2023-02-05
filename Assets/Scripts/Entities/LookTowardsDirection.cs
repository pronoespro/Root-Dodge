using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LookTowardsDirection : MonoBehaviour
{

    public Rigidbody2D rb;
    public float desScale=1;
    public float minVel;
    public bool vertical, horizontal;

    private void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        if (rb == null)
        {
            return;
        }
        if (horizontal && Mathf.Abs(rb.velocity.x) > minVel)
        {
            transform.localScale = new Vector3(desScale * Mathf.Sign(rb.velocity.x), transform.localScale.y,transform.localScale.z);
        }
        if(vertical && Mathf.Abs(rb.velocity.y) > minVel)
        {
            transform.localScale = new Vector3(transform.localScale.x, desScale * Mathf.Sign(rb.velocity.y), transform.localScale.z);
        }
    }
}
