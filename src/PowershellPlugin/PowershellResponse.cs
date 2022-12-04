namespace Loupedeck.PowershellPlugin.Models
{
    using System;
    using System.Collections.Generic;


    // public internal ConcurrentDictionary<String, PowershellResponse> _DataCache = new ConcurrentDictionary<String, PowershellResponse>();

    public sealed class PowershellResponse
    {
        public String mode { get; set; }
        public List<Text> text { get; set; }
        public Color bgcolor { get; set; }
        public String backgroundimage { get; set; }
        public Color indicator { get; set; }
        public Boolean IsValid { get; set; }
        public Boolean IsLoading { get; set; }

        public class Color
        {
            public Byte R { get; set; }
            public Byte G { get; set; }
            public Byte B { get; set; }
            public Byte A { get; set; }

            public Color()
            {
                this.A = 255;
            }
        }

        public class Position
        {
            public Int16 y { get; set; }
            public Int16 x { get; set; }
        }

        public class Text
        {
            public Int16 fontsize { get; set; }
            public Color color { get; set; }
            public Position position { get; set; }
            public String text { get; set; }

            public void Tex1t ()
            {
                this.text = "Null";
                this.position.x = 10;
                this.position.y = 10;
                this.color.A = 255;
                this.color.R = 155;
                this.color.G = 155;
                this.color.B = 155;
                this.fontsize = 15;
            }
        }

        public PowershellResponse()
        {
            this.IsValid = false;
            this.IsLoading = false;
        }
    }
}
