using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class KnockbackReceiverAgent : MonoBehaviour, IKnockbackable
{
    [SerializeField] private float friction = 8f;

    private NavMeshAgent agent;
    private Vector3 knockbackVelocity;
    private float stunTimer;
    public bool CanMove => stunTimer <= 0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (stunTimer > 0f)
        {
            stunTimer -= Time.deltaTime;

            var pos = transform.position;
            pos += knockbackVelocity * Time.deltaTime;
            transform.position = pos;

            knockbackVelocity = Vector3.MoveTowards(knockbackVelocity, Vector3.zero, friction * Time.deltaTime);

            if (stunTimer <= 0f) agent.isStopped = false;
        }
    }

    public void ApplyKnockback(Vector3 direction, float force, float duration)
    {
        if (force <= 0f) return;

        direction.y = 0f;
        knockbackVelocity = direction.normalized * force;
        stunTimer = duration;

        agent.isStopped = true;
    }
}
