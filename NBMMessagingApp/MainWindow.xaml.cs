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
        Dictionary<string, int> trendList = new Dictionary<string, int>();
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

            resetFiles();

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
                            string fileName = @"C:\Users\Suttie\Desktop\NBMMessagingApp-3860869dc95f3281eccea13f70f8d7ec9db90a21\JSONSIREmail.json";
                            string json = JsonConvert.SerializeObject(SIREmailList, Formatting.Indented);

                            using StreamWriter sw = new StreamWriter(fileName);
                            sw.WriteLine(json);
                            sw.Close();


                            MessageBox.Show(json);
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
                    {
                        emailList.Add(new Email(msgSender, msgSubject, msgBody, msgID, messageType));
                        string fileName = @"C:\Users\Suttie\Desktop\NBMMessagingApp-3860869dc95f3281eccea13f70f8d7ec9db90a21\JSONEmail.json";
                        string json = JsonConvert.SerializeObject(emailList, Formatting.Indented);

                        using StreamWriter sw = new StreamWriter(fileName);
                        sw.WriteLine(json);
                        sw.Close();


                        MessageBox.Show(json);
                    }
                }

            } 
            // If the sender was not an email, check if it matches the phone format
            else if (phoneFormat.IsMatch(msgSender))
            {
                // Create SMS message
                messageType = "S";


                msgList.Add(new Message(msgSender, msgBody, msgID, messageType));
                string fileName = @"C:\Users\Suttie\Desktop\NBMMessagingApp-3860869dc95f3281eccea13f70f8d7ec9db90a21\JSON\SMS.json";
                string json = JsonConvert.SerializeObject(msgList, Formatting.Indented);

                using StreamWriter sw = new StreamWriter(fileName);
                sw.WriteLine(json);
                sw.Close();


                MessageBox.Show(json);
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
                tweetList.Add(new Tweet(msgSender, msgBody, msgID, messageType));
                string fileName = @"C:\Users\Suttie\Desktop\NBMMessagingApp-3860869dc95f3281eccea13f70f8d7ec9db90a21\JSON\Tweet.json";
                string json = JsonConvert.SerializeObject(tweetList, Formatting.Indented);

                using StreamWriter sw = new StreamWriter(fileName);
                sw.WriteLine(json);
                sw.Close();
           
                MessageBox.Show(json);

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
            var orderTrend = trendList.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            MessageBox.Show(String.Join("\n", orderTrend.Reverse()), "TRENDING ON TWITTER");
            MessageBox.Show(String.Join("\n", mentionList), "MENTIONS");

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

            hashTagList.ForEach(e =>
            {
                if (trendList.ContainsKey(e))
                {
                    trendList[e]++;
                } else
                {
                    trendList[e] = 1;
                }
            }
          );
            

        }

        // Method to write all object lists to JSON format
        private void resetFiles()
        {

            
            string fileName = @"C:\Users\Suttie\Desktop\NBMMessagingApp-3860869dc95f3281eccea13f70f8d7ec9db90a21\JSON\SMS.json";

            if (System.IO.File.Exists(fileName) == true)
            {
                System.IO.File.Delete(fileName);
            }
            
            

            string fileName2 = @"C:\Users\Suttie\Desktop\NBMMessagingApp-3860869dc95f3281eccea13f70f8d7ec9db90a21\JSON\Email.json";

            if (System.IO.File.Exists(fileName2) == true)
            {
                System.IO.File.Delete(fileName2);
            }

            string fileName3 = @"C:\Users\Suttie\Desktop\NBMMessagingApp-3860869dc95f3281eccea13f70f8d7ec9db90a21\JSON\SIR.json";

            if (System.IO.File.Exists(fileName3) == true)
            {
                System.IO.File.Delete(fileName3);
            }

            string fileName4 = @"C:\Users\Suttie\Desktop\NBMMessagingApp-3860869dc95f3281eccea13f70f8d7ec9db90a21\JSON\Tweet.json";

            if (System.IO.File.Exists(fileName4) == true)
            {             
                System.IO.File.Delete(fileName4);
            }
        }


    }
}
