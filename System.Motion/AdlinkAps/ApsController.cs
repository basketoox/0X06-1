using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Motion.Interfaces;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Motion.AdlinkAps
{
    /// <summary>
    ///     凌华科技AMP-204C/AMP-208C运动控制卡控制器。修改于2018.7.10 Finley Jiang
    /// </summary>
    public class ApsController : Automatic, ISwitchController, IMotionController, INeedInitialization, IDisposable
    {
        private readonly List<int> _axises;
        private readonly int[] _cardNos;
        private bool _disposed;
        private bool _isInitialized;
        public bool IsLoadXmlFile { get; private set; }

        public ApsController(params int[] cardNos)
        {
            _cardNos = cardNos;
            _axises = new List<int>();
        }

        #region Implementation of INeedInitialization

        public void Initialize()
        {
            if (!_isInitialized)
            {
                InitializeCard();               
            }
            _isInitialized = true;
        }
        public bool LoadParamFromFile(string xmlfilename)
        {
            IsLoadXmlFile = (APS168.APS_load_param_from_file(xmlfilename) == 0);
            if (IsLoadXmlFile)
            {
                //AutoClosingMessageBox.Show("Load XML File OK !", "204C208C", 2000);
            }
            else
            {
                //MessageBox.Show("Load XML File Failed!");
            }
            return IsLoadXmlFile;
        }
        #endregion

        public int RelativeMove(int axisNo, double position, double maxSpeed)
        {
            SetAxisParam(axisNo, 36, maxSpeed);

            var wait = new ASYNCALL();
            var ret = APS168.APS_ptp(axisNo, 1, position, ref wait);
            return ret;
        }
        //绝对值移动
        public int  AbsoluteMove(int axisNo, double position, double maxSpeed)
        {
            SetAxisParam(axisNo, 36, maxSpeed);
            var wait = new ASYNCALL();
            var ret = APS168.APS_ptp(axisNo, 0, position, ref wait);
            return ret;
        }

        public int MoveOrigin(int axisNo)
        {
            var ret = APS168.APS_home_move(axisNo);
            return ret;
        }

        #region 获取当前轴的 IO 信号
        /// <summary>
        ///     是否报警
        /// </summary>
        /// <returns></returns>
        public bool IsAlm(int axisNo)
        {
            return ((GetMotionIoStatus(axisNo) >> (int)APS_Define.MIO_ALM) & 1) == 1;
        }
        /// <summary>
        ///     是否到达正限位
        /// </summary>
        /// <returns></returns>
        public bool IsPel(int axisNo)
        {
            return ((GetMotionIoStatus(axisNo) >> (int) APS_Define.MIO_PEL) & 1) == 1;
        }
        /// <summary>
        ///     是否到达正负位
        /// </summary>
        /// <returns></returns>
        public bool IsMel(int axisNo)
        {
            return ((GetMotionIoStatus(axisNo) >> (int) APS_Define.MIO_MEL) & 1) == 1;
        }

        /// <summary>
        ///     是否在轴原点
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public bool IsOrg(int axisNo)
        {
            return ((GetMotionIoStatus(axisNo) >> (int) APS_Define.MIO_ORG) & 1) == 1;
        }
        /// <summary>
        ///     是否急停
        /// </summary>
        /// <returns></returns>
        public bool IsEmg(int axisNo)
        {
            return ((GetMotionIoStatus(axisNo) >> (int)APS_Define.MIO_EMG) & 1) == 1;
        }
        /// <summary>
        /// 是否在轴Z相
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public bool IsSZ(int axisNo)
        {
            return ((GetMotionIoStatus(axisNo) >> (int)APS_Define.MIO_EZ) & 1) == 1;
        }
        /// <summary>
        /// 是否在轴Z相
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public bool IsINP(int axisNo)
        {
            return ((GetMotionIoStatus(axisNo) >> (int)APS_Define.MIO_INP) & 1) == 1;
        }
        /// <summary>
        ///     获取电机励磁状态。
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public bool GetServo(int axisNo)
        {
            return ((GetMotionIoStatus(axisNo) >> (int)APS_Define.MIO_SVON) & 1) == 1;
        }
        #endregion
        /// <summary>
        ///     获取轴当前位置
        /// </summary>
        /// <param name="axisNo">轴标识</param>
        /// <returns>当前位置</returns>
        public int GetCurrentCommandPosition(int axisNo)
        {
            int position = 0;
            var ret = APS168.APS_get_command(axisNo, ref position);
            ThrowIfResultError(ret);
            return position;
        }
        public int GetCurrentFeedbackPosition(int axisNo)
        {
            int position = 0;
            var ret = APS168.APS_get_position(axisNo, ref position);
            ThrowIfResultError(ret);
            return position;
        }
        public int GetCurrentCommandSpeed(int axisNo)
        {
            int speed = 0;
            var ret = APS168.APS_get_command_velocity(axisNo, ref speed);
            ThrowIfResultError(ret);
            return speed;
        }
        public int GetCurrentFeedbackSpeed(int axisNo)
        {
            int speed = 0;
            var ret = APS168.APS_get_feedback_velocity(axisNo, ref speed);
            ThrowIfResultError(ret);
            return speed;
        }
        /// <summary>
        ///     设置指令位置计数器计数值
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="position"></param>
        public void SetCommandPosition(int axisNo, int position)
        {
            int ret = APS168.APS_set_command(axisNo, position);
            ThrowIfResultError(ret, string.Format("APS_set_command_f({0},{1})", axisNo, position));
        }
        /// <summary>
        ///     设置指令位置计数器计数值
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="position"></param>
        public void SetFeedbackPosition(int axisNo, int position)
        {
            int ret = APS168.APS_set_position(axisNo, position);
            ThrowIfResultError(ret, string.Format("APS_set_command_f({0},{1})", axisNo, position));
        }
        public void Direct(int noId, double maxSpeed)
        {
            APS168.APS_velocity_move(noId, (int)(maxSpeed * (int)MoveDirection.Postive));
        }

        public void Reverse(int noId, double maxSpeed)
        {
            APS168.APS_velocity_move(noId, (int)(maxSpeed * (int)MoveDirection.Negative));
        }
        /// <summary>
        ///     轴上电
        /// </summary>
        /// <param name="noId"></param>
        public void ServoOn(int noId)
        {
            APS168.APS_set_servo_on(noId, 1);
        }

        /// <summary>
        ///     轴掉电
        /// </summary>
        /// <param name="noId"></param>
        public void ServoOff(int noId)
        {
            APS168.APS_set_servo_on(noId, 0);
        }
        public void ServoOn()
        {
            _axises.ForEach(axisNo =>
            {
                var result = APS168.APS_set_servo_on(axisNo, 1);
                var innerMsg = "";
                innerMsg = GetErrorCodeDesc((APS_Define)result);
                if (innerMsg != "No Error")
                    MessageBox.Show(string.Format("伺服打开功能错误:{0}", innerMsg));
                //ThrowIfResultError(result);
            });
        }

        public void ServoOff()
        {
            _axises.ForEach(axisNo =>
            {
                var result = APS168.APS_set_servo_on(axisNo, 0);
                var innerMsg = "";
                innerMsg = GetErrorCodeDesc((APS_Define)result);
                if (innerMsg != "No Error")
                    MessageBox.Show(string.Format("伺服关闭功能错误:{0}", innerMsg));
                //ThrowIfResultError(result);
            });
        }

        #region Error Code

        private string GetErrorCodeDesc(APS_Define errorCode)
        {
            switch (errorCode)
            {
                case APS_Define.ERR_NoError:
                    return "No Error";
                case APS_Define.ERR_OSVersion:
                    return "Operation System type mismatched";
                case APS_Define.ERR_OpenDriverFailed:
                    return "Open device driver failed - Create driver interface failed";
                case APS_Define.ERR_InsufficientMemory:
                    return "System memory insufficiently";
                case APS_Define.ERR_DeviceNotInitial:
                    return "Cards not be initialized";
                case APS_Define.ERR_NoDeviceFound:
                    return "Cards not foundNo card in your system";
                case APS_Define.ERR_CardIdDuplicate:
                    return "Cards' ID is duplicated.";
                case APS_Define.ERR_DeviceAlreadyInitialed:
                    return "Cards have been initialed";
                case APS_Define.ERR_InterruptNotEnable:
                    return "Cards' interrupt events not enable or not be initialized";
                case APS_Define.ERR_TimeOut:
                    return "Function time out";
                case APS_Define.ERR_ParametersInvalid:
                    return "Function input parameters are invalid";
                case APS_Define.ERR_SetEEPROM:
                    return "Set data to EEPROM or nonvolatile memory failed";
                case APS_Define.ERR_GetEEPROM:
                    return "Get data from EEPROM or nonvolatile memory failed";
                case APS_Define.ERR_FunctionNotAvailable:
                    return
                        "Function is not available in this step,The device is not support this function or Internal process failed";
                case APS_Define.ERR_FirmwareError:
                    return "Firmware error,please reboot the system";
                case APS_Define.ERR_CommandInProcess:
                    return "Previous command is in process";
                case APS_Define.ERR_AxisIdDuplicate:
                    return "Axes' ID is duplicated.";
                case APS_Define.ERR_ModuleNotFound:
                    return "Slave module not found.";
                case APS_Define.ERR_InsufficientModuleNo:
                    return "System ModuleNo insufficiently";
                case APS_Define.ERR_HandShakeFailed:
                    return "HandSake with the DSP out of time.";
                case APS_Define.ERR_FILE_FORMAT:
                    return "Config file format error.cannot be parsed";
                case APS_Define.ERR_ParametersReadOnly:
                    return "Function parameters read only.";
                case APS_Define.ERR_DistantNotEnough:
                    return "Distant is not enough for motion.";
                case APS_Define.ERR_FunctionNotEnable:
                    return "Function is not enabled.";
                case APS_Define.ERR_ServerAlreadyClose:
                    return "Server already closed.";
                case APS_Define.ERR_DllNotFound:
                    return "Related dll is not found,not in correct path.";
                case APS_Define.ERR_TrimDAC_Channel:
                    return " ";
                case APS_Define.ERR_Satellite_Type:
                    return " ";
                case APS_Define.ERR_Over_Voltage_Spec:
                    return " ";
                case APS_Define.ERR_Over_Current_Spec:
                    return " ";
                case APS_Define.ERR_SlaveIsNotAI:
                    return " ";
                case APS_Define.ERR_Over_AO_Channel_Scope:
                    return " ";
                case APS_Define.ERR_DllFuncFailed:
                    return "Failed to invoke dll function. Extension Dll version is wrong.";
                case APS_Define.ERR_FeederAbnormalStop:
                    return "Feeder abnormal stop, External stop or feeding stop";
                case APS_Define.ERR_Read_ModuleType_Dismatch:
                    return " ";
                case APS_Define.ERR_Win32Error:
                    return "No such INT number, or WIN32_API error, contact with ADLINK's FAE staff.";
                case APS_Define.ERR_DspStart:
                    return "The base for DSP error";
                default:
                    return null;
            }
        }

        #endregion

        /// <summary>
        ///     设置电机励磁状态。
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="isOn"></param>
        public int SetServo(int axisNo, bool isOn)
        {
            var result = APS168.APS_set_servo_on(axisNo, isOn ? 1 : 0);
            return result;
        }

        /// <summary>
        ///     单轴相对运动
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="pulseNum"></param>
        /// <param name="velocityCurveParams"></param>
        /// <returns></returns>
        public void MoveRelPulse(int axisNo, int pulseNum, VelocityCurve velocityCurveParams)
        {
            //设置速度
            SetAxisVelocity(axisNo, velocityCurveParams);

            //启动运动
            APS168.APS_relative_move(axisNo, pulseNum, (int) velocityCurveParams.Maxvel);
        }

        /// <summary>
        ///     单轴绝对运动
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="pulseNum"></param>
        /// <param name="velocityCurveParams"></param>
        public void MoveAbsPulse(int axisNo, int pulseNum, VelocityCurve velocityCurveParams)
        {
            //设置速度
            SetAxisVelocity(axisNo, velocityCurveParams);
      
            //启动运动
            APS168.APS_absolute_move(axisNo, pulseNum, (int) velocityCurveParams.Maxvel);
        }

        /// <summary>
        ///     连续运动
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="moveDirection"></param>
        /// <param name="velocityCurveParams"></param>
        /// <returns></returns>
        public void ContinuousMove(int axisNo, MoveDirection moveDirection, VelocityCurve velocityCurveParams)
        {
            //启动运动
            APS168.APS_velocity_move(axisNo, (int)(velocityCurveParams.Maxvel * (int)moveDirection));
        }
        /// <summary>
        ///     立即停止
        /// </summary>
        /// <param name="axisNo"></param>
        public void ImmediateStop(int axisNo)
        {
            APS168.APS_emg_stop(axisNo);
        }

        /// <summary>
        ///     减速停止指定机构轴脉冲输出
        /// </summary>
        /// <param name="axisNo"></param>
        public int DecelStop(int axisNo)
        {
            var reult = APS168.APS_stop_move(axisNo);
            return reult;
        }

        /// <summary>
        ///     是否停止移动
        /// </summary>
        /// <returns></returns>
        public bool IsDown(int axisNo, bool hasExtEncode = false)
        {
            var status = APS168.APS_motion_status(axisNo);

            //判断是否停止
            if (((status >> (int)APS_Define.MTS_NSTP) & 1) == 0)
                return false;

            //判断是否正常停止
            if (((status >> (int)APS_Define.MTS_ASTP) & 1) == 1)
            {
                var reason = GetStopReason(axisNo);
                return false;
            }

            //判断INP鑫海
            if (hasExtEncode)
            {
                var inp = APS168.APS_motion_io_status(axisNo);
                if (((inp >> (int)APS_Define.MIO_INP) & 1) == 0)
                    return false;
            }

            return true;
        }

        /// <summary>
        ///     检测指定轴的运动状态
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="hasExtEncode">是否有编码器接入(步进电机无外部编码器)</param>
        /// <remarks>判断INP鑫海</remarks>
        public int CheckDone(int axisNo, double timeoutLimit, bool hasExtEncode = false)
        {
            var status = 0;
            var inp = 1;

            var strtime = new Stopwatch();
            strtime.Start();

            do
            {
                //判断是否正常停止
                status = APS168.APS_motion_status(axisNo);
                if (((status >> (int) APS_Define.MTS_ASTP) & 1) == 1)
                    return -1;
                status = (status >> (int) APS_Define.MTS_NSTP) & 1;

                //判断INP鑫海
                if (hasExtEncode)
                {
                    inp = APS168.APS_motion_io_status(axisNo);
                    inp = (inp >> (int) APS_Define.MIO_INP) & 1;
                }

                //break条件是否满足
                if ((status == 1) && (inp == 1))
                    break;

                //检查是否超时
                strtime.Stop();
                if (strtime.ElapsedMilliseconds/1000.0 > timeoutLimit)
                {
                    APS168.APS_emg_stop(axisNo);
                    return -2;
                }
                strtime.Start();

                //延时
                Thread.Sleep(20);
            } while (true);

            return 0;
        }

        /// <summary>
        /// 两轴做插补相对移动
        /// </summary>
        /// <param name="axisNo1">轴1ID</param>
        /// <param name="axisNo2">轴2ID</param>
        /// <param name="pulseNum1">坐标1</param>
        /// <param name="pulseNum2">坐标2</param>
        /// <param name="velocityCurveParams">速度参数</param>
        public void MoveLine2Relative(int axisNo1, int axisNo2, int pulseNum1, int pulseNum2,
            VelocityCurve velocityCurveParams)
        {
            var axis = new int[2];
            var pos = new int[2];

            axis[0] = axisNo1;
            axis[1] = axisNo2;
            pos[0] = pulseNum1;
            pos[1] = pulseNum2;

            //设置速度
            SetAxisVelocity(axisNo1, velocityCurveParams);
            //启动运动
            APS168.APS_relative_linear_move(2, axis, pos, (int) velocityCurveParams.Maxvel);
        }
        /// <summary>
        ///     两轴直线插补绝对移动
        /// </summary>
        /// <param name="axisNo1">轴1ID</param>
        /// <param name="axisNo2">轴2ID</param>
        /// <param name="pulseNum1">坐标1</param>
        /// <param name="pulseNum2">坐标2</param>
        /// <param name="velocityCurveParams">速度参数</param>
        public void MoveLine2Absolute(int axisNo1, int axisNo2, int pulseNum1, int pulseNum2,
            VelocityCurve velocityCurveParams)
        {
            var axis = new int[2];
            var pos = new int[2];

            axis[0] = axisNo1;
            axis[1] = axisNo2;
            pos[0] = pulseNum1;
            pos[1] = pulseNum2;

            //设置速度
            SetAxisVelocity(axisNo1, velocityCurveParams);

            //启动运动
            APS168.APS_absolute_linear_move(2, axis, pos, (int) velocityCurveParams.Maxvel);
        }
        /// <summary>
        /// 两轴直线插补轨迹移动
        /// </summary>
        /// <param name="axisNo1">轴1ID</param>
        /// <param name="axisNo2">轴2ID</param>
        /// <param name="pulseNum1">坐标1</param>
        /// <param name="pulseNum2">坐标2</param>
        /// <param name="velocityCurveParams">速度参数</param>
        /// <param name="Option">位集指定选项，该选项可以启用指定的参数和函数。</param>
        public void MoveLine2(int axisNo1, int axisNo2, double pulseNum1, double pulseNum2,
            VelocityCurve velocityCurveParams, int Option)
        {
            var axis = new int[2];
            var pos = new double[2];
            double TransPara = 0;
            ASYNCALL wait = new ASYNCALL(); //A waiting call

            axis[0] = axisNo1;
            axis[1] = axisNo2;
            pos[0] = pulseNum1;
            pos[1] = pulseNum2;

            //设置速度
            SetAxisVelocity(axisNo1, velocityCurveParams);
            //启动运动
            APS168.APS_line_all(2, axis, Option, pos, ref TransPara, velocityCurveParams.Strvel, velocityCurveParams.Maxvel, velocityCurveParams.Strvel, velocityCurveParams.Svacc, velocityCurveParams.Svdec, velocityCurveParams.Sfac, ref wait);
        }
        /// <summary>
        /// 两轴圆弧插补相对移动
        /// </summary>
        /// <param name="axisNo1">轴1ID</param>
        /// <param name="axisNo2">轴2ID</param>
        /// <param name="centreNum1">圆心1</param>
        /// <param name="centreNum2">圆心2</param>
        /// <param name="Angle">角度</param>
        /// <param name="velocityCurveParams">速度参数</param>
        public void MoveArc2Relative(int axisNo1,int axisNo2,int centreNum1,int centreNum2,int Angle,VelocityCurve velocityCurveParams)
        {
            var axis = new int[2];
            var pos = new int[2];

            axis[0] = axisNo1;
            axis[1] = axisNo2;
            pos[0] = centreNum1;
            pos[1] = centreNum2;

            //设置速度
            SetAxisVelocity(axisNo1, velocityCurveParams);
            //启动运动
            APS168.APS_relative_arc_move(2, axis, pos, (int)velocityCurveParams.Maxvel, Angle);
        }
        
        /// <summary>
        /// 两轴圆弧插补绝对移动
        /// </summary>
        /// <param name="axisNo1">轴1ID</param>
        /// <param name="axisNo2">轴2ID</param>
        /// <param name="centreNum1">圆心1</param>
        /// <param name="centreNum2">圆心2</param>
        /// <param name="Angle">角度</param>
        /// <param name="velocityCurveParams">速度参数</param>
        public void MoveArc2Absolute(int axisNo1, int axisNo2, int centreNum1, int centreNum2, int Angle, VelocityCurve velocityCurveParams)
        {
            var axis = new int[2];
            var pos = new int[2];

            axis[0] = axisNo1;
            axis[1] = axisNo2;
            pos[0] = centreNum1;
            pos[1] = centreNum2;

            //设置速度
            SetAxisVelocity(axisNo1, velocityCurveParams);
            //启动运动
            APS168.APS_absolute_arc_move(2, axis, pos, (int)velocityCurveParams.Maxvel, Angle);
        }
        /// <summary>
        /// 两轴圆弧插补轨迹移动
        /// </summary>
        /// <param name="axisNo1">轴1ID</param>
        /// <param name="axisNo2">轴2ID</param>
        /// <param name="centreNum1">圆心1</param>
        /// <param name="centreNum2">圆心2</param>
        /// <param name="endNum1">终点1</param>
        /// <param name="endNum2">终点1</param>
        /// <param name="dir">方向</param>
        /// <param name="velocityCurveParams">速度参数</param>
        /// <param name="Option">位集指定选项，该选项可以启用指定的参数和函数。</param>
        public void MoveArc2(int axisNo1, int axisNo2, double centreNum1, double centreNum2, double endNum1,double endNum2, short dir,VelocityCurve velocityCurveParams,int Option)
        {
            var axis = new int[2];
            var pos1 = new double[2];
            var pos2 = new double[2];
            double TranPara = 0; 
            ASYNCALL wait = new ASYNCALL();

            axis[0] = axisNo1;
            axis[1] = axisNo2;
            pos1[0] = centreNum1;
            pos1[1] = centreNum2;

            //设置速度
            SetAxisVelocity(axisNo1, velocityCurveParams);
            //启动运动
            APS168.APS_arc2_ce_all(axis,Option,pos1,pos2, dir,ref TranPara, velocityCurveParams.Strvel, velocityCurveParams.Maxvel, velocityCurveParams.Strvel, velocityCurveParams.Svacc, velocityCurveParams.Svdec, velocityCurveParams.Sfac, ref wait);
        }
        /// <summary>
        ///     回零
        /// </summary>
        /// <param name="axisNo"></param>
        public void BackHome(int axisNo)
        {
            APS168.APS_home_move(axisNo);
        }

        /// <summary>
        ///     检查回零是否完成
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public int CheckHomeDone(int axisNo, double timeoutLimit)
        {
            var status = 0;

            var strtime = new Stopwatch();
            strtime.Start();

            do
            {
                //检查运动是否完成
                status = APS168.APS_motion_status(axisNo);
                if (((status >> (int) APS_Define.MTS_ASTP) & 1) == 1)
                    return -1;
                status = (status >> (int) APS_Define.MTS_HMV) & 1;
                if (status == 0)
                    break;

                //检查是否超时
                strtime.Stop();
                if (strtime.ElapsedMilliseconds/1000.0 > timeoutLimit)
                {
                    APS168.APS_emg_stop(axisNo);
                    return -2;
                }
                strtime.Start();

                //延时
                Thread.Sleep(20);
            } while (true);

            return 0;
        }

        /// <summary>
        ///     轴卡电平信号配置(板卡初始化后必须配置)
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="axisSignalParams"></param>
        public void SetAxisSignalConfig(int axisNo, AxisSignalParams axisSignalParams)
        {
            //配置各轴
            APS168.APS_set_axis_param(axisNo, (int) APS_Define.PRA_EL_LOGIC, axisSignalParams.ElLogic);
            // 限位信号: 0-not inverse, 1-inverse
            APS168.APS_set_axis_param(axisNo, (int) APS_Define.PRA_ORG_LOGIC, axisSignalParams.OrgLogic);
            // ORG信号: 0-not inverse, 1-inverse    
            APS168.APS_set_axis_param(axisNo, (int) APS_Define.PRA_ALM_LOGIC, axisSignalParams.AlmLogic);
            // ALM信号：0-low active, 1-high active    
            APS168.APS_set_axis_param(axisNo, (int) APS_Define.PRA_EZ_LOGIC, axisSignalParams.EzLogic);
            // EZ信号： 0-low active, 1-high active 
            APS168.APS_set_axis_param(axisNo, (int) APS_Define.PRA_INP_LOGIC, axisSignalParams.InpLogic);
            // INP信号：0-low active, 1-high active 
            APS168.APS_set_axis_param(axisNo, (int) APS_Define.PRA_SERVO_LOGIC, axisSignalParams.ServoLogic);
            // SERVO信号： 0-low logic, 1-high active
            APS168.APS_set_axis_param(axisNo, (int) APS_Define.PRA_PLS_OPT_MODE, axisSignalParams.PlsOutMode);
            //PLS Output Mode: pulse/dir
            APS168.APS_set_axis_param(axisNo, (int) APS_Define.PRA_PLS_IPT_MODE, axisSignalParams.PlsInMode);
            //PLS Input Mode: 1xAB
            APS168.APS_set_axis_param(axisNo, (int) APS_Define.PRA_ENCODER_DIR, axisSignalParams.EncodeDir);
            //encoder dir: positive
        }

        /// <summary>
        ///     回零配置
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="homeConfigParams"></param>
        public void SetAxisHomeConfig(int axisNo, HomeParams homeConfigParams)
        {
            APS168.APS_set_axis_param(axisNo, (int) APS_Define.PRA_HOME_MODE, homeConfigParams.Mode);
            // home mode  0:ORG  1:EL   2:EZ
            APS168.APS_set_axis_param(axisNo, (int) APS_Define.PRA_HOME_DIR, homeConfigParams.Dir);
            // Set home direction   0:p-dir   1:n-dir
            APS168.APS_set_axis_param(axisNo, (int) APS_Define.PRA_HOME_EZA, homeConfigParams.EZ);
            // EZ alignment enable 0-no 1-yes    
            APS168.APS_set_axis_param_f(axisNo, (int) APS_Define.PRA_HOME_CURVE, 0.5);
            // homing curve parten(T or s curve)  0-T 0~1.0-S       	      
            APS168.APS_set_axis_param_f(axisNo, (int) APS_Define.PRA_HOME_ACC, homeConfigParams.MaxVel*5);
            // Acceleration deceleration rate    
            APS168.APS_set_axis_param_f(axisNo, (int) APS_Define.PRA_HOME_VS, 0); // homing start velocity          
            APS168.APS_set_axis_param_f(axisNo, (int) APS_Define.PRA_HOME_VM, homeConfigParams.MaxVel);
            // homing max velocity              
            APS168.APS_set_axis_param_f(axisNo, (int) APS_Define.PRA_HOME_VO, homeConfigParams.OrgVel);
            // Homing leave ORG velocity     
            APS168.APS_set_axis_param_f(axisNo, (int) APS_Define.PRA_HOME_SHIFT, homeConfigParams.ZeroOffset);
            // The shift from ORG
        }

        /// <summary>
        ///     清除限位配置
        /// </summary>
        /// <param name="axisNo"></param>
        public void ClearSoftConfig(int axisNo)
        {
            APS168.APS_set_axis_param(axisNo, (int) APS_Define.PRA_SPEL_EN, 0);
            APS168.APS_set_axis_param(axisNo, (int) APS_Define.PRA_SMEL_EN, 0);
        }

        /// <summary>
        ///     限位配置
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="softLimitParams"></param>
        public void SetSoftELConfig(int axisNo, SoftLimitParams softLimitParams)
        {
            if (softLimitParams.Enable)
            {
                APS168.APS_set_axis_param_f(axisNo, (int) APS_Define.PRA_SPEL_POS, softLimitParams.SPelPosition);
                APS168.APS_set_axis_param_f(axisNo, (int) APS_Define.PRA_SMEL_POS, softLimitParams.SMelPosition);
                APS168.APS_set_axis_param(axisNo, (int) APS_Define.PRA_SPEL_EN, 2);
                APS168.APS_set_axis_param(axisNo, (int) APS_Define.PRA_SMEL_EN, 2);
            }
            else
            {
                ClearSoftConfig(axisNo);
            }
        }

        private void SetAxisVelocity(int axisNo, VelocityCurve velocityCurveParams)
        {
            //设置速度
            //if (velocityCurveParams.Sfac != 0)
            //{ 
            //SetAxisParam(axisNo, (int) APS_Define.PRA_SF, velocityCurveParams.Sfac);
            SetAxisParam(axisNo, (int) APS_Define.PRA_ACC, velocityCurveParams.Svacc);
            SetAxisParam(axisNo, (int) APS_Define.PRA_DEC, velocityCurveParams.Svdec);
            //}
        }

        /// <summary>
        ///     专用输入口使能(不是所有板卡都有此功能)
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="inputEn"></param>
        /// <returns></returns>
        public void InputEnable(int cardNo, int inputEn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     配置限位停止方式
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="ELStopType"></param>
        public void ConfigELStopType(int axisNo, ELStopType ELStopType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     读取指令位置计数器计数值
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public double GetCommandPosition(int axisNo)
        {
            var tmp = 0.0;
            APS168.APS_get_command_f(axisNo, ref tmp);
            return tmp;
        }

        /// <summary>
        ///     设置指令位置计数器计数值
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="position"></param>
        public int SetCommandPosition(int axisNo, double position)
        {
            int ret = APS168.APS_set_command_f(axisNo, position);
            //var innerMsg = "";
            //innerMsg = GetErrorCodeDesc((APS_Define)ret);
            //if (innerMsg != "No Error")
            //    MessageBox.Show(string.Format("设置指令位置计数器计数值错误:{0}", innerMsg));
            //ThrowIfResultError(ret, string.Format("APS_set_command_f({0},{1})", axisNo, position));
            return ret;
        }

        /// <summary>
        ///     读取反馈位置计数器计数值
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public double GetFeedbackPosition(int axisNo)
        {
            var tmp = 0.0;
            APS168.APS_get_position_f(axisNo, ref tmp);

            return tmp;
        }

        /// <summary>
        ///     设置反馈位置计数器计数值
        /// </summary>
        /// <param name="axisNo"></param>
        /// <param name="position"></param>
        public int SetFeedbackPosition(int axisNo, double position)
        {
            int ret = APS168.APS_set_position_f(axisNo, position);
            //var innerMsg = "";
            //innerMsg = GetErrorCodeDesc((APS_Define)ret);
            //if (innerMsg != "No Error")
            //    MessageBox.Show(string.Format("设置反馈位置计数器计数值错误:{0}", innerMsg));
            //ThrowIfResultError(ret, string.Format("APS_set_position_f({0},{1})", axisNo, position));
            return ret;
        }

        /// <summary>
        ///     获取轴停止原因
        /// </summary>
        /// <param name="axisNo"></param>
        public StopReasons GetStopReason(int axisNo)
        {
            var code = 0;
            APS168.APS_get_stop_code(axisNo, ref code);
            return (StopReasons)code;
        }

        /// <summary>
        ///     获取轴的插补模式
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public GearModes GetGearMode(int axisNo)
        {
            var status = 0;
            APS168.APS_get_gear_status(axisNo, ref status);
            return (GearModes)status;
        }
        private void ThrowIfResultError(int errorCode, string function = null)
        {
            if (errorCode == (int)APS_Define.ERR_NoError) return;
            var innerMsg = GetErrorCodeDesc((APS_Define)errorCode);
            throw new ApsException(string.Format("凌华运动控制卡功能 [{0}] 错误:{1}", function, innerMsg));
        }

        private void InitializeCard()
        {
            var boardBits = 0;
            var ret = APS168.APS_initial(ref boardBits, 0);
            if (ret == 0)
            {
                foreach (var cardNo in _cardNos)
                {
                    if (((boardBits >> cardNo) & 1) == 0)
                        throw new SystemException("Board Id search error !");
                    var cardName = 0;
                    ret = APS168.APS_get_card_name(cardNo, ref cardName);
                    var innerMsg = "";
                    innerMsg = GetErrorCodeDesc((APS_Define)ret);
                    if (innerMsg != "No Error")
                        MessageBox.Show(string.Format("APS_get_card_name Error:{0}", innerMsg));
                    //ThrowIfResultError(ret, "Initial");


                    if ((cardName == (int) APS_Define.DEVICE_NAME_PCI_825458) ||
                        (cardName == (int) APS_Define.DEVICE_NAME_AMP_20408C))
                    {
                        var startAxisId = 0;
                        var totalAxisNum = 0;
                        APS168.APS_get_first_axisId(cardNo, ref startAxisId, ref totalAxisNum);
                        for (var index = 0; index < totalAxisNum; ++index)
                            _axises.Add(startAxisId + index);

                        APS168.APS_load_parameter_from_flash(cardNo);
                    }
                    else
                    {
                        MessageBox.Show("Board type not matching!");
                        //throw new ApsException("Board type not matching!");
                    }
                }
            }
            else
            {
                var innerMsg = "";
                innerMsg = GetErrorCodeDesc((APS_Define)ret);
                if (innerMsg != "No Error")
                    MessageBox.Show(string.Format("板卡初始化错误:{0}", innerMsg));
                //ThrowIfResultError(ret, "APS_initial");
            }
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     线性差补方式运动。
        /// </summary>
        /// <param name="axisNo1"></param>
        /// <param name="axisNo2"></param>
        /// <param name="pulseNum1"></param>
        /// <param name="pulseNum2"></param>
        /// <param name="velocityCurve"></param>
        public void MoveLine(int axisNo1, int axisNo2, int pulseNum1, int pulseNum2, VelocityCurve velocityCurve)
        {
            throw new NotImplementedException();
        }

        ~ApsController()
        {
            Dispose(false);
        }

        /// <summary>
        ///     Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
            }
            try
            {
                //APS168.APS_close();
            }
            catch (Exception)
            {
                //ignorl
            }
            _isInitialized = false;
            _disposed = true;
        }

        #endregion

        #region Implementation of ISwitchController

        public bool Read(IoPoint ioPoint)
        {
            var value = 0;

            if ((ioPoint.IoMode & IoModes.Responser) != 0)
            {
                var ret = APS168.APS_read_d_output(ioPoint.BoardNo, 0, ref value);
            }
            else if ((ioPoint.IoMode & IoModes.Senser) != 0)
            {
                var ret = APS168.APS_read_d_input(ioPoint.BoardNo, 0, ref value);
            }
            return (value & (1 << ioPoint.PortNo)) > 0;
        }

        public void Write(IoPoint ioPoint, bool value)
        {
            var ret = APS168.APS_write_d_channel_output(ioPoint.BoardNo, 0, ioPoint.PortNo, value ? 1 : 0);
            if (ret < 0)
            {
                ThrowIfResultError(ret, string.Format("APS_write_d_channel[{0},{1}]", ioPoint.BoardNo, ioPoint.PortNo));
            }
        }

        #endregion

        /// <summary>
        ///     读端口数据
        /// </summary>
        /// <param name="boardNo"></param>
        /// <returns></returns>
        public int ReadPort(int boardNo)
        {
            var value = 0;
            var ret = APS168.APS_read_d_input(boardNo, 0, ref value);
            if (ret < 0)
            {
                //var innerMsg = "";
                //innerMsg = GetErrorCodeDesc((APS_Define)ret);
                //if (innerMsg != "No Error")
                //    MessageBox.Show(string.Format("APS_read_d_input Error:{0}", innerMsg));
                ThrowIfResultError(ret, string.Format("APS_read_d_input[{0}]", boardNo));
            }
            return value;
        }

        private void SetAxisParam(int axisNo, int paramNo, int paramValue)
        {
            int ret = APS168.APS_set_axis_param(axisNo, paramNo, paramValue);
            var innerMsg = "";
            innerMsg = GetErrorCodeDesc((APS_Define)ret);
            if (innerMsg != "No Error")
                MessageBox.Show(string.Format("设置轴参数错误:{0}", innerMsg));
            //ThrowIfResultError(ret, string.Format("APS_set_axis_param({0},{1},{2})", axisNo, paramNo, paramValue));
        }

        private void SetAxisParam(int axisNo, int paramNo, double paramValue)
        {
            int ret = APS168.APS_set_axis_param_f(axisNo, paramNo, paramValue);//5000);
            //var innerMsg = "";
            //innerMsg = GetErrorCodeDesc((APS_Define)ret);
            //if (innerMsg != "No Error")
            //    MessageBox.Show(string.Format("设置轴参数错误:{0}", innerMsg));
            //ThrowIfResultError(ret, string.Format("APS_set_axis_param_f({0},{1},{2})", axisNo, paramNo, paramValue));
        }

        private int GetMotionIoStatus(int axisNo)
        {
            var ret = APS168.APS_motion_io_status(axisNo);
            //if (ret < 0)
            //{
            //    var innerMsg = "";
            //    innerMsg = GetErrorCodeDesc((APS_Define)ret);
            //    if (innerMsg != "No Error")
            //        MessageBox.Show(string.Format("APS_motion_io_status Error:{0}", innerMsg));
            //    //ThrowIfResultError(ret);
            //}
            return ret;
        }

        private int GetMotionStatus(int axisNo)
        {
            var ret = APS168.APS_motion_status(axisNo);
            //if (ret < 0)
            //{
            //    var innerMsg = "";
            //    innerMsg = GetErrorCodeDesc((APS_Define)ret);
            //    if (innerMsg != "No Error")
            //        MessageBox.Show(string.Format("APS_motion_status Error:{0}", innerMsg));
            //    //ThrowIfResultError(ret);
            //}
            return ret;
        }
    }
}