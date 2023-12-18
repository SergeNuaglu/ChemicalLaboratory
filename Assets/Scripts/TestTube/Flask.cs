using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Flask : MonoBehaviour
{
    [SerializeField] private Material _liquidMaterial;
    [SerializeField] private Reactor _reactor;
    [SerializeField] private Color _originalLiquidColor;

    private const float FullAmount = 0.2f;
    private const float Treshhold = 0.01f;

    private float _maxZ;
    private float _currentAmount;
    private List<Reagent> _reagents = new List<Reagent>();

    public event Action<float> Entered;
    public event Action Exited;

    public Reagent LastReagent { get; private set; }
    public IReadOnlyList<Reagent> Reagents => _reagents;

    public bool IsFull
    {
        get { return FullAmount - _currentAmount <= Treshhold && Reagents.Count >= 2; }
    }

    private void Awake()
    {
        _liquidMaterial.SetColor(ShaderGraphProperties.SideColor, _originalLiquidColor);
        _liquidMaterial.SetColor(ShaderGraphProperties.TopColor, LightenTopColor(_originalLiquidColor));
        Collider collider = GetComponent<Collider>();
        _maxZ = collider.bounds.max.z;

        Empty();
    }

    private void OnDisable()
    {
        _liquidMaterial.SetFloat(ShaderGraphProperties.FillAmount, 0);
    }

    public void Fill(float amount, Reagent reagent)
    {
        float half = FullAmount / 2;
        bool isHalfFull = half - _currentAmount <= Treshhold;

        if (amount < _currentAmount)
        {
            if (isHalfFull == false)
            {
                return;
            }
            else
            {
                if (amount + half < _currentAmount)
                {
                    return;
                }

                amount += half;
            }
        }

        _liquidMaterial.SetFloat(ShaderGraphProperties.FillAmount, amount);
        _currentAmount = _liquidMaterial.GetFloat(ShaderGraphProperties.FillAmount);
        CheckNewReagent(reagent);
    }

    public void Titrate(Indicator indicator)
    {
        Color color = indicator.GetColor(_reactor.ProductEnvironment);
        _liquidMaterial.SetColor(ShaderGraphProperties.SideColor, color);
        _liquidMaterial.SetColor(ShaderGraphProperties.TopColor, LightenTopColor(color));
    }

    private void CheckNewReagent(Reagent reagent)
    {
        if (LastReagent == null)
        {
            LastReagent = reagent;
            _reagents.Add(reagent);
        }
        else if (LastReagent != reagent)
        {
            LastReagent = reagent;
            _reagents.Add(reagent);
            _reactor.PerformReaction(Reagents);
        }
    }

    private Color LightenTopColor(Color sideColor)
    {
        const float LightenFactor = 0.4f;

        float r = sideColor.r + (1 - sideColor.r) * LightenFactor;
        float g = sideColor.g + (1 - sideColor.g) * LightenFactor;
        float b = sideColor.b + (1 - sideColor.b) * LightenFactor;

        return new Color(r, g, b, sideColor.a);
    }

    private void Empty()
    {
        const float emptyValue = 0f;

        _liquidMaterial.SetFloat(ShaderGraphProperties.FillAmount, emptyValue);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<TestTube>(out TestTube _))
        {
            Entered?.Invoke(_maxZ);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent<TestTube>(out TestTube _))
        {
            Exited?.Invoke();
        }
    }
}
