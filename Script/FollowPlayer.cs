using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform Target;
    public Vector3 offset;
    public float smoothSpeed = 0.25f;
    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {        
        Vector3 desiredPosition = Target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
