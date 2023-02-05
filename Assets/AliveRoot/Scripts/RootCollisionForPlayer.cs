using PronoesPro.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootCollisionForPlayer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Root"))
        {
            Debug.Log("JUGADOR DAÑADO");
            GetComponent<Health>().Hurt(1);
        }
    }
}
