using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Terrain : MonoBehaviour, IPointerClickHandler
{
    public event Action TerrainClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        TerrainClicked?.Invoke();
    }
}
