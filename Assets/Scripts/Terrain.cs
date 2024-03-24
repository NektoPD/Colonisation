using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Terrain : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent OnTerrainClicked = new UnityEvent();

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTerrainClicked?.Invoke();
    }
}
