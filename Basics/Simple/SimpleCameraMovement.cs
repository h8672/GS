using System.Collections.Generic;
using UnityEngine;

namespace GS.Basics.Simple
{
    public class SimpleCameraMovement : CameraMovement
    {
        #region Position_Camera
        
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Vector3 cameraDistance = new Vector3(0f, 5f, -10f);
        
        private void InitCamera()
        {
            if ( cameraTransform == null )
            {
                cameraTransform = Camera.current.transform;
            }

            cameraTransform.localPosition = cameraDistance;
            cameraTransform.LookAt(transform);

            ResetRotation();
            ResetZoom();
        }
        
        #endregion // Position_Camera
        
        #region Rotate_Camera
    
        [SerializeField] private float rotationSpeed = 90f;
		private Vector2 angleCurrent = Vector2.zero;

		[SerializeField] private bool disableHorizontal = false, disableVertical = true;
		[SerializeField] private bool invertHorizontal = false, invertVertical = true;
		
        public override void RotateHorizontal(float _delta)
        {
            if ( disableHorizontal ) { return; }
            angleCurrent.x += _delta * rotationSpeed * Time.deltaTime * (invertHorizontal ? -1 : 1);
        }
        
        public override void RotateVertical(float _delta)
        {
            if ( disableVertical ) { return; }
            angleCurrent.y += _delta * rotationSpeed * Time.deltaTime * (invertVertical ? -1 : 1);
            angleCurrent.y = Mathf.Clamp(angleCurrent.y, -90f, 45f);
        }

        private void ResetRotation() {
            angleCurrent = Vector2.zero;
        }
        
        private void RotateUpdate()
        {
            transform.localRotation = Quaternion.Euler(0f, angleCurrent.x, angleCurrent.y);
        }
        
        #endregion // Rotate_Camera
        
        #region Zoom_Camera
        
        [SerializeField] private float minZoom = 0.3f, maxZoom = 2f;
        private float zoom = 1f;

        [SerializeField] private bool cameraClipsThroughTerrain = false;
        
        // Dolly zoom by changing the parent scale for same effect.
        public override void Zoom(float _delta)
        {
            // Note, zoom reduces the distance...
            zoom = Mathf.Clamp(zoom - _delta, minZoom, maxZoom);
            transform.localScale = Vector3.one * zoom;
        }

        private void CheckZoomCollision()
        {
            // Skips if camera is allowd to go through terrain.
            if (cameraClipsThroughTerrain) return;

            // Draws white line between camera stand and camera
            Debug.DrawLine (cameraTransform.position, transform.position);

            Ray ray = new Ray();
            RaycastHit hit;

            // Set ray values
            // Ray starts from camera stand
            ray.origin = transform.position;
            // Calculate direction, fixing see through floor by lowering the actual camera position.
            ray.direction = (cameraTransform.position + Vector3.down * 0.5f) - transform.position;
            float scale = zoom;

            // If ray collides, scale the distance down.
            if (Physics.Raycast (ray, out hit, -cameraTransform.localPosition.x)) {
                    if (hit.collider.gameObject.tag.Contains("Trigger")) return;

                    // Draws blue line between stand and ray hit point
                    Debug.DrawLine (transform.position, hit.point, Color.blue);
                    //Logs the collision data to console
                    print ("Hit point: " + hit.point.x + ", " + hit.point.y + ", " + hit.point.z + "\nDistance: " + hit.distance);

                    // Change floating scale and change its value linear curve, no sudden changes when hitting things.
                    if (hit.collider.tag != "Player" || hit.collider.tag != "MainCamera") {
                            scale = ((hit.distance) / -cameraTransform.localPosition.x);
                    } else { Debug.Log (string.Format("{0} tries collide with player or main camera", this.transform.name), this); }
            }
            // If ray doesn't collide, extend camera back to original position.
            else scale += Time.deltaTime;
            Zoom(scale);
        }

        private void ResetZoom() { zoom = 1f; }
        
        #endregion // Zoom_Camera
        
        #region Target_Camera

        public override void Target()
        {
            angleCurrent.x = 0f;
        }

        #endregion // Target_Camera
        
        void Awake()
        {
            InitCamera();
        }
        
        void Update()
        {
            // Rotate camera
            RotateUpdate();

            // If camera collides
            CheckZoomCollision();
        }
    }
}
