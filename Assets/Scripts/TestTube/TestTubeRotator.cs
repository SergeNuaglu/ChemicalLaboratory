using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTubeRotator : MonoBehaviour
{
    [SerializeField] private Flask _flask;
    [SerializeField] private List<PourZone> _pourZones;
    [SerializeField] private float _pouringRadius;
    [SerializeField] private float _duration;
    [SerializeField] private float _targetAngle = 95f;

    private bool _hasTestTube;
    private bool _canRotate;
    private TestTube _currentTestTube;
    private PourZone _currentPourZone;
    private float _currentTargetAngle;
    private Coroutine _rotationRoutine;

    public event Action<float, Reagent, Flask> AngleChanged;
    public event Action Ended;

    public float TargetAngle => _targetAngle;

    private void OnEnable()
    {
        foreach (PourZone pourZone in _pourZones)
        {
            pourZone.Entered += OnPourZoneEntered;
            pourZone.Exited += OnPourZoneExited;
        }
    }

    private void Update()
    {
        if (_hasTestTube == false)
        {
            return;
        }

        if (_canRotate == false)
        {
            if (CheckPouringRadius())
            {
                _canRotate = true;
                _rotationRoutine = StartCoroutine(RotateTestTube(_currentTargetAngle));
            }
        }
        else
        {
            if (CheckPouringRadius() == false)
            {
                StopCoroutine(_rotationRoutine);
                _rotationRoutine = null;
                _currentTestTube.transform.eulerAngles = Vector3.zero;
                _canRotate = false;
                Ended?.Invoke();

                if (_currentTestTube.IsEmpty == false)
                {
                    _currentTestTube.Fill();
                }
            }
        }
    }

    private void OnDisable()
    {
        foreach (PourZone pourZone in _pourZones)
        {
            pourZone.Entered -= OnPourZoneEntered;
            pourZone.Exited -= OnPourZoneExited;
        }
    }

    private bool CheckPouringRadius()
    {
        float distance = Vector3.Distance(_currentTestTube.transform.position, _currentPourZone.transform.position);
        return distance <= _pouringRadius;
    }

    private void OnPourZoneEntered(TestTube testTube, PourZone pourZone)
    {
        if (_flask.IsFull && testTube.Reagent is not Indicator)
        {
            return;
        }

        _hasTestTube = true;
        _currentTestTube = testTube;
        _currentPourZone = pourZone;
        _currentTargetAngle = pourZone.Side == Side.Left ? _targetAngle * -1 : _targetAngle;

        if (CheckPouringRadius())
        {
            _canRotate = true;
            _rotationRoutine = StartCoroutine(RotateTestTube(_currentTargetAngle));
        }
    }

    private void OnPourZoneExited()
    {
        _hasTestTube = false;
    }

    private IEnumerator RotateTestTube(float targetAngle)
    {
        float time = 0;
        float angle;
        float lerpValue;

        Transform testTubeTransform = _currentTestTube.transform;

        while (time < _duration)
        {
            lerpValue = time / _duration;
            angle = Mathf.Lerp(0, targetAngle, lerpValue);
            testTubeTransform.eulerAngles = new Vector3(0, 0, angle);

            if (_currentTestTube.IsEmpty == false)
            {
                AngleChanged?.Invoke(Mathf.Abs(angle), _currentTestTube.Reagent, _flask);
            }

            time += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        testTubeTransform.eulerAngles = new Vector3(0, 0, targetAngle);
        Ended?.Invoke();
        _currentTestTube.SetIsEmpty();

        if (_flask.IsFull)
        {
            if (_currentTestTube.Reagent is Indicator)
            {
                Indicator indicator = (Indicator)_currentTestTube.Reagent;
                _flask.Titrate(indicator);
            }
        }
    }
}
