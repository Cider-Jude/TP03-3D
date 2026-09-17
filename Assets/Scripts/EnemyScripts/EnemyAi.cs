using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;

    [SerializeField] private LayerMask Ground, IsPlayer;

    [Header("Attacks")]
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] float attackDamage = 10f;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackHitboxRadius = 1f;
    [SerializeField] float attackDelay;
    private bool alreadyAttacked;

    [Header("States")]
    [SerializeField] float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, IsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, IsPlayer);

        if (playerInSightRange && !playerInAttackRange) 
        {
            animator.SetBool("isWalking", true);
            ChasePlayer();
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            animator.SetTrigger("attack");
            Invoke(nameof(DealDamage), attackDelay);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    // Appelée par un Animation Event, placé sur la frame d'impact de l'animation d'attaque.
    // C'est elle qui inflige réellement les dégâts, synchronisée avec le visuel.
    private void DealDamage()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, attackHitboxRadius, IsPlayer);

        foreach (Collider playerCollider in hitPlayers)
        {
            PlayerHealth playerHealth = playerCollider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Damage(attackDamage);
            }
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    // Visualise les rayons de detection/attaque dans la Scene View pour faciliter le réglage
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (attackPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(attackPoint.position, attackHitboxRadius);
        }
    }
}