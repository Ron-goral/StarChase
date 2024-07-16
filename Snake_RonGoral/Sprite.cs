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
    internal class Sprite
    {
        protected int x;
        protected int y;
        protected Bitmap bitmap; // the picture of the sprite
        protected int screenWidth;
        protected int screenHeight;
        protected Rect rect; // a rectangle around the sprite

        public Sprite(int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.rect = new Rect();
        }
        public void Move()
        {
            rect.Set(x, y, x + bitmap.Width, y + bitmap.Height);
        }

        public Rect GetRect()
        {
            return this.rect;
        }

        public void Draw(Canvas canvas, Paint paint)
        {
            canvas.DrawBitmap(bitmap, x, y, paint);
        }
    }
    
}