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
    internal class Record
    {
        private int location;
        private int score;
        private string name;
        private DateTime date;
        
        
        public Record(string name, int score, DateTime date)
        {
            this.name = name;
            this.score = score;
            this.date = date;
        }

        public int Score { get => score; set => score = value; }
        public string Name { get => name; set => name = value; }
        public DateTime Date { get => date; set => date = value; }
    }
}