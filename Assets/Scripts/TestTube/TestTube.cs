using System;
using UnityEngine;

public class TestTube : MonoBehaviour
{
    [SerializeField] private Reagent _reagent;
    [SerializeField] private Display _reagentDisplay;
    [SerializeField] private Renderer _liquidRenderer;
    [SerializeField] private bool _isEmpty;

    public event Action<TestTube> DragStarted;
    public event Action DragEnded;

    public Reagent Reagent => _reagent;
    public bool IsEmpty => _isEmpty;

    private void Start()
    {
        if (_isEmpty == false)
        {
            Fill();
            _liquidRenderer.material = _reagent.LiquidMaterial;
            _liquidRenderer.enabled = true;
            _reagentDisplay.DisplayData(_reagent.Name);
            _reagentDisplay.TurnOn();
        }
        else
        {
            _liquidRenderer.enabled = false;
            _reagentDisplay.TurnOff();
        }
    }

    public void SetIsEmpty()
    {
        _isEmpty = true;
    }

    public void Fill()
    {
        const float AmountFullValue = 0.5f;
        const float ScaleFullValue = 1f;

        Material liquidMaterial = _reagent.LiquidMaterial;
        liquidMaterial.SetFloat(ShaderGraphProperties.FillAmount, AmountFullValue);
        liquidMaterial.SetVector(ShaderGraphProperties.ScaleMultiplier, new Vector2(ScaleFullValue, 0));
    }

    private void OnMouseDown()
    {
        _reagentDisplay.TurnOff();
        DragStarted?.Invoke(this);
    }

    private void OnMouseUp()
    {
        if (_isEmpty == false)
        {
            _reagentDisplay.TurnOn();
        }

        DragEnded?.Invoke();
    }
}
