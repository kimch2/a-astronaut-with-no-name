using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;
        public float yPosRestriction = - 1f;
        public float searchTargetRate = 0.5f;

        private float m_OffsetZ;
        private float m_NextTimeToSearch = 2f;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;

        // Use this for initialization
        private void Start()
        {
            m_LastTargetPosition = target.position;
            m_OffsetZ = (transform.position - target.position).z;
            transform.parent = null;
        }


        // Update is called once per frame
        private void Update()
        {
            if (!target) {
                FindPlayer ();
                return;
            }

            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward*m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

            Vector3 clampedPos = new Vector3 (newPos.x, Mathf.Clamp (newPos.y, yPosRestriction, Mathf.Infinity), newPos.z);
            
            transform.position = clampedPos;

            m_LastTargetPosition = target.position;
        }

        void FindPlayer () {
            if (m_NextTimeToSearch <= Time.time) {
                GameObject player = GameObject.FindGameObjectWithTag ("Player");

                if (player) {
                    target = player.transform;
                }

                m_NextTimeToSearch = Time.time + searchTargetRate;
            }
        }
    }
}
