namespace Loupedeck.PowershellPlugin.Models
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    // public internal ConcurrentDictionary<String, PowershellResponse> _DataCache = new ConcurrentDictionary<String, PowershellResponse>();

    public sealed class PowershellResponse
    {
        public String Text { get; set; }
        public Byte[] Icon { get; set; }
        public String Icon64 { get; set; }

        public Boolean IsValid { get; set; }

        public PowershellResponse() => this.IsValid = false;

    }

}
