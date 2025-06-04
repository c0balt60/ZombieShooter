using System;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{
    public float distance;
    public Transform Player;
    public NavMeshAgent navMeshAgent;
    public float rotationSpeed = 360f;

    private Animator PlayerAnim;

    void Start()
    {
        PlayerAnim = GetComponent<Animator>();

        navMeshAgent.stoppingDistance = 2f;
        navMeshAgent.updateRotation = false;  // we rotate manually
        navMeshAgent.updatePosition = true;   // let NavMeshAgent move the zombie

        PlayerAnim.SetBool("Z_Run", false);
        PlayerAnim.SetBool("Z_Idle", true);
        PlayerAnim.SetBool("Z_Attack", false);

        Debug.Log("On NavMesh: " + navMeshAgent.isOnNavMesh);
    }

    void Update()
    {
        if (Player == null || navMeshAgent == null) return;

        distance = Vector3.Distance(transform.position, Player.position);

        // Only move and rotate if far enough from player
        if (distance > navMeshAgent.stoppingDistance)
        {
            if (navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.SetDestination(Player.position);
            }

            // Rotate based on path direction, not direct player direction
            Vector3 velocity = navMeshAgent.desiredVelocity;
            velocity.y = 0f;
            if (velocity.magnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(velocity);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    lookRotation,
                    rotationSpeed * Time.deltaTime
                );
            }

            // Animate running
            PlayerAnim.SetBool("Z_Run", true);
            PlayerAnim.SetBool("Z_Idle", false);
            PlayerAnim.SetBool("Z_Attack", false);
        }
        else
        {
            // Within attack range
            navMeshAgent.ResetPath();

            PlayerAnim.SetBool("Z_Run", false);
            PlayerAnim.SetBool("Z_Idle", false);
            PlayerAnim.SetBool("Z_Attack", true);
        }
    }

}



