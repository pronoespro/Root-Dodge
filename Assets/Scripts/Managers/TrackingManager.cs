using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Entity.Tracking
{

    [System.Serializable]
    public class TargetEntity
    {
        public string name;
        public string[] targetingTags;

        public Transform entity;

        public bool HasTag(string tag)
        {
            foreach(string targetTag in targetingTags)
            {
                if (targetTag == tag)
                {
                    return true;
                }
            }
            return false;
        }

    }

    public class TrackingManager : MonoBehaviour
    {

        #region Instancing
        public static TrackingManager instance;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;

        }
        #endregion

        public List<TargetEntity> targets;

        public void AddTarget(TargetEntity entity)
        {
            if (targets == null)
            {
                targets = new List<TargetEntity>();
            }
            if (entity!=null && !targets.Contains(entity))
            {
                targets.Add(entity);
            }
        }

        public void RemoveEntity(TargetEntity entity)
        {
            if (targets == null)
            {
                targets = new List<TargetEntity>();
            }else if (entity != null && targets.Contains(entity))
            {
                targets.Remove(entity);
            }
        }

        public List<TargetEntity> GetPosibleTargets(string[] tags)
        {
            List<TargetEntity> entities = new List<TargetEntity>();
            foreach (string tag in tags)
            {
                foreach (TargetEntity entity in targets)
                {
                    if (!entities.Contains(entity) && entity.HasTag(tag))
                    {
                        entities.Add(entity);
                    }
                }
            }
            return entities;
        }

    }

}