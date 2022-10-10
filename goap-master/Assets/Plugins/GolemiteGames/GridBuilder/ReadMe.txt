Thankyou for purchasing Grid Builder.

Demo scenes are included in the scenes folder to help understand using the components. 
Current controls are set to Left mouse button to place or remove objects and right mouse button or escape to cancel either. 
You will find these Input controls in GridSelector.cs and RemoveMode.cs. 

Easiest way to start is to use a grid, and gridSelector prefab.

To set up quickly - 

1. Navigate to /Prefabs/WholeGrids, and drag in one of the premade grids and adjust the General settings such as height, width and cellSize. 
Best to keep the cellSize at 1 = 1 metre. However everything will adjust if you decide to change it. 
You can use as many grids as you want, 1 - 5 - 50!
If you want the grid to be above some existing terrain, move the grid on the Y axis slightly above. A value of 0.015 works good. 
Leave AutoCellBlock off for now. If you want to use it, please see the Setup.pdf supplied with the package. 

2. Navigate to /Prefabs and drag in a GridSelector, ObjectPlacer, and ObjectRemover. 
You should only have one of each of these. 
Assign the ObjectPlacer to the gridSelector slot. 

3. Back in /Prefabs, drag in a SelectObjectBtn and RemoveModeBtn. 
This will create a canvas so you can move the buttons somewhere convenient. 
By default the SelectObjectBtn has a demo prefab assigned but feel free to change it. Please note there is no scaling done to prefabs,
so the models will have to match the grid size.

4. Thats it! Hit play and you should be able to move, click on an object and place it down on the grid. 
You can also hit the removeMode button to highlight and delete objects. 

5. After this there is quite a bit to play around with, particularly the AutoCellBlock feature. Please read the Setup.pdf on how to set this up.
Its super simple and quick to set up once you know how it works. Also tyhe full Documentation.pdf has all sorts of helpful information for setting up and limitations of the system.

6. Happy building! If you are happy with the asset please leave me a review if you have the time, it really helps!
If there is anything you are not happy with, please contact me on the email below, so I can try to resolve the issue. 

If you have any questions or are having trouble setting up please dont hesitate to contact me at support@golemitegames.com





