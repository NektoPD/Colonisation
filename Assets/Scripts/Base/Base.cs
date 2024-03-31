using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OnBaseClickHandler))]
[RequireComponent(typeof(BaseFlagCounter))]
[RequireComponent(typeof(BaseResourceHandler))]
[RequireComponent(typeof(BaseBuilder))]
public class Base : MonoBehaviour
{
    private UnitSpawner _spawner;
    private Queue<Unit> _units = new Queue<Unit>();
    private OnBaseClickHandler _onBaseClickHandler;
    private BaseFlagCounter _flagCounter;
    private BaseBuilder _baseBuilder;
    private BaseResourceHandler _baseResourceHandler;

    public Map Map { get; private set; }
    public Scanner Scanner { get; private set; }

    public event Action ReadyToBuildBase;
    public event Action StopedBuildingBase;

    private void Awake()
    {
        _onBaseClickHandler = GetComponent<OnBaseClickHandler>();
        _spawner = GetComponentInChildren<UnitSpawner>();
        _flagCounter = GetComponent<BaseFlagCounter>();
        _baseBuilder = GetComponent<BaseBuilder>();
        _baseResourceHandler = GetComponent<BaseResourceHandler>();
    }

    private void OnEnable()
    {
        _baseBuilder.NewBaseSpawned += ReturnToBuildingUnits;
        _baseResourceHandler.ReadyToBuild += ReadyToBuildBase.Invoke;
    }

    private void OnDisable()
    {
        _baseBuilder.NewBaseSpawned -= ReturnToBuildingUnits;
        _baseResourceHandler.ReadyToBuild -= ReadyToBuildBase.Invoke;
    }

    private void Start()
    {
        CreateNewUnit();
    }

    public void SetScanner(Scanner scanner)
    {
        Scanner = scanner;
        StartCoroutine(FindAvailableUnit());
    }

    public void ReturnToBuildingUnits()
    {
        _baseBuilder.StoppedBuildingBase();
        _onBaseClickHandler.SetClickedToFalse();
        StopedBuildingBase?.Invoke();
        Map.FlagSpawned -= _baseBuilder.IsReadyToBuildBase;
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
        Map.FlagSpawned += _baseBuilder.IsReadyToBuildBase;
        Map.FlagSpawned += _flagCounter.ClearCount;
    }

    public void CreateNewUnit()
    {
        Unit unit = _spawner.CreateUnit();
        unit.BroughtResourceToBase += _baseResourceHandler.ObtainResource;
        _units.Enqueue(unit);
    }

    private IEnumerator FindAvailableUnit()
    {
        while (true)
        {
            if (_units.Count > 0)
            {
                if (!_baseBuilder.ReadyToBuildBase)
                {
                    Unit unit = _units.Dequeue();
                    Scanner.ScanForResources();
                    _baseResourceHandler.AssignResourcesToUnits(unit);
                }
                else
                {
                    Unit unit = _units.Dequeue();
                    _baseBuilder.CreateNewBase(unit);
                }
            }

            yield return null;
        }
    }
}
