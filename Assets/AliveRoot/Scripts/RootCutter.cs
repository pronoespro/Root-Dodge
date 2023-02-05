using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootCutter : MonoBehaviour
{
    Tentacle tentacleController;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        tentacleController = collision.gameObject.GetComponent<Tentacle>();
        if (tentacleController != null)
        {
            tentacleController.CutTentacle(collision.contacts[0].point);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        tentacleController = collision.gameObject.GetComponent<Tentacle>();
        if (tentacleController != null)
        {
            tentacleController.CutTentacle(collision.contacts[0].point);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
}
