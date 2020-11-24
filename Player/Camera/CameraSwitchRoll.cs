using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.Player.Camera
{
    public class CameraSwitchRoll : MonoBehaviour
    {
        private UnityEngine.Camera[] cameras;
        private uint id = 1;
        private uint Id
        {
            get { return this.id; }
            set { this.id = value % (uint) cameras.Length; }
        }

        private void SetCamera()
        {
            // Debug.Log(string.Format("Current Id: {0}", Id));
            // Debug.Log(string.Format("There is {0} cameras in scene.", cameras.Length));
            for ( int i = 0; i < cameras.Length; i++ )
            {
                cameras[i].gameObject.SetActive(i == this.Id);
            }
        }

        void Awake()
        {
            cameras = GetComponentsInChildren<UnityEngine.Camera>();
            SetCamera();
        }

        void Update()
        {
            // Debug.Log("Update...");
            if ( Input.GetKeyDown(KeyCode.RightArrow) ) { Id++; SetCamera(); }
            if ( Input.GetKeyDown(KeyCode.LeftArrow) ) { Id--; SetCamera(); }
        }
    }
}