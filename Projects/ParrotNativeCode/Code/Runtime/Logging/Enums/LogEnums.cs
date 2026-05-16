namespace ParrotCode.Native.Common
{
    /// <summary>
    /// The filter for setting log type.
    /// </summary>
    public enum LogVerbosity
    {
        Debug,
        Warning,
        Error,
        Exception,
        Assert
    }

    /// <summary>
    /// The channel used to log messages.
    /// </summary>
    public enum LogChannel
    {
        Audio,
        Assets,
        General,
        Events,
        UI,
        GraphSystem,
        Scripting,
        Internal,
        InputSystem
    }
}
