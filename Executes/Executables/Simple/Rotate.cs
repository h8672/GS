using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Executes.Executables
{
    public class Rotate : GS.Executes.Executable
    {
        [SerializeField]
        private Vector3 axis = Vector3.up;
        [SerializeField]
        private float speed = 90f;
        [SerializeField]
        private bool fixedUpdate = false;

        public override void Execute()
        {
            transform.Rotate(axis * speed * (fixedUpdate ? Time.fixedDeltaTime : Time.deltaTime));
        }
    }
}