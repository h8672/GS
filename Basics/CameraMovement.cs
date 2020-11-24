using UnityEngine;

namespace GS.Basics {
	public abstract class CameraMovement : MonoBehaviour
    {
        public abstract void RotateHorizontal(float _delta);
        public abstract void RotateVertical(float _delta);
        public abstract void Zoom(float _delta);

        public abstract void Target();
    }
}
