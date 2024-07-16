using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Random = System.Random;

namespace Snake_RonGoral
{
    internal class GameBoard:SurfaceView
    {
        private const int FRAME_WIDTH = 100;
        private int frmWidth, frmHeight;
        private Paint paint;
        private Chain chain;
        private Bitmap bitmapBall,bitmapapple,bitmapmet,bitmapback;
        private ThreadStart ts;
        private Thread t;
        private Color bgColor;
        private GameManager gameManager;
        private bool run;
        private Apple apple;
        ISharedPreferences sp;
        private MyHandler scoreHandler;
        private Met[]mets;
        static Random rnd = new Random();
        public GameBoard(Context context, int frmWidth, int frmHeight, int charachter) : base(context)
        {
            this.frmWidth = frmWidth;
            this.frmHeight = frmHeight;


            this.paint = new Paint();

            if (charachter==0)// עדכון דמות בה השחקן משתמש במשחק
            {
                this.bitmapBall = BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.swc1);
            }
            else if (charachter == 1)
            {
                this.bitmapBall = BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.swc2);
            }
            else if (charachter == 2)
            {
                this.bitmapBall = BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.swc3);
            }
            else if (charachter == 3)
            {
                this.bitmapBall = BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.swc4);
            }
            else if (charachter == 4)
            {
                this.bitmapBall = BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.swc5);
            }
            else if (charachter == 5)  
            {
                this.bitmapBall = BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.swc6);
            }
            else if (charachter == 6)
            {
                this.bitmapBall = BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.swc7);
            }
            else if (charachter == 7)
            {
                this.bitmapBall = BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.swc8);
            }
            else if (charachter == 8)
            {
                this.bitmapBall = BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.swc9);
            }


            this.bitmapBall = Bitmap.CreateScaledBitmap(bitmapBall, 70, 60, false);
            this.chain = new Chain(bitmapBall, 0, 0, frmWidth, frmHeight);
            this.gameManager = new GameManager(frmWidth, frmHeight);
            this.bitmapapple = BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.coin);
            this.bitmapapple = Bitmap.CreateScaledBitmap(bitmapapple, 60, 60, false);
            this.apple = new Apple(frmWidth,frmHeight, bitmapapple);

            this.bitmapback= this.bitmapapple = BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.gamebackground1);

            this.bitmapmet = BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.met);
            this.bitmapmet = Bitmap.CreateScaledBitmap(bitmapmet, 90, 90, false);

            scoreHandler = new MyHandler(context);
            gameManager.SetApple();


            mets = new Met[10];
            for (int i = 1; i < mets.Length; i++)// אתחול עצמים מסוג מטאור
            {
                mets[i] = new Met(bitmapmet, frmWidth/9*i-10, 0, frmWidth, frmHeight);
            }

            this.paint.SetStyle(Paint.Style.Fill);
            this.bgColor = Color.LightCyan;
            

            this.run = true;
            this.ts = new ThreadStart(Run);
            this.t = new Thread(ts);
            this.t.Start();
        }

        internal void UpdateDirection(string v)
        {
            chain.Direction = v;
        }
       
        
        private void Run()
        {
            
            Message msg = scoreHandler.ObtainMessage();          
            Canvas canvas = null;
            int count = 0;
            int lives = 3;
            int numMets=0;
            bool once = true;
            while (run)
            {
                if (Holder.Surface.IsValid)
                {
                    try
                    {
                        canvas = Holder.LockCanvas();
                        canvas.DrawBitmap(bitmapback, 0, 0,null);
                        //DrawFrame(canvas);
                        DrawChain(canvas);
                        DrawApple(canvas);
                        if (count % 3 == 0 && count != 0)//כל פעם אחרי שהשחקן אוסף 3 מטבעות מחילים ליפול מטאורים, הכמות שלהם תלויה בניקוד השחקן כל פעם
                        {
                            if (count < 10)//מספר המטאורים נקבע לפי טווח הניקוד של השחקן, ככל שהשחקן מתקדם יותר יהיו יותר מטאורים
                                numMets = 4;
                            else if (count < 20)
                                numMets = 5;
                            else if (count < 30)
                                numMets = 6;
                            else if (count < 40)
                                numMets = 7;
                            else if (count < 50)
                                numMets = 8;
                            else if (count < 60)
                                numMets = 9;
                            else
                                numMets = 10;
                            if (!IsThereActive())
                            {
                                once = true;
                                RandomizeActives(numMets);
                            }
                            else
                            {
                                //kill and init new
                            }
                            //DrawMet(canvas,numMets);
                        }
                        DrawMet(canvas, numMets);
                        
                        if (CheckEaten(chain, apple))//אם השחקן אוכל תפוח
                        {
                            msg = scoreHandler.ObtainMessage();
                            gameManager.SetApple();
                            msg.Arg1 = 0;
                            scoreHandler.SendMessage(msg);
                            count++;
                        }
                        if (CheckHit(chain,mets) && once)//אם מטאור פוגע בשחקן
                        {
                            once = false;
                            lives--;
                            chain.Direction = "right";
                            chain.SetY(500);
                            chain.SetX(200);
                            msg = scoreHandler.ObtainMessage();
                            msg.Arg1 = 2;
                            scoreHandler.SendMessage(msg);
                        }
                        if (lives==0)//אם לשחקן אין יותר חיים
                        {
                            for (int i = 0; i < mets.Length; i++)
                            {
                                mets[i] = null;
                            }
                            msg = scoreHandler.ObtainMessage();
                            run = false;
                            msg.Arg1 = 1;
                            scoreHandler.SendMessage(msg);
                        }
                        
                        if (CheckLost())//אם השחקן מתנגש בקיר ונפסל
                        {
                            for (int i = 0; i < mets.Length; i++)
                            {
                                mets[i] = null;
                            }
                            msg = scoreHandler.ObtainMessage();
                            run = false;
                            msg.Arg1 = 1;
                            scoreHandler.SendMessage(msg);
                            
                        }
                        
                    }
                    catch (Exception e)
                    {
                        Log.Debug("###", e.Message);
                    }
                    finally
                    {
                        if (canvas != null)
                            Holder.UnlockCanvasAndPost(canvas);
                    }
                }
            }
        }

        private void ChangeDirection()
        {
            if (chain.Direction=="up")
            {
                chain.Direction = "down";
            }
            else if (chain.Direction == "down")
            {
                chain.Direction = "up";
            }
            else if (chain.Direction == "right")
            {
                chain.Direction = "left";
            }
            else 
            {
                chain.Direction = "right";
            }
        }

        private void RandomizeActives(int numofmets)
        {
            int num;
            while (numofmets>1)
            {
                num = rnd.Next(1, 10);
                for (int i = 1; i < mets.Length; i++)
                {
                    mets[i].Sety(-10);
                }
                for (int i = 1; i < mets.Length; i++)
                {
                    if (!mets[num].GetActive())
                    {
                        mets[num].SetActive(true);
                        numofmets--;
                    }
                }
            }           
        }

        private bool IsThereActive()
        {
            for (int i = 1; i < mets.Length; i++)
            {
                if (mets[i].GetActive())
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckEaten(Chain c, Apple s)
        {        
            Rect c_s = c.GetRect();
            Rect s_c = s.GetRect();
            
            return Rect.Intersects(c_s, s_c);
        }


        private bool CheckHit(Chain c, Met[]s)
        {
            Rect c_s = c.GetRect();
            Rect s_c;
            for (int i = 1; i < s.Length; i++)
            {
                s_c = s[i].GetRect();
                if (Rect.Intersects(c_s, s_c))
                {
                    return true;
                }

            }
            //Log.Debug("debug", "("+c_s.Bottom+","+c_s.Left+")"+"("+s_c.Bottom+","+s_c.Left+")");
            return false ;  
        }



        private void NoAction(object sender, DialogClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void YesAction(object sender, DialogClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        private bool CheckLost()
        {
            return chain.Limit();
        }

        ~GameBoard()
        {
            this.run = false;
        }

        private void DrawChain(Canvas canvas)
        {
            chain.Move();
            chain.Draw(canvas, paint);
        }
        private void DrawMet(Canvas canvas,int num)
        {
            
            for (int i = 1; i < mets.Length; i++)
            {
                if (mets[i].GetActive())
                {
                    mets[i].Draw(canvas, paint);
                    mets[i].Move(num-1);                   
                }
            }                                    
        }

        private void DrawFrame(Canvas canvas)
        {
            paint.Color = Color.DarkBlue;
            canvas.DrawRect(0, 0, canvas.Width, FRAME_WIDTH, paint);  // top
            canvas.DrawRect(0, canvas.Height - FRAME_WIDTH, canvas.Width, canvas.Height, paint); //bottom
            canvas.DrawRect(0, FRAME_WIDTH, FRAME_WIDTH, canvas.Height - FRAME_WIDTH, paint); //left
            canvas.DrawRect(canvas.Width - FRAME_WIDTH, FRAME_WIDTH, canvas.Width, canvas.Height - FRAME_WIDTH, paint);//right
        }
        private void DrawApple(Canvas canvas)
        {
            Location l = gameManager.Findapple();            
            apple.Move(l.X, l.Y);
            apple.Draw(canvas, paint);
        }
        public override bool OnTouchEvent(MotionEvent e)
        {           
            return true;
        }
    }
}