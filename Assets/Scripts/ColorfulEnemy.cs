using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ColorChanger))]
public class ColorfulEnemy : Enemy
{
    [SerializeField, Min(1)] private float _delay = 2f;

    private ColorChanger _colorChanger;

    protected override void Awake()
    {
        base.Awake();

        _colorChanger = GetComponent<ColorChanger>();
    }

    private void OnEnable()
    {
        StartCoroutine(ChangeColor());
    }

    private IEnumerator ChangeColor()
    {
        var wait = new WaitForSecondsRealtime(_delay);

        while (isActiveAndEnabled)
        {
            _colorChanger.Change();

            yield return wait;
        }
    }
}