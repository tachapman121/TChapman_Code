11/16/15
Xiaoyung Ding and Trevor Chapman

As discussed in the previous entry we added in a start button and added the ability to press "Enter" to connect. Outside of that few design decisions were reverted or 
changed. One large issue we ran into was since we draw to a game panel instead of the form itself we had to create a custom class called "Game_Panel" that extends Panel 
and only contains a constructor which sets DoubleBuffered = True since that is a protected property. After we did that it removed the flickering issue we were previously experiencing. 
Currently we are redrawing as fast as possible, which the FPS being consistanly above 500FPS on our machines.

The other large issue we are running into but have yet to figure out is the client freezing after the 4th or 5th reconnect attempt. It seems to be occuring after being eaten 
on that attempt; rather than displaying the screen the client simply freezes. No more redrawing, the labels updating, just freezes. It has also been incredibly difficult to debug 
since it does not throw any sort of exception or error in the stack trace. It just says "Start Button Pushed", some time goes by, then all the threads exit and stop responding.


11/11/15
Xiaoyung Ding and Trevor Chapman

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