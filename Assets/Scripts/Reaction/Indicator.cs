using UnityEngine;

[CreateAssetMenu(fileName = "NewIndicator", menuName = "ReactionData/Indicator", order = 51)]
public class Indicator : Reagent
{
    [SerializeField] private Color _neutralColor;
    [SerializeField] private Color _acidicColor;
    [SerializeField] private Color _alkalineColor;

    public Color GetColor(EnvironmentType environMent)
    {
        switch (environMent)
        {
            case EnvironmentType.Neutral:
                return _neutralColor;
            case EnvironmentType.Alkaline:
                return _alkalineColor;
            case EnvironmentType.Acidic:
                return _acidicColor;
            default:
                return _neutralColor;
        }
    }
}

