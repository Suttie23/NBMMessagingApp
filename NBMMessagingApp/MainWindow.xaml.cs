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
        List<Tweet> tweetList = new List<Tweet>();

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

            if (emailFormat.IsMatch(msgSender))
            {
                if (subjectTextBox.Text.Contains("SIR"))
                {
                    msgSubject = msgSubject + DateTime.Now.ToString("dd/mm/yy");
                }
                messageType = "E";
                emailList.Add(new Email(msgSender, msgSubject, msgBody, msgID, messageType, msgSortCode, msgIncidentType));

            }
            else if (msgSender.Contains("+44"))
            {
                messageType = "S";
                msgList.Add(new Message(msgSender, msgBody, msgID, messageType));
            }
            else if (msgSender.Substring(0, 1).Equals("@"))
            {
                messageType = "T";
                tweetList.Add(new Tweet(msgSender, msgBody, msgID, messageType));

            }


            senderTextBox.Clear();
            subjectTextBox.Clear();
            bodyTextBox.Clear();
            sortcodeTextBox.Clear();
            indicentComboBox.SelectedIndex = -1;

        }

        private void endSessionButton_Click(object sender, RoutedEventArgs e)
        {


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

        private void testTweetButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Tweet tweetNum in tweetList)
            {

                testDisplayMessage.Text = tweetNum.getTweetData();


            }
        }

        private void senderHelpButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("When entering sender information, please adhere to the follwing to ensure correct categorisation: \n\n SMS: International phone number (+44) \n Email: Standard email address (Example@example.com) \n Tweet: Twitter ID (@Example)", "Sender Help");
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

    }
}
