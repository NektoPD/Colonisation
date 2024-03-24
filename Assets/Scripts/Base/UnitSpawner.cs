using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit _template;

    private Base _base;

    private void Awake()
    {
        _base = GetComponentInParent<Base>();
    }

    public Unit CreateUnit()
    {
        Unit newUnit = Instantiate(_template, transform.position, Quaternion.identity);

        newUnit.SetBase(_base);

        return newUnit;
    }
}
