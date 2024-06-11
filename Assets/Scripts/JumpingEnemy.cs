using System.Collections;
using UnityEngine;

public class JumpingEnemy : Enemy
{
    [SerializeField, Min(0)] private float _force = 0.5f;

    private float _height = 1f;

    private void Start()
    {
        StartCoroutine(Jump());
    }

    private IEnumerator Jump()
    {
        while (isActiveAndEnabled)
        {
            Rigidbody.AddForce(Vector3.up * _force, ForceMode.Impulse);

            yield return new WaitUntil(() => transform.position.y == _height);
        }
    }
}