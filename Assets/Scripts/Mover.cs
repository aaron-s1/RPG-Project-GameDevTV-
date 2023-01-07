using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] Transform target;
        
        NavMeshAgent navMeshAgent;
        Animator playerAnim;

        Ray lastRay;

        void Start() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            playerAnim = GetComponent<Animator>();
        }


        void Update() =>
            UpdateAnimator();
        

        public void MoveTo(Vector3 destination)
        {
            GetComponent<NavMeshAgent>().destination = destination;
            navMeshAgent.isStopped = false;
            // playerAgent.destination = destination;
        }

        public void StopMovement()
        {
            navMeshAgent.isStopped = true;
        }

        void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;

            playerAnim.SetFloat("forwardSpeed", speed);
        }
    }
}