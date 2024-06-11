using System.Collections;
using UnityEngine;

public class JumpingEnemy : Enemy
{
    [SerializeField, Min(0)] private float _force = 150f;

    private float _height = 1f;

    private void OnEnable()
    {
        StartCoroutine(Jump());
    }

    private IEnumerator Jump()
    {
        while (isActiveAndEnabled)
        {
            Rigidbody.AddForce(Vector3.up * _force * Time.deltaTime, ForceMode.Impulse);

            yield return new WaitUntil(() => transform.position.y <= _height);
        }
    }
}