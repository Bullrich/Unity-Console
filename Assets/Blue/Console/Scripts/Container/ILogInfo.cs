using UnityEngine;

namespace Blue.Console.Container
{
    public interface ILogInfo
    {
        string Log { get; }
        string Stack { get; }
        LogType Type { get; }
    }
}