Beautiful Transitions v4.1.1

Thank you for using Beautiful Transitions. 

If you have any thoughts, comments, suggestions or otherwise then please contact us through our website or 
drop me a mail directly on mark_a_hewitt@yahoo.co.uk

Please consider rating this asset on the asset store.

Regards,
Mark Hewitt

For more details please visit: http://www.flipwebapps.com/ 
For tutorials visit: http://www.flipwebapps.com/

- - - - - - - - - -

QUICK START

	2. Check out the demo scenes under /FlipWebApps/BeautifulTransitions/_Demo
	3. If you are using Game Framework (free or bundle) then you can enable Beautiful Transitions integration by 
	   selecting the Beautiful Transitions option in the Game Framework Integrations Window (under the editor 
	   Window Menu | Game Framework)

NOTE: Unity 5.0 - 5.1 does not support the dropdown used in the ScreenCamera demo scene. The transitions work, but you will have to preview them manually.

- - - - - - - - - -

UPGRADE GUIDE

IMPORTANT: If you have an older version installed then simply upgrading over the top can sometimes cause problems. To upgrade:
	1. Make a backup of your project.
	2. Delete the old /FlipWebApps/BeautifulTransitions folder to cater for possible conflicts.
	3. Import the latest version from the asset store.

- - - - - - - - - -

CHANGE LOG

v4.1.1

	Fixes
	- Transitions: Events are no longer added / called multiple times when reusing a transition with events setup.

v4.1

	Improvements
	- Demo: Added Camera Cross Fade / Wipe demo
	- Transitions: Added Camera Cross Fade / Wipe

	Fixes
	- Transitions: Fixed warning in Unity 5.5 about deprecated OnLevelWasLoaded method

v4.0

NOTE: Several components and classes have been moved to different namesapces and so if you access the transitions through code you may need
to change your 'using' statements to reflect the new locations. See the change details below for further details.

	Improvements
	- Demo: SceneSwap demo now demonstrates use of cross scene transitions
	- Demo: GameObjectTransitionsDemo demo now demonstrates use of sprite fading
	- Transitions: Cross scene transitions added for scene wipes and fades
	- Transitions: Fade now works with Sprites.
	- Transitions: All components have updated namespaces and are move under FlipWebApps.BeautifulTransitions.Scripts.Transitions.Components.xxx
	- Transitions: All abstract classes and components have updated namespaces and are move under FlipWebApps.BeautifulTransitions.Scripts.Transitions.Components.xxx.AbstractClasses
	- Transitions: TransitionHelper added TakeScreenshot and LoadScene methods.
	- Transitions: TransitionStep Completed method renamed TransitionCompleted
	- Transitions: TransitionStep TransitionInternal method renamed TransitionLoop
	- Transitions: TransitionStep SetProgress call moved from Start to TransitionLoop for better control

v3.2

NOTE: This version contains an important updates relating to initial transition setup. While this should not give any noticable impact we 
advise taking a backup copy before upgrading. If you have any issues or problems then please contact us as listed above.

	Improvements
	- Shake: Updated comments
	- Transition: Updated tooltips
	- Transition: Deprecated MoveTarget TransitionStep as the functionality is provided by Move (Note: MoveTarget component remains)
	- Transition: Added option to specify the axis on which MoveTarget should work so you can easier move multiple items (see GameObjectTransitionsDemo)
	- Transitions: Added RepeatWhenEnabled option for auto running transitions multiple times.	

	Fixes
	- Transition: Moved initial transition setup to before transition call to avoid possible execution order issues when using the API

v3.1

NOTE: To update you will need to remove the old /FlipWebApps/BeautifulTransitions/ folder before updating, or delete the file 
/FlipWebApps/BeautifulTransitions/_Demo/DisplayItem/Scripts/TestController.cs after updating.

	Improvements
	- Demo: Added attention button to the DisplayItem demo scene
	- Demo: Shake demo updated with visual controls for modifying the shake settings
	- DisplayItem: Removed unnecessary DisplayItemSetInitialState component
	- DisplayItem: Added SetAttention and SetActiveAnimated functions to DisplayItemHelper.cs
	- General: Added links to documentation and support to the editor menu
	- Shake: Moved scripts from ShakeCamera to Shake folder and namespace.
	- Shake: Improved tooltip text for ShakeCamera component
	- Shake: Renamed Shake method to ShakeCoroutine and added new replacement Shake method that is callable from code.
	- Shake: Code documentation improved
	- Transitions: Updated component menu name
	- Transitions: Screen & Camera wipes now support smoothing.

	Fixes
	- Fix: Correctly handle transitions that outlive a scene change causing error when transition targets are destroyed.

v3.0

NOTE: To update you will need to remove the old /FlipWebApps/BeautifulTransitions/ folder before updating, or delete the file 
/FlipWebApps/BeautifulTransitions/Scripts/Transitions/GameObject/TransitionMoveAnchoredPosition.cs after updating.

	Improvements
	- Rewritten from the ground up to expose the whole API through scripting including calls and notifications.
	- GameObject: Removed deprecated TransitionMoveAnchoredPosition component
	- Demo: New scripting demo
	- Demo: Added auto transition in / out button to GameObject demo

v2.3
	Improvements
	- Demo: GameObject demo updated to use the new TransitionMove component
	- Editor: Simplified inspector UI
	- GameObject: Support for global and local rotations
	- GameObject: New TransitionMove component with support for global, local and anchored position values.
	- GameObject: Deprecated TransitionMoveAnchoredPosition in vavour of new TransitionMove component.	

	Fixes
	- GameObject: TransitionMoveTarget now supports standard Transform and not restricted to using a RectTransform.

v2.2
	Improvements
	- Exposed Transition Start and Complete events through the inspector so you can easily hook up other code.
	- Transitions are now shown on the component menu
	- Demo: Added Events Demo
	- Demo: Added SceneSwap Demo

v2.1.1
	Fixes
	- Under certain build conditions the shaders would not be included. Moved shaders to a resources folder and 
	improved load validation.

v2.1
	Improvements
	- Shake Camera: Added ShakeCamera component and shake helper for shaking other gameobjects.
	- Code refactor improvements (some shared script files moved to the Helper folder)

	Fixes
	- Added demo titles

v2.0
	Improvements
	- All previous transition curves are now built in, removing the dependency on iTween and giving improved performance.
	- Updated inspector gui with visual curves.
	- Code refactor improvements (if you experience any build errors with your own derived classes that you don't know how to fix then let us know)

	Fixes
	- Fixed unpredictable handling of concurrent transition calls and multiple calls to the same transition

v1.2
	Improvements
	- Added the ability to define your own animation curves.
	- Added Property Tool Tips. 
	- Added Custom Property Editor for improved UI. 
	- Added help link to components

	Fixes
	- Demo scene camera demo correctly only runs the specified transition.
	- iTween bug where start transition value was not always set until after 1 frame.
	- A disabled component would wrongly be run when placed as a nested transition.

v1.1
	Improvements
	- New Rotate UI / Game Object transition. 
	- Transition core code refactor.

	Fixes
	- Demo Black tint was setting wrong color.

v1.0
	Improvements
	- Rewritten core for easier extensibility
	- Added Screen transitions including fade and multiple wipe transitions
	- Added Camera transitions including fade and multiple wipe transitions
	- Added the possibility to easily create your own custom transitions by uploading a new Alpha texture
	- Added new demo for screen and camera transitions.

v0.8
	First public release