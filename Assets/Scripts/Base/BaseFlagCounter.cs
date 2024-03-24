using UnityEngine;

public class BaseFlagCounter : MonoBehaviour
{
    private int _minCount = 0;
    private int _maxCount = 1;

    public int Count { get; private set; }

    private void Awake()
    {
        Count = _maxCount;
    }

    public void ClearCount()
    {
        Count = _minCount;
    }

    public void RestoreCount()
    {
        Count = _maxCount;
    }
}
