using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientCtrl
{
    class ClientInfoBean
    {

        public String deviceId;

        public String alias;

        public String phoneModle;

        /**
         * 双卡,手机号码
         */
        public String phoneNumber;

        public String androidVersion;

        public String screenSize;

        public String address;


        public List<AppInfoBean> appList;

        public bool isInterceptSMS;
       // public ArrayList<SmsInfoBean> smsList;
    }
}
