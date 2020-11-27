using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCameraManager : MonoBehaviour
{
    [Header("Zoom")]
    public float distance = 5.0f;
    public Vector3 targetOffset;
    [Range(1, 10)]
    public float zoomRate = 1f;
    public float zoomDampening = 5.0f;
    private float currentDistance;
    private float desiredDistance;

    Vector3 position;
    Quaternion rotation;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;

    public Transform target;

    private void Start()
    {
        currentDistance = distance;
        //If there is no target, create a temporary target at 'distance' from the cameras current viewpoint
        if (!target)
        {
            GameObject go = new GameObject("Cam Target");
            go.transform.position = transform.position + (transform.forward * distance);
            target = go.transform;
        }

        position = transform.position;
        rotation = transform.rotation;
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
        //desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        // For smoothing of the zoom, lerp distance
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);

        // calculate position based on the new currentDistance 
        position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
        transform.position = position;
    }

}
