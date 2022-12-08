namespace Loupedeck.PowershellPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    // This class implements an example adjustment that counts the rotation ticks of a dial.

    public class PowershellAdjustment : PluginDynamicAdjustment
    {
        // This variable holds the current value of the counter.
        private String _result = "init";
        private readonly Helpers.PowershellHelper _PowershellHelper;

        // Initializes the adjustment class.
        // When `hasReset` is set to true, a reset command is automatically created for this adjustment.
        public PowershellAdjustment()
                : base(displayName: "knobs", description: "left;right;click", groupName: "Knobs", hasReset: false)
        {
            this._PowershellHelper = new Helpers.PowershellHelper();
            this.MakeProfileAction("text;Filename to ps1 file");
        }

        // This method is called when the dial associated to the plugin is rotated.
        protected override void ApplyAdjustment(String actionParameter, Int32 diff)
        {
            var s = this.GetAdjustmentDisplayName(actionParameter,PluginImageSize.None);
            RunComandAsync(actionParameter, diff < 0 ? "left" : "right");
        }

        protected override BitmapImage GetAdjustmentImage(String actionParameter, PluginImageSize imageSize)
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
                    BitmapColor bgColor;
                    var maxx = PowershellDrawingHelper.GetDimension(imageSize);

                    if (data.bgcolor != null)
                    {
                        bgColor = new BitmapColor(data.bgcolor.R, data.bgcolor.G, data.bgcolor.B);
                    }
                    else
                    {
                        bgColor = new BitmapColor(0, 0, 0);
                    }

                    iconBuilder.Clear(bgColor);

                    if (File.Exists(data.backgroundimage))
                    {
                        iconBuilder.SetBackgroundImage(BitmapImage.FromFile(data.backgroundimage));
                    }


                    if (data.indicator != null)
                    {
                        var ci = new BitmapColor(data.indicator.R, data.indicator.G, data.indicator.B);

                        var max15 = maxx * (Single)0.15;
                        var max40 = (Single)maxx * 0.4;

                        iconBuilder.DrawLine(max15, max15, 0, 0, ci, (Single)max40);

                    }
                    if (data.text.Count > 0)
                    {
                        if (data.text[0].text != null)
                        {
                            var c = new BitmapColor(data.text[0].color.R, data.text[0].color.B, data.text[0].color.B);
                            iconBuilder.DrawText(text: data.text[0].text, x: data.text[0].position.x, y: data.text[0].position.y, color: c, fontSize: data.text[0].fontsize, height: 0, width: maxx);
                            // iconBuilder.DrawCircle(data.text[0].position.x, data.text[0].position.y, 3, BitmapColor.White);

                            if (data.text.Count > 1)
                            {
                                c = new BitmapColor(data.text[1].color.R, data.text[1].color.B, data.text[1].color.B);

                                iconBuilder.DrawText(text: data.text[1].text, x: data.text[1].position.x, y: data.text[1].position.y, color: c, fontSize: data.text[1].fontsize, height: 0, width: maxx);
                            }
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


        protected async Task RunComandAsync(String actionParameter, String mode)
        {
            await this._PowershellHelper.CallPowershellAsync(mode, actionParameter);
            this._result = mode;
            this.AdjustmentValueChanged(); // Notify the Loupedeck service that the adjustment value has changed.
        }
        // This method is called when the reset command related to the adjustment is executed.

        protected override void RunCommand(String actionParameter)
        {
            RunComandAsync(actionParameter, "click");
        }

        // Returns the adjustment value that is shown next to the dial.
        protected override String GetAdjustmentValue(String actionParameter) => this._result;

    }
}

