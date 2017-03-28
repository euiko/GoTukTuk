//----------------------------------------------
// Flip Web Apps: Beautiful Transitions
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//
// Please direct any bugs/comments/suggestions to http://www.flipwebapps.com
// 
// The copyright owner grants to the end user a non-exclusive, worldwide, and perpetual license to this Asset
// to integrate only as incorporated and embedded components of electronic games and interactive media and 
// distribute such electronic game and interactive media. End user may modify Assets. End user may otherwise 
// not reproduce, distribute, sublicense, rent, lease or lend the Assets. It is emphasized that the end 
// user shall not be entitled to distribute or transfer in any way (including, without, limitation by way of 
// sublicense) the Assets in any other way than as integrated components of electronic games and interactive media. 

// The above copyright notice and this permission notice must not be removed from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//----------------------------------------------

using UnityEngine;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps.AbstractClasses;
using UnityEngine.UI;

namespace FlipWebApps.BeautifulTransitions._Demo.Transitions.Scripts
{
    /// <summary>
    /// Demonstration of using some of the scripting features in Beautiful Transitions
    /// </summary>
    public class ScriptingDemo : MonoBehaviour
    {

        public GameObject TestGameObject;
        public GameObject TestGameObject2;
        public GameObject TestGameObject3;
        public GameObject TestGameObject4;
        public Text Description;

        void Start()
        {
            ShowTransitionedDescription("Basic linked transitions  with events");

            // Create a new transition to move the gameobject with a delay of 1 and duration of 3 and the specified actions
            var startPosition = new Vector3(10, 0, 0);
            var endPosition = Vector3.zero;
            var transition = new Move(TestGameObject, startPosition, endPosition, 1, 3,
                tweenType: TransitionHelper.TweenType.easeInOutBack, onStart: LogStart, onUpdate: LogUpdate, onComplete: LogComplete);

            // Add an additional complete action with custom data
            transition.AddOnCompleteAction(LogComplete, "Complete Parameter");

            // chaing some additional transitions and on complete call next transition
            transition.ScaleToOriginal(Vector3.zero, 1, 1, runAtStart: true).
                RotateFromOriginal(new Vector3(360, 0, 0), 1, 3, coordinateMode: TransitionStep.CoordinateSpaceType.Local, runAtStart: true).
                RotateToOriginal(new Vector3(180, 0, 0), duration: 2).
                ScaleFromOriginal(Vector3.zero, delay: 1, duration: 2, runAtStart: true, onComplete: TransitionItem2);

            // start everything.
            transition.Start();
        }

        void TransitionItem2()
        {
            ShowTransitionedDescription("Linked transitions in one call.");

            // transition the second item.
            var transition = new Scale(TestGameObject2, Vector3.zero, Vector3.one * 5, 0, 3, tweenType: TransitionHelper.TweenType.easeInOutBack).
                ScaleFromOriginal(new Vector3(7.5f, 2.5f, 1), 1, 2, tweenType: TransitionHelper.TweenType.easeInOutBack).
                ScaleFromOriginal(new Vector3(10, 10, 1), 1, 2, tweenType: TransitionHelper.TweenType.easeInOutBack).
                ScaleFromOriginal(Vector3.zero, 1, 2, tweenType: TransitionHelper.TweenType.easeInOutBack, onComplete: TransitionItem3).GetChainRoot();
            transition.Start();
        }

        void TransitionItem3()
        {
            ShowTransitionedDescription("Trigger a transition component with callback.");

            // transition the third item which is a component that triggers its own transition out on complete.
            TransitionHelper.TransitionIn(TestGameObject3, TransitionItem4);
        }

        void TransitionItem4()
        {
            ShowTransitionedDescription("Fade and Scale in one.");

            // fade the end text in while at the same time scaling.
            new Fade(TestGameObject4, 0, 1, 0, 4).
                ScaleToOriginal(Vector3.zero, 0, 3, runAtStart: true).GetChainRoot().Start();
        }

        void ShowTransitionedDescription(string text)
        {
            // fade the end text in while at the same time scaling.
            Description.text = text;
            var transition = new Fade(Description.gameObject, 0, 1);
            transition.FadeFromOriginal(0, delay: 2);
            transition.GetChainRoot().Start();
        }

        void LogStart()
        {
            Debug.Log("Start");
        }

        void LogUpdate(float progress)
        {
            Debug.Log("Update:" + progress);
        }

        void LogComplete()
        {
            Debug.Log("Complete no parameter");
        }

        void LogComplete(object parameter)
        {
            Debug.Log("Complete with parameter: " + parameter);
        }

        public void ShowRatePage()
        {
            Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/content/56442");
        }

    }
}