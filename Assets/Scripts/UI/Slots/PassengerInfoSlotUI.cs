using TMPro;
using UnityEngine;

public class PassengerInfoSlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _roleText;
    [SerializeField] private TextMeshProUGUI _traitText;
    [SerializeField] private TextMeshProUGUI _stationsLeftText;

    private ManData _data;

    public ManData GetData => _data;

    public PassengerInfoSlotUI Initialize(ManData data)
    {
        _data = data;
        UpdateTexts();
        return this;
    }
    public void Refresh()
    {
        UpdateTexts();
    }

    private void UpdateTexts()
    {
        if (_data == null) return;
        _nameText.text = $"{_data.Name}";
        _roleText.text = $"Role: {_data.role}";
        _traitText.text = $"Trait: {_data.trait}";
        _stationsLeftText.text = $"Stations left: {_data.StationsLeft}";
    }
}