﻿using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

namespace GamesAI
{
    [RequireComponent(typeof(Character))]
    public class CharacterControl : MonoBehaviour
    {
        protected Character m_Character; // A reference to the Character on the object
        protected LastQueue<Vector3> targets = new LastQueue<Vector3>(); // List of targets to head towards
        // Most recent click/current last target, used to prevent adding many targets too close together. Null when no targets exist

        protected virtual void Start()
        {
            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<Character>();
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
                    if ((targets.Count == 1) && (distance < 0.01))
                    {
                        targets.Dequeue();
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