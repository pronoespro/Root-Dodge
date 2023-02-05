using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Entity.Tracking
{
    public class TargetableEntity : MonoBehaviour
    {

        public TargetEntity entity;

        private bool trackingStarted;

        private void Start()
        {
            trackingStarted = true;
            EnableTracking();
        }

        private void OnEnable()
        {
            if (trackingStarted)
            {
                TrackingManager.instance.AddTarget(entity);
            }
        }

        private void OnDisable()
        {
            if (trackingStarted)
            {
                TrackingManager.instance.RemoveEntity(entity);
            }
        }

        private void OnDestroy()
        {
            if (trackingStarted)
            {
                TrackingManager.instance.RemoveEntity(entity);
            }
        }

        public void DisableTracking()
        {
            TrackingManager.instance.RemoveEntity(entity);
        }

        public void EnableTracking()
        {
            TrackingManager.instance.AddTarget(entity);
        }

    }
}