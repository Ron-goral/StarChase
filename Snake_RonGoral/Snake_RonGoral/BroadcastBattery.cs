using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snake_RonGoral
{
    [BroadcastReceiver(Enabled = true,Exported = true)]
    [IntentFilter(new[] { Intent.ActionBatteryChanged, Intent.ActionBatteryLow })]
    public class BroadcastBattery : BroadcastReceiver
    {
        private TextView tv;
        
        public BroadcastBattery(TextView tv)
        {
            this.tv = tv;
        }
        public BroadcastBattery()
        {
        }

        public override void OnReceive(Context context, Intent intent)
        {
            int battery = intent.GetIntExtra("level", 0);
            tv.Text = "" + battery + "%";
            if (intent.Action == Intent.ActionBatteryLow)
            {
                Toast.MakeText(context, "Low Battery--> please plug in charger!!", ToastLength.Long);
            }
        }
    }
}
