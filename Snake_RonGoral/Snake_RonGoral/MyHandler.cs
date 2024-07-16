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
    internal class MyHandler : Handler
    {
        private Context context;
        
        public MyHandler(Context context)
        {
             this.context = context;
        }
        public override void HandleMessage(Message msg)
        {
            if (msg.Arg1 == 0) 
                ((GameActivity)context).UpdateScore();
            else if (msg.Arg1 == 1) 
            {
                ((GameActivity)context).Nohearts();
                ((GameActivity)context).CreatedialogGameover();
               
            }
            else if (msg.Arg1 == 2)
            {
                ((GameActivity)context).UpdateHearts();
            }
            base.HandleMessage(msg);
        }
    }
}