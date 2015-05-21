using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BufferWorkers
{
    public class Reader
    {
        List<string> _messages = new List<string>();

        public void Read()
        {
            while (!Buffer.IsClosed)
            {
                if (!Buffer.IsEmpty)
                {
                    lock ("read")
                    {
                        if (!Buffer.IsEmpty)
                        {
                            Messages.Add(Buffer.Value);
                        }
                    }
                }
            }
        }

        public List<string> Messages
        {
            get { return _messages; }
        }
    }
}
