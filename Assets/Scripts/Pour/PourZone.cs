using System;
using UnityEngine;

public class PourZone : MonoBehaviour
{
    [SerializeField] private Side _side;
    [SerializeField] private GameObject _targetPoint;

    public event Action<TestTube, PourZone> Entered;
    public event Action Exited;

    public Side Side => _side;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<TestTube>(out TestTube testTube))
        {
            _targetPoint.SetActive(true);
            Entered?.Invoke(testTube, this);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent<TestTube>(out TestTube _))
        {
            _targetPoint.SetActive(false);
            Exited?.Invoke();
        }
    }
}

public enum Side
{
    Left,
    Right
};
