using UnityEngine;

namespace GS.Basics.Advanced {
    [RequireComponent(typeof(Rigidbody))]
	public class AdvancedMovement : CharacterMovement {
		[SerializeField] private float moveSpeed = 10f;
		[SerializeField] private float turnSpeed = 90f;
		[SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float jumpReachTime = .5f;
        [SerializeField] private float jumpCooldown = .5f;
        [SerializeField] private bool resetJumpCooldownOnGround = true;

		private Rigidbody rb;

		private void Start()
		{
			rb = GetComponentInChildren<Rigidbody> ();
		}

		#region implemented abstract members of Movement

		public override void Move (float _forward)
		{
            rb.velocity += (transform.forward * _forward * moveSpeed * Time.fixedDeltaTime);
		}
		public override void Strafe (float _strafeRight)
		{
            rb.velocity += (transform.right * _strafeRight * moveSpeed * Time.fixedDeltaTime);
		}
		public override void Turn (float _turnRight)
		{
			transform.localEulerAngles += (Vector3.up * _turnRight * turnSpeed * Time.fixedDeltaTime);
		}

        private float jumpTimer = 0f;
        private float currentJumpheight = 0f;
        private float jumpMomentum = 0f;
        public override void Jump (float _jump)
		{
            // Simple jump, if pressed, jump.
            jumpMomentum = Mathf.Clamp(jumpMomentum / jumpReachTime * Time.deltaTime + _jump, 0f, 1f);
            if(_jump != 0f)
            {
                RaycastHit hit;
                if (Physics.Raycast(new Ray(transform.position, -transform.up), out hit, 10f))
                {
                    //Debug.Log ("Jump! " + rb.ClosestPointOnBounds (hit.collider.transform.localPosition));
                    if ( ( rb.ClosestPointOnBounds(transform.position).y - transform.position.y ) < 0.00001f )
                    {
                        if(resetJumpCooldownOnGround) { jumpTimer = 0f; }

                        if(jumpTimer <= Time.time)
                        {
                            currentJumpheight = 0f;
                            jumpMomentum = 0f;
                            jumpTimer = Time.time + jumpCooldown;
                            //Debug.Log ("Jump for real!!!");

                            // Current: Jump with intention to reach said height without reaching it...
                        }
                    }
                }
            }
		}

        public override void UpdateMovement()
        {
            if(currentJumpheight < jumpHeight * jumpMomentum)
            {
                float jumpThisFrame = (jumpHeight * Time.deltaTime - Physics.gravity.y * Time.deltaTime) / this.jumpReachTime;
                currentJumpheight += jumpThisFrame;
                transform.Translate(Vector3.up * (currentJumpheight < jumpHeight * jumpMomentum ? jumpThisFrame : currentJumpheight - jumpHeight));
            }
        }

        #endregion
    }
}
