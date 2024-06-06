using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(Collider), typeof(ColorChanger))]
public class Plane : MonoBehaviour
{
	private Rigidbody _rigidbody;
	private ColorChanger _colorChanger;
	
	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_colorChanger = GetComponent<ColorChanger>();
		
		_rigidbody.isKinematic = true;
	}
	
	private void Start()
	{
		_colorChanger.Change();
	}
}