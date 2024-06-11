using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SpawnZone _spawnZone;
    [SerializeField] private List<SpawnPoint> _spawnPoints;

    [SerializeField, Min(1)] private int _poolCapacity = 8;
    [SerializeField, Min(1)] private int _poolMaxSize = 8;

    [SerializeField, Min(0)] private float _delay = 2f;
    [SerializeField] private KeyCode _spawnStopKey = KeyCode.Space;

    private ObjectPool<Enemy> _pool;
    private List<Coroutine> _coroutines;

    private int _currentSpawnPointNumber;

    private void Awake()
    {
        _coroutines = new List<Coroutine>();

        _pool = new ObjectPool<Enemy>(
           createFunc: () => Create(),
           actionOnGet: (enemy) => ActOnGet(enemy),
           actionOnRelease: (enemy) => enemy.gameObject.SetActive(false),
           actionOnDestroy: (enemy) => ActOnDestroy(enemy),
           collectionCheck: true,
           defaultCapacity: _poolCapacity,
           maxSize: _poolMaxSize);
    }

    private void OnEnable()
    {
        _spawnZone.EnemyLeftZone += ReturnToPool;
    }

    private void Start()
    {
        _coroutines.Add(StartCoroutine(Spawn()));
    }

    private void Update()
    {
        if (Input.GetKeyDown(_spawnStopKey))
        {
            if (_coroutines != null)
                StopAllCoroutines();
        }
    }

    private void OnDisable()
    {
        _spawnZone.EnemyLeftZone -= ReturnToPool;
    }

    private Enemy Create()
    {
        Enemy enemy = ChooseSpawnPointRandomly().CreateEnemy();
        enemy.ReachedTarget += ReturnToPool;

        return enemy;
    }

    private void ActOnGet(Enemy enemy)
    {
        enemy.transform.position = enemy.StartPosition;
        enemy.gameObject.SetActive(true);

        _coroutines.Add(StartCoroutine(enemy.Move()));
    }

    private void ActOnDestroy(Enemy enemy)
    {
        enemy.ReachedTarget -= ReturnToPool;

        Destroy(enemy.gameObject);
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
        SpawnPoint spawnPoint = _spawnPoints[_currentSpawnPointNumber];

        _currentSpawnPointNumber++;

        if (_currentSpawnPointNumber == _spawnPoints.Count)
            _currentSpawnPointNumber = 0;

        return spawnPoint;
    }
}