using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Entity.Tracking
{

    public enum TrackingType
    {
        none,
        closest,
        furthest,
        mostHealth,
        lowestHealth
    }

    public class TrackEntity : MonoBehaviour
    {

        [HideInInspector]
        public Transform target;
        public string[] targetTags;

        public TrackingType tracking;

        public float range=2,maxRange=10;

        private void Update()
        {
            if (target == null || Vector2.Distance(transform.position,target.position)>maxRange)
            {
                switch (tracking)
                {
                    default:
                        GetClosest();
                        break;
                    case TrackingType.closest:
                        GetClosest();
                        break;
                    case TrackingType.furthest:
                        GetFurthest();
                        break;
                }
            }
        }

        public void GetClosest()
        {
            List<TargetEntity> possibleTargets= TrackingManager.instance.GetPosibleTargets(targetTags);
            Transform targ=null;

            foreach(TargetEntity entity in possibleTargets)
            {
                if (entity != null && entity.entity!=null)
                {
                    if (targ == null)
                    {
                        if (Vector2.Distance(transform.position, entity.entity.position) < range)
                        {
                            targ = entity.entity;
                        }
                    }
                    else
                    {
                        if (targ != entity.entity)
                        {
                            if (Vector2.Distance(transform.position, entity.entity.position) < Vector2.Distance(transform.position, targ.position) && Vector2.Distance(transform.position, entity.entity.position) < range)
                            {
                                targ = entity.entity;
                            }
                        }
                    }
                }
            }
            target = targ;

        }

        public void GetFurthest()
        {
            List<TargetEntity> possibleTargets = TrackingManager.instance.GetPosibleTargets(targetTags);
            Transform targ = null;

            foreach (TargetEntity entity in possibleTargets)
            {
                if (targ == null && Vector2.Distance(transform.position, entity.entity.position) < range)
                {
                    targ = entity.entity;
                }
                else
                {
                    if (targ != entity.entity)
                    {
                        if (Vector2.Distance(transform.position, entity.entity.position) > Vector2.Distance(transform.position, targ.position))
                        {
                            targ = entity.entity;
                        }
                    }
                }
            }
            if (targ != target)
            {
                Debug.Log(targ.name);
            }
            target = targ;
        }

        public Transform GetTarget()
        {
            return target;
        }

    }
}