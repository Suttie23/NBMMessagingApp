using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        List<Message> msgList = new List<Message>();

        List<Email> emailList = new List<Email>();
        List<SIREmail> SIREmailList = new List<SIREmail>();

        List<Tweet> tweetList = new List<Tweet>();
        List<string> hashTagList = new List<string>();
        List<string> mentionList = new List<string>();

        public MainWindow()
        {

            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            indicentComboBox.Items.Add("Theft");
            indicentComboBox.Items.Add("Staff Attack");
            indicentComboBox.Items.Add("ATM Theft");
            indicentComboBox.Items.Add("Raid");
            indicentComboBox.Items.Add("Customer Attack");
            indicentComboBox.Items.Add("Staff Abuse");
            indicentComboBox.Items.Add("Bomb Threat");
            indicentComboBox.Items.Add("Terrorism");
            indicentComboBox.Items.Add("Suspicious Incident");
            indicentComboBox.Items.Add("Intelligence");
            indicentComboBox.Items.Add("Cash Loss");

        }

        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();

            
            string msgSender = senderTextBox.Text;
            string msgSubject = subjectTextBox.Text;
            string msgBody = bodyTextBox.Text;
            string messageType;
            int msgID = rnd.Next();
            string msgSortCode = sortcodeTextBox.Text;
            string msgIncidentType = indicentComboBox.SelectedItem?.ToString();

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

                if (subjectTextBox.Text.Contains("SIR"))
                {
                    
                    msgSubject = msgSubject + DateTime.Now.ToString("dd/mm/yy");

                    if (sortFormat.IsMatch(msgSortCode))
                    {

                        validSIR = true;

                        if (validSIR == true)
                        {
                            SIREmailList.Add(new SIREmail(msgSender, msgSubject, msgBody, msgID, messageType, msgSortCode, msgIncidentType));
                            MessageBox.Show("SIR email added", "ERROR");
                        }

                    } else
                    {
                        MessageBox.Show("Invalid sort code, enter in format: ##-##-##", "ERROR");
                        sortcodeTextBox.Clear();
                    }

                } else
                {
                    MessageBox.Show("Email added", "ERROR");
                    emailList.Add(new Email(msgSender, msgSubject, msgBody, msgID, messageType));
                }


            } else if (phoneFormat.IsMatch(msgSender))
            {
                messageType = "S";
                msgList.Add(new Message(msgSender, msgBody, msgID, messageType));
            } else if (msgSender.Substring(0, 1).Equals("@"))
            {
                messageType = "T";

                var tags = bodyTextBox.Text.Split(" ");
                for (int i = 0; i < tags.Length; i++)
                {
                    if (tags[i].Contains("#"))
                    {
                        hashTagList.Add(tags[i]);
                    }

                }

                var mentions = bodyTextBox.Text.Split(" ");
                for (int i = 0; i < mentions.Length; i++)
                {
                    if (mentions[i].Contains("@"))
                    {
                        mentionList.Add(mentions[i]);
                    }

                }

                tweetList.Add(new Tweet(msgSender, msgBody, msgID, messageType));

            } else
            {
                MessageBox.Show("Incorrect formatting, please enter valid sender details. See HELP button for more information", "ERROR");
                senderTextBox.Clear();
            }

            clearInput();
            visibleSIR();
            visibleSubject();

        }

        private void endSessionButton_Click(object sender, RoutedEventArgs e)
        {

            string SIRLine = "";
            foreach (SIREmail SIREmailNum in SIREmailList)
            {
                SIRLine = SIRLine + "\nSort Code: " + SIREmailNum.sortCode + " / Incident Type: " + SIREmailNum.incidentType + "\n";
            }

            MessageBox.Show(SIRLine, "SIR LIST");
            MessageBox.Show(String.Join("\n", hashTagList), "TRENDING ON TWITTER");
            MessageBox.Show(String.Join("\n", mentionList), "MENTIONS");

            countHash();
        }

        private void testSMSButton_Click(object sender, RoutedEventArgs e)
        {

            foreach (Message msgNum in msgList)
            {
                
                testDisplayMessage.Text = msgNum.getSMSData();

            }
        }

        private void testEmailButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Email emailNum in emailList)
            {

                testDisplayMessage.Text = emailNum.getEmailData();

            }
        }

        private void testSIREmailButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (SIREmail SIREmailNum in SIREmailList)
            {

                testDisplayMessage.Text = SIREmailNum.getSIREmailData();

            }
        }

        private void testTweetButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Tweet tweetNum in tweetList)
            {

                testDisplayMessage.Text = tweetNum.getTweetData();

            }
        }

        private void senderHelpButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("When entering sender information, please adhere to the follwing to ensure correct categorisation: \n\n SMS: International phone number (+447222555555 / +44 7222 555555) \n Email: Standard email address (Example@example.com) \n Tweet: Twitter ID (@Example)", "Sender Help");
        }

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

        private void subjectTextBox_KeyUp(object sender, KeyEventArgs e)
        {

            visibleSIR();

        }

        private void senderTextBox_KeyUp(object sender, KeyEventArgs e)
        {

            visibleSubject();
 
        }

        private void visibleSIR()
        {

            if (subjectTextBox.Text.Contains("SIR"))
            {

                body_label.HorizontalAlignment = HorizontalAlignment.Left;
                body_label.Margin = new Thickness(105, 252, 0, 0);
                body_label.VerticalAlignment = VerticalAlignment.Top;

                incident_label.Visibility = Visibility.Visible;
                indicentComboBox.Visibility = Visibility.Visible;
                sortcode_label.Visibility = Visibility.Visible;
                sortcodeTextBox.Visibility = Visibility.Visible;

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
                indicentComboBox.Visibility = Visibility.Hidden;
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

        private void visibleSubject()
        {

            string ePattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            Regex emailFormat = new Regex(ePattern, RegexOptions.IgnoreCase);

            if (emailFormat.IsMatch(senderTextBox.Text))
            {

                subject_label.Visibility = Visibility.Visible;
                subjectTextBox.Visibility = Visibility.Visible;
                subjectHelpButton.Visibility = Visibility.Visible;

            }
            else
            {

                subject_label.Visibility = Visibility.Hidden;
                subjectTextBox.Visibility = Visibility.Hidden;
                subjectHelpButton.Visibility = Visibility.Hidden;

            }

        }

        private void clearInput()
        {
            senderTextBox.Clear();
            subjectTextBox.Clear();
            bodyTextBox.Clear();
            sortcodeTextBox.Clear();
            indicentComboBox.SelectedIndex = -1;
        }

        private void countHash()
        {
        }


    }
}
