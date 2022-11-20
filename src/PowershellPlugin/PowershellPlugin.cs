namespace Loupedeck.PowershellPlugin
{
    using System;
    using Microsoft.Win32;

    public sealed class PowershellPlugin : Plugin
    {
        internal static PowershellPlugin Instance { get; private set; }
        public override Boolean HasNoApplication => true;
        public override Boolean UsesApplicationApiOnly => true;

        public PowershellPlugin()
        {
            PowershellPlugin.Instance = this;
        }

        public override void Load() => this.Init();

        public override void Unload() { }

        private void OnApplicationStarted(Object sender, EventArgs e) { }

        private void OnApplicationStopped(Object sender, EventArgs e) { }

        public override void RunCommand(String commandName, String parameter) { }

        public override void ApplyAdjustment(String adjustmentName, String parameter, Int32 diff) { }

        private void Init()
        {
        }
    }
}
