using System.Collections.Generic;
using UnityEngine;

public class TestTubeMover : MonoBehaviour
{
    [SerializeField] private List<TestTube> _testTubes;
    [SerializeField] private Flask _flask;
    [SerializeField] private float _minX = -2.5f;
    [SerializeField] private float _maxX = 2.55f;
    [SerializeField] private float _minY = 0f;
    [SerializeField] private float _maxY = 1f;

    private bool _isDragging = false;
    private float _forwardShift;
    private float _distanceToCamera;
    private Vector3 _initialPosition;
    private Transform _currentTestTube;
    private Camera _mainCamera;

    private void OnEnable()
    {
        _flask.Entered += OnFlaskEntered;
        _flask.Exited += OnFlaskExited;

        foreach (TestTube testTube in _testTubes)
        {
            testTube.DragStarted += OnDragStarted;
            testTube.DragEnded += OnDragEnded;
        }
    }

    private void Start()
    {
        _initialPosition = transform.position;
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (_isDragging)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(_distanceToCamera);

            float clampedX = Mathf.Clamp(rayPoint.x, _minX, _maxX);
            float clampedY = Mathf.Clamp(rayPoint.y, _minY, _maxY);

            _currentTestTube.position = new Vector3(clampedX, clampedY, _initialPosition.z - _forwardShift);
        }
    }

    private void OnDisable()
    {
        _flask.Entered -= OnFlaskEntered;
        _flask.Exited -= OnFlaskExited;

        foreach (TestTube testTube in _testTubes)
        {
            testTube.DragStarted -= OnDragStarted;
            testTube.DragEnded -= OnDragEnded;
        }
    }

    private void OnFlaskEntered(float forwardShift)
    {
        _forwardShift = forwardShift;
    }

    private void OnFlaskExited()
    {
        _forwardShift = 0;
    }

    private void OnDragStarted(TestTube testTube)
    {
        _isDragging = true;
        _currentTestTube = testTube.transform;
        _distanceToCamera = Vector3.Distance(_currentTestTube.position, _mainCamera.transform.position);
    }

    private void OnDragEnded()
    {
        _isDragging = false;
        _currentTestTube.position = new Vector3(_currentTestTube.position.x, _initialPosition.y, _initialPosition.z - _forwardShift);
    }
}
