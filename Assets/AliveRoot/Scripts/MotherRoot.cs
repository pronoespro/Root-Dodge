using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherRoot : MonoBehaviour
{
    public GameObject rootPrefab;
    public GameObject spriteGameObj;
    public Transform subSpriteGameObjA;
    public Transform subSpriteGameObjB;
    public int rootCount;
    

    [Header("Recommended range:")]
    public float minMotherRootRadio;
    public float maxMotherRootRadio;

    public float minMotherRootAmplitude;
    public float maxMotherRootAmplitude;
    public float minMotherRootFrecuency;
    public float maxMotherRootFrecuency;

    public float motherGrowthThreshold;

    [Header("Roots configuration")]
    public float maxRootsExpansionRadio;
    [Header("Recommended range:")]
    public float minMoveSpeed;
    public float maxMoveSpeed;
    [Header("Recommended range:")]
    public float minFrecuency;
    public float maxFrecuency;
    [Header("Recommended range:")]
    public float minAmplitude;
    public float maxAmplitude;


    private RootMovement rootMovement;
    private Tentacle[] roots;
    private bool canGrowth = true;
    

    void Start()
    {
        roots = new Tentacle[rootCount];
        for (int i = 0; i < rootCount; i++)
        {
            rootMovement = Instantiate(rootPrefab, transform.position, Quaternion.identity, transform).GetComponent<RootMovement>();

            rootMovement.moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
            rootMovement.frecuency = Random.Range(minFrecuency, maxFrecuency);
            rootMovement.amplitude = Random.Range(minAmplitude, maxAmplitude);
            rootMovement.angle = Random.Range(0f, 360f);
            roots[i] = rootMovement.GetComponent<Tentacle>();
            roots[i].maxRootRadio = maxRootsExpansionRadio;
        }
    }
    public float rotationVelocityA;
    private void Update()
    {
        GrowthCheck();
        if (canGrowth)
        {
            spriteGameObj.transform.localScale = Vector2.MoveTowards(spriteGameObj.transform.localScale, new Vector2(growthTarget, growthTarget), growthSpeed * Time.deltaTime);
        }

        subSpriteGameObjA.Rotate(0, 0, rotationVelocityA * Time.deltaTime);
        subSpriteGameObjB.Rotate(0, 0, -rotationVelocityA * Time.deltaTime);
    }

    public float growthSpeed = 5;
    public float growthTarget = 0;
    public int shoterRoot;
    private void GrowthCheck()
    {
        canGrowth = true;
        shoterRoot = 0;
        for (int i = 0; i < roots.Length; i++)
        {
            if (roots[i].DistanceTraveled < motherGrowthThreshold)
            {
                canGrowth = false;
            }

            if(roots[shoterRoot].DistanceTraveled > roots[i].DistanceTraveled)
            {
                shoterRoot = i;
            }
        }

        growthTarget = roots[shoterRoot].DistanceTraveled * 0.18f;
        growthTarget = Mathf.Clamp(growthTarget, minMotherRootRadio, 100f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRootsExpansionRadio);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minMotherRootRadio);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, motherGrowthThreshold);
    }
}
