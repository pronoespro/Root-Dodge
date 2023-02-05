using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Entity.Tracking
{
    [RequireComponent(typeof(TrackEntity))]
    public class EntityTrackingBase : MonoBehaviour
    {

        public Transform target;

        private TrackEntity tracking;

        private void Start()
        {
            tracking = GetComponent<TrackEntity>();
        }

        public virtual void Update()
        {
            target = tracking.GetTarget();
        }

    }
}