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
    [Activity(Label = "Shop")]
    public class Shop_Activity : Activity
    {
        ISharedPreferences sp;
        private Button[] buttons;
        private int[] prices = { 0, 15, 25, 50, 75, 100, 150, 200, 250 };      
        private ImageButton btnhome;       
        private TextView tvcoins;
        private ImageView swcimage;
        private int character,total,app,num;
        Android.App.AlertDialog buy_dialog;
        private Bitmap bitmapBall;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.shop);
            // Create your application here
            btnhome = FindViewById<ImageButton>(Resource.Id.btn_homeshop);
            btnhome.Click += Btn_Click;
            int id,index;
            buttons = new Button[9];
            for (int i = 0; i < 9; i++)
            {
                index = i + 1;
                id = base.Resources.GetIdentifier("btn_buy" + index, "id", PackageName);
                buttons[i] = FindViewById<Button>(id);
                buttons[i].Click += Btn_Click;
            }
            

            sp = GetSharedPreferences("details", FileCreationMode.Private);
            tvcoins = FindViewById<TextView>(Resource.Id.tvcoinsshop);
            tvcoins.Text= sp.GetInt("totalscore", 0).ToString();
            swcimage = FindViewById<ImageView>(Resource.Id.imageView3);
            

            ResetPic();
            ResetApplyBuy();

            if (sp.GetInt("applied",0)==0)
            {
                buttons[0].SetBackgroundColor(Android.Graphics.Color.Gray);
            }
        }

        private void Shop_Activity_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        public void ResetApplyBuy()//עדכון סטטוס הקנייה והשימוש של הדמיות להתאמת הכפתורים
        {
            for (int i = 0; i < 9; i++)
            {
                if (i!=0 && sp.GetInt("buy"+i,0)==0) //item was never bought
                {
                    buttons[i].Clickable = true;
                    buttons[i].SetBackgroundColor(Android.Graphics.Color.RoyalBlue);
                    if (prices[i]!=0)
                    {
                        buttons[i].Text = prices[i].ToString();
                    }
                   
                }
                else
                {
                    if (sp.GetInt("applied", 0) != i)
                    {
                        buttons[i].Clickable = true;
                        buttons[i].SetBackgroundColor(Android.Graphics.Color.RoyalBlue);
                        buttons[i].Text = "Apply";
                    }
                    else if (sp.GetInt("applied",0)==i)
                    {
                        buttons[i].Clickable = false;
                        buttons[i].SetBackgroundColor(Android.Graphics.Color.Gray);
                        buttons[i].Text = "Apply";
                    }
                }
            }
        }
       
        public void ResetPic()// פעולה המעדכנת את תמונת הדמות בפינת המסך לדמות השחקן הנוכחית
        {
            character = sp.GetInt("applied", 0);

            if (character == 0)
            {
                this.bitmapBall = BitmapFactory.DecodeResource(this.Resources, Resource.Drawable.swc1);
            }
            if (character == 1)
            {
                this.bitmapBall = BitmapFactory.DecodeResource(this.Resources, Resource.Drawable.swc2);
            }
            if (character == 2)
            {
                this.bitmapBall = BitmapFactory.DecodeResource(this.Resources, Resource.Drawable.swc3);
            }
            if (character == 3)
            {
                this.bitmapBall = BitmapFactory.DecodeResource(this.Resources, Resource.Drawable.swc4);
            }
            if (character == 4)
            {
                this.bitmapBall = BitmapFactory.DecodeResource(this.Resources, Resource.Drawable.swc5);
            }
            if (character == 5)
            {
                this.bitmapBall = BitmapFactory.DecodeResource(this.Resources, Resource.Drawable.swc6);
            }
            if (character == 6)
            {
                this.bitmapBall = BitmapFactory.DecodeResource(this.Resources, Resource.Drawable.swc7);
            }
            if (character == 7)
            {
                this.bitmapBall = BitmapFactory.DecodeResource(this.Resources, Resource.Drawable.swc8);
            }
            if (character == 8)
            {
                this.bitmapBall = BitmapFactory.DecodeResource(this.Resources, Resource.Drawable.swc9);
            }
            swcimage.SetImageBitmap(bitmapBall);
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            total = sp.GetInt("totalscore", 0);
            Android.App.AlertDialog.Builder builder;
            if (sender==btnhome)
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }
            else {
                for (int i = 0; i < 9; i++)
                {
                    if (sender == buttons[i])
                        num = i;
                }
                builder = new Android.App.AlertDialog.Builder(this);
                if (num==0)
                {
                    builder.SetMessage("Apply");
                    builder.SetPositiveButton("Yes", YesAction_apply);
                    builder.SetNegativeButton("Cancel", NoAction);
                    builder.SetCancelable(true);
                    buy_dialog = builder.Create();
                    buy_dialog.Show();
                }
                else
                {
                    if (buttons[num].Text == prices[num].ToString())
                    {
                        builder.SetMessage("Buy this skin for " + prices[num] + " coins?");
                        builder.SetPositiveButton("Yes", YesAction_Buy);
                    }
                    else
                    {
                        builder.SetMessage("Apply");
                        builder.SetPositiveButton("Yes", YesAction_apply);

                    }
                    builder.SetNegativeButton("Cancel", NoAction);
                    builder.SetCancelable(true);
                    buy_dialog = builder.Create();
                    buy_dialog.Show();
                }
                
  
            }
        }

        private void YesAction_Buy(object sender, DialogClickEventArgs e)//פעולת קנייה המעדכנת את סכום הכסף ואת סטטוס הקניות של השחקן
        {
            int left = total - prices[num];
            if (left>=0)
            {
                ISharedPreferencesEditor editor = sp.Edit();
                editor.PutInt("buy" + num, 1);
                editor.PutInt("totalscore", left);
                editor.Commit();
                tvcoins.Text = left.ToString();                
                ResetApplyBuy();
            }
            else
            {
                Toast.MakeText(this, "Dont have enough money to buy this charachter", ToastLength.Long).Show();
            }
        }

        private void YesAction_apply(object sender, DialogClickEventArgs e)//פעולת שימוש בדמות שמעדכנת את הדמות בה השחקן משתמש בנוסף לתמונה שלה בחנות
        {
            
            ISharedPreferencesEditor editor = sp.Edit();
            editor.PutInt("applied", num);
            editor.Commit();
            ResetPic();           
            ResetApplyBuy();
        }
        
        private void NoAction(object sender, DialogClickEventArgs e)//סגירת דיאלוג קנייה או שינוי דמות
        {
            buy_dialog.Cancel();
        }        
    }
}