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
using Android.Media;


namespace Snake_RonGoral
{
    [Service]
    class MusicService : Service
    {
        private MediaPlayer mp;
        private ISharedPreferences sp;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            MediaPlayer mediaPlayer = MediaPlayer.Create(this, Resource.Raw.swsong);
            mp = mediaPlayer;
            mp.Looping = true;
            mp.SetVolume(100, 100);
            sp = this.GetSharedPreferences("details", FileCreationMode.Private);
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            int position = sp.GetInt("position", 0);
            mp.SeekTo(position);
            mp.Start();
            return base.OnStartCommand(intent, flags, startId);
        }

        public override void OnDestroy()
        {
            ISharedPreferencesEditor editor = sp.Edit();
            editor.PutInt("position", mp.CurrentPosition);
            editor.Commit();
            mp.Stop();
            mp.Release();
            base.OnDestroy();
        }
    }
}

