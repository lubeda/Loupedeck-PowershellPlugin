namespace Loupedeck.PowershellPlugin.Helpers
{ 
    using System;
    using Newtonsoft.Json;
    using Loupedeck.PowershellPlugin.Models;
    using System.Management.Automation;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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

        public async Task CallPowershellAsync(String mode, String actionParameter)
        {
            var result = new PowershellResponse();
            
            this.DataGet(actionParameter, out result);

            if (! result.IsLoading)
            {
                var _ps = PowerShell.Create(); // System.Management.Automation.Powershell.Create();

                _ps.AddCommand(actionParameter)
                                     .AddParameter("mode", mode)
                                     .AddCommand("convertto-json");

                result.IsLoading = true;
                
                var task = Task<PSDataCollection<PSObject>>.Factory.FromAsync(_ps.BeginInvoke(), _ps.EndInvoke);
                var objs = await task;
    
                if (_ps.Streams.Error.Count > 0)
                {
                    result.Text = "PS\nError";
                }
                else
                {
                    if (objs.Count > 0)
                    {
                        var str = objs[0].ToString();
                        if (str.IsNullOrEmpty())
                        {
                            result.Text = "empty\nresponse";
                        }
                        else
                        {
                            result = JsonConvert.DeserializeObject<PowershellResponse>(str);
                            result = JsonConvert.DeserializeObject<PowershellResponse>(str);
                        }
                    }
                    result.IsValid = true;
                }
                result.IsLoading = false;
            }
            
            if (_DataCache.ContainsKey(actionParameter))
            {
                _DataCache[actionParameter] = result;
            } else
            {
                _DataCache.TryAdd(actionParameter, result);
            }
        }
    }
}
