using UnityEngine;

namespace GS.Basics
{
    public abstract class CharacterMovement : MonoBehaviour
    {
        public abstract void Move(float _forward);

        public abstract void Strafe(float _strafeRight);

        public abstract void Turn(float _turnRight);

        public abstract void Jump(float _jump);

        public abstract void UpdateMovement();
    }
}