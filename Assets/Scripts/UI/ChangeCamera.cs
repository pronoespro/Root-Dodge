using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Cameras
{
    public class ChangeCamera : MonoBehaviour
    {

        public int camType;
        public CameraStateManager cam;

        public void OnTriggerEnter2D(Collider2D collision)
        {
            cam.ChangeCameraState(camType);
        }

    }
}