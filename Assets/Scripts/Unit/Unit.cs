using UnityEngine;

[RequireComponent(typeof(UnitResourceCollector))]
[RequireComponent(typeof(UnitMover))]
[RequireComponent(typeof(UnitBaseBuilder))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Base _base;

    [SerializeField] private Resource _currentResource;
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

    private void Update()
    {
       
    }

    public void SetBase(Base @base)
    {
        _base = @base;
    }

    public void AssignCurrentResource(Resource resource, Transform position)
    {
        if (resource != null)
        {
            _currentResource = resource;
            SetTarget(position.position);
            IsBusy = true;
        }
        else
        {
            _base.AssignResourcesToUnits(this);
        }
    }
    public void SetReadyToBuildBase()
    {
        IsReadyToBuildBase = true;
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
            _base.AddUnit(this);
        }

        if (IsReadyToBuildBase && other.TryGetComponent<Flag>(out Flag flag))
        {
            flag.gameObject.SetActive(false);
            _builder.BuildBase(_base.Map);
            IsBusy = false;
            IsReadyToBuildBase = false;
        }
    }

    public void SetTarget(Vector3 targetPosition)
    {
        _mover.SetTarget(targetPosition);
    }
}
