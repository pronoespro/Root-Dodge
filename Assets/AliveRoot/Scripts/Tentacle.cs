using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    
    [Header("Config")]
    public float smoothSpeed;
    public float distanceBetweenTentaclePoints;
    public int minTentacleLength = 2;
    public int maxPointsCount;
    public int maxEdgeColliderPointsCount;
    public float maxRootRadio = 10;
    public float distanceThreshold;
    public float cutThreshold = 0.2f;
    

    [Header("References")]
    public Transform targetDir;
    public LineRenderer lineRend;
    public EdgeCollider2D edgeCollider;
    public RootMovement rootMovement;

    public int NodeCount { get { return segmentPosesList.Count; } }
    public float DistanceTraveled { get { return Vector3.Distance(initialPos, transform.position); } }
    private Vector2 initialPos;
    private List<Vector3> segmentPosesList;
    private List<Vector2> segmentVList;

    void Start()
    {
        segmentPosesList = new List<Vector3>();
        segmentVList = new List<Vector2>();
        initialPos = transform.position;

        for (int i = 0; i < minTentacleLength; i++)
        {
            segmentPosesList.Add(initialPos); // dummy
            segmentVList.Add(Vector3.zero); // dummy
        }

        lineRend.positionCount = segmentPosesList.Count;

        StartCoroutine(GrowthAnimation());
    }
    /*
    private void Update()
    {
        if (segmentPosesList.Count < maxPointsCount)
        {
            float distanceWithFirstPoint = Vector3.Distance(initialPos, transform.position);

            if (distanceWithFirstPoint > maxRootRadio)
            {
                rootMovement.PauseMovement();
                return;
            }
            else
            {
                rootMovement.ContinueMovement();
            }

            float distanceWithLastPoint = Vector3.Distance(initialPos, segmentPosesList[segmentPosesList.Count - 1]);
            float distanceBetweenFirstAndLastPoint = Vector3.Distance(segmentPosesList[segmentPosesList.Count - 1], transform.position);
            if (distanceWithLastPoint > distanceThreshold && distanceWithFirstPoint > distanceBetweenFirstAndLastPoint)
            {
                segmentPosesList.Add(initialPos);
                segmentVList.Add(Vector3.zero);
            }
        }

        segmentPosesList[0] = targetDir.position;
        for (int i = 1; i < segmentPosesList.Count; i++)
        {
            Vector3 segmentV = segmentVList[i];
            segmentPosesList[i] = Vector3.SmoothDamp(segmentPosesList[i], (segmentPosesList[i - 1] + (-targetDir.right) * distanceBetweenTentaclePoints), ref segmentV, smoothSpeed);

        }
        lineRend.positionCount = segmentPosesList.Count;

        lineRend.SetPositions(segmentPosesList.ToArray());
        edgeCollider.SetPoints(segmentPosesList.Select((v3) => (Vector2)(v3 - transform.position)).Take(maxEdgeColliderPointsCount).ToList());

    }
    */
    IEnumerator GrowthAnimation()
    {
        while (true)
        {
            if (segmentPosesList.Count < maxPointsCount)
            {
                float distanceWithFirstPoint = Vector3.Distance(initialPos, transform.position);

                if (distanceWithFirstPoint > maxRootRadio)
                {
                    rootMovement.PauseMovement();
                    yield return null;
                }
                else
                {
                    rootMovement.ContinueMovement();
                }

                float distanceWithLastPoint = Vector3.Distance(initialPos, segmentPosesList[segmentPosesList.Count - 1]);
                float distanceBetweenFirstAndLastPoint = Vector3.Distance(segmentPosesList[segmentPosesList.Count - 1], transform.position);
                if (distanceWithLastPoint > distanceThreshold && distanceWithFirstPoint > distanceBetweenFirstAndLastPoint)
                {
                    segmentPosesList.Add(initialPos);
                    segmentVList.Add(Vector3.zero);
                }
            }

            segmentPosesList[0] = targetDir.position;
            for (int i = 1; i < segmentPosesList.Count; i++)
            {
                Vector3 segmentV = segmentVList[i];
                segmentPosesList[i] = Vector3.SmoothDamp(segmentPosesList[i], (segmentPosesList[i - 1] + (-targetDir.right) * distanceBetweenTentaclePoints), ref segmentV, smoothSpeed);

            }
            lineRend.positionCount = segmentPosesList.Count;

            lineRend.SetPositions(segmentPosesList.ToArray());
            edgeCollider.SetPoints(segmentPosesList.Select((v3) => (Vector2)(v3 - transform.position)).Take(maxEdgeColliderPointsCount).ToList());

            yield return new WaitForSeconds(0.02f);
        }
    }
    
    public void CutTentacle(Vector3 point)
    {
        try
        {
            Vector3 closestPoint = (Vector3)edgeCollider.ClosestPoint(point) + transform.position;
            for (int i = segmentPosesList.Count - 1; i >= 0; i--)
            {
                if (Vector3.Distance(segmentPosesList[i], closestPoint) > cutThreshold)
                {
                    segmentPosesList = segmentPosesList.TakeLast(i).ToList();
                    rootMovement.ResetPosition(segmentPosesList[0]);
                    break;
                }
            }
        } catch 
        {
            Destroy(this.gameObject);
        }
    }
}