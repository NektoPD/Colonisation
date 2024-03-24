using UnityEngine;

public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Vector3 _targetPosition;
    private bool _haveTarget = false;

    private void Update()
    {
        if (_haveTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

            if (transform.position == _targetPosition)
            {
                _haveTarget = false;
            }
        }
    }

    public void SetTarget(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        _haveTarget = true;
    }
}