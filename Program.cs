using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace StopDamnWindowsUpdateService
{
    class Program
    {
        String winUpdateName = "wuauserv";
        static void Main(string[] args)
        {
            // Create an Instance of ServiceController
            
            int sflag = 0;
            // Define the name of your service here. 
            // I am using the 'ImapiService' for this example
            // because everyone should have this service
            // After this point, myService is now refering to "ImapiService"
            
            //   myService.
            // Get the status of myService
            // Possible Status Returns: { StartPending, Running, PausePending, Paused, 
            //          StopPending, Stopped, ContinuePending }
            // For more info on Service Status,
            // go to: http://msdn.microsoft.com/en-us/library/
            //             system.serviceprocess.servicecontrollerstatus.aspx
            ServiceController myService;// = new ServiceController();
            while (true)
            {
                //ServiceController myService = new ServiceController();
                myService = new ServiceController();
                myService.ServiceName = "wuauserv";
                Console.WriteLine("Start to check!");
                string svcStatus = myService.Status.ToString();
                try
                {
                    myService.Refresh();
                    if (svcStatus == "Running")
                    {
                        Console.WriteLine(myService.ServiceName + " is in a " +
                                          svcStatus + "State");
                        Console.WriteLine("Attempting to Stop!");
                        myService.Stop();   // STOP the service if it is already Running
                        // This next block is for example only to show you the states 
                        // that the service is going through when it is stopping
                        string svcStatusWas = "";   // This is used for this example only
                        while (svcStatus != "Stopped")
                        {
                            svcStatusWas = svcStatus;
                            myService.Refresh();
                            // REMEMBER: svcStatus was SET TO myService.Status.ToString above. 
                            // Use the Refresh() Method to refresh the value of myService.Status and 
                            // reassign it to svcStatus

                            svcStatus = myService.Status.ToString();
                        }
                        Console.WriteLine("Service Stopped!! sleep 1min to check");
                        sflag = 1;
                        System.Threading.Thread.Sleep(60000);
                    }
                    else if (svcStatus == "Stopped")
                    {
                        Console.WriteLine(myService.ServiceName +
                                " is in a " + svcStatus + "State");
                        if (sflag == 0)
                        {
                            Console.WriteLine("Sleep 5min!");
                            // START the service if it is already Stopped
                            /*myService.Start();
                            // This is used for this example only
                            string svcStatusWas = "";
                            while (svcStatus != "Running")
                            {
                                if (svcStatus != svcStatusWas)
                                // Check to see if the Staus is the same as it was before
                                {
                                    Console.WriteLine("Status: " + svcStatus);
                                }
                                svcStatusWas = svcStatus;
                                myService.Refresh();
                                svcStatus = myService.Status.ToString();
                            }
                            Console.WriteLine("Service Started!!");
                             */
                            System.Threading.Thread.Sleep(300000);
                            //System.Threading.Thread.Sleep(60000);
                        }
                        else
                        {
                            Console.WriteLine("stop once so sleep 2min to keep monitor!");
                            System.Threading.Thread.Sleep(180000);
                        } // end if
                    }
                    else
                    {
                        // STOP the service if it is in any other state
                        myService.Stop();
                        Console.WriteLine("Status: " + svcStatus);
                        while (svcStatus != "Stopped")
                        {
                            myService.Refresh();
                            svcStatus = myService.Status.ToString();
                        }
                        Console.WriteLine("Service Stopped!!");
                    }
                    // Notification that the program is going into a sleep state
                    Console.WriteLine("----Sleeping----");
                    myService = null;
                    // This is a way to pause your program. This is set to 30 seconds.
                    System.Threading.Thread.Sleep(10000);
                }
                catch (InvalidOperationException exp)
                {
                    Console.WriteLine("Exception: " + exp.ToString());
                }
            }
        }


    }
}
