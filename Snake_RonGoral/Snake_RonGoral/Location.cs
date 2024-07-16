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
    public class Location
    {
        private int x;
        private int y;

        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;         
        }   

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
    }
}