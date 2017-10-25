using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClientCtrl
{
    class UIAppList
    {
        internal static void updataAppsData(DataGridView dataGridView_apps, List<AppInfoBean> appList)
        {
            if (appList == null || appList.Count == 0)
            {
                return;
            }

            Console.WriteLine("文件路径1==" + Environment.CurrentDirectory.ToString());

            foreach (AppInfoBean info in appList)
            {
                int index = dataGridView_apps.Rows.Add();
                //((DataGridViewImageCell)dataGridView_apps.Rows[index].Cells[0]).Value = Application.StartupPath.ToString() + "/images/icon_app_defout.png";

                dataGridView_apps.Rows[index].Cells[1].Value = info.name;

                dataGridView_apps.Rows[index].Cells[1].Value = info.name;
                dataGridView_apps.Rows[index].Cells[2].Value = info.isSystem;
                dataGridView_apps.Rows[index].Cells[3].Value = info.packageName;
                dataGridView_apps.Rows[index].Cells[4].Value = info.versionName;

            }


        }

        internal static void updataPhoneInfo(ListBox listBox, ClientInfoBean clientInfo)
        {
            if (listBox.Items.Count > 0)
            {
                //清空所有项
                listBox.Items.Clear();
            }
            if (clientInfo == null)
            {
                return;


            }
            listBox.Items.Add("设备标识：" + clientInfo.deviceId);
            listBox.Items.Add("备注：" + clientInfo.alias);
            listBox.Items.Add("手机型号：" + clientInfo.phoneModle);
            listBox.Items.Add("系统版本：" + clientInfo.androidVersion);
            listBox.Items.Add("屏幕分辨率：" + clientInfo.screenSize);
            listBox.Items.Add("系统版本：" + clientInfo.androidVersion);
            listBox.Items.Add("地址：" + clientInfo.address);
        }

        internal static void updataSmsList(DataGridView dataGridView_sms, List<SmsInfoBean> smsList)
        {
            if (smsList==null) {
                return;
            }
            foreach (SmsInfoBean sms in smsList) {
                int index = dataGridView_sms.Rows.Add();
                dataGridView_sms.Rows[index].Cells[0].Value = sms.address;
                dataGridView_sms.Rows[index].Cells[1].Value = sms.type == 2;
                dataGridView_sms.Rows[index].Cells[2].Value = sms.body;
                dataGridView_sms.Rows[index].Cells[3].Value = AllUtils.getTimeFormat(sms.date);
            }
        }
    }
}
