namespace Loupedeck.PowershellPlugin.Commands
{
    using System;
    using System.Collections.Concurrent;
    using System.Timers;
    using Loupedeck.PowershellPlugin.Models;

    using Timer = System.Timers.Timer;

    public sealed class PowershellCommand : PluginDynamicCommand
    {
        private readonly Helpers.PowershellHelper _PowershellHelper;
        private readonly static PowershellPlugin Parent = PowershellPlugin.Instance;
        private readonly Timer Timer = new Timer(60*1000) { Enabled = false, AutoReset = true };

        static internal ConcurrentDictionary<String, PowershellResponse> DataCache = new ConcurrentDictionary<String, PowershellResponse>();

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
                this._PowershellHelper.CallPowershell("refresh", ap);
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
                var iconBuilder = new BitmapBuilder(imageSize);
                iconBuilder.Clear(BitmapColor.Black);

                if (data.Icon != null)
                {
                    iconBuilder.FillRectangle(0, 0, iconBuilder.Width, iconBuilder.Height, new BitmapColor(100, 200, 0, 128));
                } else
                {
                    iconBuilder.FillRectangle(0, 0, iconBuilder.Width, iconBuilder.Height, new BitmapColor(100, 0, 0, 128));
                }
                
                if (data.IsValid)
                {
                    // iconBuilder.DrawText(data.Text, color: BitmapColor.Black, fontSize: 17);
                    iconBuilder.DrawText(data.Text, color: BitmapColor.White, fontSize: 17);
                } else
                {
                    iconBuilder.DrawText("waiting\nfor data", color: new BitmapColor(200, 100, 50));
                }
                
                var renderedImage = iconBuilder.ToImage();
                iconBuilder.Dispose();

                return renderedImage;
            } 
            return null;           
        }

        protected override void RunCommand(String actionParameter) //  => System.Diagnostics.Process.Start($"Powershell:{actionParameter.Split(':')[0]}");
        {
            this._PowershellHelper.CallPowershell("trigger", actionParameter);
            this.ActionImageChanged(actionParameter);
        }
    }
}
