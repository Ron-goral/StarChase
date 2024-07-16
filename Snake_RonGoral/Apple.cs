using Android.App;
using Android.Content;
using Android.Graphics;
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
    internal class Apple : Sprite
    {
        public Apple(int screenWidth, int screenHeight,Bitmap b) : base(screenWidth, screenHeight)
        {
            this.bitmap = b;
        }
        public void Move(int x, int y)
        {
            this.x = x; 
            this.y= y;
            base.Move();
        }
    }
}