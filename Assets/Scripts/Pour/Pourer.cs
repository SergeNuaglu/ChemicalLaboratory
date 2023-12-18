using UnityEngine;

public class Pourer : MonoBehaviour
{
    [SerializeField] private AnimationCurve _scaleCurve;
    [SerializeField] private AnimationCurve _fillAmountCurve;
    [SerializeField] private TestTubeRotator _testTubeRotator;

    private void OnEnable()
    {
        _testTubeRotator.AngleChanged += OnAngleChanged;
    }

    private void OnDisable()
    {
        _testTubeRotator.AngleChanged -= OnAngleChanged;
    }

    private void Pour(float angle, Reagent reagent, Flask flask)
    {
        const float FullValue = 0.1f;
        const int PourAngle = 85;

        Material liquidMaterial = reagent.LiquidMaterial;

        liquidMaterial.SetFloat(ShaderGraphProperties.FillAmount, _fillAmountCurve.Evaluate(angle));
        liquidMaterial.SetVector(ShaderGraphProperties.ScaleMultiplier, new Vector2(_scaleCurve.Evaluate(angle), 0));

        if (angle >= PourAngle)
        {
            float amount = (FullValue / _testTubeRotator.TargetAngle) * angle;
            flask.Fill(amount, reagent);
        }
    }

    private void OnAngleChanged(float angle, Reagent reagent, Flask flask)
    {
        Pour(angle, reagent, flask);
    }
}
