using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Base : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private int _resourceCount = 0;

    public UnityEvent ReadyToBuildBase;
    public UnityEvent StopedBuildingBase;
    private UnitSpawner _spawner;
    private Scanner _scanner;
    private List<Unit> _units = new List<Unit>();
    private List<Resource> _filteredResources = new List<Resource>();
    private Unit _availableUnit;
    private OnBaseClickHandler _onBaseClickHandler;
    private BaseFlagCounter _flagCounter;
    private int _unitSpawnValue = 3;
    private int _baseBuildValue = 5;

    public bool IsBulidingBase { get; private set; }

    private void Awake()
    {
        _onBaseClickHandler = GetComponent<OnBaseClickHandler>();
        _spawner = GetComponentInChildren<UnitSpawner>();
        _flagCounter = GetComponent<BaseFlagCounter>();
    }

    public void ObtainResource()
    {
        _resourceCount++;

        if (_resourceCount == _unitSpawnValue && !IsBulidingBase)
        {
            CreateNewUnit();
        }
        else if (_resourceCount == _baseBuildValue && IsBulidingBase)
        {
            ReadyToBuildBase?.Invoke();
            CreateNewBase();
        }
    }

    public void SetScanner(Scanner scanner)
    {
        _scanner = scanner;

        StartCoroutine(FindAvailableUnit());
        _scanner.Detected += AssignResourcesToUnits;
    }

    public Map GetMap()
    {
        return _map;
    }

    public void GarheringResourcesToBuildBase()
    {
        IsBulidingBase = true;
    }

    public void StopGarheringResourcesToBuildBase()
    {
        IsBulidingBase = false;
        StopedBuildingBase?.Invoke();
        _onBaseClickHandler.SetClickedToFalse();
        StartCoroutine(FindAvailableUnit());
    }

    public bool IsFlagAvailable()
    {
        return _flagCounter.Count > 0;
    }

    public void SetMap(Map map)
    {
        _map = map;
        _map.SetBase(this, _onBaseClickHandler);
        _map.FlagSpawned.AddListener(GarheringResourcesToBuildBase);
        _map.FlagSpawned.AddListener(_flagCounter.ClearCount);
    }

    private void AssignResourcesToUnits()
    {
        Resource resource = _scanner.GetResource();

        if (resource != null && !_filteredResources.Contains(resource))
        {
            _filteredResources.Add(resource);
            _availableUnit.AssignCurrentResource(resource, resource.transform);
        }
        else
        {
            return;
        }
    }

    private IEnumerator FindAvailableUnit()
    {
        if (_units.Count == 0)
        {
            _units.Add(_spawner.CreateUnit());
        }

        while (true)
        {
            foreach (Unit unit in _units)
            {
                if (!unit.IsBusy && !unit.IsReadyToBuildBase)
                {
                    _availableUnit = unit;
                    _scanner.ScanForResources();
                }
            }

            yield return null;
        }
    }

    private void CreateNewUnit()
    {
        _units.Add(_spawner.CreateUnit());
        ClearResourceCount();
    }

    private void CreateNewBase()
    {
        if (_availableUnit != null)
        {
            _availableUnit.SetReadyToBuildBase();
            _availableUnit.GetComponent<UnitBaseBuilder>().BaseBuilt.AddListener(StopGarheringResourcesToBuildBase);
            _availableUnit.AssignBaseToBuild(_map.GetCurrentFlagPosition());
            ClearResourceCount();
            StopCoroutine(FindAvailableUnit());
        }
    }

    private void ClearResourceCount()
    {
        _resourceCount = 0;
    }
}
