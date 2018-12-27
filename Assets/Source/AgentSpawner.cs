using System.Collections;
using UnityEngine;

public class AgentSpawner : MonoBehaviour, IAgentSpawner
{
    #region Fields

    [SerializeField]
    private Bounds _spawnBounds;

    [SerializeField]
    private Agent _agentPrefab;

    [SerializeField]
    private float _spawnRate;

    [SerializeField]
    private int _minAgents;

    [SerializeField]
    private int _maxAgents;

    #endregion

    #region Properties

    public Agent Prefab => _agentPrefab;

    #endregion

    #region Methods

    /// <inheritdoc />
    public Agent SpawnOne()
    {
        var x = Random.Range(_spawnBounds.min.x, _spawnBounds.max.x);
        var y = Random.Range(_spawnBounds.min.y, _spawnBounds.max.y);
        var position = new Vector2(x, y);
        var instance = Instantiate(Prefab, position, Quaternion.identity);
        return instance;
    }

    private void Awake()
    {
        var amount = Random.Range(_minAgents, _maxAgents);
        Debug.Log($"Spawning {amount} {Prefab.name}");
        for (var i = 0; i < amount; i++)
        {
            SpawnOne();
        }

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_spawnRate, _spawnRate * 2f));
            SpawnOne();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(_spawnBounds.center, _spawnBounds.size);
    }

    #endregion
}