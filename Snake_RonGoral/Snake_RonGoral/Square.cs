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
    internal class Square
    {
        private bool isApple;
        private bool isSnake;

        public Square()
        {
            this.isSnake = false;
            this.isApple = false;   
        }   

        public bool IsApple { get => isApple; set => isApple = value; }
        public bool IsSnake { get => isSnake; set => isSnake = value; }
    }
    
}