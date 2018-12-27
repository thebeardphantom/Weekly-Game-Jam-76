public abstract class EventBusData
{
    #region Fields

    public EventBusData Sender;

    #endregion

    #region Methods

    public bool ExistsUpstream(EventBusData data)
    {
        var test = this;
        while (test != null)
        {
            if (test == data)
            {
                return true;
            }

            test = test.Sender;
        }

        return false;
    }

    public T ExistsUpstream<T>() where T : EventBusData
    {
        var testType = typeof(T);
        var test = this;
        while (test != null)
        {
            if (test.GetType() == testType)
            {
                return (T)test;
            }

            test = test.Sender;
        }

        return null;
    }

    #endregion
}