using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChase : MonoBehaviour
{
    [Header("Agent Tuning")]
    public float speed = 3.5f;
    public float angularSpeed = 120f;
    public float acceleration = 8f;

    private NavMeshAgent agent;
    private Transform player;
    private IKnockbackable kb;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        kb = GetComponent<IKnockbackable>();
    }

    private void Start()
    {
        agent.speed = speed;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        EnemyManager.RegisterEnemy(gameObject);
    }

    private void Update()
    {
        if (player == null) return;
        if (kb != null && !kb.CanMove) return;
        if (!agent.isOnNavMesh) return;

        if (!agent.isStopped) agent.SetDestination(player.position);
    }
}
