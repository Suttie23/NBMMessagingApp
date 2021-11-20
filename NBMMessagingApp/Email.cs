using System;
using System.Collections.Generic;
using System.Text;

namespace NBMMessagingApp
{
    class Email : Message
    {

        public string messageSubject { get; set; }
        public string sortCode { get; set; }
        public string incidentType { get; set; }

        public Email(string msgsender, string msgsubject, string msgbody, int msgID, string msgType, string msgSortCode, string msgIncidentType) : base(msgsender, msgbody, msgID, msgType)
        {
            this.messageSubject = msgsubject;
            this.sortCode = msgSortCode;
            this.incidentType = msgIncidentType;

        }

        public string getEmailData()
        {
            if (this.messageSubject.Contains("SIR"))
            {
                return "Message ID:" + this.messageType + this.messageID + "\n\n" + "Sender: " + this.messageSender + "\n\n" + "Subject: " + this.messageSubject + "\n\n" + "Sort Code: " + this.sortCode + "\n\n" + "Incident Type: " + this.incidentType + "\n\n" + this.messageBody;
            } else
            {
                return "Message ID:" + this.messageType + this.messageID + "\n\n" + "Sender: " + this.messageSender + "\n\n" + "Subject: " + this.messageSubject + "\n\n" + this.messageBody;
            }
        }
    }
}
