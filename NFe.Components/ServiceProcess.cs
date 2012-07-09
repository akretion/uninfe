using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;

namespace NFe.Components
{
    public class ServiceProcess
    {
        public static void StartService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                if (service.Status == ServiceControllerStatus.Running)
                    return;// throw new Exception("Serviço já está em execução");

                if (service.Status == ServiceControllerStatus.StopPending)
                    throw new Exception("Serviço está em processo de paralização");

                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                }
            }
            catch
            {
                throw;
            }
        }
        public static void StopService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                if (service.Status == ServiceControllerStatus.Running)
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                }
            }
            catch
            {
                throw;
            }
        }
        public static void ContinueService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                if (service.Status == ServiceControllerStatus.Paused)
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                    service.Continue();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                }
            }
            catch
            {
                throw;
            }
        }
        public static void PauseService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                if (service.Status == ServiceControllerStatus.Running)
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                    service.Pause();
                    service.WaitForStatus(ServiceControllerStatus.Paused, timeout);
                }
            }
            catch
            {
                throw;
            }
        }
        public static void RestartService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                if (service.Status == ServiceControllerStatus.Running)
                {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                }
                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch
            {
                throw;
            }
        }
        public static bool IsServiceInstalled(string serviceName)
        {
            // get list of Windows services
            ServiceController[] services = ServiceController.GetServices();

            // try to find service name
            foreach (ServiceController service in services)
            {
                if (service.ServiceName == serviceName)
                    return true;

                //if (service.ServiceName.ToLower().StartsWith("u"))
                //MessageBox.Show(service.ServiceName);
            }
            return false;
        }

        public static ServiceControllerStatus StatusService(string serviceName)
        {
            if (IsServiceInstalled(serviceName))
            {
                ServiceController service = new ServiceController(serviceName);
                return service.Status;
            }
            else
                return ServiceControllerStatus.Stopped;
        }
    }
}
