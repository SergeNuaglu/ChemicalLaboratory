
using UnityEngine;

[CreateAssetMenu(fileName = "NewReagent", menuName = "ReactionData/Reagent", order = 51)]
public class Reagent : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _symbol;
    [SerializeField] private Material _liquidMaterial;

    public string Name => _name;
    public string Symbol => _symbol;
    public Material LiquidMaterial => _liquidMaterial;
}
