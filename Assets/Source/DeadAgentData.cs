using System;

public class DeadAgentData
{
    #region Fields

    public readonly int Id;

    public readonly Type AgentType;

    public readonly int AscensionLevel;

    public readonly bool IsPlayer;

    public readonly bool Succeeded;

    public DeadAgentData(int id, Type agentType, bool succeeded, int ascensionLevel, bool isPlayer)
    {
        Id = id;
        AgentType = agentType;
        Succeeded = succeeded;
        AscensionLevel = ascensionLevel;
        IsPlayer = isPlayer;
    }

    #endregion
}