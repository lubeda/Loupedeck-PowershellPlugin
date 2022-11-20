namespace Loupedeck.PowershellPlugin.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public sealed class PowershellResponse
    {
        public string Text { get; set; }
        public BitmapImage Icon { get; set; }
        public bool IsValid { get; set; }
        public bool IsLoading { get; set; }
        public bool IsToggled { get; set; }
    }

}
