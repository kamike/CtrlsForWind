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
    }
}
