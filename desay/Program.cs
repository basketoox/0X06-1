using System;
using System.Windows.Forms;
using log4net;
using System.Toolkit;
using System.Toolkit.Helpers;
using System.Threading;
using desay.ProductData;
using Vision_Assistant;

namespace desay
{
    static class Program
    {

        static ILog log = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            InitialPalte();
            bool isRunning;
            Mutex mutex = new Mutex(true, "RunOneInstanceOnly", out isRunning);

            if (isRunning)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += new ThreadExceptionEventHandler(UI_ThreadException);//处理UI线程异常
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);//处理非UI线程异常

                //加载配置文件
                try
                {
                    Config.Instance = SerializerManager<Config>.Instance.Load(AppConfig.ConfigFileName);
                }
                catch { MessageBox.Show("Config.xml出错"); Application.Exit(); }
                try
                {
                    AxisParameter.Instance = SerializerManager<AxisParameter>.Instance.Load(AppConfig.ConfigAxisName);
                }
                catch { MessageBox.Show("AxisParameter.xml出错"); Application.Exit(); }
                Thread.Sleep(200);
                try
                {
                    Position.Instance = SerializerManager<Position>.Instance.Load(AppConfig.ConfigPositionName);
                }
                catch { MessageBox.Show("Position.xml出错"); Application.Exit(); }
                try
                {
                    Delay.Instance = SerializerManager<Delay>.Instance.Load(AppConfig.ConfigDelayName);
                }
                catch { MessageBox.Show("Delay.xml出错"); Application.Exit(); }

                //SerializerManager<Config>.Instance.Save(AppConfig.ConfigFileName,Config.Instance);
                //SerializerManager<Position>.Instance.Save(AppConfig.ConfigPositionName,Position.Instance);
                //Application.Run(new frmMain());
                Application.Run(new Form2());

            }
            else
            {
                MessageBox.Show("程序已经启动！");
            }
        }
        #region jj
        static string aaPlateRecord;
        public static void InitialPalte()
        {
            aaPlateRecord = "";
            for (int i = 0; i < 100; i++)
            {
                aaPlateRecord += i.ToString() + " ";
            }
            string[] emptys = aaPlateRecord.Split(' ');
            Console.WriteLine();
        }
        #endregion
        /// <summary>
        /// 处理UI线程异常
        /// </summary>
        static void UI_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Global.isErrorExit = true;
            // SerializerManager<Config>.Instance.Save(AppConfig.ConfigFileName,Config.Instance);

            //Thread.Sleep(200);
            //SerializerManager<Position>.Instance.Save(AppConfig.ConfigPositionName, Position.Instance);
            //SerializerManager<DbModelParam>.Instance.Save(AppConfig.ConfigPositionName, DbModelParam.Instance);
            log.Fatal(e.Exception.Message);
            Application.Exit();
        }
        /// <summary>
        /// 处理非UI线程异常
        /// </summary>
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Global.isErrorExit = true;
            //SerializerManager<Config>.Instance.Save(AppConfig.ConfigFileName, Config.Instance);

            //Thread.Sleep(200);
            //SerializerManager<Position>.Instance.Save(AppConfig.ConfigPositionName, Position.Instance);
            //SerializerManager<DbModelParam>.Instance.Save(AppConfig.ConfigPositionName, DbModelParam.Instance);
            log.Fatal(e.ExceptionObject.ToString());
            Application.Exit();
        }
    }
}
