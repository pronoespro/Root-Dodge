using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferDamage : MonoBehaviour
{

    public Transform damageParent;

    public void Hurt(int damage)
    {
        if (damageParent != null)
        {
            damageParent.SendMessage("Hurt", damage);
        }
    }

    public void RemoveFromResource(string data)
    {
        if (damageParent != null)
        {
            damageParent.SendMessage("RemoveFromResource", data);
        }
    }

}
