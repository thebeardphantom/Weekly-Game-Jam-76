using System.Collections;
using System.Linq;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _fruitPrefab;

    private Transform[] _spawners;

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
        var circleSize = _fruitPrefab.GetComponentInChildren<CircleCollider2D>().radius;
        yield return new WaitForSeconds(5f);
        TrySpawnFruit(circleSize);
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 30f));
            TrySpawnFruit(circleSize);
        }
    }

    private void TrySpawnFruit(float circleSize)
    {
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
            Instantiate(_fruitPrefab, spawner.position, Quaternion.identity);
        }
    }
}