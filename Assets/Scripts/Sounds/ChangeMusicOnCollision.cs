using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicOnCollision : MonoBehaviour
{

    public string musicTrack;
    public LayerMask collisionMask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collisionMask == (collisionMask | (1 << collision.gameObject.layer)))
        {
            MusicManager.instance.ChangeTrack(musicTrack);
        }
    }

}
