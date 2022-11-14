using Microsoft.VisualStudio.TestTools.UnitTesting;
using NBMMessagingApp;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBMMessagingApp.Tests
{
    [TestClass()]
    public class MessageTests
    {
        [TestMethod()]
        public void sanitizeMessageTest()
        {

            string sender = "";
            string body = "WTF is going on, is this a test case?";
            int id = 0;
            string type = "";

            Message msg = new Message(sender, body, id, type);

            Assert.AreEqual("WTF<What the f***> is going on, is this a test case?",msg.sanitisedBody);

        }
    }
}