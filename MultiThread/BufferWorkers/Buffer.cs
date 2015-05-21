using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BufferWorkers
{
    public static class Buffer
    {
        static string s_value;
        
        public static bool IsEmpty
        {
            get { return string.IsNullOrEmpty(s_value); }
        }

        public static string Value
        {
            get
            {
                var value = s_value;
                s_value = null;
                return value;
            }
            set { s_value = value; }
        }

        public static bool IsClosed
        {
            get;
            set;
        }
    }
}
