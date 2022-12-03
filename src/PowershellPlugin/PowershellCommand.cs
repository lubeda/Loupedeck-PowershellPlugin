namespace Loupedeck.PowershellPlugin.Commands
{
    using System;
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
                this.RunComandAsync(ap, "refresh");
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

                if (data.percent > 0)
                {
                    return PowershellDrawingHelper.DrawPercent(imageSize, bgColor, fgColor, data.percent);
                }

                var iconBuilder = new BitmapBuilder(imageSize);
                iconBuilder.Clear(bgColor);

                if (data.Icon64 == null)
                {
                    iconBuilder.FillRectangle(0, 0, iconBuilder.Width, iconBuilder.Height, bgColor);
                } else
                {
                    iconBuilder.SetBackgroundImage(BitmapImage.FromBase64String(data.Icon64));
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
            RunComandAsync(actionParameter, "trigger");
        }

        protected async Task RunComandAsync(String actionParameter,String mode)
        {
            var _task = this._PowershellHelper.CallPowershellAsync(mode, actionParameter) ;
            _task.ContinueWith(_t => { this.ActionImageChanged(actionParameter); } );
        }
        
    }
}
