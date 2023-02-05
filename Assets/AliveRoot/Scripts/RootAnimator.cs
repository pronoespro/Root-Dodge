using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootAnimator : MonoBehaviour
{
    public Tentacle tentacleController;
    private Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        //UpdateTentacleConfig();
    }
}

