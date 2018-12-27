public class AgentDiedEventBusData : EventBusData
{
    #region Fields

    public readonly DeadAgentData DeadAgent;

    #endregion

    public AgentDiedEventBusData(DeadAgentData deadAgent)
    {
        DeadAgent = deadAgent;
    }
}