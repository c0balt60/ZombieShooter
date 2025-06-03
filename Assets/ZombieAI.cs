using System;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{
    public float distance;
    public Transform Player;
    public NavMeshAgent navMeshAgent;
    public float rotationSpeed = 360f;

    private float updateRate = 0.2f;
    private float nextUpdateTime = 0f;
    private Vector3 lastPlayerPosition;

    Animator PlayerAnim;

    void Start()
    {
        PlayerAnim = GetComponent<Animator>();
        navMeshAgent.stoppingDistance = 2f;
        navMeshAgent.updateRotation = false;
        navMeshAgent.updatePosition = false;
        PlayerAnim.SetBool("Z_Run", false);
        PlayerAnim.SetBool("Z_Idle", true);
        PlayerAnim.SetBool("Z_Attack", false);
        Debug.Log("On NavMesh: " + navMeshAgent.isOnNavMesh);
    }

    void Update()
    {
        if (Player == null || navMeshAgent == null) return;

        distance = Vector3.Distance(transform.position, Player.position);

        // Rotate toward the player
        Vector3 direction = (Player.position - transform.position).normalized;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                lookRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // If far enough, move toward the player
        if (distance > navMeshAgent.stoppingDistance)
        {
            if (navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.SetDestination(Player.position);
            }

            // Manually move forward
            float speed = navMeshAgent.speed;
            transform.position += transform.forward * speed * Time.deltaTime;

            // Run animation
            PlayerAnim.SetBool("Z_Run", true);
            PlayerAnim.SetBool("Z_Idle", false);
            PlayerAnim.SetBool("Z_Attack", false);
        }
        else
        {
            // Within stopping distance — stop
            PlayerAnim.SetBool("Z_Run", false);
            PlayerAnim.SetBool("Z_Idle", false);
            PlayerAnim.SetBool("Z_Attack", true);
            navMeshAgent.ResetPath();
        }
    }
}



