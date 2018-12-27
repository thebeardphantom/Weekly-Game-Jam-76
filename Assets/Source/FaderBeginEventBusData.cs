public class FaderBeginEventBusData : EventBusData
{
    #region Fields

    public readonly bool FadingIn;

    #endregion

    public FaderBeginEventBusData(bool fadingIn)
    {
        FadingIn = fadingIn;
    }
}