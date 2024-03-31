using System;
using UnityEngine;

[RequireComponent(typeof(Scanner))]
public class Map : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private Terrain _terrain;
    [SerializeField] private Flag _flagTemplate;
    [SerializeField] private MousePosition _mousePosition;

    private Scanner _scanner;
    private OnBaseClickHandler _baseClicker;
    private Flag _currentFlag;
    private bool _baseIsBuildingBase = false;

    public event Action FlagSpawned;

    private void Awake()
    {
        _scanner = GetComponent<Scanner>();
    }

    private void Start()
    {
        _base.SetMap(this);
    }

    public void SetBase(Base baseToassign, OnBaseClickHandler clicker)
    {
        _base = baseToassign;
        _baseClicker = clicker;
        _baseClicker.OnBaseClick += DetectBaseClick;
        _base.ReadyToBuildBase += SetBaseBuildingStateToTrue;
        _base.StopedBuildingBase += SetBaseBuildingStateToFalse;
        _base.SetScanner(_scanner);
    }

    public Vector3 GetCurrentFlagPosition()
    {
        if (_currentFlag != null)
        {
            return _currentFlag.transform.position;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private void DetectBaseClick()
    {
        if (_baseClicker != null)
        {
            DetectTerrainClick();
        }
    }

    private void PutFlag()
    {
        if (!_baseIsBuildingBase)
        {
            if (_base.IsFlagAvailable())
            {
                _currentFlag = Instantiate(_flagTemplate, _mousePosition.ReceiveRaycastPosition(), Quaternion.identity);
                FlagSpawned.Invoke();
            }
            else
            {
                _currentFlag.transform.position = _mousePosition.ReceiveRaycastPosition();
            }
        }
    }

    private void SetBaseBuildingStateToTrue()
    {
        _baseIsBuildingBase = true;
    }

    private void SetBaseBuildingStateToFalse()
    {
        _baseIsBuildingBase = false;
        _base.ReadyToBuildBase -= SetBaseBuildingStateToTrue;
        _base.StopedBuildingBase -= SetBaseBuildingStateToFalse;
        _baseClicker.OnBaseClick -= DetectBaseClick;
        _terrain.TerrainClicked -= PutFlag;
    }

    private void DetectTerrainClick()
    {
        if (_terrain != null)
        {
            _terrain.TerrainClicked += PutFlag;
        }
    }  
}
