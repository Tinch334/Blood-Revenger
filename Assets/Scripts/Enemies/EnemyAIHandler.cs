using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace EnemyAI
{
    public class EnemyAIHandler : MonoBehaviour
    {
        [Header("Player detection")]
        [SerializeField]
        private int detectionAngle;
        [SerializeField]
        private float detectionDistance;
        [SerializeField]
        private float verticalRaycastOffset;
        [SerializeField]
        private LayerMask playerLayer;//Player, Obstacles
        [Header("Patrol")]
        [SerializeField]
        private Transform[] patrolPoints;
        [SerializeField]
        private float pointDetectionDistance;//1

        
        //Pathfiding
        private NavMeshAgent agent;
        private EnemyStates state;
        private GameObject player;
        //Patroling
        private int currentPatrolPoint;
        private bool reversePath;


        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            state = EnemyStates.Patroling;
            player = GameObject.Find("Player");

            currentPatrolPoint = 0;
            reversePath = false;
            agent.SetDestination(patrolPoints[currentPatrolPoint].position);
        }


        void FixedUpdate()
        {
            Debug.Log(PlayerVisible());

            switch(state)
            {
                case EnemyStates.Patroling:
                    Patrol();

                    break;
                case EnemyStates.Chasing:
                    break;
                case EnemyStates.Searching:
                    break;
                case EnemyStates.Attacking:
                    break;
            }
        }


        private void Patrol()
        {
            Vector2 playerPos2D = new Vector2(transform.position.x, transform.position.z);
            Vector2 patrolPos2D = new Vector2(patrolPoints[currentPatrolPoint].position.x, patrolPoints[currentPatrolPoint].position.z);

            if (Vector2.Distance(playerPos2D, patrolPos2D) < pointDetectionDistance)
            {
                //Check whether we are going forwards or backwards through the path.
                currentPatrolPoint += (reversePath) ? -1 : 1;

                //Check whether we have reached the end of either the forwards or backwards path.
                if (currentPatrolPoint + 1 == patrolPoints.Length || currentPatrolPoint == 0)
                {
                    reversePath = !reversePath;
                }

                agent.SetDestination(patrolPoints[currentPatrolPoint].position);
            }
        }


        private bool PlayerVisible()
        {
            //Gets the angle between the player and the enemy.
            Vector3 targetDir = player.transform.position - transform.position;
            float angleToPlayer = Vector3.Angle(targetDir, transform.forward);

            Debug.DrawRay(transform.position, targetDir.normalized * detectionDistance, Color.blue);

            Vector3 raycastOrigin = new Vector3(transform.position.x, transform.position.y + verticalRaycastOffset, transform.position.z);

            if (angleToPlayer >= -detectionAngle && angleToPlayer <= detectionAngle)
            {
                RaycastHit hitInfo;
                bool hit = Physics.Raycast(raycastOrigin, targetDir, out hitInfo, detectionDistance, playerLayer);
                
                //Check we have line of sight with the player
                if (hit && hitInfo.transform.CompareTag("Player"))
                    return true;
            }

            return false;
        }
    }
}