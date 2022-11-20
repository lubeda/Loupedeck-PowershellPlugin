namespace Loupedeck.PowershellPlugin
{
    using System;

    public sealed class PowershellApplication : ClientApplication
    {
        public PowershellApplication()
        {

        }

        protected override String GetProcessName() => "";

        protected override String GetBundleName() => "";
    }
}