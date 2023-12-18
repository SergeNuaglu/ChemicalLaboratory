using UnityEngine;

public class PouringEffect : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private TestTubeRotator _testTubeRotator;
    [SerializeField] private ParticleSystem _smookeEffect;
    [SerializeField] private AudioSource _pourSound;

    private void OnEnable()
    {
        _testTubeRotator.AngleChanged += OnAngleChanged;
        _testTubeRotator.Ended += OnRotationEnded;
    }

    private void OnDisable()
    {
        _testTubeRotator.AngleChanged -= OnAngleChanged;
        _testTubeRotator.Ended -= OnRotationEnded;

    }

    private void OnAngleChanged(float angle, Reagent reagent, Flask flask)
    {
        const int PourAngle = 85;

        if (angle >= PourAngle && _lineRenderer.enabled == false)
        {
            _lineRenderer.startColor = reagent.LiquidMaterial.GetColor(ShaderGraphProperties.SideColor);
            _lineRenderer.endColor = reagent.LiquidMaterial.GetColor(ShaderGraphProperties.TopColor);
            _lineRenderer.enabled = true;

            TryPlayPourSound();
            TryShowSmooke(flask, reagent);
        }
    }

    private void TryShowSmooke(Flask flask, Reagent reagent)
    {
        if (_smookeEffect.isPlaying == false)
        {
            bool isOtherReagent = flask.LastReagent != null && flask.LastReagent != reagent;

            if (isOtherReagent && reagent is not Indicator)
            {
                _smookeEffect.Play();
            }
        }
    }

    private void TryPlayPourSound()
    {
        if (_pourSound.isPlaying == false)
        {
            _pourSound.Play();
        }
    }

    private void OnRotationEnded()
    {
        _lineRenderer.enabled = false;
        _pourSound.Stop();
    }
}
