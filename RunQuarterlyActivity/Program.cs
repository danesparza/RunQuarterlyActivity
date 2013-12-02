using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunQuarterlyActivity
{
    class Program
    {
        static void Main(string[] args)
        {
            //  Helper array of quarters.  Pass in a month (as an index) 
            //  and you'll get back the quarter that the month is in:
            int[] MonthToQuarter = new int[] {4,4,1,1,1,2,2,2,3,3,3,4};

            //  Our current month:
            int currentMonth = DateTime.Now.Month;

            //  Determine what quarter it is:
            int currentQuarter = MonthToQuarter[currentMonth - 1];
            Trace.TraceInformation("The current month is: {0}, the quarter is: {1}", currentMonth, currentQuarter);

            //  Find the configuration item for the current quarter:
            string configurationCommand = string.Format("quarter{0}Command", currentQuarter);
            string configurationArgs = string.Format("quarter{0}Args", currentQuarter);

            string command = ConfigurationManager.AppSettings[configurationCommand];
            string commandArgs = ConfigurationManager.AppSettings[configurationArgs];

            //  Execute that item:
            Trace.TraceInformation("Executing the command: {0} {1}", command, commandArgs);

            if(configurationCommand.Trim().Length > 0 && commandArgs.Trim().Length > 0)
            {
                ProcessStartInfo commandPInfo = new ProcessStartInfo();

                commandPInfo.Arguments = commandArgs;
                commandPInfo.FileName = command;

                Process commandProcess = Process.Start(commandPInfo);
                commandProcess.WaitForExit(30000);

                //  If it hasn't exited, but it's not responding...
                //  kill the process
                if(!commandProcess.HasExited && !commandProcess.Responding)
                {
                    commandProcess.Kill();
                }
            }
        }
    }
}
