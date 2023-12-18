using TMPro;
using UnityEngine;

public class Display : MonoBehaviour
{
    [SerializeField] private TMP_Text _dataText;
    [SerializeField] private CanvasGroup _canvasGroup;

    public void DisplayData(string data)
    {
        _dataText.text = data;
    }

    public void TurnOn()
    {
        _canvasGroup.alpha = 1;
    }

    public void TurnOff()
    {
        _canvasGroup.alpha = 0;
    }
}
