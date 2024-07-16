using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;

namespace Snake_RonGoral
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Android.App.Dialog sett_dialog,about_dialog;
        private ImageButton btnplay, btnscore, btnshop, home1,shop1;
        ISharedPreferences sp;
        private Switch switch1;
        private Intent music;
        private bool isPlaying;
        TextView tvcoins,tvmusic,tvbattery;
        BroadcastBattery b;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            btnplay = FindViewById<ImageButton>(Resource.Id.btn_play);
            btnscore = FindViewById<ImageButton>(Resource.Id.btn_scoreboard);
            btnshop = FindViewById<ImageButton>(Resource.Id.btn_shop);
            tvcoins = FindViewById<TextView>(Resource.Id.tvcoinshome);
   
            shop1 = FindViewById<ImageButton>(Resource.Id.imageButton3);

            sp = GetSharedPreferences("details", FileCreationMode.Private);

            tvbattery = FindViewById<TextView>(Resource.Id.tvbattery);
    
            btnplay.Click += Btn_Click;
            btnscore.Click += Btn_Click;
            btnshop.Click += Btn_Click;
           
            tvcoins.Text=sp.GetInt("totalscore",0).ToString();
            
          
        }

        private void Switch1_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked)
            {
                if (!isPlaying)
                {
                    StartService(music);
                    isPlaying = true;
                    tvmusic.Text = "Music: On";
                }
            }
            else
            {
                if (isPlaying)
                {
                    StopService(music);
                    isPlaying = false;
                    tvmusic.Text = "Music: Off";
                }
            }
            ISharedPreferencesEditor editor = sp.Edit();
            editor.PutBoolean("isPlaying", isPlaying);
            editor.Commit();
        }
        protected override void OnResume()
        {
            base.OnResume();
            if (b == null)
                b = new BroadcastBattery(tvbattery);
            RegisterReceiver(b, new IntentFilter(Intent.ActionBatteryChanged));
        }
        protected override void OnPause()
        {
            base.OnPause();
            if (b != null)
                UnregisterReceiver(b);
        }

        private void Btn_Click(object sender, System.EventArgs e)
        {
            if (sender==btnplay)
            {
                Intent intent = new Intent(this, typeof(GameActivity));
                StartActivity(intent);
            }
            if (sender==btnshop)
            {
                Intent intent = new Intent(this, typeof(Shop_Activity));
                StartActivity(intent);
            }
            if (sender == btnscore)
            {
                Intent intent = new Intent(this, typeof(ScoreBoard));
                StartActivity(intent);
            }
            if (sender==home1)
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }
            if (sender==shop1)
            {
                Intent intent = new Intent(this, typeof(Shop_Activity));
                StartActivity(intent);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.first_menu, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            if (item.ItemId == Resource.Id.action_sett)
            {
                CreatedialogSett();
            }
         
            else if (item.ItemId == Resource.Id.action_about)
            {
                CreatedialogAbout();
            }
            return base.OnOptionsItemSelected(item);
        }
        public void CreatedialogSett()
        {
            sett_dialog = new Android.App.Dialog(this);
            sett_dialog.SetContentView(Resource.Layout.sett_dialog);
            sett_dialog.SetCancelable(true);
            home1 = sett_dialog.FindViewById<ImageButton>(Resource.Id.btn_homefsett);
            shop1 = sett_dialog.FindViewById<ImageButton>(Resource.Id.imageButton3);
            switch1 = sett_dialog.FindViewById<Switch>(Resource.Id.switch1);
            tvmusic = sett_dialog.FindViewById<TextView>(Resource.Id.textView1);
            switch1.CheckedChange += Switch1_CheckedChange;
            music = new Intent(this, typeof(MusicService));
            isPlaying = sp.GetBoolean("isPlaying", false);
            switch1.Checked = isPlaying;
            home1.Click += Btn_Click;
            shop1.Click += Btn_Click;
            sett_dialog.Show();
        }
        public void CreatedialogAbout()
        {
            about_dialog = new Android.App.Dialog(this);
            about_dialog.SetContentView(Resource.Layout.about_dialog);
            about_dialog.SetCancelable(true);
            about_dialog.Show();
        }
        public void UpdateTVCoins()
        {                      
            tvcoins.Text = sp.GetInt("totalscore", 0).ToString();
        }

    }
}