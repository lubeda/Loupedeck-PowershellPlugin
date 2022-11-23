namespace Loupedeck.PowershellPlugin.Models
{
    using System;


    // public internal ConcurrentDictionary<String, PowershellResponse> _DataCache = new ConcurrentDictionary<String, PowershellResponse>();

    public sealed class PowershellResponse
    {
        public String Text { get; set; }
        public Byte[] Icon { get; set; }
        public String Icon64 { get; set; }
        public Boolean IsValid { get; set;}

        public String bgColor{ get; set; }
        public String fgColor { get; set; }
        public Boolean IsLoading { get; set; }

        public PowershellResponse()
        {
            this.IsValid = false;
            this.IsLoading = false;
        }
    }
}
