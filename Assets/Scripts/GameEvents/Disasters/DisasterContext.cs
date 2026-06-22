using System.Collections.Generic;

public class DisasterContext
{
    public List<PassengerController> AllPassengers;
    public List<PassengerController> Victims = new List<PassengerController>();
    public List<PassengerController> Healed = new List<PassengerController>();
    public bool DisasterResolved = false;
}