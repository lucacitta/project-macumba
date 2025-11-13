using UnityEngine;

using UnityEngine.AI;

using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [Header("Catalog")]
    public List<EnemyDefinition> enemyCatalog = new(); // Spawnable enemy types

    [Header("Budget")]
    public float desiredDangerLevel = 5f;
    public float actualDangerLevel = 0f;

    [Header("Spawning")]
    public float spawnCheckInterval = 0.25f;  // Time between spawn attempts
    public int maxSimultaneous = 50;        // Safety cap

    public float defaultMinDistance = 25f;
    public float defaultMaxDistance = 50f;
    public float navmeshSampleRadius = 50f;

    public static EnemyManager Instance { get; private set; }
    public static List<GameObject> ActiveEnemies = new();
    private Transform player;

    private float _timer;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }


    public void RegisterEnemy(GameObject enemy)
    {
        if (ActiveEnemies.Contains(enemy)) return;

        ActiveEnemies.Add(enemy);
        var chase = enemy.GetComponent<EnemyChase>();
        if (chase != null)
        {
            actualDangerLevel += chase.DangerFactor;
        }
    }

    public void DeregisterEnemy(GameObject enemy)
    {
        if (!ActiveEnemies.Contains(enemy)) return;

        ActiveEnemies.Remove(enemy);
            var chase = enemy.GetComponent<EnemyChase>();
            if (chase != null)
            {
                actualDangerLevel -= chase.DangerFactor;
            }
        
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer > 0f) return;
        _timer = spawnCheckInterval;

        if (player == null) return;
        if (ActiveEnemies.Count >= maxSimultaneous) return;

        float remaining = Mathf.Max(0f, desiredDangerLevel - actualDangerLevel);
        if (remaining <= 0f) return;

        var def = PickEnemyDefinition(remaining);
        if (def == null) return;

        if (!TryGetSpawnPoint(player.position, out Vector3 spawnPos)) return;

        SpawnEnemy(def, spawnPos);
    }

    private EnemyDefinition PickEnemyDefinition(float remainingDanger)
    {
        var candidates = new List<EnemyDefinition>();
        float totalWeight = 0f;

        foreach (var def in enemyCatalog)
        {
            if (def == null || def.prefab == null) continue;
            if (def.dangerFactor <= remainingDanger)
            {
                candidates.Add(def);
                totalWeight += 1; 
            }
        }

        if (candidates.Count == 0) return null;

        float r = Random.value * totalWeight;
        foreach (var c in candidates)
        {
            if (r <= 1) return c;
            r -= 1;
        }
        return candidates[^1];
    }

    private bool TryGetSpawnPoint(Vector3 playerPos, out Vector3 spawnPos)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 dir2 = Random.insideUnitCircle.normalized;
            float dist = Random.Range(defaultMinDistance, defaultMaxDistance);
            Vector3 target = playerPos + new Vector3(dir2.x, 0f, dir2.y) * dist;
            target.y = 0f;

            if (NavMesh.SamplePosition(target, out NavMeshHit hit, navmeshSampleRadius, NavMesh.AllAreas))
            {
                spawnPos = hit.position;
                spawnPos.y = 3f; // slight offset to avoid ground clipping
                return true;
            }
        }

        spawnPos = default;
        return false;
    }

    private void SpawnEnemy(EnemyDefinition def, Vector3 pos)
    {
        var go = Instantiate(def.prefab, pos, Quaternion.identity);
        RegisterEnemy(go);
    }
}
