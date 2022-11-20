namespace Loupedeck.PowershellPlugin.Helpers
{ 
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading;
    using System.Net.Http.Json;
    using Loupedeck.PowershellPlugin.Models;
    using System.Management.Automation;

    public sealed class PowershellHelper
    {
        public PowerShell _ps; 
        public PowershellHelper()
        {
            this._ps = PowerShell.Create(); // System.Management.Automation.Powershell.Create();
        }

        ~PowershellHelper()
        {
            this._ps.Dispose();
        }

        public async Task<PowershellResponse> CallPowershell(string mode, string command)
        {
            var result = new PowershellResponse();
            try
            {
                var ret = this._ps.AddScript(command).AddParameter("mode", mode).AddParameter("result", result).Invoke();
            }
            catch
            {
                result.Text = "Error";
            }

            return result;
        }
    }
}
