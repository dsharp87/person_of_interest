Person of Interest is a matchmaking application.  It is designed to allow users to create a profile, take compatibility quizes, and chat in real time with online users.

This project was built using ASP.NET CORE backend with an Angular front end. It utilizes a MySQL database for datastorage, and SingalR sockets to support live chat functionality.


Feature List:

Registration and Login:
    - Register if you do not have a login already
    - Login if you are a returning user

Landing Page
    - Hub area designed to onboard new users
    - Provides navigation points to other sections of application
    - Navigation functionality is currently duplicated by nav bar on left (will be remvoed or resdesigned in future)

Compatibility quizes
    - Users can take compatibility quizes designed to asses general interests and preferences
    - Currently there are two quizes in the database, but more quizes can easily be added 
    - Quiz display uses Angular logic to smartly display unanswered questions without requiring users to scroll
    - Answers can be reviewed before submission, and quizes can be retaken any number of times

Chat Hub
    - Users can use chat hub once they have completed at least 1 quiz
    - Upon entering, it displays all users who are currently logged on and in the chat hub
    - Calculates user's compatitbility match percentage using quiz results, which is displayed next to each name
    - Allows users to select user they wish to send their message to, which is reflected in the chat window
    - application listens for key words, generating icebreaker questions for the two users currently connected to help start a conversation

Want to Try it Yourself?
- clone respository
- run dotnet restore
- using MySQL workbench run db.generate.sql script to create empty database
    - run starter_data.sql script to create initial quizes and three dumby users
- from top level, run dotnet run
- go to localhost:5000


Features we'd like to add
- complete password hashing and salting (can't store real passowrds thats scary)
- add profile page, allowing users to have additional infomation about themselves
- separate chat hub into one chat window per ongoing conversation