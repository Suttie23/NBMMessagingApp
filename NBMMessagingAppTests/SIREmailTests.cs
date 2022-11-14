using Microsoft.VisualStudio.TestTools.UnitTesting;
using NBMMessagingApp;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBMMessagingApp.Tests
{
    [TestClass()]
    public class SIREmailTests
    {
        [TestMethod()]
        public void sanitizeSIREmailTest()
        {
            string sender = "";
            string subject = "";
            string body = "This is a test SIR email https://thisisatest.com";
            int id = 0;
            string type = "";
            string sortCode = "";
            string incidentType = "";

            SIREmail siremail = new SIREmail(sender, subject, body, id, type, sortCode, incidentType);

            Assert.AreEqual("This is a test SIR email <URL Quarantined>", siremail.sanitisedBody);
        }
    }
}