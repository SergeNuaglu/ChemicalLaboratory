using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewData", menuName = "ReactionData/Data", order = 51)]
public class ReactionData : ScriptableObject
{
    [SerializeField] private List<Reaction> _reactions;

    public IReadOnlyList<Reaction> Reactions => _reactions;
}

[System.Serializable]
public class Reaction
{
    [SerializeField] private Reagent _firstReagent;
    [SerializeField] private Reagent _secondReagent;
    [SerializeField] private string _product;
    [SerializeField] private EnvironmentType _productEnvironment;

    public string Key => _firstReagent.Symbol + _secondReagent.Symbol;
    public string Product => _product;
    public EnvironmentType ProductEnvironment => _productEnvironment;
}
