===============================  Mouse 3D Plugin  =================================

Thank you for your purchase !

If you have an issue, or want to suggest a new feature, feel free to contact me at 
http://feldev.com

===================================================================================
- This is an Editor plugin
- To use this plugin you have to open the EditorWindow from 
	Main menu => Window => Mouse 3D.

- This window must be visible in order to update the scene view.
- Works on Windows and OS X
- Works with 3dconnexion mice (SpaceNavigator...)
===================================================================================

The Plugin has 3 different modes :

=> Helicopter Mode
	You can navigate in your scene view with freedom.
	You are holding the scene camera in your hand
		- Push your mouse down : the camera view will go down
		- Rotate to the right : the camera view will rotate to the right
		- And that works with the 6 axes	

=> Edit Mode
	Now, you are holding the object in your hand.
	That means that your hand movements will affect the selected GameObject transform. 
	You can really fast, rotate and move an object where you want. 
	By default, the movements are translated relative to your current scene camera angle
		- If you are seeing the object from top, pulling your hand will move the object up.
		- If you are seeing the object from left, pulling your hand will move the object to the left.
	There are 5 movements translation modes :
		- Camera (the best way to move an object, just hold the object)
		- Glocal : The mouse axes are not translated
		- Local : The mouse axes are translated relative to the gameObject local rotation 
			(Very usefull for moving a Camera - Try it !)
		- Plan : Almost like the camera mode, but the movements are only translated along camera's Y rotation
			(Usefull to move some objects on a plan - ground/table...)
		- Custom : Combine the 4 other modes on desired axes
	You cast apply a distance factor on the movements : the nearer the object is to the camera, the slower it will move.

=> Object Mode
	Now you are holding the object in your hand, but it won't move : the camera will rotate aroud the object.
	This is the same behaviour has the objet mode in your 3D modeler (Blender...)
	**I'm not sure that this mode is usefull in the Unity editor. Let me know...**



Configure it as you like
	- Tune the sensitivity of the movements : Translate and rotate
	- Tune the threshold : If it begins to move when you just hold your 3d mouse : you have to increase the threshold
	- Bind some predefined actions on your 3d mouse keys
		* switch to edit mode without using your classic mouse
		* Focus an object without using your keyboard
		* ...
		* Feel free to contact me if you need other actions !


I hope you will enjoy it as much as I do.


Changes :
** Version 1.1
Added the distance factor in Edit mode
Added the Plan Edit mode
Added the Custom Edit mode
