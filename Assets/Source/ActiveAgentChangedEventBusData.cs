public class ActiveAgentChangedEventBusData : EventBusData
{
    #region Fields

    public readonly Agent Previous;

    #endregion

    public ActiveAgentChangedEventBusData(Agent previous)
    {
        Previous = previous;
    }
}