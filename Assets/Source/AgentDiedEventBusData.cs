public class AgentDiedEventBusData : EventBusData
{
    #region Fields

    public readonly Agent Agent;

    #endregion

    public AgentDiedEventBusData(Agent agent)
    {
        Agent = agent;
    }
}