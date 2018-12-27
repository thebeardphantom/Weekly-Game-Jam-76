using System;

public interface IAgentSpawner
{
    #region Methods

    Agent Prefab { get; }

    Agent SpawnOne();

    #endregion
}