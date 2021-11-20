using System;
using System.Collections.Generic;
using System.Text;

namespace NBMMessagingApp
{

    class Tweet : Message
    {

        public string[] hashTags { get; set; }


        public Tweet(string msgsender, string msgbody, int msgID, string msgType) : base(msgsender, msgbody, msgID, msgType)
        {



        }

        public string getTweetData()
        {

            return "Message ID:" + this.messageType + this.messageID + "\n\n" + "Sender: " + this.messageSender + "\n\n" + this.sanitisedBody;

        }
    }
}
