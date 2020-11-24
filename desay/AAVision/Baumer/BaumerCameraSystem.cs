using BGAPI2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace desay
{
   public class BaumerCameraSystem
    {
       //Form1 mForm;
       public BaumerCameraSystem()
       {
          
       }
        public struct CameraInfo
        {
            public BGAPI2.Device pDevice;
            public string strSN;
            public string strType;
            public string strName;
        }
        private bool bForceIP = false;
        private object Mutex = new object();

        static public int m_nNum = 0;
        static public List<CameraInfo> listCamera = new List<CameraInfo>();
        static public List<BGAPI2.System> listSystem = new List<BGAPI2.System>();
        static public List<BGAPI2.Interface> listInterface = new List<Interface>();
        static public BGAPI2.SystemList mSystemList = null;
        private SystemList _systemList;

        public bool SearchAllCamera()
        {
            lock (Mutex)
            {
                if (mSystemList != null)
                    return true;
                listCamera.Clear();
                try
                {
                    BGAPI2.InterfaceList interfaceList = null;
                    BGAPI2.DeviceList deviceList = null;

                    //string filepath = Application.StartupPath; //DLL存放的相对路径
                    //_systemList = mSystemList = SystemList.CreateInstanceFromPath(filepath);
                    mSystemList = SystemList.Instance;
                    mSystemList.Refresh();
                    int nSize = mSystemList.Count;

                    foreach (KeyValuePair<string, BGAPI2.System> sys_pair in mSystemList)
                    {
                        listSystem.Add(sys_pair.Value);
                        sys_pair.Value.Open();
                        string sysType = sys_pair.Value.TLType;
                        string sysPath = sys_pair.Value.PathName;

                        interfaceList = sys_pair.Value.Interfaces;
                        interfaceList.Refresh(200);

                        int infcount = interfaceList.Count;

                        foreach (KeyValuePair<string, BGAPI2.Interface> ifc_pair in interfaceList)
                        {
                            
                            ifc_pair.Value.Open();
                            deviceList = ifc_pair.Value.Devices;
                           
                            deviceList.Refresh(200);
                            int nSizeDev = deviceList.Count;
                            if (nSizeDev > 0)
                            {
                             //   ifc_pair.Value.PnPEvent += mForm.Value_PnPEvent;
                                ifc_pair.Value.RegisterPnPEvent(BGAPI2.Events.EventMode.EVENT_HANDLER);
                                listInterface.Add(ifc_pair.Value);
                            }
                            else
                            {
                                ifc_pair.Value.Close();
                                continue;
                            }
                            bForceIP = true;
                            foreach (KeyValuePair<string, BGAPI2.Device> dev_pair in deviceList)
                            {
                                if (dev_pair.Value.TLType == "GEV")
                                {
                                    //////////Force  IP
                                    if (bForceIP)
                                    {
                                        //original device IP 
                                        long iDevIPAddress = (long)dev_pair.Value.NodeList["GevDeviceIPAddress"].Value;
                                        string strDevIpAddress = (iDevIPAddress >> 24).ToString() + "." +
                                                                 ((iDevIPAddress & 0xffffff) >> 16).ToString() + "." +
                                                                 ((iDevIPAddress & 0xffff) >> 8).ToString() + "." +
                                                                 (iDevIPAddress & 0xff).ToString();

                                        //original device subnet mask
                                        long iDevSubnetMask = (long)dev_pair.Value.NodeList["GevDeviceSubnetMask"].Value;
                                        string strDevSubnetMask = (iDevSubnetMask >> 24).ToString() + "." +
                                                                  ((iDevSubnetMask & 0xffffff) >> 16).ToString() + "." +
                                                                  ((iDevSubnetMask & 0xffff) >> 8).ToString() + "." +
                                                                  (iDevSubnetMask & 0xff).ToString();

                                        //CHECK THE SUBNETS ARE MATCHING
                                        long iDeviceSubnet = iDevIPAddress & iDevSubnetMask;
                                        long iIpAddress = (long)dev_pair.Value.Parent.NodeList["GevInterfaceSubnetIPAddress"].Value;
                                        long iSubnetMask = (long)dev_pair.Value.Parent.NodeList["GevInterfaceSubnetMask"].Value;
                                        long iInterfaceSubnet = iIpAddress & iSubnetMask;

                                        if (iDeviceSubnet != iInterfaceSubnet)
                                        {
                                            //Try ForceIP on camera to get temporary match
                                            long iDeviceMacAddress = (long)dev_pair.Value.NodeList["GevDeviceMACAddress"].Value;
                                            dev_pair.Value.NodeList["MACAddressNeededToForce"].Value = iDeviceMacAddress;

                                            long iForceIPAddress = iInterfaceSubnet + 1; // e.g. 192.168.1.1
                                            string strNewDevIpAddress = (iForceIPAddress >> 24).ToString() + "." +
                                                                        ((iForceIPAddress & 0xffffff) >> 16).ToString() + "." +
                                                                        ((iForceIPAddress & 0xffff) >> 8).ToString() + "." +
                                                                        (iForceIPAddress & 0xff).ToString();

                                            if (iForceIPAddress == iIpAddress)
                                            {
                                                iForceIPAddress = iForceIPAddress + 1; // e.g. 192.168.1.2
                                                strNewDevIpAddress = (iForceIPAddress >> 24).ToString() + "." +
                                                                     ((iForceIPAddress & 0xffffff) >> 16).ToString() + "." +
                                                                     ((iForceIPAddress & 0xffff) >> 8).ToString() + "." +
                                                                     (iForceIPAddress & 0xff).ToString();

                                                // more checks necessary, like another camera might use that IP address...

                                            }

                                            dev_pair.Value.NodeList["ForcedIPAddress"].Value = iForceIPAddress;

                                            dev_pair.Value.NodeList["ForcedSubnetMask"].Value = iSubnetMask;

                                            long iForceIPGateway = 0;
                                            dev_pair.Value.NodeList["ForcedGateway"].Value = iForceIPGateway;

                                            dev_pair.Value.NodeList["ForceIP"].Execute();

                                        }
                                    }
                                }

                                CameraInfo stCam;
                                stCam.strSN = dev_pair.Value.SerialNumber;
                                stCam.strName = dev_pair.Value.NodeList["DeviceModelName"].Value.ToString();
                                stCam.pDevice = deviceList[dev_pair.Key];
                                stCam.strType = dev_pair.Value.TLType;
                             
                                if (listCamera.FindIndex((c) => c.strSN == stCam.strSN) == -1)
                                {
                                  //  if(dev_pair.Value.Vendor.Contains("Baumer"))
                                    listCamera.Add(stCam);
                                }
                            }

                            deviceList.Refresh(100);

                        }

                    }

                }

                catch (BGAPI2.Exceptions.IException ex)
                {
                    string str;
                    str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                    MessageBox.Show(str);
                    return false;
                }

                return true;
            }

        }

        BaumerCamera camera = new BaumerCamera();
        void Value_PnPEvent(object sender, BGAPI2.Events.PnPEventArgs e)
        {
            if (e.PnPType == BGAPI2.Events.PnPType.DEVICEREMOVED)
            {
                MessageBox.Show(e.SerialNumber + ": Disconnected");
            }
            else
            {
                 BGAPI2.Device device= BaumerCameraSystem.listCamera.Find((c) => c.strSN == e.SerialNumber).pDevice;
                 camera.Release();
                 //device.Close();

                 camera.Initialize(e.SerialNumber);
                //foreach (KeyValuePair<string, BGAPI2.DataStream> item in device.DataStreams)
                //{
                //    if (!item.Value.IsOpen)
                //    {

                //        item.Value.Open();
                //    }
                //    item.Value.NewBufferEvent += camera.mDataStream_NewBufferEvent;
                //    item.Value.RegisterNewBufferEvent(BGAPI2.Events.EventMode.EVENT_HANDLER);
                //     item.Value.StartAcquisition();
                //}
                //device.RemoteNodeList["AcquisitionStart"].Execute();

	
                MessageBox.Show(e.SerialNumber + ":Connected");
            }
        }

        public bool ReleaseSystem()
        {
            try
            {
                foreach (BGAPI2.Interface ifc in listInterface)
                {
                    ifc.Close();
                }
                listInterface.Clear();

                foreach (BGAPI2.System sys in listSystem)
                {
                    sys.Close();
                }
                listSystem.Clear();
                //  BGAPI2.SystemList.ReleaseInstance();
                return true;
            }
            catch (BGAPI2.Exceptions.IException ex)
            {

                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);
                return false;
            }
        }
    }
}
