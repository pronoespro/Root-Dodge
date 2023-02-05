using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickRotation : MonoBehaviour
{

    [SerializeField] private float RotOverTime=1f;

    [SerializeField] private bool ResetOnEnable;
    private float initRot;

    private void Start()
    {
        initRot = transform.rotation.eulerAngles.z;
    }

    private void OnEnable()
    {
        if (ResetOnEnable)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, initRot);
        }
    }

    private void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 0, RotOverTime*Time.deltaTime);
    }

}
