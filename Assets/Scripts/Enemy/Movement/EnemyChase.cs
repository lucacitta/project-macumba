using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChase : MonoBehaviour
{
    [Header("Agent Tuning")]
    public float speed = 3.5f;
    public float angularSpeed = 120f;
    public float acceleration = 8f;

    public EnemyDefinition def;
    public float DangerFactor => def != null ? def.dangerFactor : 1f; //TODO: Move this to Definition asset file

    private NavMeshAgent agent;
    private Transform player;
    private IKnockbackable kb;
    private EnemyManager enemyManager;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        kb = GetComponent<IKnockbackable>();
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    private void Start()
    {
        agent.speed = speed;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        enemyManager.RegisterEnemy(gameObject);
    }

    private void Update()
    {
        if (player == null) return;
        if (kb != null && !kb.CanMove) return;
        if (!agent.isOnNavMesh) return;

        if (!agent.isStopped) agent.SetDestination(player.position);
    }
}
