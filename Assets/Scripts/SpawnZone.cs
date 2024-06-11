using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class SpawnZone : MonoBehaviour
{
    [SerializeField] private Plane _plane;
    [SerializeField] private float _height = 20f;

    private BoxCollider _boxCollider;
    private Rigidbody _rigidbody;

    public event Action<Enemy> EnemyLeftZone;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;

        ConfigureTransform();
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent<Enemy>(out Enemy enemy))
            EnemyLeftZone?.Invoke(enemy);
    }

    private void ConfigureTransform()
    {
        SetPosition();
        SetScale();
        SetColliderCenter();
    }

    private void SetPosition()
    {
        transform.position = _plane.transform.position;
    }

    private void SetScale()
    {
        transform.localScale = _plane.GetComponent<Collider>().bounds.size;
        Vector3 newScale = new Vector3(transform.localScale.x, _height, transform.localScale.z);
        transform.localScale = newScale;
    }

    private void SetColliderCenter()
    {
        float centerHeight = 0.5f;
        Vector3 newCenter = new Vector3(_boxCollider.center.x, _boxCollider.center.y + centerHeight, _boxCollider.center.z);
        _boxCollider.center = newCenter;
    }
}