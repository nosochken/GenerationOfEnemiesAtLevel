using System.Collections.Generic;
using UnityEngine;

public class DirectionFinder : MonoBehaviour
{
    [SerializeField] private List<Vector3> _directions;

    private void Awake()
    {
        AddDirections();
    }

    public Vector3 ChooseDirectionRandomly()
    {
        int index = Random.Range(0, _directions.Count);

        return _directions[index];
    }

    private void AddDirections()
    {
        _directions = new List<Vector3>
        {
            Vector3.forward,
            Vector3.back,
            Vector3.right,
            Vector3.left
        };
    }
}