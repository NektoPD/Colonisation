using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OnBaseClickHandler))]
[RequireComponent(typeof(BaseFlagCounter))]
public class Base : MonoBehaviour
{
    [SerializeField] private int _resourceCount = 0;

    public event Action ReadyToBuildBase;
    public event Action StopedBuildingBase;
    private UnitSpawner _spawner;
    private Scanner _scanner;
    private Queue<Unit> _units = new Queue<Unit>();
    private List<Resource> _filteredResources = new List<Resource>();
    private OnBaseClickHandler _onBaseClickHandler;
    private BaseFlagCounter _flagCounter;
    private int _unitSpawnValue = 3;
    private int _baseBuildValue = 5;
    private bool _readyToBuildBase = false;

    public bool IsBulidingBase { get; private set; }
    public Map Map { get; private set; }

    private void Awake()
    {
        _onBaseClickHandler = GetComponent<OnBaseClickHandler>();
        _spawner = GetComponentInChildren<UnitSpawner>();
        _flagCounter = GetComponent<BaseFlagCounter>();
    }

    private void Start()
    {
        CreateNewUnit();
    }

    public void ObtainResource()
    {
        _resourceCount++;

        if (_resourceCount == _unitSpawnValue && !IsBulidingBase)
        {
            CreateNewUnit();
            ClearResourceCount();
        }
        else if (_resourceCount == _baseBuildValue && IsBulidingBase)
        {
            ReadyToBuildBase?.Invoke();
            _readyToBuildBase = true;
            ClearResourceCount();
        }
    }

    public void SetScanner(Scanner scanner)
    {
        _scanner = scanner;
        StartCoroutine(FindAvailableUnit());
    }

    public void IsReadyToBuildBase()
    {
        IsBulidingBase = true;
    }

    public void ReturnToBuildingUnits()
    {
        _readyToBuildBase = false;
        IsBulidingBase = false;
        _onBaseClickHandler.SetClickedToFalse();
        StopedBuildingBase?.Invoke();
        Map.FlagSpawned -= IsReadyToBuildBase;
        Map.FlagSpawned -= _flagCounter.ClearCount;
    }

    public void RemoveAllUnits()
    {
        _units.Clear();
    }

    public void AddUnit(Unit unit)
    {
        _units.Enqueue(unit);
    }

    public bool IsFlagAvailable()
    {
        return _flagCounter.Count > 0;
    }

    public void SetMap(Map map)
    {
        Map = map;
        Map.SetBase(this, _onBaseClickHandler);
        Map.FlagSpawned += IsReadyToBuildBase;
        Map.FlagSpawned += _flagCounter.ClearCount;
    }

    public void AssignResourcesToUnits(Unit unit)
    {
        Resource resource = _scanner.GetResource();

        if (resource != null && !_filteredResources.Contains(resource))
        {
            _filteredResources.Add(resource);
            unit.AssignCurrentResource(resource, resource.transform);
        }
        else
        {
            _scanner.ScanForResources();
            AssignResourcesToUnits(unit);
            return;
        }
    }

    private IEnumerator FindAvailableUnit()
    {
        while (true)
        {
            if (_units.Count > 0)
            {
                if (!_readyToBuildBase)
                {
                    Unit unit = _units.Dequeue();
                    _scanner.ScanForResources();
                    AssignResourcesToUnits(unit);
                }
                else
                {
                    Unit unit = _units.Dequeue();
                    CreateNewBase(unit);
                }
            }

            yield return null;
        }
    }

    private void CreateNewUnit()
    {
        _units.Enqueue(_spawner.CreateUnit());
    }

    private void CreateNewBase(Unit unit)
    {
        if (unit != null)
        {
            unit.SetReadyToBuildBase();
            unit.SetTarget(Map.GetCurrentFlagPosition());
            ClearResourceCount();
            ReturnToBuildingUnits();
        }
    }

    private void ClearResourceCount()
    {
        _resourceCount = 0;
    }
}
