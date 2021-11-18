using System;
using System.Collections.Generic;
using System.Text;

namespace NBMMessagingApp
{
    class Message
    {

        public string messageSender { get; set; }
        public string messageSubject { get; set; }
        public string messageBody { get; set; }
        public string messageType { get; set; }
        public int messageID { get; set; }

        public Message(string msgsender, string msgsubject, string msgbody, int msgID)
        {
            this.messageSender = msgsender;
            this.messageSubject = msgsubject;
            this.messageBody = msgbody;
            this.messageID = msgID;

            if (!string.IsNullOrEmpty(messageSubject) & msgsender.Contains("@"))
            {
                this.messageType = "E";
            }
            else if (msgsender.Contains("+44"))
            {
                this.messageType = "S";
            } else if (msgsender.Contains("@"))
            {
                this.messageType = "T";
            }
        }

        public string getData()
        {
            if (string.IsNullOrEmpty(messageSubject))
            {
                return "Sender: " + this.messageSender + "\n\n" + "Body: " + "Message ID:" + this.messageType + this.messageID + "\n\n" + this.messageBody;
            } else
            {
                return "Sender: " + this.messageSender + "\n\n" + "Subject: " + this.messageSubject + "\n\n" + "Message ID:" + this.messageType + this.messageID + "\n\n" + this.messageBody;
            }            
        }

    }
}
