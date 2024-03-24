using UnityEngine;

public class UnitResourceCollector : MonoBehaviour
{
    public bool IsPicked { get; private set; }

    public void PickUpResource(Resource resource)
    {
        resource.transform.SetParent(transform);
        IsPicked = true;
    }

    public void DropOffResource(Resource resource)
    {
        if(resource != null)
        {
            resource.transform.SetParent(null);
            resource.Destroy();
            IsPicked = false;
        }
        else
        {
            return;
        }
    }
}
