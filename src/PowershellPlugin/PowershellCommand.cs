namespace Loupedeck.PowershellPlugin.Commands
{
    using System;
    
    using System.IO;
    using System.Threading.Tasks;

    using System.Timers;

    using Timer = System.Timers.Timer;

    public sealed class PowershellCommand : PluginDynamicCommand
    {
        private readonly Helpers.PowershellHelper _PowershellHelper;
        private readonly static PowershellPlugin Parent = PowershellPlugin.Instance;
        private readonly Timer Timer = new Timer(60 * 1000) { Enabled = false, AutoReset = true };

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
                var iconBuilder = new BitmapBuilder(imageSize);

                if (data.IsValid)
                {
                    

                    if (data.bgcolor != null)
                    {
                       var bgColor = new BitmapColor(data.bgcolor.R, data.bgcolor.G, data.bgcolor.B);
                    }

                    if (File.Exists(data.backgroundimage))
                    {
                        iconBuilder.SetBackgroundImage(BitmapImage.FromFile(data.backgroundimage));
                    }
                    
                    if (data.indicator != null)
                    {
                        var ci = new BitmapColor(data.indicator.R, data.indicator.G, data.indicator.B);
                        
                        var max = PowershellDrawingHelper.GetDimension(imageSize);
                        var max85 = max * (Single) 0.85;
                        var max40 = (Single) max * 0.4;
                        
                        iconBuilder.DrawLine(max85, max85, max,max, ci, (Single) max40);
                        
                    }
                    if (data.text[0].text != null)
                    {
                        var c = new BitmapColor(data.text[0].color.R, data.text[0].color.B, data.text[0].color.B);
                        iconBuilder.DrawText(text: data.text[0].text, x: data.text[0].position.x, y: data.text[0].position.y, color: c, fontSize: data.text[0].fontsize, height: 10, width: 50);
                        // iconBuilder.DrawText(text: data.text[0].text, color: c);
                        if (data.text[1].text != null)
                        {
                            c = new BitmapColor(data.text[1].color.R, data.text[1].color.B, data.text[1].color.B);
                            
                            iconBuilder.DrawText(text: data.text[1].text, x: data.text[1].position.x, y: data.text[1].position.y, color: c, fontSize: data.text[1].fontsize, height: 10, width: 50);
                        }
                    }
                }
                else
                {
                    var bgColor = BitmapColor.Black;
                    var fgColor = BitmapColor.White;
                    
                    iconBuilder.Clear(bgColor);
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

        protected async Task RunComandAsync(String actionParameter, String mode)
        {
            var _task = this._PowershellHelper.CallPowershellAsync(mode, actionParameter);
            _task.ContinueWith(_t => { this.ActionImageChanged(actionParameter); });
        }

    }
}

