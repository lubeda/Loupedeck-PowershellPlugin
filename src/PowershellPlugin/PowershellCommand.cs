namespace Loupedeck.PowershellPlugin.Commands
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Threading.Tasks;
    using System.Timers;

    using Timer = System.Timers.Timer;

    public sealed class PowershellCommand : PluginDynamicCommand
    {
        private readonly Helpers.PowershellHelper _PowershellHelper;
        private readonly static PowershellPlugin Parent = PowershellPlugin.Instance;
        private readonly Timer Timer = new Timer(60*1000) { Enabled = false, AutoReset = true };

        public PowershellCommand() : base("Scripts", "Powershell Locations", "Powershell Locations")
        {
            this.MakeProfileAction("text;Filename to ps1 file");
            this.Timer.Elapsed += this.OnTimerElapse;
            this.Timer.Enabled = true;
            this._PowershellHelper = new Helpers.PowershellHelper();
        }

        protected override Boolean OnLoad()
        {
            return base.OnLoad();
        }

        private void OnTimerElapse(Object sender, ElapsedEventArgs e)
        {
            foreach (var ap in this._PowershellHelper.DataKeys())
            {
                this._PowershellHelper.CallPowershellAsync("refresh", ap);
                this.ActionImageChanged(ap);
            }
            this.Timer.AutoReset = true;
            this.Timer.Enabled = true;
        }


        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            if (String.IsNullOrEmpty(actionParameter))
            {
                return null;
            }
           
            this._PowershellHelper.DataGet(actionParameter, out var data);
            if (data != null)
            {
                var fgColor = BitmapColor.White;
                var bgColor = BitmapColor.Black;
                
                
                if (! data.fgColor.IsNullOrEmpty())
                {
                    var fcol = (UInt32)Convert.ToInt32(data.fgColor, 16);
                    fgColor = new BitmapColor(fcol);
                }

                if (! data.bgColor.IsNullOrEmpty())
                {
                    var bcol = (UInt32)Convert.ToInt32(data.bgColor, 16);
                    bgColor = new BitmapColor(bcol);
                }

                var iconBuilder = new BitmapBuilder(imageSize);
                iconBuilder.Clear(bgColor);

                if (data.Icon == null)
                {
                    iconBuilder.FillRectangle(0, 0, iconBuilder.Width, iconBuilder.Height, bgColor);
                }
                
                if (data.IsValid)
                {
                    iconBuilder.DrawText(data.Text, color: fgColor, fontSize: 17);
                } else
                {
                    iconBuilder.DrawText("waiting\nfor data", fgColor);
                }

                var renderedImage = iconBuilder.ToImage();
                iconBuilder.Dispose();

                return renderedImage;
            } 
            return null;           
        }
        
        protected override void RunCommand(String actionParameter) //  => System.Diagnostics.Process.Start($"Powershell:{actionParameter.Split(':')[0]}");
        {
            
            this._PowershellHelper.CallPowershellAsync("trigger", actionParameter);
            this.ActionImageChanged(actionParameter);
        }
    }
}
