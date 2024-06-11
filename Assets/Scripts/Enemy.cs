using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField, Min(1)] private float _speed = 3.9f;

    private Rigidbody _rigidbody;
    private Vector3 _startPosition;
    private Target _target;

    public event Action<Enemy> ReachedTarget;

    public Rigidbody Rigidbody => _rigidbody;
    public Vector3 StartPosition => _startPosition;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Target>(out Target target))
        {
            if (target.StartPosition == _startPosition)
                ReachedTarget?.Invoke(this);
        }
    }

    public void Init(Vector3 startPosition, Target target)
    {
        _startPosition = startPosition;
        transform.position = _startPosition;

        _target = target;
    }

    public IEnumerator Move()
    {
        while (isActiveAndEnabled)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);

            yield return null;
        }
    }
}