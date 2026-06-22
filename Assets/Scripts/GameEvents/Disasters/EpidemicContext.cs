using System.Collections.Generic;

public class EpidemicContext
{
    public List<PassengerController> AllPassengers;
    public List<PassengerController> Victims = new List<PassengerController>();
    public List<PassengerController> Healed = new List<PassengerController>();
}