using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientCtrl
{
    class IpInfoBean
    {
        public int code;
        public IpAddressInfo data;

       public class IpAddressInfo {
            public String region;
            public String city;
        } 
    }
    
}
