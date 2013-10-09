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
            //  Our list of months and quarters:
            Dictionary<int, int> quarterData = new Dictionary<int, int>()
            {
                {1, 1},
                {2, 1},
                {3, 1},
                {4, 2},
                {5, 2},
                {6, 2},
                {7, 3},
                {8, 3},
                {9, 3},
                {10, 4},
                {11, 4},
                {12, 4},
            };

            //  Our current month:
            int currentMonth = DateTime.Now.Month;

            //  Determine what quarter it is:
            int currentQuarter = quarterData[currentMonth];
            Console.WriteLine("The current month is: {0}, the quarter is: {1}", currentMonth, currentQuarter);

            //  Find the configuration item for the current quarter:
            string configurationCommand = string.Format("quarter{0}Command", currentQuarter);
            string configurationArgs = string.Format("quarter{0}Args", currentQuarter);

            string command = ConfigurationManager.AppSettings[configurationCommand];
            string commandArgs = ConfigurationManager.AppSettings[configurationArgs];

            //  Execute that item:
            Console.WriteLine("Executing the command: {0} {1}", command, commandArgs);

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
