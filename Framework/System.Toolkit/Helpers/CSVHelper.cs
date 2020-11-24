using System.Data;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System;
using System.Diagnostics;
namespace System.Toolkit.Helpers
{
    public class CSVHelper
    {

        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public static readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        /// <summary>
        /// 将CSV文件的数据读取到DataTable中
        /// </summary>
        /// <param name="fileName">CSV文件路径</param>
        /// <returns>返回读取了CSV数据的DataTable</returns>
        public static DataTable CSV2DataTable(string filePath)
        {
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            //StreamReader sr = new StreamReader(fs, encoding);
            //string fileContent = sr.ReadToEnd();
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        tableHead[i] = tableHead[i].Replace("\"", "");
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j].Replace("\"", "");
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[2] + " " + "DESC";
            }
            sr.Close();
            fs.Close();
            return dt;
        }

        /// <summary>
        /// 将DataTable中数据写入到CSV文件中
        /// </summary>
        /// <param name="dt">提供保存数据的DataTable</param>
        /// <param name="fileName">CSV的文件路径</param>
        public static bool DataTable2CSV(DataTable dt, string fullPath)
        {
            try
            {
                FileInfo fi = new FileInfo(fullPath);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
                FileStream fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                string data = "";
                //写出列名称
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    data += "\"" + dt.Columns[i].ColumnName.ToString() + "\"";
                    if (i < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
                //写出各行数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    data = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string str = dt.Rows[i][j].ToString();
                        str = string.Format("\"{0}\"", str);
                        data += str;
                        if (j < dt.Columns.Count - 1)
                        {
                            data += ",";
                        }
                    }
                    sw.WriteLine(data);
                }
                sw.Close();
                fs.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改文件名称 
        /// </summary>
        /// <param name="OldPath">旧的路径 完整的物理路径</param>
        /// <param name="NewPath">新的路径</param>
        /// <returns></returns>
        public static bool ChangeFileName(string OldPath, string NewPath)
        {
            bool re = false;
            //OldPath = HttpContext.Current.Server.MapPath(OldPath);虚拟的
            //NewPath = HttpContext.Current.Server.MapPath(NewPath);
            try
            {
                if (File.Exists(OldPath))
                {
                    File.Move(OldPath, NewPath);
                    re = true;
                }
            }
            catch
            {
                re = false;
            }
            return re;
        }

        //直接在网页表单提交数据保存在csv文件中 直接写入文件
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static bool SaveCSV(string fullPath, string Data)
        {
            bool re = true;
            try
            {
                FileStream FileStream = new FileStream(fullPath, FileMode.Append,FileAccess.Write,FileShare.ReadWrite);
                //FileStream FileStream = new FileStream(fullPath, FileMode.Append,FileAccess.Write);
                //FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                //StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
                StreamWriter sw = new StreamWriter(FileStream, System.Text.Encoding.UTF8);
                sw.WriteLine(Data);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                FileStream.Close();
            }
            catch
            {
                re = false;
            }
            return re;
        }

        //电池型号,时间,电池条码,开路电压,内阻,测试结果，上传结果
        public static bool SaveData2CSV(string fullPath, string Data,string Header)
        {
            bool re = true;
            try
            {
                FileInfo fi = new FileInfo(fullPath);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }

                if (!File.Exists(fullPath))
                {
                    SaveCSV(fullPath, Header);
                }

                IntPtr vHandle = _lopen(fullPath, OF_READWRITE | OF_SHARE_DENY_NONE);
                if (vHandle == HFILE_ERROR)
                {
                    //MessageBox.Show("数据文件被占用！");
                    //string str = "";
                    Process[] processes;
                    //Get the list of current active processes.
                    processes = System.Diagnostics.Process.GetProcesses();
                    foreach (var item in processes)
                    {
                        if (item.ProcessName.ToUpper() == "EXCEL")
                        {
                            item.Kill();
                        }
                    }
                    ////Grab some basic information for each process.
                    //Process process;
                    //for (int i = 0; i < processes.Length - 1; i++)
                    //{
                    //    process = processes[i];
                    //    str = str + Convert.ToString(process.Id) + " : " +
                    //    process.ProcessName + "\r\n";
                    //}
                    ////Display the process information to the user
                    //MessageBox.Show(str);
                }
                CloseHandle(vHandle);
                //MessageBox.Show("没有被占用！");

                SaveCSV(fullPath, Data);
            }
            catch
            {
                re = false;
            }
            return re;
        }

    }
}