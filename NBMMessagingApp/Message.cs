using System;
using System.Collections.Generic;
using System.Text;

namespace NBMMessagingApp
{
    class Message
    {

        public string messageSender { get; set; }
        public string messageBody { get; set; }
        public string messageType { get; set; }
        public int messageID { get; set; }

        public Message(string msgsender, string msgbody, int msgID, string msgType)
        {
            this.messageSender = msgsender;
            this.messageBody = msgbody;
            this.messageID = msgID;
            this.messageType = msgType;

        }

        public string getSMSData()
        {

                return "Message ID:" + this.messageType + this.messageID +  "\n\n" + "Sender: " + this.messageSender + "\n\n" + this.messageBody;
      
        }

    }
}
