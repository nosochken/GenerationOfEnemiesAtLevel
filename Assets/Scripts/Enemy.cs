using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{	
	[SerializeField, Min(1)] private float _speed = 2f;
	
	public IEnumerator Move(Vector3 direction)
	{
		while (isActiveAndEnabled)
		{
			transform.Translate(direction * _speed * Time.deltaTime);
		
			yield return null;
		}
	}
}