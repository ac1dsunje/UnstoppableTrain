public struct RoleWeight
{
    public Role Role;
    public float Weight;

    public RoleWeight(Role role, float weight)
    {
        Role = role;
        Weight = weight;
    }
}

//ToDo : use SO and send it to selector in bootstrap
public static class RolePresets
{
    public static readonly RoleWeight[] Easy = new[]
    {
        new RoleWeight(Role.Driver,     30f),
        new RoleWeight(Role.Mechanic,   30f),
        new RoleWeight(Role.Doctor,     30f),
        new RoleWeight(Role.NoSkill,    10f),
    };

    public static readonly RoleWeight[] Normal = new[]
    {
        new RoleWeight(Role.Driver,     25f),
        new RoleWeight(Role.Mechanic,   25f),
        new RoleWeight(Role.Doctor,     25f),
        new RoleWeight(Role.NoSkill,    25f),
    };

    public static readonly RoleWeight[] Hardcore = new[]
    {
        new RoleWeight(Role.Driver,     20f),
        new RoleWeight(Role.Mechanic,   20f),
        new RoleWeight(Role.Doctor,     20f),
        new RoleWeight(Role.NoSkill,    40f),
    };
}