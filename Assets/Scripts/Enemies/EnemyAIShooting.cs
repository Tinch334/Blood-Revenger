using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace EnemyAI
{
    public class EnemyAIShooting : MonoBehaviour
    {
        [Header("Player detection")]
        [SerializeField]
        private int detectionAngle;//50
        [SerializeField]
        private float detectionDistance;//15
        [SerializeField]
        private float verticalRaycastOffset;//1
        [SerializeField]
        private LayerMask playerLayer;//Player, Obstacles

        [Header("Patrol")]
        [SerializeField]
        private Transform[] patrolPoints;
        [SerializeField]
        private float pointDetectionDistance;//1

        [Header("Speeds")]
        [SerializeField]
        private float normalSpeed;//3.5
        [SerializeField]
        private float normalAcceleration;//8
        [SerializeField]
        private float normalAngularVelocity;//250
        [SerializeField]
        private float chaseSpeed;
        [SerializeField]
        private float chaseAcceleration;
        [SerializeField]
        private float chaseAngularVelocity;//350

        [Header("Chase")]
        [SerializeField]
        private float playerForgetTime;

        [Header("Attack")]
        [SerializeField]
        private float attackCooldown;
        [SerializeField]
        private float attackDistance;

        //AI
        private bool alive;
        //Pathfiding
        private NavMeshAgent agent;
        private EnemyStates state;
        private GameObject player;
        //Patroling
        private int currentPatrolPoint;
        private bool reversePath;
        //Searching
        private Vector3 rememberedPlayerPos;
        private float currentForgetTime;
        private bool didCheck;


        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            state = EnemyStates.Patroling;
            player = GameObject.Find("Player");

            alive = true;

            currentPatrolPoint = 0;
            reversePath = false;
            didCheck = false;
            agent.SetDestination(patrolPoints[currentPatrolPoint].position);
        }


        void FixedUpdate()
        {
            //If the enemy is dead we disable the navmesh agent and return.
            if (!alive)
            {
                agent.enabled = false;

                return;
            }

            if (PlayerVisible())
            {
                state = EnemyStates.Attacking;

                currentForgetTime = 0;
            }
            else
            {
                if (state != EnemyStates.Patroling)
                {
                    if (currentForgetTime >= playerForgetTime)
                    {
                        state = EnemyStates.Searching;
                    }
                    else
                    {
                        currentForgetTime += Time.deltaTime;
                        rememberedPlayerPos = player.transform.position;
                    }
                }
            }

            switch (state)
            {
                case EnemyStates.Patroling:
                    Patrol();
                    break;

                case EnemyStates.Attacking:
                    TryAttack();
                    break;
            }
        }


        private void Patrol()
        {
            Vector2 playerPos2D = new Vector2(transform.position.x, transform.position.z);
            Vector2 patrolPos2D = new Vector2(patrolPoints[currentPatrolPoint].position.x, patrolPoints[currentPatrolPoint].position.z);

            setAgentParams(normalSpeed, normalAngularVelocity, normalAcceleration);


            if (Vector2.Distance(playerPos2D, patrolPos2D) < pointDetectionDistance)
            {
                //Check whether we are going forwards or backwards through the path.
                currentPatrolPoint += (reversePath) ? -1 : 1;

                //Check whether we have reached the end of either the forwards or backwards path.
                if (currentPatrolPoint + 1 == patrolPoints.Length || currentPatrolPoint == 0)
                {
                    reversePath = !reversePath;
                }
            }

            agent.SetDestination(patrolPoints[currentPatrolPoint].position);
        }


        private void TryAttack()
        {
            setAgentParams(chaseSpeed, chaseAngularVelocity, chaseAcceleration);
            Debug.Log(Vector3.Distance(transform.position, player.transform.position));

            if (Vector3.Distance(transform.position, player.transform.position) < attackDistance)
            {
                agent.isStopped = true;
                Invoke("Shoot", attackCooldown);
            }
            else
            {
                agent.SetDestination(player.transform.position);
            }
        }


        private void Shoot()
        {
            RaycastHit hitInfo;
            Vector3 raycastOrigin = new Vector3(transform.position.x, transform.position.y + verticalRaycastOffset, transform.position.z);
            Vector3 targetDir = player.transform.position - transform.position;

            bool hit = Physics.Raycast(raycastOrigin, targetDir, out hitInfo, detectionDistance, playerLayer);

            if (hit && hitInfo.transform.CompareTag("Player") && hitInfo.distance < attackDistance)
            {
                hitInfo.transform.GetComponent<PersonajeVida>().die();
            }

            agent.isStopped = false;
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


        private Vector3 RandomNavmeshLocation(float radius)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += transform.position;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;

            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                finalPosition = hit.position;
            }

            return finalPosition;
        }


        private void setAgentParams(float speed, float angularVelocity, float acceleration)
        {
            agent.speed = speed;
            agent.angularSpeed = angularVelocity;
            agent.acceleration = acceleration;
        }


        public void AIDie()
        {
            alive = false;
        }
    }
}