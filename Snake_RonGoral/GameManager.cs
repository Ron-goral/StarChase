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
    
    public class GameManager
    {
        Random rnd=new Random();
        Square[,] games;
        private int width;
        private int height; 
        public GameManager(int width, int height)
        {
            games=new Square[8, 9];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    games[i,j] = new Square();  
                }
            }
            this.width = width;
            this.height = height;

        }
        public void SetApple()
        {
            int x = rnd.Next(0, 8);
            int y = rnd.Next(0, 9);
            while(games[x,y].IsSnake)
            {
                x = rnd.Next(0, 8);
                y = rnd.Next(0, 9);
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    games[i, j].IsApple = false;
                }
            }
            games[x, y].IsApple=true;
        }

        public Location FindSnake()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (games[i,j].IsSnake)
                    {
                        return new Location(this.width  * i /8, this.height *j/9);
                    }
                }
            }
            return new Location(0, 0);
        }
        public Location Findapple()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (games[i, j].IsApple)
                    {
                        return new Location(this.width / 8 * i, this.height / 9 * j);
                    }
                }
            }
            return new Location(0, 0);  
        }
        public Location Center()
        {
            return new Location(this.width/2,this.height/2);
        }
    }   
}