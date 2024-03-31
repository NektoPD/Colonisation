using System;
using UnityEngine;

public class UnitBaseBuilder : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;

    private Unit _unit;
    private Base _baseToBuild;

    public event Action BaseBuilt;

    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    public void BuildBase(Map map)
    {
         BaseBuilt?.Invoke();
        _baseToBuild = Instantiate(_basePrefab, transform.position, Quaternion.identity);
        _baseToBuild.RemoveAllUnits();
        _baseToBuild.SetMap(map);
        _baseToBuild.AddUnit(_unit);
        _unit.SetBase(_baseToBuild);
    }
}
