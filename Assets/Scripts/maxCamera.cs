using UnityEngine;
using System.Collections;


[AddComponentMenu("Camera-Control/3dsMax Camera Style")]
public class maxCamera : MonoBehaviour
{

    public ElementPlacingScript eps;
    public Transform target;
    public Transform targetShadow;
    public Vector3 targetOffset;
    public float distance = 5.0f;
    public float maxDistance = 20;
    public float minDistance = .6f;
    public float xSpeed = 200.0f;
    public float ySpeed = 200.0f;
    public int yMinLimit = -80;
    public int yMaxLimit = 80;
    public int zoomRate = 40;
    public float panSpeed = 0.3f;
    public float panSmooth = 5f;
    
    public float zoomDampening = 5.0f;

    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;

    public Transform targetForTrombone;
    private float initHeightAtDist;
    private bool dzEnabled;

    [Header("Pointers")]
    public Texture2D[] curs;
    public CursorMode cursorMode = CursorMode.Auto;
    private Vector2 spot = Vector2.zero;

    // recorded camera stuff
    public Vector3 RecTargetPos;
    public float RecDesiredDistance;
    public Quaternion RecdesiredRotation;

    public bool canEditCamera;

    public void SaveCamData()
    {
        RecTargetPos = targetShadow.position;
        RecDesiredDistance = desiredDistance;
        RecdesiredRotation = desiredRotation;
    }

    public void ReturnToSavePoint()
    {
        targetShadow.position = RecTargetPos;
        desiredDistance = RecDesiredDistance;
        desiredRotation = RecdesiredRotation;
    }

    // Calculate the frustum height at a given distance from the camera.
    float FrustumHeightAtDistance(float distance)
    {
        return 2.0f * distance * Mathf.Tan(GetComponent<Camera>().fieldOfView * 0.5f * Mathf.Deg2Rad);
    }

    // Calculate the FOV needed to get a given frustum height at a given distance.
    float FOVForHeightAndDistance(float height, float distance)
    {
        float result = 2.0f * Mathf.Atan(height * 0.5f / distance) * Mathf.Rad2Deg;
        return result;
    }
    // Start the dolly zoom effect.
    void StartDZ()
    {
        var distance = Vector3.Distance(transform.position, target.position);
        initHeightAtDist = FrustumHeightAtDistance(distance);
        dzEnabled = true;
    }

    // Turn dolly zoom off.
    void StopDZ()
    {
        dzEnabled = false;
    }

    void Start() 
    { 
        Init();
        SaveCamData();

    }

    void OnEnable() { Init(); }

    public void Init()
    {
        //If there is no target, create a temporary target at 'distance' from the cameras current viewpoint
        if (!target)
        {
            GameObject go = new GameObject("Cam Target");
            go.transform.position = transform.position + (transform.forward * distance);
            target = go.transform;
        }

        distance = Vector3.Distance(transform.position, target.position);
        currentDistance = distance;
        desiredDistance = distance;

        //be sure to grab the current rotations as starting points.
        position = transform.position;
        rotation = transform.rotation;
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;

        xDeg = Vector3.Angle(Vector3.right, transform.right);
        yDeg = Vector3.Angle(Vector3.up, transform.up);
    }

    private void Update()
    {
        if (eps.active)
            canEditCamera = true;
        else
            canEditCamera = false;
    }

    /*
     * Camera logic on LateUpdate to only update after all character movement logic has been handled. 
     */
    void LateUpdate()
    {
        if (canEditCamera)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftAlt))
                {
                    xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                    yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                    ////////OrbitAngle

                    //Clamp the vertical axis for the orbit
                    yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
                    // set camera rotation 
                    desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
                }
                /*else if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift))
                 {
                     desiredDistance -= (Input.GetAxis("Mouse Y")/10) * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
                 }*/
                else if (Input.GetMouseButton(0))
                {
                    // Grab the rotation of the camera so we can move in a psuedo local XY space
                    targetShadow.rotation = transform.rotation;
                    targetShadow.Translate(Vector3.right * -Input.GetAxis("Mouse X") * panSpeed);
                    targetShadow.Translate(transform.up * -Input.GetAxis("Mouse Y") * panSpeed, Space.World);


                }

                // Zoom effect
                if (Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
                }
            }

            // Trombone effect
            if (Input.GetKeyDown(KeyCode.LeftShift))
                StartDZ();
            else if (Input.GetKeyUp(KeyCode.LeftShift))
                StopDZ();


            if (Input.GetKey(KeyCode.A))
            {
                Cursor.SetCursor(curs[3], spot, cursorMode);
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    if (Input.GetKey(KeyCode.LeftAlt))
                    {
                        Cursor.SetCursor(curs[1], spot, cursorMode);
                    }
                    else if (Input.GetKey(KeyCode.LeftShift))
                    {
                        Cursor.SetCursor(curs[2], spot, cursorMode);

                    }
                    else
                    {
                        Cursor.SetCursor(curs[0], spot, cursorMode);
                    }
                }
                else
                {
                    Cursor.SetCursor(null, spot, cursorMode);
                }
            }
        }

/*        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            print("hit " + hitInfo.point);
        }

        if (Input.GetMouseButtonDown(2))
        {
            target.position = hitInfo.point;
        }*/

        // CTRL allows movement of camera
        

        if (dzEnabled)
        {
            // Measure the new distance and readjust the FOV accordingly.
            var currDistance = Vector3.Distance(transform.position, target.position);
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, FOVForHeightAndDistance(initHeightAtDist, currDistance), Time.deltaTime * zoomRate);
        }

        // Rotation efect
        currentRotation = transform.rotation;
        rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
        transform.rotation = rotation;

        

        //clamp the zoom min/max
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        // For smoothing of the zoom, lerp distance
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);

        // calculate position based on the new currentDistance
        target.position = Vector3.Lerp(target.position, targetShadow.position, Time.deltaTime * panSmooth);
        if (Vector3.Distance(target.position, targetShadow.position) < 0.005)
            target.position = targetShadow.position;

        position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
        transform.position = position;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}