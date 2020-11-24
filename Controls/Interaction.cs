using System;
using System.Collections.Generic;
using UnityEngine;

namespace GS.Controls
{
    /// <summary>
    /// Interaction to go through interactable objects using layermask.
    /// </summary>
    public class Interaction : MonoBehaviour
    {
        public static readonly string ActionInteract = "Action_Interact";
        public static readonly string ActionSwitch = "Action_Switch";

        List<GameObject> uses = new List<GameObject>();
        private int usesFound = 0, currentUse = 0;
        void LateUpdate() { usesFound = 0; }
        void Update()
        {
            if (usesFound == 0) return;

            // Search all objects inside the sphere in Interactable layer. SphereCast works different way...
            Collider[] colliders = Physics.OverlapSphere(transform.position, 50f, LayerMask.GetMask("Interactable"), QueryTriggerInteraction.Ignore);
            GS.Controls.Interface.Interact usable;
            foreach (Collider collision in colliders)
            {
                usable = collision.GetComponent<GS.Controls.Interface.Interact>();
                if(usable != null)
                {
                    uses.Add(collision.gameObject);
                }
            }
        }

        // Add listeners when component is active.
        void OnEnable()
        {
            GS.Data.EventManager.StartListening(ActionInteract, OnInteract);
            GS.Data.EventManager.StartListening(ActionSwitch, OnSwitch);
        }
        // Remove listeners when component is deactivated.
        void OnDisable()
        {
            GS.Data.EventManager.StopListening(ActionInteract, OnInteract);
            GS.Data.EventManager.StopListening(ActionSwitch, OnSwitch);
        }

        // Triggerable methods
        private void OnInteract()
        {
            if(usesFound > 0 && uses.Count > 0)
            {
                if (currentUse < uses.Count)
                {
                    uses[currentUse].GetComponent<GS.Controls.Interface.Interact>().Interact();
                }
            }
        }
        private void OnSwitch()
        {
            if (usesFound > 1 && uses.Count > 1)
            {
                currentUse = (uses.Count < currentUse ? currentUse + 1 : 0);
            }
        }
    }
}
