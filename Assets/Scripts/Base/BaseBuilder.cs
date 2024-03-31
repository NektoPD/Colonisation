using System;
using UnityEngine;

public class BaseBuilder : MonoBehaviour
{
    private Base _motherBase;
    private BaseResourceHandler _motherBaseResourceHandler;
    public bool ReadyToBuildBase { get; private set; }

    public bool IsBulidingBase { get; private set; }

    public event Action NewBaseSpawned;

    private void Awake()
    {
        _motherBase = GetComponent<Base>();
        _motherBaseResourceHandler = GetComponent<BaseResourceHandler>();
    }

    private void OnEnable()
    {
        _motherBaseResourceHandler.ReadyToBuild += IsBuildingBase;
    }

    private void OnDisable()
    {
        _motherBaseResourceHandler.ReadyToBuild -= IsBuildingBase;
    }

    public void CreateNewBase(Unit unit)
    {
        if (unit != null)
        {
            unit.SetReadyToBuildBase();
            unit.SetTarget(_motherBase.Map.GetCurrentFlagPosition());

            NewBaseSpawned?.Invoke();
        }
    }

    public void IsReadyToBuildBase()
    {
        IsBulidingBase = true;
    }

    public void IsBuildingBase()
    {
        ReadyToBuildBase = true;
    }

    public void StoppedBuildingBase()
    {
        IsBulidingBase = false;
        ReadyToBuildBase = false;
    }
}

