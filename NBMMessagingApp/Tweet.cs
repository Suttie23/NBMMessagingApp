using System;
using System.Collections.Generic;
using System.Text;

namespace NBMMessagingApp
{

    public class Tweet : Message
    {
        // Getters / Setters
        public string hashTags { get; set; }

        // Tweet Constructor
        public Tweet(string msgsender, string msgbody, int msgID, string msgType) : base(msgsender, msgbody, msgID, msgType)
        {

            this.hashTags = getTags(msgbody);

        }

        // Get hashtags
        public string getTags(string msgbody)
        {
            // Picking out hashtags
            var tags = msgbody.Split(" ");
            for (int i = 0; i < tags.Length; i++)
            {
                if (tags[i].Contains("#"))
                {
                    hashTags = hashTags + tags[i] + " ";
                }            

            }

            return hashTags;

        }

        // Tweet ToString
        public string getTweetData()
        {

            return "Message ID:" + this.messageType + this.messageID + "\n\n" + "Sender: " + this.messageSender + "\n\n" + this.sanitisedBody + "\n\n\n\n" + this.hashTags;

        }

    }

}
