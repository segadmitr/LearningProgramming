using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BufferWorkers2
{
    public class Reader
    {
        #region private

        readonly AutoResetEvent _bufferFullEvent;
        readonly AutoResetEvent _bufferEmptyEvent;
        List<string> _messages = new List<string>();

        #endregion

        public Reader(AutoResetEvent bufferFullEvent, AutoResetEvent bufferEmptyEvent)
        {
            _bufferFullEvent = bufferFullEvent;
            _bufferEmptyEvent = bufferEmptyEvent;
        }

        public void Read()
        {
            while (!Buffer.IsClosed)
            {
                if (Buffer.IsClosed)
                {
                    //генерируем ложное сообщение
                    _bufferFullEvent.Set();
                    _bufferEmptyEvent.Set();
                    break;
                }
                _bufferFullEvent.WaitOne();
                var message = Buffer.Value;
                Messages.Add(message);
                Console.WriteLine("{0} прочитал сообщение {1}", Task.CurrentId, message);
                _bufferEmptyEvent.Set();
            }
        }

        public List<string> Messages
        {
            get { return _messages; }
        }
    }
}
