using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billboardScript : MonoBehaviour
{
    public Camera cam;
    private Transform camT;
    public float dist;
    [Range(0.2f, 1)]
    public float scaleFactor = 0.5f;

    private void Start()
    {
        cam = Camera.main;
        camT = cam.GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + camT.forward);

        dist = Vector3.Distance(transform.position, camT.transform.position);
        
        transform.localScale = Vector3.one * (FrustumHeightAtDistance(dist)) / (50/scaleFactor);
    }

    float FrustumHeightAtDistance(float distance)
    {
        return 2.0f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
    }
}
