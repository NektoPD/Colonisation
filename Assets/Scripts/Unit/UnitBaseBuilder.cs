using UnityEngine;
using UnityEngine.Events;

public class UnitBaseBuilder : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;

    public UnityEvent BaseBuilt;
    private Unit _unit;
    private Base _baseToBuild;

    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    public void BuildBase(Map map)
    {
        BaseBuilt?.Invoke();
        _baseToBuild = Instantiate(_basePrefab, transform.position, Quaternion.identity);
        _baseToBuild.SetMap(map);
        _unit.SetBase(_baseToBuild);
    }
}
