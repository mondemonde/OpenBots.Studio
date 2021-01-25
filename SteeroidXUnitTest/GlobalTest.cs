using System;
using System.Collections.Generic;
using System.Text;

namespace SteeroidXUnitTest
{
   public static  class GlobalTest
    {
        public static string ServiceBusConnectionString {
            get
            {
                return @"Endpoint=sb://devnote.servicebus.windows.net/;SharedAccessKeyName=mac3Client;SharedAccessKey=Byg7cxUw4KwmmimkUZds1bD7XtPLBvIx3MtAYsWFl3c=";
            }
        }
    }
}
