12/10/15
Xiaoyun Ding and Trevor Chapman

Database Description:
--------------------
Our database is split across 2 tables: Scores and Players_Eaten. Scores contains all information about the cube outside of 
what players were eaten, Players_Eaten contains the players the cube has eaten. They are linked together by a column called 
PlayerID, if the player has not eaten any cubes they will not have an entry in Players_Eaten. The SQL we use in C# are mostly 
select statements (or select with a left join for GetEaten) on PlayerID. Most of it is done in the final methods in our Server code. In addition 
we have a method called UpdateDB which does an insert into the two tables as needed. After inserting the record, we do an 
Update on rank to set all equal to zero, then we do another select and order by the mass. Afterwards we then update the first 5 
records with the appropriate rank (1-5). To insert into the Players_Eaten table we get the new ID by doing a Select MAX(PlayerID) on 
the Scores table since the number auto-increments and the max will be the newest record, and use that ID to insert into Players_Eaten.
---------------------

Unfortunately with the HTML we ended up repeating some codes, there are just small enough differences between the two that 
it made it difficult to separate it, likely it would have been more code/work to build functions around them than to just 
type it again. In addition we created a separate web error page and stored it in a file called web_page.txt, which we just 
send in case of an error which details options and examples. This webpage does have a name (Example) and id hardcoded in, 
so it assumes these entries are in the database. If they are not then it will return a page with no information as with any 
webpage with invalid information.

To get the information we modified the Cube class to have a new inner class called RecordingInfo which contains all the info 
we record. We do this instead adding the information so that split cubes can directly access them by reference instead of 
needing to update the list each time.

There are a few minor issues with the server described below, we did not address those issues so they are still present in 
this build. However for basic testing (having 2 clients, eat one, having another eat the 1st to check Players_Eaten updated) 
it should work as intended. For some reason it is not updating if we attempt to eat a client that has disconnect but still has 
a cube, or if we eat them when they are split. However for normal procedure (cube eating cube) it should update the database 
correctly.

There were also a couple of minor issues when getting the webpage that we ran into. Besides SQL Injects (which Jim said don't 
worry about in class), if they included a space in the name it causes issues since for HTML it converts 'test 2' to 'test%202'. 
We were going to add a check on the string for %20 and replace it with ' ', but then it runs into an issue if a player enters 
%20 as part of the name for some reason. Space is probably more likely to occur so that's probably the better solution but 
since we did not explicitly ban %20 we left it alone.

-----------------------------------------------------------------------------------------------------------------------------
12/3/15
Xiaoyun Ding and Trevor Chapman

We tried again to get it working with our client and is does not, Jim's seems to work fine however.

One weird issue we ran into was with attempting to conenct. We tried a couple of ways, one using IPAddress.Any which allowed 
any outside client to connect, however entering in LocalHost did not. We then updated it to use the DNS.GetHostAddress and pass 
that in which let LocalHost as the server name work great but outside connections not. We did some research and couldn't figure it 
out so just decided to listen on both which works with multiple outside and local clients.

Somewhat related to the networking issues we kept running into a problem where a socket would be disposed of, however it would 
still be accessed while processing the rest of its Send/Move requests. We added a few try/catch blocks around these, which seemed 
to fix the majority of the issues.

For the virus we based it on how many cubes have been generated, so every X cube is a virus determined by the the virus rate. 
When eaten it explodes the cube into 3 smaller cubes, the 1st 1/2 the size of the orignal and the other two 1/4 of the original size. 

The last bits of our efforts was spent on testing. Frankly we did not spend nearly as much time as we would have liked, so a large 
part of our testing ended up being on the more easily done portions such as checking that the Cube class worked, moving, attriting, 
generating random cubes and viruses, and so on. We would have liked to spend more time on checking Split since that was the method 
that was by far the most complex and hardest to get working but since the dictionaries are private it makes checking them 
difficult. We could have used the PrivateAccessors but at the time we weren't sure on how exactly they worked, and again due 
to time constraints felt we could better spend our time elsewhere which we did getting Split to perform smoother instead of 
teleporting the cubes to their location.

For that we added another field in the Cube class called momentumFrames. This is a counter for how many moves to do with an 
additional boost when coming off of a split. We then add these frames to the speed before doing further computations, therefore 
allowing it to move faster at the beginning and decay over time back to what the normal speed should be.

12/2/15
Xiaoyun Ding and Trevor Chapman
For the server we moved most of the new gameplay functions into the world class (move, split, attrit, etc.) and called them in the 
server class when needed. Currently at the start of a game we set the variables in the constructor though we will change it to an 
XML document if time permits. In addtion we will a list with about 13 colors for drawing the cube and start a UID counter. When 
drawing food we increment the UID counter, do a % operation, and choose the appropriate color. We considered just keeping a counter 
for the list that would cycle as well but since this works well enough we decided against adding another. 

To keep track of all this we are currenlty using 2 new Dictionaries in the Server class that are set up to be inverses of one 
another. However while any time a new player is added both are updated they are not always kept in sync and removed at the same 
time. We use the socketToUID dictionary to keep track of all players that are correctly connected, and the UIDToSocket dictionary 
to keep track of all of the cubes and which player they belonged to. We initially did have them in sync and tried a few other 
ideas, but since the player cube will stay even after being disconnected we modified it. It's not the best solution and has 
caused some confusion but due to time constraints it was the one that seemed the easiest to modify, if we have time at the end 
we will possibly look into other ways.

For actually handling incoming messages wwe are using Regex commands to parse whether or not they are a valid message, and only 
acting on them if they are. Currently if it is an invalid message we are not handling it, for example if someone other than 
a correct client attempts to connect and sends a message like "move 5 10" we don't detect it's not the correct syntax and 
remove them. It seems like it should be an easy problem to fix however since we are usually connected on our laptops through 
WiFi there will be instances where we receive \0 due to momentary drops. It doesn't make sense to disconnect them entirely if 
it picks up and sends valid messages a few moments later so we are looking into solutions such as adding a counter, where if they 
send X invalid messages in a row we disconnect them, or we detect that it is all \0 and they are still connected to do not 
disconnect them but haven't decided on anything yet. Thinking on it as I'm typing this likely what we can do is exactly that since 
we clean up conenctions in the Update() method anyways so the occasionaly \0 message should be fine.

As for the Update() method it is working functionally but could use some cleanup/optimizations at the end. For example right now 
for checking if any cubes have been eaten by a player by looking through the entire world for each player and seeing if they are 
within the correct range. With a small number of players and the food set around 5000 it works fast enough that it causes no 
noticable performance problems but doubt it would scale well. Again as is a common theme if time permits we'll look at a better 
solution.

For handling disconnected clients we take care of it at the end of our Update() method. Throughout the update method we keep 
track of if a player has been eaten or disconnected and add them to a list, and take care of them at the end. Something we 
didn't know about earlier was there is a property called "Connected" for Sockets that tells if it is connected or not, so we use 
this to determine if we need to send to them or not. This fixed a problem we were having where it would still attempt to send 
data even after clsoing the socket, resulting in an error for trying to use a disposed object.

Related to that was similar to PS7 we never ran into a situation where we received 0 bytes, if a client disconnected we would still 
be receiving messages filled with \0 and a full buffer. We thought about playing around with it some however since those portions 
(the Send and Receive) are also used by the client we held off in case it would break something.

-----------------------------------------------------------------------------------------------------------------------------------
11/17/15
Xiaoyun Ding and Trevor Chapman

To solve the freezing issue we changed Invoke in the CheckDeath() method to BeginInvoke. We were also running into a JSON error when attempting to reconnect after several times, 
and it seems to be because we were not clearing the buffer correctly. To fix this we created a separate method in the State class and called that instead of having to worry about 
constantly including both lines. We also added messages to alert the user when they have been disconnected or are unable to connect to the server and possible reasons why.


11/16/15
Xiaoyun Ding and Trevor Chapman

As discussed in the previous entry we added in a start button and added the ability to press "Enter" to connect. Outside of that few design decisions were reverted or 
changed. One large issue we ran into was since we draw to a game panel instead of the form itself we had to create a custom class called "Game_Panel" that extends Panel 
and only contains a constructor which sets DoubleBuffered = True since that is a protected property. After we did that it removed the flickering issue we were previously experiencing. 
Currently we are redrawing as fast as possible, which the FPS being consistanly above 500FPS on our machines.

The other large issue we are running into but have yet to figure out is the client freezing after the 4th or 5th reconnect attempt. It seems to be occuring after being eaten 
on that attempt; rather than displaying the screen the client simply freezes. No more redrawing, the labels updating, just freezes. It has also been incredibly difficult to debug 
since it does not throw any sort of exception or error in the stack trace. It just says "Start Button Pushed", some time goes by, then all the threads exit and stop responding.


11/11/15
Xiaoyun Ding and Trevor Chapman

For our program we split it up into the model, view/controller, and networking. The model contains the Cube object which has the neccesary properties according to the specifications, 
and the World object contains a model of all the cubes in the world along with functions for adding cubes and getting all the cubes in the world. Since we would be handling 
adding and removing many cubes very quickly we used a Dictionary, using the cube's UID (unique ID) as it's key. 

For the View/Controller we kept it in one class called AgCubioGUI. Currently after entering in the hostname and username into the GUI the user presses enter to beging connecting 
to the host. After a few moments the screen clears and begins drawing the world, or displays an error message saying the client cannot connect. We are considering adding in a 
button to click instead of having to press enter to make it more clear but that may change. At the moment we are also drawing every cube in the world at the same time instead 
of just the ones nearby with the thought that it will be fast enough regardless, but need to do some more testing to determine if instead we should just draw the local ones 
by calculating if their relative position is within X units. Outside of the GUI we have several methods that are called by the controller to update the World with the Cubes. 
To get the cubes we deserialize them using a JSON string and then add them to the cube. This method has a lock on it, as does the paint on the World object so they are not 
interfering with each other. The largest design problem so far has been attempting to deserialize several cubes that come in as one message. Currently we are spliting them 
on '\n' and storing them in a string[] array and adding each on except for the last item since it is usually an incomplete JSON string. To handle this we clear our buffer and 
StringBuilder (stored in the Network_Controller) and append it to the beginning of it for the next time it is painted which seems to be working well. After adding all the cubes to 
the world we then ask for more data and call the paint function with the idea being while it's repainting more data is coming in so once it finishes it can immediately beging 
creating more cubes again. We have done testing to make sure that the conenction stays and it is constantly pulling in more data and adding them to the world, though we are 
not drawing them entirely yet so this may require some modifications.

For the Network_Controller library we have two classes: The NetWorkController which contains the methods to handle the actual networking part and a State class which contains 
things such as the socket being uses, the buffer, the UTF8 encoder, and the function to call in the View/Controller once the data is sent/recieved. It contains a couple of small 
functions that will decode the data stored in the buffer into the StringBuilder and other minor functions for getting and setting. This object is initially created when 
attempting to establish connection, and then going forward is passed through to the other fucntions and callback functions as a way of passing lots of data back and forth. 
All of this is accomplished by chaining one function to another. After connecting for the first time another function is called to start recieving and to call another 
function in the View once initially connected to set up what it needs to. That function then updates the function that should be called and calls the Send function, 
which sets up another callback for recieving more data, which sets up a callback to add a player cube, which then calls a function asking for more data which calls 
the function to add the cubes to the world, which then calls for data and so on. By doing so after the initial connection data will be sent and processed constantly 
without requiring any more interaction by the user. Even though it interacts heavily with the View it does not contain any direct references to it, so it should be able to 
be taken and used for another project with very little to no necessary modifications.