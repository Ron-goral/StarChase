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
    [Activity(Label = "Activity1")]
    public class GameActivity : Activity
    {
        private Android.App.AlertDialog resetDialog;
        private Android.App.Dialog gameover_dialog;
        private FrameLayout frm;
        private GameBoard gameBoard;
        private ImageButton btnUp,btnDown,btnLeft,btnRight,btn_home,close_d;
        private TextView tvscore,tvdes;
        private int score,recentscore;
        private ImageView heart1,heart2,heart3;


        private ISharedPreferences sp;
        private Button backhome, newgame;
        private FirebaseManager fb;
        
        private EditText edittext;
        private Record r, bestscore;

        

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.game);
            // Create your application here
            btnDown = FindViewById<ImageButton>(Resource.Id.btnDown);
            btnLeft = FindViewById<ImageButton>(Resource.Id.btnLeft);    
            btnRight = FindViewById<ImageButton>(Resource.Id.btnRight);
            btnUp = FindViewById<ImageButton>(Resource.Id.btnUp);
            frm = FindViewById<FrameLayout>(Resource.Id.frameLayout1);
            btn_home= FindViewById<ImageButton>(Resource.Id.btn_home);
            tvscore = FindViewById<TextView>(Resource.Id.tvscore);
           

            heart1 = FindViewById<ImageView>(Resource.Id.heart1);
            heart2 = FindViewById<ImageView>(Resource.Id.heart2);
            heart3 = FindViewById<ImageView>(Resource.Id.heart3);

            

            btnRight.Click += Btn_Click;
            btnUp.Click += Btn_Click;
            btnDown.Click += Btn_Click;
            btnLeft.Click += Btn_Click;
            btn_home.Click += Btn_Click;
            score = 0;
            sp = GetSharedPreferences("details", FileCreationMode.Private);
            fb =new FirebaseManager();
            
            
        }

        private void Btn_Click(object sender, EventArgs e)//שינוי כיוון תזוזת השחקן בעזרת חצי המקלדת
        {
            if (sender == btnUp)
                gameBoard.UpdateDirection("up");
            if (sender == btnDown)
                gameBoard.UpdateDirection("down");
            if (sender == btnLeft)
                gameBoard.UpdateDirection("left");
            if (sender == btnRight)
                gameBoard.UpdateDirection("right");
            if (sender==btn_home)
            {
                SaveCoins(int.Parse(tvscore.Text));
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }
            if (sender==close_d)
            {
                SaveCoins(int.Parse(tvscore.Text));
                Toast.MakeText(this, "Record not saved!", ToastLength.Long).Show();
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }
            if (sender==backhome)
            {
                SaveCoins(int.Parse(tvscore.Text));
                if (edittext.Text=="")
                {
                    Toast.MakeText(this, "Please enter name", ToastLength.Short).Show();
                }
                else
                {
                    UpdateRecord(edittext.Text);
                    Intent intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);
                    Toast.MakeText(this, "Record Saved", ToastLength.Short).Show();
                }
                
            }
            else if (sender==newgame)
            {
                if (edittext.Text == "")
                {
                    Toast.MakeText(this, "Please enter name", ToastLength.Short).Show();
                }
                else
                {
                    UpdateRecord(edittext.Text);
                    Intent intent = new Intent(this, typeof(GameActivity));
                    StartActivity(intent);
                    Toast.MakeText(this, "Record Saved", ToastLength.Short).Show();
                }

            }
        }
        public override void OnWindowFocusChanged(bool hasFocus)
        {
            if (hasFocus)
            {
                int width = frm.Width;
                int height = frm.Height;
                int chr = sp.GetInt("applied", 0);
                gameBoard = new GameBoard(this, width, height,chr);
                frm.AddView(gameBoard);
            }
        }
        public void UpdateScore()//  עדכון ניקוד השחקן בזמן המשחק
        {
            this.score++;
            tvscore.Text = this.score.ToString();    
        }
        public async void UpdateRecord(string name)//עדכון שיא
        {
            recentscore = int.Parse(tvscore.Text);
            DateTime dt = DateTime.Now;
            r=new Record(name, recentscore, dt);              
            await fb.AddRecord(r, "recent");
            bestscore = await fb.GetRecord("best");
            if (bestscore!=null)
            {
                if (recentscore > bestscore.Score)
                {
                    await fb.AddRecord(r, "best");
                    Toast.MakeText(this, "New best score!!!", ToastLength.Long).Show();
                }
             
            }
            else
            {
                await fb.AddRecord(r, "best");
            }
        }
        public int Getscore()
        {
            return this.score;   
        }
        public void CreatedialogGameover()
        {
            gameover_dialog = new Android.App.Dialog(this);
            gameover_dialog.SetContentView(Resource.Layout.gameover_dialog);
            gameover_dialog.SetCancelable(true);
            backhome = gameover_dialog.FindViewById<Button>(Resource.Id.backhome);
            newgame = gameover_dialog.FindViewById<Button>(Resource.Id.newgame);
            close_d = gameover_dialog.FindViewById<ImageButton>(Resource.Id.btnclose);
            tvdes = gameover_dialog.FindViewById<TextView>(Resource.Id.tvdescription);
            edittext = gameover_dialog.FindViewById<EditText>(Resource.Id.editText1);
            backhome.Click += Btn_Click;
            newgame.Click += Btn_Click;
            close_d.Click += Btn_Click;
            gameover_dialog.Show();
        }

        private void NoAction(object sender, DialogClickEventArgs e)
        {
            SaveCoins(int.Parse(tvscore.Text));
            Intent i = new Intent(this, typeof(MainActivity));
            StartActivity(i);
            Finish();
        }

        private void YesAction(object sender, DialogClickEventArgs e)
        {
            SaveCoins(int.Parse(tvscore.Text));
            Intent i = new Intent(this, typeof(GameActivity));
            StartActivity(i);           
        }


        public void SaveCoins(int c)//עדכון ניקוד השחקן לאחר המשחק בשרד פרפרנס
        {
            ISharedPreferencesEditor editor = sp.Edit();
            score=sp.GetInt("totalscore",0) + c;
            editor.PutInt("totalscore", score);
            editor.Commit();            
        }
        public void UpdateHearts()//עדכון הלבבות שמייצגות את חיי השחקן במקרה של פגיעת מטאור
        {
            if (heart1.Visibility==ViewStates.Visible)
            {
                heart1.Visibility=ViewStates.Invisible;
            }
            else if (heart2.Visibility==ViewStates.Visible)
            {
                heart2.Visibility = ViewStates.Invisible;
            }
            else
            {
                heart3.Visibility = ViewStates.Invisible;
            }
        }
        public void Nohearts()//עדכון חיים במקרה של פסילה על ידי פגיעה באחד הקירות
        {
            heart1.Visibility = ViewStates.Invisible;
            heart2.Visibility = ViewStates.Invisible;
            heart3.Visibility = ViewStates.Invisible;
        }
       


    }
}