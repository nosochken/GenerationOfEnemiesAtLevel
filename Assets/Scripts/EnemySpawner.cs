using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(DirectionFinder))]
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SpawnZone _spawnZone;
    [SerializeField] private List<SpawnPoint> _spawnPoints;
    [SerializeField] private Enemy _enemyPrefab;

    [SerializeField, Min(1)] private int _poolCapacity = 8;
    [SerializeField, Min(1)] private int _poolMaxSize = 8;

    [SerializeField, Min(0)] private float _delay = 2f;
    [SerializeField] private KeyCode _spawnStopKey = KeyCode.Space;

    private DirectionFinder _directionFinder;

    private ObjectPool<Enemy> _pool;
    private List<Coroutine> _coroutines;

    private void Awake()
    {
        _coroutines = new List<Coroutine>();

        _pool = new ObjectPool<Enemy>(
           createFunc: () => Instantiate(_enemyPrefab),
           actionOnGet: (enemy) => ActOnGet(enemy),
           actionOnRelease: (enemy) => enemy.gameObject.SetActive(false),
           actionOnDestroy: (enemy) => Destroy(enemy.gameObject),
           collectionCheck: true,
           defaultCapacity: _poolCapacity,
           maxSize: _poolMaxSize);

        _directionFinder = GetComponent<DirectionFinder>();
    }

    private void OnEnable()
    {
        _spawnZone.EnemyLeftZone += ReturnToPool;
    }

    private void Start()
    {
        Debug.Log($"to stop spawning press {_spawnStopKey}");

        _coroutines.Add(StartCoroutine(Spawn()));
    }

    private void Update()
    {
        if (Input.GetKeyDown(_spawnStopKey))
        {
            if (_coroutines != null)
            {
                StopAllCoroutines();

                Debug.Log("spawn completed");
            }
        }
    }

    private void OnDisable()
    {
        _spawnZone.EnemyLeftZone -= ReturnToPool;
    }

    private void ActOnGet(Enemy enemy)
    {
        enemy.transform.position = ChooseSpawnPointRandomly().transform.position;
        enemy.gameObject.SetActive(true);

        _coroutines.Add(StartCoroutine(enemy.Move(_directionFinder.ChooseDirectionRandomly())));
    }

    private IEnumerator Spawn()
    {
        var wait = new WaitForSecondsRealtime(_delay);

        while (!Input.GetKey(_spawnStopKey))
        {
            yield return wait;

            _pool.Get();
        }
    }

    private void ReturnToPool(Enemy enemy)
    {
        _pool.Release(enemy);
    }

    private SpawnPoint ChooseSpawnPointRandomly()
    {
        int index = Random.Range(0, _spawnPoints.Count);

        return _spawnPoints[index];
    }
}