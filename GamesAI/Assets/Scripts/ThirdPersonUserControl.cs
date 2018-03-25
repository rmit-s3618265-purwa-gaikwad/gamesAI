using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace GamesAI
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        public GameObject ground;   // Used for click positioning
        public Camera cam;          // Used for both click positioning
        public Transform camTarget; // Target transform for camera
        
        private ThirdPersonCharacter m_Character;                       // A reference to the ThirdPersonCharacter on the object
        private Collider groundCollider;                                // Used for click positioning, cached here to avoid calling GetComponent<Collider>() every Update
        private readonly Queue<Vector3> targets = new Queue<Vector3>(); // List of targets to head towards
        private Vector3? lastTarget;                                    // Most recent click/current last target, used to prevent adding many targets too close together. Null when no targets exist
        
        private void Start()
        {
            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
            groundCollider = ground.GetComponent<Collider>();
        }


        private void Update()
        {
            camTarget.position = transform.position;
            if (Input.GetMouseButton(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (groundCollider.Raycast(ray, out hit, 1000))
                {
                    Vector3 target = hit.point;
                    if (!lastTarget.HasValue || ((target - lastTarget.Value).magnitude > 1))
                    {
                        targets.Enqueue(target);
                        lastTarget = target;
                    }
                }
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            if (targets.Count > 0)
            {
                Vector3 target = targets.Peek();
                Vector3 move = target - transform.position;
                float distance = move.magnitude;
                if (distance < m_Character.slowingRadius)
                {
                    if (targets.Count == 1 && distance < 0.01)
                    {
                        targets.Dequeue();
                        lastTarget = null;
                    }
                    else if (targets.Count > 1)
                    {
                        targets.Dequeue();
                    }
                }
                m_Character.Move(move);
            }
        }
    }
}
