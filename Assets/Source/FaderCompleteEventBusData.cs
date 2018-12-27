public class FaderCompleteEventBusData : EventBusData
{
    #region Fields

    public readonly bool FadingIn;

    #endregion

    public FaderCompleteEventBusData(bool fadingIn)
    {
        FadingIn = fadingIn;
    }
}