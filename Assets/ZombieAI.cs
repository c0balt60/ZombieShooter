using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    public float distance;
    public Transform Player;
    public NavMeshAgent navMeshAgent;
    public float rotationSpeed = 360f;

    public int maxHealth = 100;
    private int currentHealth;

    public Animator PlayerAnim;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        PlayerAnim = GetComponent<Animator>();

        navMeshAgent.stoppingDistance = 2f;
        navMeshAgent.updateRotation = false;
        navMeshAgent.updatePosition = true;

        PlayerAnim.SetBool("Z_Run", false);
        PlayerAnim.SetBool("Z_Idle", true);
        PlayerAnim.SetBool("Z_Attack", false);
        PlayerAnim.SetBool("Z_Death", false);

        Debug.Log("On NavMesh: " + navMeshAgent.isOnNavMesh);
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Zombie killed");
        isDead = true;

        // Stop movement
        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();

        // Set only death animation
        PlayerAnim.SetBool("Z_Run", false);
        PlayerAnim.SetBool("Z_Idle", false);
        PlayerAnim.SetBool("Z_Attack", false);
        //PlayerAnim.SetBool("Z_Dead", true);
        PlayerAnim.SetBool("Z_Death", true);

        // Destroy the zombie after 3 seconds (adjust based on your animation length)
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        if (isDead || Player == null || navMeshAgent == null) return;

        distance = Vector3.Distance(transform.position, Player.position);

        if (distance > navMeshAgent.stoppingDistance)
        {
            if (navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.SetDestination(Player.position);
            }

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

            PlayerAnim.SetBool("Z_Run", true);
            PlayerAnim.SetBool("Z_Idle", false);
            PlayerAnim.SetBool("Z_Attack", false);
            PlayerAnim.SetBool("Z_Death", false);
        }
        else
        {
            navMeshAgent.ResetPath();

            PlayerAnim.SetBool("Z_Run", false);
            PlayerAnim.SetBool("Z_Idle", false);
            PlayerAnim.SetBool("Z_Attack", true);
            PlayerAnim.SetBool("Z_Death", false);
        }
    }
}



