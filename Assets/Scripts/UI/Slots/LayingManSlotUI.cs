using TMPro;
using UnityEngine;

public class LayingManSlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _roleText;
    [SerializeField] private TextMeshProUGUI _traitText;
    [SerializeField] private TextMeshProUGUI _stationsText;

    public void Set(ManData data)
    {
        SetTexts(data);
    }

    private void SetTexts(ManData data)
    {
        _nameText.text = $"{data.Name}";
        _roleText.text = $"Role: {data.role.ToString()}";
        _traitText.text = $"Trait: {data.trait.ToString()}";
        _stationsText.text = $"Stations: {data.StationsNeeded.ToString()}";
    }
}