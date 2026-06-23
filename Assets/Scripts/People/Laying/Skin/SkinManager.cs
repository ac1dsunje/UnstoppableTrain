using UnityEngine;

public class SkinManager : MonoBehaviour
{
    [SerializeField] private Material _wallflowerMat;
    [SerializeField] private Material _psychopathMat;
    [SerializeField] private Material _leaderMat;
    [SerializeField] private Material _empathMat;

    private ISkin _skinHolder;

    private void Awake()
    {
        _skinHolder = GetComponent<ISkin>();
    }

    private void OnEnable()
    {
        _skinHolder.OnManDataInitialized += ApplySkin;
    }

    private void OnDisable()
    {
        _skinHolder.OnManDataInitialized -= ApplySkin;
    }

    private void ApplySkin(ManData data)
    {
        ApplyMaterial(data.trait);
        ApplyHat(data.role);
    }

    private void ApplyMaterial(Trait trait)
    {
        Material mat = null;
        Material current = _skinHolder.GetShape().material;
        switch (trait)
        {
            case Trait.Wallflower:
                mat = _wallflowerMat;
                break;

            case Trait.Psychopath:
                mat = _psychopathMat;
                break;

            case Trait.Leader:
                mat = _leaderMat;
                break;

            case Trait.Empath:
                mat = _empathMat;
                break;
        }
        _skinHolder.GetShape().material = mat?? current;
    }

    private void ApplyHat(Role role)
    {
        switch (role)
        {
            case Role.NoSkill:
                Debug.Log("Apply NoSkill hat");
                break;

            case Role.Doctor:
                Debug.Log("Apply Doctor hat");
                break;

            case Role.Driver:
                Debug.Log("Apply Driver hat");
                break;

            case Role.Mechanic:
                Debug.Log("Apply Mechanic hat");
                break;
        }
    }
}