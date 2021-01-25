using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QR.API.Mapper.Std
{
  public static  class MyEnumHelper
    {
        public static Dictionary<int, string> GetDictionaryFromEnum<T>() where T : System.Enum
        {
            Dictionary<int, string> dictionary  = new Dictionary<int, string>(); 

            foreach (var e in Enum.GetValues(typeof(T)))
            {
               dictionary.Add((int)e, e.ToString());
            }
            return dictionary;
        }
    }
}
