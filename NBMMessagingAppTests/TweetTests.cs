using Microsoft.VisualStudio.TestTools.UnitTesting;
using NBMMessagingApp;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBMMessagingApp.Tests
{
    [TestClass()]
    public class TweetTests
    {
        [TestMethod()]
        public void getTagsTest()
        {

            string sender = "";
            string body = "Such a cool tweet bro #awesometweet #besttweet";
            int id = 0;
            string type = "T";

            Tweet tweet = new Tweet(sender, body, id, type);

            Assert.AreEqual("#awesometweet #besttweet ", tweet.hashTags);

        }
    }
}