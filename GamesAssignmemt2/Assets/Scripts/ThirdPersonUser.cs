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
        public float zoomSensitivity;
		public float moveSpeed = 5f;

        private Collider groundCollider;    // Used for click positioning, cached here to avoid calling GetComponent<Collider>() every Update
        private Material material;

        protected override void Start()
        {
            base.Start();
            groundCollider = ground.GetComponent<Collider>();
            material = GetComponent<MeshRenderer>().material;
			targets = new Queue<Vector3>(); 
        }

        private void Update()
        {
            UpdateCamera();
			transform.Translate(moveSpeed * Input.GetAxis("Horizontal")*Time.deltaTime, 0f, moveSpeed * Input.GetAxis("Vertical")*Time.deltaTime);
			CurrentTarget = transform.position;
			/*
            if (Input.GetMouseButton(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (groundCollider.Raycast(ray, out hit, 1000))
                {
                    Vector3 target = hit.point;
                    List<Node> listNodes = GameManager.Instance.PathFinding.process(transform.position, target);
                    if (listNodes != null)
                    {
                        targets = new Queue<Vector3>(listNodes.Select(node => node.getGridWorldPos()));
                    }
                }
            }*/
        }

        private void UpdateCamera()
        {
            camTarget.position = transform.position;
            cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel")*zoomSensitivity;
        }

        public override void Damage(float damage)
        {
            base.Damage(damage);
            if (health <= 0)
            {
                GameManager.Instance.GameOver();
                material.color = Color.black;
            }
            else
            {
                material.color = Color.Lerp(Color.black, Color.yellow, health / maxHealth);
            }
        }
    }
}