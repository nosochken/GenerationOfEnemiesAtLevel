using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Target : MonoBehaviour
{
    [SerializeField, Min(1)] private float _speed = 4f;

    private List<Waypoint> _waypoints;

    private Rigidbody _rigidbody;
    private Vector3 _startPosition;
    private Vector3 _targetPosition;

    public Vector3 StartPosition => _startPosition;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
    }

    private void Start()
    {
        StartCoroutine(Move());
    }

    public void Init(Vector3 startPosition, List<Waypoint> waypoints)
    {
        _startPosition = startPosition;
        transform.position = _startPosition;

        _waypoints = waypoints;
    }

    private IEnumerator Move()
    {
        while (isActiveAndEnabled)
        {
            _targetPosition = ChooseWaypointRandomly().transform.position;

            while (transform.position != _targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

                yield return null;
            }
        }
    }

    private Waypoint ChooseWaypointRandomly()
    {
        int index = Random.Range(0, _waypoints.Count);

        return _waypoints[index];
    }
}