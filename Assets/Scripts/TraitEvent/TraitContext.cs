using System.Collections.Generic;
using System.Linq;

public class TraitContext
{
    public List<PassengerController> AllPassengers;
    public bool ConflictStarted = false;
    public bool ConflictResolved = false;
    public PassengerController Victim = null;

    public int GetCount(Trait trait) => AllPassengers.Count(p => p.GetData.trait == trait);
}