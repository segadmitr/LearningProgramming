using System;
using System.Collections.Generic;
using System.Linq;

namespace BufferWorkers
{
    public class Writer
    {
        #region private

        private readonly int _messagesCount;
        readonly string _name;
        List<string> _messages = new List<string>();
        StateWorker _state;

        #endregion

        public event EventHandler Finished;
        
        public Writer(int messagesCount, string name)
        {
            _messagesCount = messagesCount;
            _name = name;
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
            while(Messages.Any())
            {
                lock ("write")
                {
                    if (Buffer.IsEmpty)
                    {
                        var message = Messages.FirstOrDefault();
                        Buffer.Value = message;
                        Messages.Remove(message);
                    }
                }
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
