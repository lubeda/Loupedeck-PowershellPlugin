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

