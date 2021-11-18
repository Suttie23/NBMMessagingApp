using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        public MainWindow()
        {

            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;


        }

        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();

            
            string msgSender = senderTextBox.Text;
            string msgSubject = subjectTextBox.Text;
            string msgBody = bodyTextBox.Text;
            int msgID = rnd.Next();

            msgList.Add(new Message(msgSender, msgSubject, msgBody, msgID));

        }

        private void endSessionButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void testDisplayButton_Click(object sender, RoutedEventArgs e)
        {

            foreach (Message msgNum in msgList)
            {
                
                testDisplayMessage.Text = msgNum.getData();

            }
        }
    }
}
