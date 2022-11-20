namespace Loupedeck.PowershellPlugin.Commands
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Timers;

    using Loupedeck.PowershellPlugin.Models;

    using Timer = System.Timers.Timer;

    public sealed class PowershellCommand : PluginDynamicCommand
    {
        private CancellationToken _instanceCancellation = new CancellationToken();

        private readonly Helpers.PowershellHelper _PowershellHelper;
        private readonly static PowershellPlugin Parent = PowershellPlugin.Instance;
        private readonly Timer Timer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds) { Enabled = false, AutoReset = true };

        static internal ConcurrentDictionary<string, PowershellResponse> DataCache = new ConcurrentDictionary<string, PowershellResponse>();

        public PowershellCommand() : base("Locations", "Powershell Locations", "Powershell Locations")
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

        private void OnTimerElapse(object sender, ElapsedEventArgs e)
        {
            foreach (var ap in DataCache.Keys)
            {
                this.RetrieveData(ap, this._instanceCancellation);
                this.ActionImageChanged(ap);
                DataCache.TryRemove(ap, out var _);
            }

            this.Timer.AutoReset = true;
            this.Timer.Enabled = true;
            this._instanceCancellation = new CancellationToken(false);
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            if (string.IsNullOrEmpty(actionParameter))
            {
                return null;
            }

            PowershellResponse data = this.GetData(actionParameter);
            if (!data.IsValid)
            {
                return null;
            }

            var iconBuilder = new BitmapBuilder(imageSize);
            iconBuilder.Clear(BitmapColor.Black);

            if (data.Icon != null)
            {
                iconBuilder.DrawImage(data.Icon);
            }

            iconBuilder.FillRectangle(0, 0, iconBuilder.Width, iconBuilder.Height, new BitmapColor(0, 0, 0, 128));

            /*
            if(data.Temperature.C != data.Temperature.F)
            {
                iconBuilder.DrawText($"{locationName}{Math.Round(data.Temperature.C)}°C/{Math.Round(data.Temperature.F)}°F", color: BitmapColor.Black, fontSize: 17);
                iconBuilder.DrawText($"{locationName}{Math.Round(data.Temperature.C)}°C/{Math.Round(data.Temperature.F)}°F", color: BitmapColor.White);
            }
            else
            {
            */
            iconBuilder.DrawText($"PARAMETER\nERROR", color: BitmapColor.Black, fontSize: 17);
            iconBuilder.DrawText($"PARAMETER\nERROR", color: BitmapColor.White);
            

            var renderedImage = iconBuilder.ToImage();
            iconBuilder.Dispose();

            return renderedImage;
        }

        protected override void RunCommand(String actionParameter) //  => System.Diagnostics.Process.Start($"Powershell:{actionParameter.Split(':')[0]}");
        {
            var Powershell =  this._PowershellHelper.CallPowershell("trigger", actionParameter);
            this.ActionImageChanged(actionParameter);
        }

        private async void RetrieveData(string actionParameter, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(actionParameter))
            {
                return;
            }

            PowershellResponse data = this.GetData(actionParameter);

            if (data.IsLoading)
            {
                return;
            }

            data.IsLoading = true;

            try
            {

                var Powershell = await this._PowershellHelper.CallPowershell("refresh",actionParameter);
                if (Powershell == null)
                {
                    // Data is from an older version, clearing. 
                    data.Text = "ERR";
                    return;
                }

                data.Text = Powershell.Text;

                // data.Icon = BitmapImage.FromBase64String(Convert.ToBase64String(Powershell.Powershell[0].iconBytes));
            }
            finally
            {
                data.IsLoading = false;
                data.IsValid = true;
                this.ActionImageChanged(actionParameter);
            }
        }

        private PowershellResponse GetData(string actionParameter)
        {
            if (DataCache.TryGetValue(actionParameter, out var data))
            {
                return data;
            }

            data = new PowershellResponse();
            DataCache.TryAdd(actionParameter, data);
            
            this.RetrieveData(actionParameter, this._instanceCancellation);

            return data;
        }
    }
}
