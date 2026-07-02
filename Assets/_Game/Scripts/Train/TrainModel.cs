public class TrainModel
{
    public int MaxAmount { get; private set; }
    public float MoveSpeed { get; private set; }

    public TrainStats Stats { get; private set; } = new();


    public RoadController CurrentRoad { get; set; }

    public TrainModel(TrainDataSO data)
    {
        MaxAmount = data.MaxAmount;
        MoveSpeed = data.MoveSpeed;
    }
}