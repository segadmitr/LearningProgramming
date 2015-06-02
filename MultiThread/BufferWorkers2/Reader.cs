using System.Collections.Generic;
using System.Threading;

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
            while (true)
            {
                _bufferFullEvent.WaitOne();
                if (Buffer.IsClosed)
                    break;

                Messages.Add(Buffer.Value);
                _bufferEmptyEvent.Set();
            }
        }

        public List<string> Messages
        {
            get { return _messages; }
        }
    }
}
