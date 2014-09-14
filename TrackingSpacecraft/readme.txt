This directory holds both the API for spacecraft tracking and the example
game developed for this project. The directory structure has been left so that
Unity can still run the code - all scripts, models, textures and materials are 
included.

The key scripts can all be found in the Assets folder.

1) THE MAIN GAMIFICATION API SCRIPT
This is found in the following location:

Assets -> Scripts -> Gamification -> GamificationAPI1.js


2) EXAMPLE GAME
The most important scripts:

MAIN:
Assets -> Scripts -> Gamification -> gameMain.js

CAMERA ROTATOR USING API METHODS:
Assets -> Scripts -> Gamification -> CameraROT.js

TRACKING SPACECRAFT SCRIPT USING API METHODS:
Assets -> Scripts -> Gamification -> showCraft.js

THE YAMTRAK SCRIPT (NOT WRITTEN BY ALEX PARROTT):
Assets -> Standard Assets -> YAMTRAK.cs


To run an code in Unity, open a new project and replace the Assets and ProjectSettings folders
with those included here.