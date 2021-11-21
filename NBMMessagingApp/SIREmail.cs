﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NBMMessagingApp
{
    class SIREmail : Message
    {

        public string messageSubject { get; set; }
        public string sortCode { get; set; }
        public string incidentType { get; set; }

        public SIREmail(string msgsender, string msgsubject, string msgbody, int msgID, string msgType, string msgSortCode, string msgIncidentType) : base(msgsender, msgbody, msgID, msgType)
        {
            this.messageSubject = msgsubject;
            this.sortCode = msgSortCode;
            this.incidentType = msgIncidentType;
            this.sanitisedBody = sanitizeSIREmail(msgbody);

        }

        public string getSIREmailData()
        {

                return "Message ID:" + this.messageType + this.messageID + "\n\n" + "Sender: " + this.messageSender + "\n\n" + "Subject: " + this.messageSubject + "\n\n" + "Sort Code: " + this.sortCode + "\n\n" + "Incident Type: " + this.incidentType + "\n\n" + this.sanitisedBody;

        }

        public string getSIRList()
        {

            return "Sort Code: " + this.sortCode + "\n\n" + "Incident Type: " + this.incidentType + "\n\n";

        }

        public string sanitizeSIREmail(string msgbody)
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
