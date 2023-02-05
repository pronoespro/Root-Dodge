using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{

    public Rigidbody2D rb; 
    public float rotOverTime;

    void Update()
    {

        if (rb != null)
        {
            rotOverTime = Mathf.Sign(rb.velocity.x) * Mathf.Abs(rotOverTime);
        }

        transform.rotation *= Quaternion.Euler(0, 0, rotOverTime);

    }
}
