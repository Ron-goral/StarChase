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
    internal class Met:Sprite
    {
        private bool active;
        public Met(Bitmap bitmap, int x, int y, int screenWidth, int screenHeight) : base(screenWidth, screenHeight)
        {
            this.bitmap = bitmap;
            this.x = x;
            this.y = y;
            this.active = false;
        }
        public void Move(int speed)
        {   
           
            this.y += speed;
            if (this.y>=screenHeight)
            {
                this.active = false;
            }
            base.Move();
        }
        public void SetActive(bool active)
        {
            this.active = active;
        }
        public bool GetActive()
        {
            return this.active;
        }
        public void Sety(int y)
        {
            this.y = y;
        }
    }
}