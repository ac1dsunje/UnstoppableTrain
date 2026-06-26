using System.Linq;
using System.Collections.Generic;

public enum Role
{
    Driver,
    Mechanic,
    Doctor,
    NoSkill
}

public static class RoleStatistics
{
    public static int CountRole(List<PassengerController> passengers, Role role)
    {
        return passengers.Count(p => p.GetData.role == role);
    }
}