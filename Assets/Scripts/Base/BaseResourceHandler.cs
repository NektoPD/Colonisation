using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseResourceHandler : MonoBehaviour
{
    [SerializeField] private int _resourceCount = 0;

    private int _unitSpawnValue = 3;
    private int _baseBuildValue = 5;
    private Base _motherBase;
    private BaseBuilder _baseBuilder;
    private List<Resource> _filteredResources = new List<Resource>();

    public event Action ReadyToBuild;

    private void Awake()
    {
        _motherBase = GetComponent<Base>();
        _baseBuilder = GetComponent<BaseBuilder>();
    }

    public void ObtainResource()
    {
        _resourceCount++;

        if (_resourceCount == _unitSpawnValue && !_baseBuilder.IsBulidingBase)
        {
            _motherBase.CreateNewUnit();
            ClearResourceCount();
        }
        else if (_resourceCount == _baseBuildValue && _baseBuilder.IsBulidingBase)
        {
            ReadyToBuild?.Invoke();
            ClearResourceCount();
        }
    }

    public void AssignResourcesToUnits(Unit unit)
    {
        Resource resource = _motherBase.Scanner.GetResource();

        if (resource != null && !_filteredResources.Contains(resource))
        {
            _filteredResources.Add(resource);
            unit.AssignCurrentResource(resource, resource.transform);
        }
        else
        {
            _motherBase.Scanner.ScanForResources();
            AssignResourcesToUnits(unit);
            return;
        }
    }

    private void ClearResourceCount()
    {
        _resourceCount = 0;
    }
}
