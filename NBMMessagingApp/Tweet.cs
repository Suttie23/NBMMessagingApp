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
    }
}
