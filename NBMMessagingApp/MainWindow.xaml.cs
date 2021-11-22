using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NBMMessagingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Lists
        List<Message> msgList = new List<Message>();

        List<Email> emailList = new List<Email>();
        List<SIREmail> SIREmailList = new List<SIREmail>();

        List<Tweet> tweetList = new List<Tweet>();
        List<string> hashTagList = new List<string>();
        SortedDictionary<int, string> trendList = new SortedDictionary<int, string>();
        List<string> mentionList = new List<string>();

        public MainWindow()
        {

            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // Populating combo box
            incidentComboBox.Items.Add("Theft");
            incidentComboBox.Items.Add("Staff Attack");
            incidentComboBox.Items.Add("ATM Theft");
            incidentComboBox.Items.Add("Raid");
            incidentComboBox.Items.Add("Customer Attack");
            incidentComboBox.Items.Add("Staff Abuse");
            incidentComboBox.Items.Add("Bomb Threat");
            incidentComboBox.Items.Add("Terrorism");
            incidentComboBox.Items.Add("Suspicious Incident");
            incidentComboBox.Items.Add("Intelligence");
            incidentComboBox.Items.Add("Cash Loss");

        }

        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {

            Random rnd = new Random();

            // Variables
            string msgSender = senderTextBox.Text;
            string msgSubject = subjectTextBox.Text;
            string msgBody = bodyTextBox.Text;
            string messageType;
            int msgID = rnd.Next();
            string msgSortCode = sortcodeTextBox.Text;
            string msgIncidentType = incidentComboBox.SelectedItem?.ToString();

            // Regex pattenrs for email, phone number and sort code
            string ePattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            Regex emailFormat = new Regex(ePattern, RegexOptions.IgnoreCase);

            string phonePattern = @"^(((\+44\s?\d{4}|\(?0\d{4}\)?)\s?\d{3}\s?\d{3})|((\+44\s?\d{3}|\(?0\d{3}\)?)\s?\d{3}\s?\d{4})|((\+44\s?\d{2}|\(?0\d{2}\)?)\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$";
            Regex phoneFormat = new Regex(phonePattern, RegexOptions.IgnoreCase);

            string sortPattern = @"^\d\d-\d\d-\d\d$";
            Regex sortFormat = new Regex(sortPattern, RegexOptions.IgnoreCase);

            bool validSIR = false;

            // Validate sender input and construct a new SMS, Email or Tweet based on the type of message
            if (emailFormat.IsMatch(msgSender))
            {

                messageType = "E";

                // Check if the subject is a SIR
                if (subjectTextBox.Text.Contains("SIR"))
                {
                    
                    msgSubject = msgSubject + DateTime.Now.ToString("dd/mm/yy");

                    // Validate sort code
                    if (sortFormat.IsMatch(msgSortCode) && !string.IsNullOrEmpty(incidentComboBox.Text))
                    {

                        validSIR = true;

                        // If valid, create new SIREmail
                        if (validSIR == true)
                        {
                            SIREmailList.Add(new SIREmail(msgSender, msgSubject, msgBody, msgID, messageType, msgSortCode, msgIncidentType));
                            MessageBox.Show("SIR email sent", "SUCCESS");
                        }


                    } 
                    // Else, do not create new SIREmail and show error message
                    else
                    {
                        MessageBox.Show("Please ensure correct format for sort code. Enter in format: ##-##-## \n\n If missing, select an incident type.", "ERROR");
                        sortcodeTextBox.Clear();
                    }

                } 
                // If the message did not contain SIR, create a normal email
                else
                {
                    MessageBox.Show("Email sent", "SUCCESS");
                    emailList.Add(new Email(msgSender, msgSubject, msgBody, msgID, messageType));
                }

            } 
            // If the sender was not an email, check if it matches the phone format
            else if (phoneFormat.IsMatch(msgSender))
            {
                // Create SMS message
                messageType = "S";
                MessageBox.Show("SMS sent", "SUCCESS");
                msgList.Add(new Message(msgSender, msgBody, msgID, messageType));
            } 
            // If the sender was not an email OR SMS, check if it matches a Tweet
            else if (msgSender.Substring(0, 1).Equals("@"))
            {
                messageType = "T";

                // Picking out hashtags
                var tags = bodyTextBox.Text.Split(" ");
                for (int i = 0; i < tags.Length; i++)
                {
                    if (tags[i].Contains("#"))
                    {
                        hashTagList.Add(tags[i]);
                    }

                }

                // Picking out mentions
                var mentions = bodyTextBox.Text.Split(" ");
                for (int i = 0; i < mentions.Length; i++)
                {
                    if (mentions[i].Contains("@"))
                    {
                        mentionList.Add(mentions[i]);
                    }

                }

                // Create a new Tweet
                MessageBox.Show("Tweet sent", "SUCCESS");
                tweetList.Add(new Tweet(msgSender, msgBody, msgID, messageType));

            }
            // If sender does not match any accepted format, display an error.
            else
            {
                MessageBox.Show("Incorrect formatting, please enter valid sender details. See HELP button for more information", "ERROR");
                senderTextBox.Clear();
            }

            clearInput();
            visibleSIR();
            visibleSubject();

        }

        // Displays the SIR list, Trending list and Mentions list before shutting down the application
        private void endSessionButton_Click(object sender, RoutedEventArgs e)
        {

            string SIRLine = "";
            foreach (SIREmail SIREmailNum in SIREmailList)
            {
                SIRLine = SIRLine + "\nSort Code: " + SIREmailNum.sortCode + " / Incident Type: " + SIREmailNum.incidentType + "\n";
            }

            MessageBox.Show(SIRLine, "SIR LIST");
            countHash();
            MessageBox.Show(String.Join("\n", trendList.Reverse()), "TRENDING ON TWITTER");
            MessageBox.Show(String.Join("\n", mentionList), "MENTIONS");


            toJSON();
            System.Windows.Application.Current.Shutdown();

        }

        // Sends a help message box to screen for sender field
        private void senderHelpButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("When entering sender information, please adhere to the follwing to ensure correct categorisation: \n\n SMS: International phone number (+447222555555 / +44 7222 555555) \n Email: Standard email address (Example@example.com) \n Tweet: Twitter ID (@Example)", "Sender Help");
        }

        // Sends a help message box to screen for subject field
        private void subjectHelpButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Please only enter a subject if sending an email, failure to do so could result in wrong categorisation. \n\n If the email subject is a Serious Incident report, please enter 'SIR' as subject matter.","Subject Help");
        }

        // Setting the size of the body based on the categorisation of message
        private void bodyTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(subjectTextBox.Text) & senderTextBox.Text.Contains("@"))
            {
                this.bodyTextBox.MaxLength = 1024; 
            }
            else if (senderTextBox.Text.Contains("+44"))
            {
                this.bodyTextBox.MaxLength = 140;
            }
            else if (senderTextBox.Text.Contains("@"))
            {
                this.bodyTextBox.MaxLength = 140;
            }

        }

        // When the key is lifted, call visibleSIR to check if the SIR fields should be visible
        private void subjectTextBox_KeyUp(object sender, KeyEventArgs e)
        {

            visibleSIR();

        }

        // When the key is lifted, call visibleSubject to check if the subject field should be visible
        private void senderTextBox_KeyUp(object sender, KeyEventArgs e)
        {

            visibleSubject();
 
        }

        // Method to alter the visibility and position of UI elements based on subject type
        private void visibleSIR()
        {

            // If the subject box contains "SIR" then rearrange the body elements and make the incident type / sortcode fields visible
            if (subjectTextBox.Text.Contains("SIR"))
            {

                // Body Label
                body_label.HorizontalAlignment = HorizontalAlignment.Left;
                body_label.Margin = new Thickness(105, 252, 0, 0);
                body_label.VerticalAlignment = VerticalAlignment.Top;

                // Incident label / combo box & sortcode label / text box
                incident_label.Visibility = Visibility.Visible;
                incidentComboBox.Visibility = Visibility.Visible;
                sortcode_label.Visibility = Visibility.Visible;
                sortcodeTextBox.Visibility = Visibility.Visible;

                // Body text box
                bodyTextBox.HorizontalAlignment = HorizontalAlignment.Left;
                bodyTextBox.Margin = new Thickness(105, 283, 0, 0);
                bodyTextBox.TextWrapping = TextWrapping.Wrap;
                bodyTextBox.VerticalAlignment = VerticalAlignment.Top;
                bodyTextBox.Width = 464;
                bodyTextBox.Height = 223;
            }
            else
            {
                body_label.HorizontalAlignment = HorizontalAlignment.Left;
                body_label.Margin = new Thickness(105, 149, 0, 0);
                body_label.VerticalAlignment = VerticalAlignment.Top;

                incident_label.Visibility = Visibility.Hidden;
                incidentComboBox.Visibility = Visibility.Hidden;
                sortcode_label.Visibility = Visibility.Hidden;
                sortcodeTextBox.Visibility = Visibility.Hidden;

                bodyTextBox.HorizontalAlignment = HorizontalAlignment.Left;
                bodyTextBox.Margin = new Thickness(105, 176, 0, 0);
                bodyTextBox.TextWrapping = TextWrapping.Wrap;
                bodyTextBox.VerticalAlignment = VerticalAlignment.Top;
                bodyTextBox.Width = 464;
                bodyTextBox.Height = 327;
            }

        }

        // Method to alter the visiblity and position of subject UI elements
        private void visibleSubject()
        {

            // Setting the regex pattern for a valid email address
            string ePattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            Regex emailFormat = new Regex(ePattern, RegexOptions.IgnoreCase);

            // if the regex pattern is matched by the sender text box, then make the subject field visible.
            if (emailFormat.IsMatch(senderTextBox.Text))
            {

                subject_label.Visibility = Visibility.Visible;
                subjectTextBox.Visibility = Visibility.Visible;
                subjectHelpButton.Visibility = Visibility.Visible;

            }
            // else, keep it hidden
            else
            {

                subject_label.Visibility = Visibility.Hidden;
                subjectTextBox.Visibility = Visibility.Hidden;
                subjectHelpButton.Visibility = Visibility.Hidden;

            }

        }

        // Method to clear textboxes
        private void clearInput()
        {
            senderTextBox.Clear();
            subjectTextBox.Clear();
            bodyTextBox.Clear();
            sortcodeTextBox.Clear();
            incidentComboBox.SelectedIndex = -1;
        }

        private void countHash()
        {

            var q = from x in hashTagList
                    group x by x into g
                    let count = g.Count()
                    orderby count descending
                    select new { Value = g.Key, Count = count };
            foreach (var x in q)
            {
                trendList.Add(x.Count, x.Value);
            }

        }

        // Method to write all object lists to JSON format
        private void toJSON()
        {
            //SMS to JSON
            var jsonSMSFormattedContent = Newtonsoft.Json.JsonConvert.SerializeObject(msgList);
            string fileName = @"C:\Users\Suttie\source\repos\NBMMessagingApp\NBMMessagingApp\JSON Files\SMS.json";

            if (System.IO.File.Exists(fileName) == false)
            {
                System.IO.File.WriteAllText(fileName, jsonSMSFormattedContent);
            } else
            {
                System.IO.File.Delete(fileName);
            }
            
            //Email to JSON
            var jsonEmailFormattedContent = Newtonsoft.Json.JsonConvert.SerializeObject(emailList);
            string fileName2 = @"C:\Users\Suttie\source\repos\NBMMessagingApp\NBMMessagingApp\JSON Files\Email.json";

            if (System.IO.File.Exists(fileName2) == false)
            {
                System.IO.File.WriteAllText(fileName2, jsonEmailFormattedContent);
            }
            else
            {
                System.IO.File.Delete(fileName2);
            }

            //SIR Email to JSON
            var jsonSIREmailFormattedContent = Newtonsoft.Json.JsonConvert.SerializeObject(SIREmailList);
            string fileName3 = @"C:\Users\Suttie\source\repos\NBMMessagingApp\NBMMessagingApp\JSON Files\SIREmail.json";

            if (System.IO.File.Exists(fileName3) == false)
            {
                System.IO.File.WriteAllText(fileName3, jsonSIREmailFormattedContent);
            }
            else
            {
                System.IO.File.Delete(fileName3);
            }

            // Tweet to JSON
            var jsonTweetFormattedContent = Newtonsoft.Json.JsonConvert.SerializeObject(tweetList);
            string fileName4 = @"C:\Users\Suttie\source\repos\NBMMessagingApp\NBMMessagingApp\JSON Files\Tweet.json";

            if (System.IO.File.Exists(fileName4) == false)
            {
                System.IO.File.WriteAllText(fileName4, jsonTweetFormattedContent);
            }
            else
            {
                System.IO.File.Delete(fileName4);
            }
        }


    }
}
