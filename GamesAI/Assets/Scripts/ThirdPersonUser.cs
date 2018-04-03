using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace GamesAI
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ThirdPersonUser : Character
    {
        public GameObject ground;           // Used for click positioning
        public Camera cam;                  // Used for both click positioning
        public Transform camTarget;         // Target transform for camera
        private Collider groundCollider;    // Used for click positioning, cached here to avoid calling GetComponent<Collider>() every Update
        private Material material;

        protected override void Start()
        {
            base.Start();
            groundCollider = ground.GetComponent<Collider>();
            material = GetComponent<MeshRenderer>().material;
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
                    Vector3? last = targets.Last;
                    if (!last.HasValue || ((target - last.Value).magnitude > 1))
                    {
                        targets.Enqueue(target);
                    }
                }
            }
        }

        public override void Damage(float damage)
        {
            base.Damage(damage);
            if (health <= 0)
            {
                // TODO: End game
            }
            material.color = Color.Lerp(Color.black, Color.yellow, health/maxHealth);
        }
    }
}
