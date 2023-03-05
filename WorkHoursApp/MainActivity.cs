using System;
using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace WorkHoursApp
{
    [Activity(Label = "Work Hours App", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private EditText dateEditText;
        private EditText hoursEditText;
        private Button saveButton;
        private Button clearButton;

        private List<string[]> dataList = new List<string[]>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set the activity layout
            SetContentView(Resource.Layout.main);

            // Get references to the input fields and buttons
            dateEditText = FindViewById<EditText>(Resource.Id.dateEditText);
            hoursEditText = FindViewById<EditText>(Resource.Id.hoursEditText);
            saveButton = FindViewById<Button>(Resource.Id.saveButton);
            clearButton = FindViewById<Button>(Resource.Id.clearButton);

            // Add click event handlers to the buttons
            saveButton.Click += SaveButton_Click;
            clearButton.Click += ClearButton_Click;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Get the input values
            string date = dateEditText.Text.Trim();
            string hours = hoursEditText.Text.Trim();

            // Validate the input values
            if (string.IsNullOrEmpty(date))
            {
                Toast.MakeText(this, "Please enter a", ToastLength.Short).Show();
                // Add the data to the list
                dataList.Add(new string[] { date, hours });

                // Clear the input fields
                dateEditText.Text = "";
                hoursEditText.Text = "";

                Toast.MakeText(this, "Data saved", ToastLength.Short).Show();
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            // Clear the input fields
            dateEditText.Text = "";
            hoursEditText.Text = "";
        }

        private void SaveDataToCsvFile(string fileName)
        {
            // Create the CSV file
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var filePath = Path.Combine(documentsPath, fileName + ".csv");
            using (var writer = new StreamWriter(filePath))
            {
                // Write the data to the CSV file
                foreach (var data in dataList)
                {
                    writer.WriteLine(string.Join(",", data));
                }
            }

            // Send the CSV file as an email attachment
            var emailIntent = new Intent(Intent.ActionSend);
            emailIntent.PutExtra(Intent.ExtraSubject, fileName);
            emailIntent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file://" + filePath));
            emailIntent.SetType("text/csv");
            StartActivity(Intent.CreateChooser(emailIntent, "Send email"));
        }
    }
}
