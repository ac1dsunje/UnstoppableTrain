using System;
using System.Collections.Generic;

[Serializable]
public class TrainStats
{
    public int stationsPassed = 0;
    public List<PassengerController> Passengers = new();
}