using UnityEngine;

namespace GS.Basics.Simple {
	public class SimpleMovement : CharacterMovement {
		[SerializeField]
		private float moveSpeed = 10f;
		[SerializeField]
		private float turnSpeed = 90f;

		#region implemented abstract members of Movement

		public override void Move (float _forward)
		{
			transform.localPosition += (transform.forward * _forward * moveSpeed * Time.fixedDeltaTime);
		}
		public override void Strafe (float _strafeRight)
		{
			transform.localPosition += (transform.right * _strafeRight * moveSpeed * Time.fixedDeltaTime);
		}
		public override void Turn (float _turnRight)
		{
			transform.localEulerAngles += (Vector3.up * _turnRight * turnSpeed * Time.fixedDeltaTime);
		}
		public override void Jump (float _jump)
		{
			Debug.Log ("SimpleMovement doesn't have Jump action as it requires Rigidbody to make it useful", this);
        }
        public override void UpdateMovement()
        {
            Debug.Log("SimpleMovement has nothing to update");
        }

        #endregion
    }
}
