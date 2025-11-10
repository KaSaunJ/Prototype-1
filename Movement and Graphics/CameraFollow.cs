using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      // player
    public float smoothSpeed = 5f; 
    public Vector3 offset = new Vector3(0, 0, -10); // fixed Z

    void LateUpdate()
    {
        if (target == null) return;

        // Always keep Z = -10
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, 0) + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
