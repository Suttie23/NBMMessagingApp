using System;
using System.Collections.Generic;
using System.Text;

namespace NBMMessagingApp
{
    public class Email : Message
    {
        // Getters / Setter
        public string messageSubject { get; set; }

        // Email Constructor
        public Email(string msgsender, string msgsubject, string msgbody, int msgID, string msgType) : base(msgsender, msgbody, msgID, msgType)
        {
            this.messageSubject = msgsubject;
            this.sanitisedBody = sanitizeEmail(msgbody);

        }

        // ToString
        public string getEmailData()
        {
                return "Message ID:" + this.messageType + this.messageID + "\n\n" + "Sender: " + this.messageSender + "\n\n" + "Subject: " + this.messageSubject + "\n\n" + this.sanitisedBody;
        }

        // Quarantine URLs in Email
        public string sanitizeEmail(string msgbody)
        {

            var words = messageBody.Split(" ");
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Contains("www.") || words[i].Contains("http://") || words[i].Contains("https://"))
                {
                    words[i] = "<URL Quarantined>";
                }
            }

            string sanitisedBody = string.Join(" ", words);
            return sanitisedBody;

        }
    }
}
