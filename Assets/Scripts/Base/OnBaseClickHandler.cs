using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BaseFlagCounter))]
public class OnBaseClickHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Material _material;

    public UnityEvent OnBaseClick = new UnityEvent();
    private MeshRenderer _meshRenderer;
    private Material _startMaterial;
    private BaseFlagCounter _flagCounter;

    public bool ClickedOnBase { get; private set; }

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _startMaterial = _meshRenderer.material;
        _flagCounter = GetComponent<BaseFlagCounter>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_flagCounter.Count > 0)
        {
            OnBaseClick?.Invoke();
            ClickedOnBase = true;
            _meshRenderer.material = _material;
        }
        else
        {
            return;
        }
    }

    public void SetClickedToFalse()
    {
        ClickedOnBase = false;
        _meshRenderer.material = _startMaterial;
    }
}
