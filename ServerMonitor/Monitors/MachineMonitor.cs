using ServerMonitor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerMonitor.Monitors
{
    public class MachineMonitor
    {
        private static bool _isRunning = false;
        private static DateTime deadTime;
        private static DateTime startTime;
        private static String recallUrl;
        private static int configId;
        private static int itemId;
        //private static int machineId;
        private static Thread thread;

        //private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private MachineMonitor()
        {
            startTime = DateTime.Now.ToLocalTime();
            deadTime = DateTime.Now.ToLocalTime();
        }
        public static void SetStartTime(DateTime t)
        {
            startTime = t;
        }

        public static void SetDeadTime(DateTime t)
        {
            deadTime = t;
        }

        public static void SetRecalUrl(string t)
        {
            recallUrl = t;
        }

        public static void SetConfigId(int t)
        {
            configId = t;
        }

        public static void SetItemId(int t)
        {
            itemId = t;
        }

        public static void StartMonitor()
        {
            if (_isRunning) {
                //Logger.Info("监控已启动");
                return;
            }

            DateTime time = DateTime.Now.ToLocalTime();
            //string ip = ServerInfo.GetLocalIP();
            //MachineInfo machineInfo = db.MachineInfo.Where(o => o.IP == ip).First();

            //if (time < deadTime && time > startTime)
            //Logger.Info("time:" + time);
            if (time < deadTime) {
                thread = new Thread(GetInfo);
                thread.Start();
                //Logger.Info("监控开启");
                _isRunning = true;
            } else {
                //Logger.Info("监控未开始或者已经结束");
            }

        }

        public static void StopMonitor()
        {
            thread.Abort();
            _isRunning = false;

            //Logger.Info("监控关闭");

        }

        public static void GetInfo()
        {
            //Logger.Info("Start GetInfo Process");
            DateTime dt = DateTime.Now;

            //int totalMemory = (int)((MemoryMonitor.getMemoryInfo().totalPhys / (1024 * 1024)));
            //int coreNum = ProcessorCountMonitor.getValue();
            MachineMonitorInfo info;
            //Logger.Info("Now:" + dt);

            while (dt < deadTime) {
                info = MonitorUtil.GetMachineMonitorInfo();

                //var client = new RestClient(recallUrl);
                string param = @"/createStressMonitorInfo.action?"
                    + "configId=" + configId
                    + "&itemId=" + itemId
                    //+ "&cpuData=" + info.cpuData
                    //+ "&coreNumber=" + info.coreNumber
                    //+ "&memoryData=" + info.memoryData
                    //+ "&memoryTotalData=" + info.memoryTotalData
                    + "&diskReadData=" + info.diskReadData
                    + "&diskWriteData=" + info.diskWriteData
                    + "&networkReceiveData=" + info.networkReceiveData
                    + "&networkSendData=" + info.networkSendData
                    + "&addInfoTime=" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                //Logger.Info("recallUrl : " + recallUrl);
                //Logger.Info("request : " + param);

                //var request = new RestRequest(param, Method.GET);
                //var response = client.Execute(request);

                //Logger.Info("response :" + response.StatusCode);

                //System.Threading.Thread.Sleep(1000);
                //dt = DateTime.Now;
                //Logger.Info("Now:" + dt);

            }

            _isRunning = false;

        }
    }
}
