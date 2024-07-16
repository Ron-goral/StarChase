using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Android.Views.View;

namespace Snake_RonGoral
{
    [Activity(Label = "ScoreBoard")]
    public class ScoreBoard : Activity, IOnClickListener, PopupMenu.IOnMenuItemClickListener
    {
        private ImageButton btn_addimage,btn_home;
        private TextView tv_name1, tv_name2, tv_record1, tv_record2,tv_date1,tv_date2;
        private Bitmap selectedImageBitmap;
        private static readonly int TAKE_PHOTO_REQUEST = 1;
        private static readonly int CHOOSE_PHOTO_REQUEST = 2;
        private FirebaseManager fb;
        private Record recentscore, bestscore;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.scoreboard);
            // Create your application here
            btn_addimage = FindViewById<ImageButton>(Resource.Id.btn_addimage);
            btn_home = FindViewById<ImageButton>(Resource.Id.btn_home2);

            tv_name1 = FindViewById<TextView>(Resource.Id.tv_name1);
            tv_name2 = FindViewById<TextView>(Resource.Id.tv_name2);
           
            tv_record1 = FindViewById<TextView>(Resource.Id.tv_record1);
            tv_record2 = FindViewById<TextView>(Resource.Id.tv_record2);

            tv_date1 = FindViewById<TextView>(Resource.Id.tv_date1);
            tv_date2 = FindViewById<TextView>(Resource.Id.tv_date2);

            btn_home.Click += Btn_home_Click;
            btn_addimage.SetOnClickListener(this);

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.Camera }, 0);
            }
            fb = new FirebaseManager();
            recentscore = await fb.GetRecord("recent");
            bestscore = await fb.GetRecord("best");
            tv_name1.Text = "";
            tv_name2.Text = "";
            tv_record1.Text = "0";
            tv_record2.Text = "0";
            tv_date1.Text ="";
            tv_date2.Text = "";
            if (recentscore != null)
            {                
                    tv_name1.Text = bestscore.Name;
                    tv_name2.Text = recentscore.Name;
                    tv_record1.Text = bestscore.Score.ToString();
                    tv_record2.Text = recentscore.Score.ToString();
                    tv_date1.Text = bestscore.Date.ToShortDateString();
                    tv_date2.Text = recentscore.Date.ToShortDateString();

             }
           
        }

        private void Btn_home_Click(object sender, EventArgs e)
        {
            if (sender==btn_home)
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }       
        }

        public void OnClick(View v)
        {
            if (v.Id == btn_addimage.Id)
            {
                PopupMenu popup = new PopupMenu(this, v);
                MenuInflater inflater = popup.MenuInflater;
                inflater.Inflate(Resource.Menu.editphotomenu, popup.Menu);
                popup.SetOnMenuItemClickListener(this);
                popup.Show();
            }
        }

        public bool OnMenuItemClick(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.action_takephoto)
            {
                Intent intent = new Intent(MediaStore.ActionImageCapture);
                StartActivityForResult(intent, TAKE_PHOTO_REQUEST);
                return true;
            }
            if (item.ItemId == Resource.Id.action_gallery)
            {
                Intent intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
                intent.SetType("image/*");
                StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), CHOOSE_PHOTO_REQUEST);
                return true;
            }
            if (item.ItemId == Resource.Id.action_remove)
            {
                btn_addimage.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.addimage));
                return true;
            }
            return false;
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                switch (requestCode)
                {
                    case 1:
                        Bundle extras = data.Extras;
                        Bitmap bitmap = (Bitmap)extras.Get("data");
                        Bitmap croppedBitmapTaken = CropBitmap(bitmap);
                        Bitmap resizedBitmapTaken = ResizeBitmap(croppedBitmapTaken, 85, 85);
                        btn_addimage.SetImageBitmap(resizedBitmapTaken);
                        btn_addimage.SetBackgroundDrawable(null);
                        break;

                    case 2:
                        Android.Net.Uri uri = data.Data;
                        try
                        {
                            selectedImageBitmap = MediaStore.Images.Media.GetBitmap(ContentResolver, uri);
                            Bitmap croppedBitmapChosen = CropBitmap(selectedImageBitmap);
                            Bitmap resizedBitmapChosen = ResizeBitmap(croppedBitmapChosen, 85, 85);
                            btn_addimage.SetImageBitmap(resizedBitmapChosen);
                            btn_addimage.SetBackgroundDrawable(null);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                }
            }

        }
        private Bitmap CropBitmap(Bitmap sourceBitmap)
        {
            int width = sourceBitmap.Width;
            int height = sourceBitmap.Height;
            int length = Math.Min(width, height);

            int x = (width - length) / 2;
            int y = (height - length) / 2;

            Bitmap croppedBitmap = Bitmap.CreateBitmap(sourceBitmap, x, y, length, length);
            return croppedBitmap;
        }

        private Bitmap ResizeBitmap(Bitmap sourceBitmap, int width, int height)
        {
            int sourceWidth = sourceBitmap.Width;
            int sourceHeight = sourceBitmap.Height;

            float widthRatio = (float)width / sourceWidth;
            float heightRatio = (float)height / sourceHeight;

            float scaleFactor = Math.Min(widthRatio, heightRatio);

            int targetWidth = (int)(sourceWidth * scaleFactor);
            int targetHeight = (int)(sourceHeight * scaleFactor);

            Bitmap resizedBitmap = Bitmap.CreateScaledBitmap(sourceBitmap, targetWidth, targetHeight, true);
            return resizedBitmap;
        }
    }
}