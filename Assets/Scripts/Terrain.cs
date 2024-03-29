using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Terrain : MonoBehaviour, IPointerClickHandler
{
    public event Action OnTerrainClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTerrainClicked?.Invoke();
    }
}
