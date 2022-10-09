using Cinemachine;
using UnityEngine;

// MoveBehaviour inherits from GenericBehaviour. This class corresponds to basic walk and run behaviour, it is the default behaviour.
public class MoveBehaviour : GenericBehaviour
{
	public float walkSpeed = 0.15f;                 // Default walk speed.
	public float runSpeed = 1.0f;                   // Default run speed.
	public float sprintSpeed = 2.0f;                // Default sprint speed.
	public float speedDampTime = 0.1f;              // Default damp time to change the animations based on current speed.

	private Vector3 m_DefaultPosition = new Vector3(0, 1.5f, -3f);

	private float speed, speedSeeker;               // Moving speed.
	private int jumpBool;                           // Animator variable related to jumping.
	private int groundedBool;                       // Animator variable related to whether or not the player is on ground.
	private bool jump;                              // Boolean to determine whether or not the player started a jump.
	private bool isColliding;                       // Boolean to determine if the player has collided with an obstacle.

//	private Rigidbody _rigidbody;

	private Vector3 moveDir;


	public float moveSpeed =0;

	public bool isAttack = false;
	private bool isLock = false;
	private bool isSkill = false;
	public bool isKnockLock = false;
	private Transform lockTarget;

	public float m_Horizontal;
	public float m_Vertical;

	[HideInInspector]
	public CinemachineFreeLook m_thirdPersonOrbit;

	private string m_DefultInputAxisNameX = "Mouse X";
	private string m_DefultInputAxisNameY = "Mouse Y";

	[HideInInspector]
	public Transform playerCamera;

	private CinemachineVirtualCameraBase vcam;
	private CinemachineVirtualCamera AimVacam;
	private int PriorityBoostAmount = 10;
	private bool boosted = false;
	private CinemachineVirtualCamera m_TopRig;
	private CinemachineVirtualCamera m_MidRig;
	private CinemachineVirtualCamera m_BtmRig;
	private CinemachineOrbitalTransposer m_TopRigBody;

	private CinemachinePOV m_AimPOV;
	// Start is always called after any Awake functions.
	void Start()
	{
		// Subscribe and register this behaviour as the default behaviour.
		behaviourManager.SubscribeBehaviour(this);
		behaviourManager.RegisterDefaultBehaviour(this.behaviourCode);
		//_rigidbody = GetComponent<Rigidbody>();
		speedSeeker = runSpeed;
		playerCamera = behaviourManager.playerCamera;
		m_thirdPersonOrbit = behaviourManager.CameraControl.GetComponent<CinemachineFreeLook>();
		vcam = behaviourManager.CameraControl_Lock.GetComponent<CinemachineVirtualCameraBase>();
		AimVacam = behaviourManager.CameraControl_Aim.GetComponent<CinemachineVirtualCamera>();
		 m_TopRig = m_thirdPersonOrbit.GetRig(0);
		m_MidRig = m_thirdPersonOrbit.GetRig(1);
		m_BtmRig = m_thirdPersonOrbit.GetRig(2);
	 m_TopRigBody = m_TopRig.GetCinemachineComponent<CinemachineOrbitalTransposer>();
		//m_thirdPersonOrbit.m_YAxis.Value = 0.7f;
		//m_thirdPersonOrbit.m_YAxis.m_MaxValue = 0.8f;
		m_thirdPersonOrbit.m_YAxis.m_MinValue = 0.4f;
		m_AimPOV = AimVacam.GetCinemachineComponent<CinemachinePOV>();
	}

	public void SetCameraGruop()
    {
		behaviourManager.m_CameraGroup.transform.parent =null;
    }


	private void ReSetCamera()
    {
		m_thirdPersonOrbit.transform.rotation = Quaternion.identity;

	}

    // Update is used to set features regardless the active behaviour.
    void Update()
	{
        if (Input.GetKeyDown(KeyCode.N))
        {
			
        }
		if (isLock)
		{
			behaviourManager.m_LookAt.position = lockTarget.position;
			behaviourManager.m_LookAt.rotation = lockTarget.rotation;

			behaviourManager.m_Follow.LookAt(behaviourManager.m_LookAt);
		}
        if (AimVacam != null)
        {
            if (m_IsBowAim)
            {
                if (!boosted)
                {
					//m_AimPOV.m_VerticalAxis.Value = 0;
					//m_AimPOV.m_HorizontalAxis.Value = 0;
					AimVacam.Priority += PriorityBoostAmount;
					ReSetCamera();
					boosted = true;
                }
            }
            else if (boosted)
            {
				AimVacam.Priority -= PriorityBoostAmount;
				ReSetCamera();
				//m_AimPOV.m_VerticalAxis.Value = 0;
				//	m_AimPOV.m_HorizontalAxis.Value = 0;
				boosted = false;
            }
        }
        if (behaviourManager.aimCanvas != null)
        {
			behaviourManager.aimCanvas.gameObject.SetActive(boosted);
        }



    }
	private bool isRun = false;

	private bool m_IsBowAim;

	public void BowAimOn()
    {
		m_IsBowAim = true;

	}

	public void BowAimOff()
    {
		m_IsBowAim = false;
	}

	private void FixedUpdate()
    {
		//Debug.Log(m_TopRigBody.m_ZDamping);
    }

    // LocalFixedUpdate overrides the virtual function of the base class.
    public override void LocalFixedUpdate()
	{
		m_Horizontal = behaviourManager.GetH;
		m_Vertical = behaviourManager.GetV;
		// Call the basic movement manager.
		MovementManagement(behaviourManager.GetH, behaviourManager.GetV);

		// Call the jump manager.
	//	JumpManagement();
	}

	private string m_InputAxisNameX;
	private string m_InputAxixNameY;
	public void SetOnChat(Transform target)
    {
  //      if (!string.IsNullOrEmpty(m_thirdPersonOrbit.m_XAxis.m_InputAxisName))
  //      {
		//	m_InputAxisNameX = m_thirdPersonOrbit.m_XAxis.m_InputAxisName;
		//}
		//m_thirdPersonOrbit.m_XAxis.m_InputAxisName = null;
		//m_thirdPersonOrbit.LookAt = target;
	
    }

	public void UnLockOnChat()
    {
		//m_thirdPersonOrbit.LookAt = null;
  //      if (!string.IsNullOrEmpty(m_InputAxisNameX))
		//{
		//	m_thirdPersonOrbit.m_XAxis.m_InputAxisName = m_InputAxisNameX;

		//}
  //      else
  //      {
		//	m_thirdPersonOrbit.m_XAxis.m_InputAxisName = m_DefultInputAxisNameX;

		//}
		
	}

	public void SetLockLookAt(Transform target)
    {
		lockTarget = target;
		isLock = true;

        if (!string.IsNullOrEmpty(m_thirdPersonOrbit.m_XAxis.m_InputAxisName))
        {
            m_InputAxisNameX = m_thirdPersonOrbit.m_XAxis.m_InputAxisName;
            m_thirdPersonOrbit.m_XAxis.m_InputAxisName = null;
        }

        if (!string.IsNullOrEmpty(m_thirdPersonOrbit.m_YAxis.m_InputAxisName))
        {
            m_InputAxixNameY = m_thirdPersonOrbit.m_YAxis.m_InputAxisName;
            m_thirdPersonOrbit.m_YAxis.m_InputAxisName = null;

        }
		
        m_thirdPersonOrbit.m_YAxis.Value = 0.7f;
		m_thirdPersonOrbit.m_XAxis.Value = 0f;
		m_thirdPersonOrbit.m_YAxis.m_InputAxisValue = 0f;
        m_thirdPersonOrbit.m_XAxis.m_InputAxisValue = 0f;
        m_thirdPersonOrbit.LookAt = behaviourManager.m_LookAt;
        m_thirdPersonOrbit.m_BindingMode = CinemachineTransposer.BindingMode.LockToTargetWithWorldUp;
	}

	public void UnLockLookAt()
    {
		lockTarget = null;
		isLock = false;
     //   m_thirdPersonOrbit.transform.position = vcam.transform.position;
        m_thirdPersonOrbit.m_YAxis.Value = 0.7f;

        m_thirdPersonOrbit.LookAt = null;
        m_thirdPersonOrbit.m_BindingMode = CinemachineTransposer.BindingMode.LockToTargetOnAssign;
        if (!string.IsNullOrEmpty(m_InputAxisNameX))
        {
            m_thirdPersonOrbit.m_XAxis.m_InputAxisName = m_InputAxisNameX;

        }
        else
        {
            m_thirdPersonOrbit.m_XAxis.m_InputAxisName = m_DefultInputAxisNameX;

        }

        if (!string.IsNullOrEmpty(m_InputAxixNameY))
        {
            m_thirdPersonOrbit.m_YAxis.m_InputAxisName = m_InputAxixNameY;

        }
        else
        {
            m_thirdPersonOrbit.m_YAxis.m_InputAxisName = m_DefultInputAxisNameY;

        }


    }

    public void IsKnockLock(bool isLock)
    {
		isKnockLock = isLock;
	}

	public void IsSkill(bool m_isSkill)
    {
		isSkill = m_isSkill;
    }
	float m_VerticalSpeed;
	// Deal with the basic player movement
	void MovementManagement(float horizontal, float vertical)
	{

		if (isAttack)
        {
			return;
        }

		//speedDown.y -= Time.deltaTime * Physics.gravity.y;
	//	behaviourManager.GetRigidBody.Move(speedDown * Time.deltaTime);
			//behaviourManager.GetRigidBody.useGravity = true;

		// Avoid takeoff when reached a slope end.
		//else if (behaviourManager.GetRigidBody.velocity.y > 0)
		//{
		//	RemoveVerticalVelocity();
		//}

		if (!isLock && !isKnockLock && !isSkill && !m_IsBowAim)
        {
			Rotating(horizontal, vertical);
		}
        else
        {
            if (lockTarget != null)
            {

				Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);
				Quaternion targetRotation = Quaternion.LookRotation(forward);

				//Quaternion newRotation = Quaternion.Slerp(behaviourManager.GetRigidBody.rotation, targetRotation, behaviourManager.turnSmoothing);
				Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, behaviourManager.turnSmoothing);
				Quaternion myRotation = Quaternion.Euler(0, newRotation.eulerAngles.y, 0);
				//behaviourManager.GetRigidBody.MoveRotation(myRotation);
				transform.rotation = myRotation;
				behaviourManager.SetLastDirection(forward);
				//transform.LookAt(lockTarget);

			}
			else if (m_IsBowAim)
            {
				Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);
				Quaternion targetRotation = Quaternion.LookRotation(forward);

				//Quaternion newRotation = Quaternion.Slerp(behaviourManager.GetRigidBody.rotation, targetRotation, behaviourManager.turnSmoothing);
				Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, behaviourManager.turnSmoothing*2);
				Quaternion myRotation = Quaternion.Euler(0, newRotation.eulerAngles.y, 0);
				//behaviourManager.GetRigidBody.MoveRotation(myRotation);
				transform.rotation = myRotation;
				behaviourManager.SetLastDirection(forward);
			}
			
			
        }
		// Call function that deals with player orientation.
		

		// Set proper speed.
		//Vector2 dir = new Vector2(horizontal, vertical);

		//speed = Vector2.ClampMagnitude(dir, 1f).magnitude;
		// This is for PC only, gamepads control speed via analog stick.
		//speedSeeker += Input.GetAxis("Mouse ScrollWheel");
		//speedSeeker = Mathf.Clamp(speedSeeker, walkSpeed, runSpeed);
		//speed *= speedSeeker;
		//if (behaviourManager.IsSprinting())
		//{
		//	speed = sprintSpeed;
		//}

		Quaternion rot = Quaternion.Euler(0, behaviourManager.playerCamera.rotation.eulerAngles.y, 0);
        moveDir = Vector3.ClampMagnitude(rot * Vector3.forward * Input.GetAxis("Vertical") + rot * Vector3.right * Input.GetAxis("Horizontal"), 1f);
        //_rigidbody.MovePosition(_rigidbody.position+moveDir * Time.deltaTime*moveSpeed);
        //behaviourManager.GetAnim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
    }

    // Remove vertical rigidbody velocity.
    private void RemoveVerticalVelocity()
	{
		Vector3 horizontalVelocity = behaviourManager.GetRigidBody.velocity;
		horizontalVelocity.y = 0;
	//	behaviourManager.GetRigidBody.velocity = horizontalVelocity;
	}

	// Rotate the player to match correct orientation, according to camera and key pressed.
	Vector3 Rotating(float horizontal, float vertical)
	{
		// Get camera forward direction, without vertical component.
		Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);

		// Player is moving on ground, Y component of camera facing is not relevant.
		forward.y = 0.0f;
		forward = forward.normalized;

		// Calculate target direction based on camera forward and direction key.
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		Vector3 targetDirection;
		targetDirection = forward * vertical + right * horizontal;

		// Lerp current direction to calculated target direction.
		if ((behaviourManager.IsMoving() && targetDirection != Vector3.zero))
		{
			Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
		//	Quaternion newRotation = Quaternion.Slerp(behaviourManager.GetRigidBody.rotation, targetRotation, behaviourManager.turnSmoothing);
			Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, behaviourManager.turnSmoothing);
			//behaviourManager.GetRigidBody.MoveRotation(newRotation);
			transform.rotation = newRotation;
			behaviourManager.SetLastDirection(targetDirection);
		}
		// If idle, Ignore current camera facing and consider last moving direction.
		if (!(Mathf.Abs(horizontal) > 0.9 || Mathf.Abs(vertical) > 0.9))
		{
			behaviourManager.Repositioning();
		}

		return targetDirection;
	}

	public void AttackRotating()
    {
		float angle = behaviourManager.playerCamera.eulerAngles.y - transform.eulerAngles.y;
		Quaternion targetQuaternion = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0,
			  transform.localRotation.eulerAngles.y + angle, 0), 0.02f);
		transform.rotation = targetQuaternion;
		Vector3 forward = transform.TransformDirection(Vector3.forward);
		behaviourManager.SetLastDirection(forward);
	}

	// Collision detection.
	private void OnCollisionStay(Collision collision)
	{
		isColliding = true;
		// Slide on vertical obstacles
		if (behaviourManager.IsCurrentBehaviour(this.GetBehaviourCode()) && collision.GetContact(0).normal.y <= 0.1f)
		{
			GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
			GetComponent<CapsuleCollider>().material.staticFriction = 0f;
		}
	}
	private void OnCollisionExit(Collision collision)
	{
		isColliding = false;
		GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
		GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;
	}
}
