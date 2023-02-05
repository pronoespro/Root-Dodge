using UnityEngine;

public class RootMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform tentacleDir;
    [Header("Root config")]
    [Range(0f, 3f)]
    public float moveSpeed;
    [Range(0, 20)]
    public float frecuency;
    [Range(0f, 0.10f)]
    public float amplitude;
    [Range(0, 360f)]
    public float angle;

    private void Start()
    {
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        tentacleDir.rotation = q;
    }
    void Update()
    {
        DirectionalMovement();
    }

    public void ResetPosition(Vector3 point)
    {
        transform.position = point;
    }
    private bool canMove = true;
    public void PauseMovement()
    {
        canMove = false;
    }

    public void ContinueMovement()
    {
        canMove = true;
    }

    void DirectionalMovement()
    {
        if (canMove)
        {
            Vector2 posTarget = transform.position + Vector3.right * moveSpeed * Time.deltaTime;
            posTarget = RotatePoint(posTarget, transform.position, angle);

            //float xOffset = Mathf.Sin(Time.time * frecuency) * amplitude;
            float yOffset;
            if (Random.Range(0, 2) == 0)
            {
                yOffset = Mathf.Cos(Time.time * frecuency) * amplitude;
            }
            else
            {
                yOffset = Mathf.Sin(Time.time * frecuency) * amplitude;
            }
            Vector2 sinFrecuencyVector = new Vector2(0, yOffset);
            sinFrecuencyVector = RotatePoint(sinFrecuencyVector, Vector2.zero, angle);
            posTarget = posTarget + sinFrecuencyVector;

            transform.position = posTarget;
        }
    }

    private Vector2 RotatePoint(Vector2 point, Vector2 pivot, float angle)
    {
        Vector2 dir = point - pivot;
        dir = Quaternion.Euler(0, 0, angle) * dir;
        point = dir + pivot;
        return point;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector2 targetDirection = transform.position + Vector3.right * 3;
        targetDirection = RotatePoint(targetDirection, transform.position, angle);
        Gizmos.DrawLine(transform.position, targetDirection);

        /* // Just Frecuency Debugging  
        Gizmos.color = Color.yellow;
        Vector2 gizmosTargetPattern = transform.right * 5;
        gizmosTargetPattern = RotatePoint(gizmosTargetPattern, transform.position, angle);

        float xOffset = Mathf.Sin(Time.time * frecuency) * amplitude;
        float yOffset = Mathf.Cos(Time.time * frecuency) * amplitude;
        Vector2 sinFrecuencyVector = new Vector2(xOffset, yOffset);
        sinFrecuencyVector = RotatePoint(sinFrecuencyVector, transform.position, angle);
        gizmosTargetPattern += sinFrecuencyVector;

        Gizmos.DrawLine(transform.position, gizmosTargetPattern);
        */
    }
}
