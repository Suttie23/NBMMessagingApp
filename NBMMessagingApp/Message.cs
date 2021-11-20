using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NBMMessagingApp
{
    class Message
    {

        public string messageSender { get; set; }
        public string messageBody { get; set; }
        public string messageType { get; set; }
        public int messageID { get; set; }
        public string sanitisedBody { get; set; }

        public Message(string msgsender, string msgbody, int msgID, string msgType)
        {
            this.messageSender = msgsender;
            this.messageBody = msgbody;
            this.messageID = msgID;
            this.messageType = msgType;
            this.sanitisedBody = sanitizeMessage(msgbody);

        }

        public string sanitizeMessage(string msgbody)
        {
            Dictionary<string, string> textSpeak = new Dictionary<string, string>();

            using (var reader = new StreamReader(@"D:\My Folders\Uni\Software Engineering\Coursework\Materials\textwords.csv"))
            {

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    textSpeak.Add(values[0], values[1]);

                }

            }

            var words = messageBody.Split(" ");
            for (int i = 0; i < words.Length; i++)
            {
                if (textSpeak.TryGetValue(words[i], out string newWord))
                {
                    words[i] = words[i] + "<" + newWord + ">";
                }
            }

            string sanitisedBody = string.Join(" ", words);
            return sanitisedBody;

        }

        public string getSMSData()
        {

                return "Message ID:" + this.messageType + this.messageID +  "\n\n" + "Sender: " + this.messageSender + "\n\n" + this.sanitisedBody;
      
        }

    }
}
