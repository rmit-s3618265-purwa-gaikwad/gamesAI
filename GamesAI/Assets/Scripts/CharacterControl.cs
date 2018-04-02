using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamesAI
{
    [RequireComponent(typeof(Character))]
    public class CharacterControl : MonoBehaviour
    {
        protected Character m_Character; // A reference to the Character on the object
        protected LastQueue<Vector3> targets = new LastQueue<Vector3>(); // List of targets to head towards

        public Vector3? CurrentTarget => targets.Count > 0 ? new Vector3?(targets.Peek()) : null;

        protected virtual void Start()
        {
            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<Character>();
        }

        // Fixed update is called in sync with physics
        protected virtual void FixedUpdate()
        {
            if (targets.Count > 0)
            {
                Vector3 target = targets.Peek();
                Character.ArriveResult arrive = m_Character.Arrive(target);
                if (arrive.isSlowing)
                {
                    if ((targets.Count == 1) && (arrive.distance < 0.25))
                    {
                        targets.Dequeue();
                    }
                    else if (targets.Count > 1)
                    {
                        targets.Dequeue();
                    }
                }
                m_Character.Move(arrive.desiredVelocity);
            }
        }

    }
}