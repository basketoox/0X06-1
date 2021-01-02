using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Motion.Interfaces;
using Motion.AdlinkAps;
using Motion.AdlinkDash;
namespace desay.ProductData
{
    /// <summary>
    ///     设备 IO 项
    /// </summary>
    public class IoPoints
    {
        private const string ApsControllerName = "ApsController";
        private const string daskControllerName = "daskController";
        internal static readonly int Card208C = 0;
        internal static readonly byte PCI7432 = 0;
        public static ApsController m_ApsController = new ApsController(Card208C) { Name = ApsControllerName };
        public static DaskController m_DaskController = new DaskController(PCI7432) { Name = daskControllerName };

        #region Card208_0 IO list
        //由于IO表改动较多，IO命名规则重新整理：Name 根据IO表地址

        /// <summary>
        ///   启动按钮1/2
        /// </summary>
        public static IoPoint TDI0 = new IoPoint(m_ApsController, Card208C, 8, IoModes.Senser)
        {
            Name = "DI5.0",
            Description = "启动按钮1/2"
        };
        /// <summary>
        ///   停止按钮
        /// </summary>
        public static IoPoint TDI1 = new IoPoint(m_ApsController, Card208C, 9, IoModes.Senser)
        {
            Name = "DI5.1",
            Description = "停止按钮"
        };

        /// <summary>
        ///   暂停按钮
        /// </summary>
        public static IoPoint TDI2 = new IoPoint(m_ApsController, Card208C, 10, IoModes.Senser)
        {
            Name = "DI5.2",
            Description = "暂停按钮"
        };

        /// <summary>
        ///   复位按钮
        /// </summary>
        public static IoPoint TDI3 = new IoPoint(m_ApsController, Card208C, 11, IoModes.Senser)
        {
            Name = "DI5.3",
            Description = "复位按钮"
        };

        /// <summary>
        ///   急停1
        /// </summary>
        public static IoPoint TDI4 = new IoPoint(m_ApsController, Card208C, 12, IoModes.Senser)
        {
            Name = "DI5.4",
            Description = "急停按钮1"
        };

        /// <summary>
        ///   急停按钮2
        /// </summary>
        public static IoPoint TDI5 = new IoPoint(m_ApsController, Card208C, 13, IoModes.Senser)
        {
            Name = "DI5.5",
            Description = "急停按钮2"
        };

        /// <summary>
        ///   进气压力信号
        /// </summary>
        public static IoPoint TDI6 = new IoPoint(m_ApsController, Card208C, 14, IoModes.Senser)
        {
            Name = "DI5.6",
            Description = "进气压力信号"
        };

        /// <summary>
        ///   下压气缸原点信号
        /// </summary>
        public static IoPoint TDI7 = new IoPoint(m_ApsController, Card208C, 15, IoModes.Senser)
        {
            Name = "DI5.7",
            Description = "下压气缸原点信号"   //20201101 XiaoW 改
        };

        /// <summary>
        ///   前接驳台夹具到位信号
        /// </summary>
        public static IoPoint TDI8 = new IoPoint(m_ApsController, Card208C, 16, IoModes.Senser)
        {
            Name = "DI5.8",
            Description = "前接驳台夹具到位信号"
        };
        /// <summary>
        ///   前接驳台平移气缸OFF感应
        /// </summary>
        public static IoPoint TDI9 = new IoPoint(m_ApsController, Card208C, 17, IoModes.Senser)
        {
            Name = "DI5.9",
            Description = "前接驳台平移气缸OFF感应"
        };
        /// <summary>
        ///   前接驳台平移气缸ON感应
        /// </summary>
        public static IoPoint TDI10 = new IoPoint(m_ApsController, Card208C, 18, IoModes.Senser)
        {
            Name = "DI5.10",
            Description = "前接驳台平移气缸ON感应"
        };
        /// <summary>
        ///   前接驳台顶升气缸上感应
        /// </summary>
        public static IoPoint TDI11 = new IoPoint(m_ApsController, Card208C, 19, IoModes.Senser)
        {
            Name = "DI5.11",
            Description = "前接驳台顶升气缸上感应"
        };
        /// <summary>
        ///   前接驳台顶升气缸下感应
        /// </summary>
        public static IoPoint TDI12 = new IoPoint(m_ApsController, Card208C, 20, IoModes.Senser)
        {
            Name = "DI5.12",
            Description = "前接驳台顶升气缸下感应"
        };
        /// <summary>
        ///   前接驳台开夹气缸OFF感应
        /// </summary>
        public static IoPoint TDI13 = new IoPoint(m_ApsController, Card208C, 21, IoModes.Senser)
        {
            Name = "DI5.13",
            Description = "前接驳台开夹气缸OFF感应"
        };

        /// <summary>
        ///   前接驳台开夹气缸ON感应
        /// </summary>
        public static IoPoint TDI14 = new IoPoint(m_ApsController, Card208C, 22, IoModes.Senser)
        {
            Name = "DI5.14",
            Description = "前接驳台开夹气缸ON感应"
        };
        /// <summary>
        ///   下压气缸动点信号
        /// </summary>
        public static IoPoint TDI15 = new IoPoint(m_ApsController, Card208C, 23, IoModes.Senser)
        {
            Name = "DI5.15",
            Description = "下压气缸动点信号"   //20201110  XiaoW 改
        };
        #endregion

        #region PCI7432 IO list

        /// <summary>
        ///   送料线plasma段到位信号
        /// </summary>
        public static IoPoint IDI0 = new IoPoint(m_DaskController, PCI7432, 0, IoModes.Senser)
        {
            Name = "DI6.0",
            Description = "送料线plasma段到位信号"
        };

        /// <summary>
        ///   plasma夹具顶升气缸上感应
        /// </summary>
        public static IoPoint IDI1 = new IoPoint(m_DaskController, PCI7432, 1, IoModes.Senser)
        {
            Name = "DI6.1",
            Description = "plasma夹具顶升气缸上感应"
        };

        /// <summary>
        ///   plasma夹具顶升气缸下感应
        /// </summary>
        public static IoPoint IDI2 = new IoPoint(m_DaskController, PCI7432, 2, IoModes.Senser)
        {
            Name = "DI6.2",
            Description = "plasma夹具顶升气缸下感应"
        };

        /// <summary>
        ///   壳体有无检测
        /// </summary>
        public static IoPoint IDI3 = new IoPoint(m_DaskController, PCI7432, 3, IoModes.Senser)
        {
            Name = "DI6.3",
            Description = "壳体有无检测"
        };

        /// <summary>
        ///   plasma夹持气缸OFF感应
        /// </summary>
        public static IoPoint IDI4 = new IoPoint(m_DaskController, PCI7432, 4, IoModes.Senser)
        {
            Name = "DI6.4",
            Description = "plasma夹持气缸OFF感应"
        };

        /// <summary>
        ///   plasma夹持气缸ON感应
        /// </summary>
        public static IoPoint IDI5 = new IoPoint(m_DaskController, PCI7432, 5, IoModes.Senser)
        {
            Name = "DI6.5",
            Description = "plasma夹持气缸ON感应"
        };

        /// <summary>
        ///   plasma翻转气缸OFF感应
        /// </summary>
        public static IoPoint IDI6 = new IoPoint(m_DaskController, PCI7432, 6, IoModes.Senser)
        {
            Name = "DI6.6",
            Description = "plasma翻转气缸OFF感应"
        };

        /// <summary>
        ///   plasma翻转气缸ON感应
        /// </summary>
        public static IoPoint IDI7 = new IoPoint(m_DaskController, PCI7432, 7, IoModes.Senser)
        {
            Name = "DI6.7",
            Description = "plasma翻转气缸ON感应"
        };

        /// <summary>
        ///   plasma上下气缸上感应
        /// </summary>
        public static IoPoint IDI8 = new IoPoint(m_DaskController, PCI7432, 8, IoModes.Senser)
        {
            Name = "DI6.8",
            Description = "plasma上下气缸上感应"
        };

        /// <summary>
        ///   plasma上下气缸下感应
        /// </summary>
        public static IoPoint IDI9 = new IoPoint(m_DaskController, PCI7432, 9, IoModes.Senser)
        {
            Name = "DI6.9",
            Description = "plasma上下气缸下感应"
        };

        /// <summary>
        ///   白板测试光源气缸上感应
        /// </summary>
        public static IoPoint IDI10 = new IoPoint(m_DaskController, PCI7432, 10, IoModes.Senser)
        {
            Name = "DI6.10",
            Description = "白板测试光源气缸上感应"
        };

        /// <summary>
        ///   白板测试光源气缸下感应
        /// </summary>
        public static IoPoint IDI11 = new IoPoint(m_DaskController, PCI7432, 11, IoModes.Senser)
        {
            Name = "DI6.11",
            Description = "白板测试光源气缸下感应"
        };

        /// <summary>
        ///   送料线点胶段到位信号
        /// </summary>
        public static IoPoint IDI12 = new IoPoint(m_DaskController, PCI7432, 12, IoModes.Senser)
        {
            Name = "DI6.12",
            Description = "送料线点胶段到位信号"
        };

        /// <summary>
        ///   里面胶水液位感应信号
        /// </summary>
        public static IoPoint IDI13 = new IoPoint(m_DaskController, PCI7432, 13, IoModes.Senser)
        {
            Name = "DI6.13",
            Description = "里面胶水液位感应信号"
        };

        /// <summary>
        ///   里面点胶完成
        /// </summary>
        public static IoPoint IDI14 = new IoPoint(m_DaskController, PCI7432, 14, IoModes.Senser)
        {
            Name = "DI6.14",
            Description = "里面点胶完成"
        };

        /// <summary>
        ///   安全光幕
        /// </summary>
        public static IoPoint IDI15 = new IoPoint(m_DaskController, PCI7432, 15, IoModes.Senser)
        {
            Name = "DI6.15",
            Description = "安全光幕"
        };

        /// <summary>
        ///   点胶夹具顶升气缸上感应
        /// </summary>
        public static IoPoint IDI16 = new IoPoint(m_DaskController, PCI7432, 16, IoModes.Senser)
        {
            Name = "DI6.16",
            Description = "点胶夹具顶升气缸上感应"
        };

        /// <summary>
        ///   点胶夹具顶升气缸下感应
        /// </summary>
        public static IoPoint IDI17 = new IoPoint(m_DaskController, PCI7432, 17, IoModes.Senser)
        {
            Name = "DI6.17",
            Description = "点胶夹具顶升气缸下感应"
        };

        /// <summary>
        ///   回流阻挡到位信号
        /// </summary>
        public static IoPoint IDI18 = new IoPoint(m_DaskController, PCI7432, 18, IoModes.Senser)
        {
            Name = "DI6.18",
            Description = "回流阻挡到位信号"
        };

        /// <summary>
        ///   镜头有无检测
        /// </summary>
        public static IoPoint IDI19 = new IoPoint(m_DaskController, PCI7432, 19, IoModes.Senser)
        {
            Name = "DI6.19",
            Description = "镜头有无检测"
        };

        /// <summary>
        ///   外面胶水液位感应信号
        /// </summary>
        public static IoPoint IDI20 = new IoPoint(m_DaskController, PCI7432, 20, IoModes.Senser)
        {
            Name = "DI6.20",
            Description = "外面胶水液位感应信号"
        };

        /// <summary>
        ///   外面点胶完成
        /// </summary>
        public static IoPoint IDI21 = new IoPoint(m_DaskController, PCI7432, 21, IoModes.Senser)
        {
            Name = "DI6.21",
            Description = "外面点胶完成"
        };

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint IDI22 = new IoPoint(m_DaskController, PCI7432, 22, IoModes.Senser)
        {
            Name = "DI6.22",
            Description = "备用"
        };

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint IDI23 = new IoPoint(m_DaskController, PCI7432, 23, IoModes.Senser)
        {
            Name = "DI6.23",
            Description = "备用"
        };

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint IDI24 = new IoPoint(m_DaskController, PCI7432, 24, IoModes.Senser)
        {
            Name = "DI6.24",
            Description = "备用，请勿接线"
        };

        /// <summary>
        ///   备用
        /// </summary>
        public static IoPoint IDI25 = new IoPoint(m_DaskController, PCI7432, 25, IoModes.Senser)
        {
            Name = "DI6.25",
            Description = "备用"
        };

        /// <summary>
        ///   对针光纤Y
        /// </summary>
        public static IoPoint IDI26 = new IoPoint(m_DaskController, PCI7432, 26, IoModes.Senser)
        {
            Name = "DI6.26",
            Description = "对针光纤Y"
        };

        /// <summary>
        ///   对针光纤X
        /// </summary>
        public static IoPoint IDI27 = new IoPoint(m_DaskController, PCI7432, 27, IoModes.Senser)
        {
            Name = "DI6.27",
            Description = "对针光纤X"
        };

        /// <summary>
        ///   点胶段前门右门禁
        /// </summary>
        public static IoPoint IDI28 = new IoPoint(m_DaskController, PCI7432, 28, IoModes.Senser)
        {
            Name = "DI6.28",
            Description = "点胶段前门右门禁"
        };

        /// <summary>
        ///   点胶段前门左门禁
        /// </summary>
        public static IoPoint IDI29 = new IoPoint(m_DaskController, PCI7432, 29, IoModes.Senser)
        {
            Name = "DI6.29",
            Description = "点胶段前门左门禁"
        };

        /// <summary>
        ///   点胶段后门右门禁
        /// </summary>
        public static IoPoint IDI30 = new IoPoint(m_DaskController, PCI7432, 30, IoModes.Senser)
        {
            Name = "DI6.30",
            Description = "点胶段后门右门禁"
        };

        /// <summary>
        ///   点胶段后门左门禁
        /// </summary>
        public static IoPoint IDI31 = new IoPoint(m_DaskController, PCI7432, 31, IoModes.Senser)
        {
            Name = "DI6.31",
            Description = "点胶段后门左门禁"
        };

        /// <summary>
        ///   前接驳台电机正转
        /// </summary>
        public static IoPoint IDO0 = new IoPoint(m_DaskController, PCI7432, 0, IoModes.Signal)
        {
            Name = "DO6.0",
            Description = "前接驳台电机正转"
        };

        /// <summary>
        ///   前接驳台电机反转
        /// </summary>
        public static IoPoint IDO1 = new IoPoint(m_DaskController, PCI7432, 1, IoModes.Signal)
        {
            Name = "DO6.1",
            Description = "前接驳台电机反转"
        };

        /// <summary>
        ///   前接驳台平移气缸伸出
        /// </summary>
        public static IoPoint IDO2 = new IoPoint(m_DaskController, PCI7432, 2, IoModes.Signal)
        {
            Name = "DO6.2",
            Description = "前接驳台平移气缸伸出"
        };

        /// <summary>
        ///   前接驳台平移气缸收回
        /// </summary>
        public static IoPoint IDO3 = new IoPoint(m_DaskController, PCI7432, 3, IoModes.Signal)
        {
            Name = "DO6.3",
            Description = "前接驳台平移气缸收回"
        };
        /// <summary>
        ///   前接驳台顶升气缸
        /// </summary>
        public static IoPoint IDO4 = new IoPoint(m_DaskController, PCI7432, 4, IoModes.Signal)
        {
            Name = "DO6.4",
            Description = "前接驳台顶升气缸"
        };

        /// <summary>
        ///   前接驳台开夹气缸
        /// </summary>
        public static IoPoint IDO5 = new IoPoint(m_DaskController, PCI7432, 5, IoModes.Signal)
        {
            Name = "DO6.5",
            Description = "前接驳台开夹气缸"
        };

        /// <summary>
        ///   前接驳台红灯
        /// </summary>
        public static IoPoint IDO6 = new IoPoint(m_DaskController, PCI7432, 6, IoModes.Signal)
        {
            Name = "DO6.6",
            Description = "前接驳台红灯"
        };

        /// <summary>
        ///   前接驳台绿灯
        /// </summary>
        public static IoPoint IDO7 = new IoPoint(m_DaskController, PCI7432, 7, IoModes.Signal)
        {
            Name = "DO6.7",
            Description = "前接驳台绿灯"
        };

        /// <summary>
        ///   点胶回流线电机反转
        /// </summary>
        public static IoPoint IDO8 = new IoPoint(m_DaskController, PCI7432, 8, IoModes.Signal)
        {
            Name = "DO6.8",
            Description = "点胶回流线电机反转"
        };

        /// <summary>
        ///   plasma送料线电机正转
        /// </summary>
        public static IoPoint IDO9 = new IoPoint(m_DaskController, PCI7432, 9, IoModes.Signal)
        {
            Name = "DO6.9",
            Description = "plasma送料线电机正转"
        };

        /// <summary>
        ///   plasma夹具阻挡气缸
        /// </summary>
        public static IoPoint IDO10 = new IoPoint(m_DaskController, PCI7432, 10, IoModes.Signal)
        {
            Name = "DO6.10",
            Description = "plasma夹具阻挡气缸"
        };

        /// <summary>
        ///   plasma夹具顶升气缸
        /// </summary>
        public static IoPoint IDO11 = new IoPoint(m_DaskController, PCI7432, 11, IoModes.Signal)
        {
            Name = "DO6.11",
            Description = "plasma夹具顶升气缸"
        };

        /// <summary>
        ///   plasma夹持气缸
        /// </summary>
        public static IoPoint IDO12 = new IoPoint(m_DaskController, PCI7432, 12, IoModes.Signal)
        {
            Name = "DO6.12",
            Description = "plasma夹持气缸"
        };

        /// <summary>
        ///   plasma翻转气缸
        /// </summary>
        public static IoPoint IDO13 = new IoPoint(m_DaskController, PCI7432, 13, IoModes.Signal)
        {
            Name = "DO6.13",
            Description = "plasma翻转气缸"
        };

        /// <summary>
        ///   plasma上下气缸
        /// </summary>
        public static IoPoint IDO14 = new IoPoint(m_DaskController, PCI7432, 14, IoModes.Signal)
        {
            Name = "DO6.14",
            Description = "plasma上下气缸"
        };

        /// <summary>
        ///   白板测试光源气缸
        /// </summary>
        public static IoPoint IDO15 = new IoPoint(m_DaskController, PCI7432, 15, IoModes.Signal)
        {
            Name = "DO6.15",
            Description = "白板测试光源气缸"
        };

        /// <summary>
        ///   plasma启动继电器
        /// </summary>
        public static IoPoint IDO16 = new IoPoint(m_DaskController, PCI7432, 16, IoModes.Signal)
        {
            Name = "DO6.16",
            Description = "plasma启动继电器"
        };

        /// <summary>
        ///   点胶夹具阻挡气缸
        /// </summary>
        public static IoPoint IDO17 = new IoPoint(m_DaskController, PCI7432, 17, IoModes.Signal)
        {
            Name = "DO6.17",
            Description = "点胶夹具阻挡气缸"
        };

        /// <summary>
        ///   点胶夹具顶升气缸
        /// </summary>
        public static IoPoint IDO18 = new IoPoint(m_DaskController, PCI7432, 18, IoModes.Signal)
        {
            Name = "DO6.18",
            Description = "点胶夹具顶升气缸"
        };

        /// <summary>
        ///   里面点胶启动(电磁阀)
        /// </summary>
        public static IoPoint IDO19 = new IoPoint(m_DaskController, PCI7432, 19, IoModes.Signal)
        {
            Name = "DO6.19",
            Description = "里面点胶启动(电磁阀)"
        };

        /// <summary>
        ///   外面点胶启动(电磁阀)
        /// </summary>
        public static IoPoint IDO20 = new IoPoint(m_DaskController, PCI7432, 19, IoModes.Signal)
        {
            Name = "DO6.20",
            Description = "外面点胶启动(电磁阀)"
        };

        /// <summary>
        ///   回流阻挡气缸
        /// </summary>
        public static IoPoint IDO21 = new IoPoint(m_DaskController, PCI7432, 21, IoModes.Signal)
        {
            Name = "DO6.21",
            Description = "回流阻挡气缸"
        };

        /// <summary>
        ///   门禁线圈
        /// </summary>
        public static IoPoint IDO22 = new IoPoint(m_DaskController, PCI7432, 22, IoModes.Signal)
        {
            Name = "DO6.22",
            Description = "门禁线圈"
        };

        /// <summary>
        ///   下压气缸
        /// </summary>
        public static IoPoint IDO23 = new IoPoint(m_DaskController, PCI7432, 23, IoModes.Signal)
        {
            Name = "DO6.23",
            Description = "下压气缸"   //20201110  XiaoW 改
        };

        /// <summary>
        ///   启动按钮指示灯
        /// </summary>
        public static IoPoint IDO24 = new IoPoint(m_DaskController, PCI7432, 24, IoModes.Signal)
        {
            Name = "DO6.24",
            Description = "启动按钮指示灯"
        };

        /// <summary>
        ///   停止按钮指示灯
        /// </summary>
        public static IoPoint IDO25 = new IoPoint(m_DaskController, PCI7432, 25, IoModes.Signal)
        {
            Name = "DO6.25",
            Description = "停止按钮指示灯"
        };

        /// <summary>
        ///   暂停按钮指示灯
        /// </summary>
        public static IoPoint IDO26 = new IoPoint(m_DaskController, PCI7432, 26, IoModes.Signal)
        {
            Name = "DO6.26",
            Description = "暂停按钮指示灯"
        };

        /// <summary>
        ///   复位按钮指示灯
        /// </summary>
        public static IoPoint IDO27 = new IoPoint(m_DaskController, PCI7432, 27, IoModes.Signal)
        {
            Name = "DO6.27",
            Description = "复位按钮指示灯"
        };

        /// <summary>
        ///   三色-红灯
        /// </summary>
        public static IoPoint IDO28 = new IoPoint(m_DaskController, PCI7432, 28, IoModes.Signal)
        {
            Name = "DO6.28",
            Description = "三色-红灯"
        };

        /// <summary>
        ///   三色-黄灯
        /// </summary>
        public static IoPoint IDO29 = new IoPoint(m_DaskController, PCI7432, 29, IoModes.Signal)
        {
            Name = "DO6.29",
            Description = "三色-黄灯"
        };

        /// <summary>
        ///   三色-绿灯
        /// </summary>
        public static IoPoint IDO30 = new IoPoint(m_DaskController, PCI7432, 30, IoModes.Signal)
        {
            Name = "DO6.30",
            Description = "三色-绿灯"
        };

        /// <summary>
        ///   三色-蜂鸣器
        /// </summary>
        public static IoPoint IDO31 = new IoPoint(m_DaskController, PCI7432, 31, IoModes.Signal)
        {
            Name = "DO6.31",
            Description = "三色-蜂鸣器"
        };

        #endregion
    }
}
