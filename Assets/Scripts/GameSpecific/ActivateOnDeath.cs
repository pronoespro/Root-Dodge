using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnDeath : MonoBehaviour
{

    public GameObject deathAnim;

    public void Die()
    {
        deathAnim.SetActive(true);
    }

}
