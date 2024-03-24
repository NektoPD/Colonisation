using UnityEngine;

[RequireComponent(typeof(UnitResourceCollector))]
[RequireComponent(typeof(UnitMover))]
[RequireComponent(typeof(UnitBaseBuilder))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Base _base;

    private Resource _currentResource;
    private UnitMover _mover;
    private UnitResourceCollector _collector;
    private UnitBaseBuilder _builder;

    public bool IsBusy {get; private set;}
    public bool IsReadyToBuildBase { get; private set; }

    private void Start()
    {
        IsBusy = false;
        _mover = GetComponent<UnitMover>();
        _collector = GetComponent<UnitResourceCollector>();
        _builder = GetComponent<UnitBaseBuilder>();
    }

    public void SetBase(Base @base)
    {
        _base = @base;
    }

    public void AssignCurrentResource(Resource resource, Transform position)
    {
        if (resource != null || IsBusy == false)
        {
            _currentResource = resource;
            SetTarget(position.position);
            IsBusy = true;
        }
    }
    public void SetReadyToBuildBase()
    {
        IsReadyToBuildBase = true;
    }

    public void AssignBaseToBuild(Vector3 position)
    {
        if (position != null)
        {
            _mover.SetTarget(position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_collector.IsPicked && other.TryGetComponent<Resource>(out Resource resource))
        {
            if (resource == _currentResource)
            {
                _collector.PickUpResource(_currentResource);
                SetTarget(_base.transform.position);
            }
        }

        if (_collector.IsPicked && other.TryGetComponent<Base>(out Base @base))
        {
            _collector.DropOffResource(_currentResource);
            IsBusy = false;
            _base.ObtainResource();
        }

        if (IsReadyToBuildBase && other.TryGetComponent<Flag>(out Flag flag))
        {
            flag.gameObject.SetActive(false);
            _builder.BuildBase(_base.GetMap());
            IsBusy = false;
            IsReadyToBuildBase = false;
        }
    }

    private void SetTarget(Vector3 targetPosition)
    {
        _mover.SetTarget(targetPosition);
    }
}
