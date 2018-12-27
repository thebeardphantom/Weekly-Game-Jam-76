using System.Collections;
using System.Linq;
using UnityEngine;

public class FruitSpawner : MonoBehaviour, IAgentSpawner
{
    #region Fields

    [SerializeField]
    private Agent _fruitPrefab;

    [SerializeField]
    private float _minSpawnTime = 2f;

    [SerializeField]
    private float _maxSpawnTime = 18f;

    private Transform[] _spawners;

    #endregion

    #region Properties

    /// <inheritdoc />
    public Agent Prefab => _fruitPrefab;

    #endregion

    #region Methods

    /// <inheritdoc />
    public Agent SpawnOne()
    {
        var circleSize = _fruitPrefab.GetComponentInChildren<CircleCollider2D>().radius;
        Transform spawner = null;
        for (var i = 0; i < 50; i++)
        {
            var s = _spawners[Random.Range(0, _spawners.Length)];
            var collision = Physics2D.OverlapCircle(s.position, circleSize);
            if (collision == null)
            {
                spawner = s;
                break;
            }
        }

        if (spawner == null)
        {
            Debug.LogError("COULD NOT SPAWN FRUIT");
        }
        else
        {
            return Instantiate(_fruitPrefab, spawner.position, Quaternion.identity);
        }

        return null;
    }

    private void Awake()
    {
        _spawners = GameObject.FindGameObjectsWithTag("Respawn")
            .Where(t => t.transform.parent == transform)
            .Select(t => t.transform)
            .ToArray();
        StartCoroutine(SpawnerRoutine());
    }

    private IEnumerator SpawnerRoutine()
    {
        yield return new WaitForSeconds(_minSpawnTime);
        SpawnOne();
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minSpawnTime, _maxSpawnTime));
            SpawnOne();
        }
    }

    #endregion
}