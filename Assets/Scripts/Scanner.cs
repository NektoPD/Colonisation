using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius;
    [SerializeField] private LayerMask _resourceLayer;

    public event UnityAction Detected;
    private Queue<Resource> _resources = new Queue<Resource>();

    public void ScanForResources()
    {
        Collider[] resourceColliders = Physics.OverlapSphere(transform.position, _scanRadius, _resourceLayer);

        foreach (var collider in resourceColliders)
        {
            Resource resource = collider.GetComponent<Resource>();
            if (resource == null) continue;
            _resources.Enqueue(resource);
            Detected?.Invoke();
        }
    }

    public Resource GetResource()
    {
        if (_resources.Count > 0)
        {
            return _resources.Dequeue();
        }
        else
        {
            return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, _scanRadius);
    }
}
