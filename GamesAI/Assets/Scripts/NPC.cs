using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GamesAI;
using UnityEngine;

namespace GamesAI
{
    public class NPC : CharacterControl
    {
        public float separation;
        private float sqrSeparation;

        protected override void Start()
        {
            base.Start();
            sqrSeparation = separation*separation;

        }

        protected Vector3 Separation(string tag, bool avoidPlayer)
        {
            IEnumerable<GameObject> others = GameObject.FindGameObjectsWithTag(tag);
            if (avoidPlayer)
            {
                others = others.Concat(GameObject.FindGameObjectsWithTag("Player"));
            }
            Vector3 force = Vector3.zero;
            var neighbors = 0;
            IEnumerable<Vector3> differences =
                from other in others
                where other != this
                let diff = other.GetComponent<Transform>().position - transform.position
                where diff.sqrMagnitude <= sqrSeparation
                select diff;
            foreach (Vector3 diff in differences)
            {
                force += diff;
                neighbors++;
            }
            if (neighbors > 0)
            {
                force /= neighbors * -1;
                force.Normalize();
                force *= separation;
            }

            return force;
        }
    }
}
