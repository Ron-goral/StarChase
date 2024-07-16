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

namespace Star_Chase
{
    internal class Trooper:Sprite
    {
        private string direction;
        public Trooper(Bitmap bitmap, int x, int y, int screenWidth, int screenHeight) : base(screenWidth, screenHeight)
        {
            this.bitmap = bitmap;
            this.x = x;
            this.y = y;
            this.direction = "right";
        }

        public string Direction { get => direction; set => direction = value; }

        public void SetX(int x)
        {
            this.x = x;
        }
        public void SetY(int y)
        {
            this.y = y;
        }
        public bool Move()
        {
            if (direction == "up")
            {
                y -=4;
            }
            else if (direction == "down")
            {
                y += 4;
            }
            else if (direction == "left")
            {
                x -= 4;
            }
            if (direction == "right")
            {
                x += 4;
            }
            base.Move();
            return true;
        }
        public bool Limit()
        {
            if (x+bitmap.Width>screenWidth)
            {
                return true;
            }
            if (y + bitmap.Height> screenHeight)
            {
                return true;
            }
            if (x < 0)
            {
                return true;
            }
            if (y < 0)
            {
                return true;
            }
            return false;
        }
        
    }
}