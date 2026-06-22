public enum Role
{
    Driver,
    Mechanic,
    Doctor,
    NoSkill
}

public static class RoleFactory
{
    public static int CountRole(System.Collections.Generic.List<PassengerController> passengers, Role role)
    {
        int count = 0;
        foreach (var p in passengers)
            if (p.GetData.role == role) count++;
        return count;
    }

    public static PassengerController FindFirst(System.Collections.Generic.List<PassengerController> passengers, Role role)
    {
        foreach (var p in passengers)
            if (p.GetData.role == role) return p;
        return null;
    }
}