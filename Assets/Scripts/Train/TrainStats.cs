using System;
using System.Collections.Generic;

[Serializable]
public class TrainStats
{
    public int chunksPassed = 0;
    public List<PassengerController> Passengers = new();
}