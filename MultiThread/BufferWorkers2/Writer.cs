using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BufferWorkers2
{
    public class Writer
    {
        #region private

        private readonly int _messagesCount;
        readonly string _name;
        readonly AutoResetEvent _bufferFullEvent;
        readonly AutoResetEvent _bufferEmptyEvent;
        List<string> _messages = new List<string>();
        StateWorker _state;

        #endregion

        public event EventHandler Finished;
        
        public Writer(int messagesCount, string name, AutoResetEvent bufferFullEvent, AutoResetEvent bufferEmptyEvent)
        {
            _messagesCount = messagesCount;
            _name = name;
            _bufferFullEvent = bufferFullEvent;
            _bufferEmptyEvent = bufferEmptyEvent;
            generateMessages();
            State = StateWorker.NotStarted;
        }

        void generateMessages()
        {
            for (var i = 0; i < _messagesCount; i++)
            {
                Messages.Add(string.Format("{0}сообщение{1}",Name,i));
            }
        }

        public void Write()
        {
            State = StateWorker.Work;
            while (Messages.Any())
            {
                if (Buffer.IsClosed)
                {
                    //генерируем ложное сообщение
                    _bufferFullEvent.Set();
                    _bufferEmptyEvent.Set();
                    break;
                }
                _bufferEmptyEvent.WaitOne();
                var message = Messages.FirstOrDefault();
                Buffer.Value = message;
                Messages.Remove(message);
                Console.WriteLine("{0} записал сообщение {1}", Task.CurrentId, message);
                _bufferFullEvent.Set();
            }
            State = StateWorker.Finish;
        }

        #region Properties

        public string Name
        {
            get { return _name; }
        }

        public List<string> Messages
        {
            get { return _messages; }
        }

        public StateWorker State
        {
            get { return _state; }
            set
            {
                _state = value;
                if (value == StateWorker.Finish)
                {
                    if (Finished != null)
                        Finished(this, EventArgs.Empty);
                }
            }
        }

        #endregion
    }
}
