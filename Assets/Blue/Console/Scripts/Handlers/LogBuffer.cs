using System.Collections.Generic;
using Blue.Console.Container;
using UnityEngine;

namespace Blue.Console.Handler
{
    // By @Bullrich
    public class LogBuffer
    {
        private readonly Queue<LogInfo> _logs;
        private readonly int _length;
        public int Count
        {
            get { return _logs.Count; }
        }
        
        public LogBuffer(int length = 1000)
        {
            _logs = new Queue<LogInfo>(length);
            _length = length;
        }

        public void Clear()
        {
            for (int i = 0; i < Count; i++)
            {
                Pop();
            }
            _logs.Clear();
        }

        public void Push(LogInfo element)
        {
            // TODO: Object pool
            if(_logs.Count == _length)
                Pop();
            _logs.Enqueue(element);
        }

        private void Pop()
        {
            Object.Destroy(_logs.Dequeue());
        }

        public LogInfo[] GetLogs()
        {
            return _logs.ToArray();
        }
    }
}