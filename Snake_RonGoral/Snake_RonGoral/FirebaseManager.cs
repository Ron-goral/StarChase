using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_RonGoral
{
    internal class FirebaseManager
    {
        FirebaseClient firebase = new FirebaseClient("https://rons-project-48737-default-rtdb.europe-west1.firebasedatabase.app");

        //add data to firebase
        //will add the new object if not exist
        //if object exists, will override it
        //async - אסינכרוני
        public async Task AddRecord(Record rec, string type)
        {
            await firebase.Child("records").Child(type).PutAsync<Record>(rec);
        }


        public async Task<Record> GetRecord(string type)
        {
            return await firebase.Child("records").Child(type).OnceSingleAsync<Record>();
        }

        //get a list of all the objects in the firebase
        //public async Task<List<Record>> GetAllRecords()
        //{
        //    return (await firebase.Child("records").OnceAsync<Record>()).Select(item => new Record(
        //        item.Object.Name,
        //        item.Object.Score,
        //        item.Object.Date)

        //    ).ToList();
        //}
        


    }
}