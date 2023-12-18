using System.Collections.Generic;
using UnityEngine;

public class Reactor : MonoBehaviour
{
    [SerializeField] private ReactionData _reactionsData;
    [SerializeField] private Display _reactionDisplay;

    public EnvironmentType ProductEnvironment { get; private set; }

    public void PerformReaction(IReadOnlyList<Reagent> reagents)
    {
        const int MaxReagentAmount = 2;

        if (reagents.Count == MaxReagentAmount)
        {
            string firstSymbol = reagents[0].Symbol;
            string secondSymbol = reagents[1].Symbol;

            foreach (Reaction reaction in _reactionsData.Reactions)
            {
                if (firstSymbol + secondSymbol == reaction.Key || secondSymbol + firstSymbol == reaction.Key)
                {
                    _reactionDisplay.DisplayData($"{firstSymbol} + {secondSymbol} → {reaction.Product}");
                    ProductEnvironment = reaction.ProductEnvironment;
                    return;
                }
            }

            _reactionDisplay.DisplayData($"{firstSymbol}, {secondSymbol}");
        }
    }
}
