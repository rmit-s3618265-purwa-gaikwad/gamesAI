using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamesAI
{
    [RequireComponent(typeof(MeshRenderer))]
    public class NPC : Character
    {
        public float groupSearchRadius;
        private float sqrGroupSearchRadius;
        public float separation;
        private float sqrSeparation;
        public Gradient HealthGradient;
        private Material material;

        protected override void Start()
        {
            base.Start();
            sqrSeparation = separation*separation;
            sqrGroupSearchRadius = groupSearchRadius*groupSearchRadius;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            material = GetComponent<MeshRenderer>().material;
            UpdateHealth();
        }

        protected Vector3 Separation(IEnumerable<GameObject> grouping)
        {
            return (from other in grouping
                where other != gameObject
                let diff = (other.transform.position - transform.position).IgnoreY()
                let sqrDistance = diff.sqrMagnitude
                where sqrDistance <= sqrSeparation
                select diff.normalized / sqrDistance).Sum(normalize: false)*separation*-1;
        }

        protected Vector3 Alignment(IEnumerable<GameObject> grouping)
        {
            return (from other in grouping
                where other != gameObject
                let dist = (other.transform.position - transform.position).IgnoreY()
                where dist.sqrMagnitude <= sqrGroupSearchRadius
                select other.GetComponent<Rigidbody>().velocity).Average(normalize: true);
        }

        protected Vector3 Cohesion(IEnumerable<GameObject> grouping)
        {
            Vector3 center = (from other in grouping
                where other != gameObject
                let dist = (other.transform.position - transform.position).IgnoreY()
                where dist.sqrMagnitude <= sqrGroupSearchRadius
                select other.transform.position.IgnoreY()).Average(normalize: false);
            return center - transform.position.IgnoreY();
        }

        protected void UpdateHealth()
        {
            material.color = HealthGradient.Evaluate(Mathf.Clamp01(health/maxHealth));
        }
    }
}