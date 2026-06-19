using TMPro;
using UnityEngine;

public class LayingManSlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roleText;
    [SerializeField] private TextMeshProUGUI _traitText;


    public void Set(ManData data)
    {
        SetTexts(data);
    }

    private void SetTexts(ManData data)
    {
        _roleText.text = $"Role - {data.role.ToString()}";
        _traitText.text = $"Trait - {data.trait.ToString()}";
    }
}