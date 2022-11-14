using Microsoft.VisualStudio.TestTools.UnitTesting;
using NBMMessagingApp;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBMMessagingApp.Tests
{
    [TestClass()]
    public class EmailTests
    {
        [TestMethod()]
        public void sanitizeEmailTest()
        {

            string sender = "";
            string subject = "";
            string body = "This is a test email https://thisisatest.com";
            int id = 0;
            string type = "";

            Email email = new Email(sender, subject, body, id, type);

            Assert.AreEqual("This is a test email <URL Quarantined>", email.sanitisedBody);

        }
    }
}