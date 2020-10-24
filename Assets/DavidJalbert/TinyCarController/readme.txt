Name : Tiny Car Controller
Version : 1.4.0
Author : David Jalbert
Last modified : 2020-06-07
Description : Simple dynamic vehicle controller for Unity
License : MIT License

Support: 
- Unity Asset Store : https://assetstore.unity.com/packages/tools/physics/tiny-car-controller-151827
- email : jalbert.d@hotmail.com
- twitter : https://twitter.com/DavidJayIndie

=====================================================
Package structure
=====================================================

Components - Scripts to add to your game objects.
Examples - Examples and tests for different use cases.


=====================================================
Getting started
=====================================================

The basics

1) Create an empty GameObject in your scene.
2) Add the script at 'Assets/DavidJalbert/TinyCarController/Components/TinyCarController.cs' to the GameObject.
3) Adjust the parameters in the 'Tiny Car Controller' component to fit the desired size, mass, and behavior.

For input and visuals

1) Add the car and wheels models as children of the root GameObject.
2) Add the script at 'Assets/DavidJalbert/TinyCarController/Components/TinyCarPlayer.cs' to the root GameObject.
3) Drag and drop your models to the respective fields of the TinyCarPlayer script.
4) Adjust the parameters to your liking.

For the camera

1) Add the script at 'Assets/DavidJalbert/TinyCarController/Components/TinyCarCamera.cs' to your camera GameObject.
2) Set the field "What to follow" to the root GameObject of the vehicle controller.
3) Adjust the parameters to define the behavior of the camera relative to the vehicle.

For surfaces

1) Add the script at 'Assets/DavidJalbert/TinyCarController/Components/TinyCarSurface.cs' to the collider you want to tweak.
2) Adjust the parameters to modify the behavior of the surface when the vehicle runs over it.


=====================================================
Tips
=====================================================

- Fields tooltips and contextual tips in the editor explain each parameter's functionality.
- Don't be afraid to experiment with parameter values to find the right behavior for your needs.
- Check out the example scene at 'Assets/DavidJalbert/TinyCarController/Examples/Example.unity'.


=====================================================
Version History:
=====================================================

-----------------------------------------------------
Version 1.4.0 - 2020-06-07 :
-----------------------------------------------------
- Added reverse speed/acceleration and brake parameters
- Improved the calculations of the angle of the car's body

-----------------------------------------------------
Version 1.3.1 - 2020-05-19 :
-----------------------------------------------------
- Fixed the third-person camera jitter when turning

-----------------------------------------------------
Version 1.3.0 - 2020-04-15 :
-----------------------------------------------------
- Minimum compatible version is now 2018.4
- Added a parameter to define slope friction when gravity is set to toward slopes
- Added the option to toggle camera mode in demo

-----------------------------------------------------
Version 1.2.1 - 2020-03-25 :
-----------------------------------------------------
- Fixed delta time for acceleration functions

-----------------------------------------------------
Version 1.2.0 - 2020-03-23 :
-----------------------------------------------------
- Added the Tiny Car Camera script
- Added the Tiny Car Surface script
- Added a parameter to define the layers checked on the controller
- Added the option to only rotate on the x axis (for motorcycles for instance)
- Misc bug fixes

-----------------------------------------------------
Version 1.1.0 - 2020-02-14 :
-----------------------------------------------------
- Added the option to change the gravity behavior
- Minor bug fixes

-----------------------------------------------------
Version 1.0.0 - 2020-01-14 :
-----------------------------------------------------
- Initial release
