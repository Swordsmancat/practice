using UnityEngine;
using DG.Tweening;
using System.Collections;

// This class corresponds to the 3rd person camera features.
public class ThirdPersonOrbitCamBasic : MonoBehaviour
{
    public Transform player;                                           // Player's reference.
    public Vector3 pivotOffset = new Vector3(0.0f, 1.7f, 0.0f);       // Offset to repoint the camera.
    public Vector3 camOffset = new Vector3(0.0f, 0f, -3.0f);           // Offset to relocate the camera related to the player position.
    public float smooth = 10f;                                         // Speed of camera responsiveness.
    public float horizontalAimingSpeed = 6f;                           // Horizontal turn speed.
    public float verticalAimingSpeed = 6f;                             // Vertical turn speed.
    public float maxVerticalAngle = 30f;                               // Camera max clamp angle. 
    public float minVerticalAngle = -60f;                              // Camera min clamp angle.
    public string XAxis = "Analog X";                                  // The default horizontal axis input name.
    public string YAxis = "Analog Y";                                  // The default vertical axis input name.
    public int aimFOV = 50;

    private float angleH = 0;                                          // Float to store camera horizontal angle related to mouse movement.
    private float angleV = 0;                                          // Float to store camera vertical angle related to mouse movement.
    private Transform cam;                                             // This transform.
    private Vector3 smoothPivotOffset;                                 // Camera current pivot offset on interpolation.
    private Vector3 smoothCamOffset;                                   // Camera current offset on interpolation.
    private Vector3 targetPivotOffset;                                 // Camera pivot offset target to iterpolate.
    private Vector3 targetCamOffset;                                   // Camera offset target to interpolate.
    private float defaultFOV;                                          // Default camera Field of View.
    private float targetFOV;                                           // Target camera Field of View.
    private float targetMaxVerticalAngle;                              // Custom camera max vertical clamp angle.
    private bool isCustomOffset;                                       // Boolean to determine whether or not a custom camera offset is being used.

    private bool isLock;
    private bool isAim;

    private bool isAccelerate;
    private bool isAccumulate;

    private Transform lockTarget;
    // Get the camera horizontal angle.
    public float GetH { get { return angleH; } }


    Quaternion camYRotation;
    Quaternion aimRotation;

    private float m_RotateSpeed = 50f;

    private Camera m_Camera;

    private bool isShake = false;

    private bool isOnChat = false;

    void Awake()
    {
        // Reference to the camera transform.
        cam = transform;
        m_Camera = cam.GetComponent<Camera>();
        // Set camera default position.
        cam.position = player.position + Quaternion.identity * pivotOffset + Quaternion.identity * camOffset;
        cam.rotation = Quaternion.identity;

        // Set up references and default values.
        smoothPivotOffset = pivotOffset;
        smoothCamOffset = camOffset;

        defaultFOV = m_Camera.fieldOfView;
        angleH = player.eulerAngles.y;

        ResetTargetOffsets();
        ResetFOV();
        ResetMaxVerticalAngle();
        transform.SetParent(player.parent);

        // Check for no vertical offset.
        if (camOffset.y > 0)
            Debug.LogWarning("Vertical Cam Offset (Y) will be ignored during collisions!\n" +
                "It is recommended to set all vertical offset in Pivot Offset.");
    }


    private void LateUpdate()
    {

        if (isOnChat)
        {
            return;
        }
        if(isShake)
        {
            return;
        }    

        if (!isLock)
        {
                    // Get mouse movement to orbit the camera.
        // Mouse:
        angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * horizontalAimingSpeed;
        angleV += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * verticalAimingSpeed;
        // Joystick:
        angleH += Mathf.Clamp(Input.GetAxis(XAxis), -1, 1) * 60 * horizontalAimingSpeed * Time.deltaTime;
        angleV += Mathf.Clamp(Input.GetAxis(YAxis), -1, 1) * 60 * verticalAimingSpeed * Time.deltaTime;

        // Set vertical movement limit.
        angleV = Mathf.Clamp(angleV, minVerticalAngle, targetMaxVerticalAngle);

        // Set camera orientation.
         camYRotation = Quaternion.Euler(0, angleH, 0);
         aimRotation = Quaternion.Euler(-angleV, angleH, 0);
         cam.rotation = aimRotation;
        }
        else
        {
            if (lockTarget != null)
            {
                var targetRotation = Quaternion.LookRotation(lockTarget.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_RotateSpeed * Time.deltaTime);

                angleH = transform.rotation.eulerAngles.y;
                angleV = transform.rotation.eulerAngles.x;


                // angleV = Mathf.Clamp(angleV, minVerticalAngle, targetMaxVerticalAngle);

                camYRotation = Quaternion.Euler(0, angleH, 0);
                aimRotation = Quaternion.Euler(angleV, angleH, 0);


            }

        }

        // Set FOV.
        //  m_Camera.fieldOfView = Mathf.Lerp(m_Camera.fieldOfView, targetFOV, Time.deltaTime);
        if (isAccelerate)
        {
            m_Camera.fieldOfView = Mathf.Lerp(m_Camera.fieldOfView, targetFOV + accelerateFOV, Time.deltaTime* accelerateFOVSpeed);
        }
        else
        {
            if (isAccumulate) return;
            m_Camera.fieldOfView = Mathf.Lerp(m_Camera.fieldOfView, targetFOV, Time.deltaTime* accelerateFOVSpeed);
        }
        // Test for collision with the environment based on current camera position.
        Vector3 baseTempPosition = player.position + camYRotation * targetPivotOffset;
        Vector3 noCollisionOffset = targetCamOffset;
        while (noCollisionOffset.magnitude >= 0.2f)
        {
            if (DoubleViewingPosCheck(baseTempPosition + aimRotation * noCollisionOffset))
                break;
            noCollisionOffset -= noCollisionOffset.normalized * 0.2f;
        }
        if (noCollisionOffset.magnitude < 0.2f)
            noCollisionOffset = Vector3.zero;

        // No intermediate position for custom offsets, go to 1st person.
        bool customOffsetCollision = isCustomOffset && noCollisionOffset.sqrMagnitude < targetCamOffset.sqrMagnitude;

        // Repostition the camera.
        smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, customOffsetCollision ? pivotOffset : targetPivotOffset, smooth * Time.deltaTime);
        smoothCamOffset = Vector3.Lerp(smoothCamOffset, customOffsetCollision ? Vector3.zero : noCollisionOffset, smooth * Time.deltaTime);
        if (!isLock && !isAim)
        {
            cam.position = player.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;
            ResetFOV();
        }
        else if (isLock)
        {
            // Vector3 tempPosition = player.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;
            Vector3 tempPosition = player.position + camYRotation * pivotOffset + aimRotation * camOffset;
            if (Vector3.Distance(cam.position, tempPosition) > 1f)
            {
                isSoomth = true;
            }
            if (isSoomth)
            {
                //cam.DOMove(tempPosition, 0.5f);
                // cam.DOMove(tempPosition, 0.5f).OnComplete(() => isSoomth = false);
                cam.position = Vector3.SmoothDamp(cam.position, tempPosition, ref velocity, smooth * Time.deltaTime);
            }
            if (Vector3.Distance(cam.position, tempPosition) < 0.01f)
            {
                isSoomth = false;
            }
        }
        else if (isAim)
        {
            Vector3 tempPosition = player.Find("AimPos").transform.position;
            if (Vector3.Distance(cam.position, tempPosition) > 0.2f)
            {
                isSoomth = true;
            }
            if (isSoomth)
            {
                cam.position = Vector3.SmoothDamp(cam.position, tempPosition, ref velocity, smooth * Time.deltaTime);
            }
            if (Vector3.Distance(cam.position, tempPosition) < 0.01f)
            {
                isSoomth = false;
            }
            if (cam.rotation.x < 0.1 && cam.rotation.x > -0.1)
            {
                player.transform.rotation = cam.rotation;
            }
            SetFOV(aimFOV);
        }



    }

    private float accelerateFOV = 10f;

    private float accelerateFOVSpeed = 5f;

    private bool isSoomth;

    private Vector3 velocity;


    public void SetOnChat(Transform target)
    {
        if (isOnChat)
        {
            return;
        }
        isOnChat = true;
        var targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_RotateSpeed * Time.deltaTime);

        var angleH = transform.rotation.eulerAngles.y;
        var angleV = transform.rotation.eulerAngles.x;

        var camYRotation = Quaternion.Euler(0, angleH, 0);
        var aimRotation = Quaternion.Euler(angleV, angleH, 0);
       // Vector3 tempPosition = target.position + camYRotation * pivotOffset + aimRotation * camOffset;
        Vector3 tempPosition = target.position + camYRotation * new Vector3(0,2,-2f) + aimRotation * Vector3.zero;
        cam.DOMove(tempPosition, 1f);
        //cam.position = Vector3.SmoothDamp(cam.position, tempPosition, ref velocity, smooth * Time.deltaTime);
    }

    public void UnLockOnChat()
    {
        isOnChat = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            DOShake(0.1f,0.1f,10);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            GameObject Npc = GameObject.FindGameObjectWithTag("NPC");
            if (Npc != null)
            {
                SetOnChat(Npc.transform);
            }
            else
            {
                Debug.Log("NPC is null");
            }
            
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            UnLockOnChat();
        }
    }

    public Transform Transformtest;
    public void DOShake(float duration =0.1f, float strength =0.1f, int vibrato =10,float random =90f , bool fadeOut = false)
    {
        if (!isShake)
        {
            isShake = true;
            m_Camera.DOShakePosition(duration, strength, vibrato, random, fadeOut).OnComplete(() => { isShake = false; });
        }
       
    }


    public void SetAccelerateOn()
    {
        isAccelerate = true;
    }

    public void SetAccelerateOff()
    {
        isAccelerate = false;
    }

    public void SetAccumulateOn()
    {
        accelerateFOV = -10;
        isAccelerate = true;
    }
    public void SetAccumulateOff()
    {
        accelerateFOV = 10;
        isAccelerate = false;
    }
    public void SetLockCamera(Transform target)
    {
        lockTarget = target;
        isLock = true;
    }

    public void UnLockCamera()
    {
        isLock = false;
        lockTarget = null;
    }

    public void SetAim()
    {
        isAim = true;
    }

    public void UnSetAim()
    {
        isAim = false;
    }

    // Set camera offsets to custom values.
    public void SetTargetOffsets(Vector3 newPivotOffset, Vector3 newCamOffset)
    {
        targetPivotOffset = newPivotOffset;
        targetCamOffset = newCamOffset;
        isCustomOffset = true;
    }

    // Reset camera offsets to default values.
    public void ResetTargetOffsets()
    {
        targetPivotOffset = pivotOffset;
        targetCamOffset = camOffset;
        isCustomOffset = false;
    }

    // Reset the camera vertical offset.
    public void ResetYCamOffset()
    {
        targetCamOffset.y = camOffset.y;
    }

    // Set camera vertical offset.
    public void SetYCamOffset(float y)
    {
        targetCamOffset.y = y;
    }

    // Set camera horizontal offset.
    public void SetXCamOffset(float x)
    {
        targetCamOffset.x = x;
    }

    // Set custom Field of View.
    public void SetFOV(float customFOV)
    {
        this.targetFOV = customFOV;
    }

    // Reset Field of View to default value.
    public void ResetFOV()
    {
        this.targetFOV = defaultFOV;
    }

    // Set max vertical camera rotation angle.
    public void SetMaxVerticalAngle(float angle)
    {
        this.targetMaxVerticalAngle = angle;
    }

    // Reset max vertical camera rotation angle to default value.
    public void ResetMaxVerticalAngle()
    {
        this.targetMaxVerticalAngle = maxVerticalAngle;
    }

    // Double check for collisions: concave objects doesn't detect hit from outside, so cast in both directions.
    bool DoubleViewingPosCheck(Vector3 checkPos)
    {
        return ViewingPosCheck(checkPos) && ReverseViewingPosCheck(checkPos);
    }

    // Check for collision from camera to player.
    bool ViewingPosCheck(Vector3 checkPos)
    {
        // Cast target and direction.
        Vector3 target = player.position + pivotOffset;
        Vector3 direction = target - checkPos;
        // If a raycast from the check position to the player hits something...
        if (Physics.SphereCast(checkPos, 0.2f, direction, out RaycastHit hit, direction.magnitude))
        {
            // ... if it is not the player...
            if(hit.transform.GetComponent<Collider>() != null)
            {
                if (hit.transform != player && !hit.transform.GetComponent<Collider>().isTrigger)
                {
                    // This position isn't appropriate.
                    return false;
                }
            }
        }
        // If we haven't hit anything or we've hit the player, this is an appropriate position.
        return true;
    }

    // Check for collision from player to camera.
    bool ReverseViewingPosCheck(Vector3 checkPos)
    {
        // Cast origin and direction.
        Vector3 origin = player.position + pivotOffset;
        Vector3 direction = checkPos - origin;
        if (Physics.SphereCast(origin, 0.2f, direction, out RaycastHit hit, direction.magnitude))
        {
            if(hit.transform.GetComponent<Collider>() != null)
            {
                if (hit.transform != player && hit.transform != transform && !hit.transform.GetComponent<Collider>().isTrigger)
                {
                    return false;
                }
            }
        }
        return true;
    }

    // Get camera magnitude.
    public float GetCurrentPivotMagnitude(Vector3 finalPivotOffset)
    {
        return Mathf.Abs((finalPivotOffset - smoothPivotOffset).magnitude);
    }
}
