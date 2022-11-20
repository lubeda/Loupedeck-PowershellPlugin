namespace Loupedeck.PowershellPlugin.Helpers
{ 
    using System;
    using Newtonsoft.Json;
    using Loupedeck.PowershellPlugin.Models;
    using System.Management.Automation;
    using System.Collections.ObjectModel;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public sealed class PowershellHelper
    {

        public static ConcurrentDictionary<String, PowershellResponse> _DataCache;
        public PowershellHelper()
        {

            _DataCache = new ConcurrentDictionary<String, PowershellResponse>();
        }

        ~PowershellHelper()
        {

        }

        public void DataRemove(String ap)
        {
            _DataCache.TryRemove(ap, out var _);
        }

        public ICollection<String> DataKeys()
        {
            return _DataCache.Keys;           
        }

        public PowershellResponse DataGet(String ap,out PowershellResponse data)
        {
            // var data = new PowershellResponse();
            if (! _DataCache.TryGetValue(ap, out data))
            {
                _DataCache[ap] = new PowershellResponse();
                data = new PowershellResponse();
            }
            return data;
        }

        public void CallPowershell(String mode, String actionParameter)
        {
            var result = new PowershellResponse();

            var _ps = PowerShell.Create(); // System.Management.Automation.Powershell.Create();
            try
            {
                
                Collection<String> retvals = _ps.AddCommand(actionParameter)
                                 .AddParameter("mode", mode)
                                 .AddCommand("convertto-json")
                                 .Invoke<String>();
                if (_ps.HadErrors)
                {
                    result.Text = "PS\nError";
                } else
                {
                    foreach (var val in retvals)
                    {
                        result = JsonConvert.DeserializeObject<PowershellResponse>(val);
                    }
                }
            }
            catch
            {
                result.Text = "Type\nError";
            }
            finally
            {
                _ps.Dispose();
            }
            result.IsValid = true;
            // var data = new PowershellResponse();
            if (_DataCache.ContainsKey(actionParameter))
            {
                // _DataCache.TryUpdate(actionParameter, result,result);
                _DataCache[actionParameter] = result;
            } else
            {
                _DataCache.TryAdd(actionParameter, result);
            }
        }
    }
}
