using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private List<Waypoint> _waypoints;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Target _targetPrefab;

    private Target _target;

    private void Start()
    {
        IdentifyTargetForEnemies();
    }

    public Enemy CreateEnemy()
    {
        Enemy newEnemy = Instantiate(_enemyPrefab);
        newEnemy.Init(transform.position, _target);

        return newEnemy;
    }

    private void IdentifyTargetForEnemies()
    {
        _target = Instantiate(_targetPrefab);
        _target.Init(transform.position, _waypoints);
    }
}