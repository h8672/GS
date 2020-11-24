using UnityEngine;
//using UnityEngine.Experimental.Input;

namespace GS.Player
{
    public class PlayerMovement : Basics.CharacterMovement
    {
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float turnSpeed = 180f;

		public bool InGround { get; private set; }		//3 meters		4 meters		5 meters
		[SerializeField] private float jumpHeight = 2f;	//3f o non		4f o non		5f
		[SerializeField] private float jumpFix = 2.8f;	//3.7f o 2.5f	4.3f o 2.2f		4.8f?
		[SerializeField] private int jumps = 1;
		private int jumpCounter = 0;

		private Rigidbody rb;

		private void Start()
		{
			rb = GetComponentInChildren<Rigidbody> ();
		}

		#region implemented abstract members of Movement

		public override void Move (float _forward)
		{
			rb.position += (transform.forward * _forward * moveSpeed * Time.deltaTime);
		}
		public override void Strafe (float _strafeRight)
		{
			rb.position += (transform.right * _strafeRight * moveSpeed * Time.deltaTime);
		}
		public override void Turn (float _turnRight)
		{
			transform.localEulerAngles += (Vector3.up * _turnRight * turnSpeed * Time.deltaTime);
		}
		public override void Jump (float _jump)
		{
			if( InGround && jumpCounter < jumps)
			{
				jumpCounter++;
				Debug.Log("Jump: Work in progress in Advanced Moving...");
				RaycastHit hit;
				if (Physics.Raycast(new Ray(transform.position, -transform.up), out hit, 10f))
				{
					Debug.Log ("Jump! " + rb.ClosestPointOnBounds (hit.collider.transform.localPosition));
					if (rb.ClosestPointOnBounds (transform.position).y - transform.position.y < 0.00001f)
					{
						Debug.Log ("Jump for real!!!");
						rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
						rb.velocity += -Physics.gravity * jumpHeight / jumpFix;
					}
				}
			}
		}

        public override void UpdateMovement()
        {
            Debug.Log("PlayerMovement has nothing to update.");
        }

        #endregion

        /*/
        void Update()
        {
			Debug.Log("testing");
            if ( PlayerInputs.Instance.Focused )
            {
				Debug.Log("testing Focused");
                // TODO test how other angles change the behaviour of this method...
                transform.Rotate(Vector3.up, Quaternion.Angle(
                    transform.rotation,
                    Camera.PivotPoint.context.transform.rotation)
                );
            }
            else
            {
                if ( PlayerInputs.Instance.Turning )
				{
					Turn(PlayerInputs.Instance.turnRight ? 1f : -1f);
					Debug.Log("turning");
				}
            }
            if ( PlayerInputs.Instance.Moving )
			{
				Move((PlayerInputs.Instance.forward ? 1f : -1f));
				Debug.Log("moving");
			}
            if ( PlayerInputs.Instance.Strafing )
			{
				Strafe(PlayerInputs.Instance.strafeRight ? 1f : -1f);
				Debug.Log("strafing");
			}
            if ( PlayerInputs.Instance.Jumping )
			{
				Debug.Log("jumping");
				Jump();
				InGround = false;
			}
			if( InGround && jumpCounter > 0 ) jumpCounter = 0;
        }

		//void LateUpdate() {}

		private void OnCollisionStay(Collision collision)
		{
			if( !InGround ) InGround = true;
			foreach (ContactPoint contact in collision.contacts)
		    {
		        Debug.DrawRay(contact.point, contact.normal, Color.white);
		    }
		}

        void OnEnable()
        {
			
            //if ( PlayerInputs.Instance == null ) Debug.Log("Didn't find instance!");
            //PlayerInputs.Instance.controls.Enable();
        }
        void OnDisable()
        {
            //PlayerInputs.Instance.controls.Disable();
        }
        // */
    }
}
