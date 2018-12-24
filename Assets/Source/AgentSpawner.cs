using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    [SerializeField]
    private Bounds _spawnBounds;

    [SerializeField]
    private Agent _agentPrefab;

    [SerializeField]
    private int _minAgents;

    [SerializeField]
    private int _maxAgents;

    private void Awake()
    {
        var amount = Random.Range(_minAgents, _maxAgents);
        Debug.Log($"Spawning {amount} {_agentPrefab.name}");
        for (int i = 0; i < amount; i++)
        {
            var x = Random.Range(_spawnBounds.min.x, _spawnBounds.max.x);
            var y = Random.Range(_spawnBounds.min.y, _spawnBounds.max.y);
            var position = new Vector2(x, y);
            var instance = Instantiate(_agentPrefab, position, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(_spawnBounds.center, _spawnBounds.size);
    }
}