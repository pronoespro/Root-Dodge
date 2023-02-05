using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{

    public LayerMask collisionMask;
    public bool destroySelf = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collisionMask == (collisionMask | (1 << collision.gameObject.layer)))
        {
            if (destroySelf)
            {
                gameObject.SetActive(false);
            }
            else
            {
                collision.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collisionMask == (collisionMask | (1 << collision.gameObject.layer)))
        {
            if (destroySelf)
            {
                gameObject.SetActive(false);
            }
            else
            {
                collision.gameObject.SetActive(false);
            }
        }
    }

}
