<h1>NBMMessagingApp</h1>

Coursework for Year 3 Software Engineering (November 2021), updated with lost final version found in 2022.

## Premise
Napier Bank is a medium-sized local bank with many thousands of users. The bank 
operates from one headquarters and a number of branches. You are required to 
develop a service, namely Napier Bank Messaging (NBM), which will validate, 
sanitize and categorise incoming messages to Napier Bank in the form of SMS text 
messages, emails and Tweets

## Requirements
The system must deal with three types of message.
All messages are strings of ASCII characters that have a Message Header
comprising a Message ID (Message-type “S”,”E” or “T” followed by 9 numeric 
characters, e.g. “E1234567701”) followed by the Body of the message.

Depending on the message type the Body will comprise:

### SMS messages

• SMS message bodies comprise Sender in the form of an international 
telephone phone number followed by the Message Text which is a 
maximum of 140 characters long. The Message Text message is 
simple text but may contain embedded “textspeak abbreviations”. 
Details of the textspeak abbreviations that may be embedded are 
supplied on Moodle in the form of a CSV file.

### Email Messages:

• Email message bodies comprise Sender in the form of a standard 
email address John Smith john.smith@example.org followed by a 20 
character Subject followed by the Message Text which is a maximum 
of 1028 characters long. The Message Text message is simple text 
but may contain embedded hyperlinks in the form of standard URLs e.g. 
http:\\www.anywhere.com. Further detail of email messages is 
provided in 3.1.2 below.
4

### Tweets

• Tweet bodies comprise Sender in the form of a Twitter ID: “@” 
followed by a maximum of 15 characters (e.g. @JohnSmith) and the 
Tweet text which is a maximum of 140 characters long. In addition to 
ordinary text the Tweet text may contain any of the following:

• textspeak abbreviations (as in SMS above)

• hashtags - strings of characters preceded by a ‘#’ sign that are 
used to group posts by topic. (such as #BBCClick, #1Donice). 

• Twitter IDs as above

## Tasks

You should complete the following tasks by exercising advanced software 
development technologies you learnt in this module:

1) Undertake a requirement analysis for NBM. You need to specify the requirement 
in a Use Case diagram, preferably in USE Case with Soft Goals (NFRs).

2) Produce a class diagram that illustrates the classes required to perform the
operations identified in the scenario. Your class diagram should include outline 
methods and attributes and the relationships among the classes. 

3) On the basis of your class diagram develop a WPF application using C# that 
realises all the functionality specified in Section 2 and 3. Development in other 
techniques such as Java etc is also acceptable.

4) Additional Requirement: Modify your system so that the messages are read 
from a text file and processed and displayed one-by-one on screen. You can
design the structure of this input text file yourself, but it shouldn’t be a JSON file.

5) Testing: i) Briefly describe your overall testing strategy for the system. What 
types of testing will you do, how will you identify test cases? ii) Provide a test plan, 
which should include Objectives and Scope, Test Items, Tasks and Deliverables, 
Testing methods, Environmental Needs, possible Tools, Test Schedule, and 
possible Risks and Solutions. iii) Develop test cases and construct tests to verify 
that messages are processed correctly for each type of message. Use Visual
Studio testing facilities (or equivalence on the platform you have chosen) to 
conduct your tests where appropriate.

6) Presuming the system is to be developed in agile approach. Propose a plan to 
use version control to support the development iteration and collaboration among 
team members.

7) Prepare an evolution strategy for the NBM system. What evolution or 
maintenance you would predict? What’s the maintainability of your system and 
what are the predicted maintenance costs? What evolution process and methods 
you plan to use?
