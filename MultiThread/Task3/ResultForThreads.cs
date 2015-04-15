﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
   /// <summary>
   /// Результат Пересчета для потоков
   /// </summary>
    public class ResultForThreads : DynamicObject
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
         
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name.ToLower();
            return dictionary.TryGetValue(name, out result);
        }
        
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            dictionary[binder.Name.ToLower()] = value;
            return true;
        }
    }
}
