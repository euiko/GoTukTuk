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

using FlipWebApps.BeautifulTransitions.Scripts.Transitions.Components.Screen.AbstractClasses;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps.AbstractClasses;
using UnityEngine;

namespace FlipWebApps.BeautifulTransitions.Scripts.Transitions.Components.Screen
{
    [AddComponentMenu("Beautiful Transitions/Screen/Fade")]
    [HelpURL("http://www.flipwebapps.com/beautiful-transitions/")]
    [ExecuteInEditMode]
    public class FadeScreen : TransitionScreenBase
    {
        [Header("Fade Specific")]
        public InSettings InConfig;
        public OutSettings OutConfig;


        #region TransitionBase Overrides

        /// <summary>
        /// Get an instance of the current transition item
        /// </summary>
        /// <returns></returns>
        public override TransitionStep CreateTransitionStep()
        {
            return new ScreenFade(gameObject);
        }

        /// <summary>
        /// Add common values to the transitionStep for the in transition
        /// </summary>
        /// <param name="transitionStep"></param>
        public override void SetupTransitionStepIn(TransitionStep transitionStep)
        {
            var transitionStepScreenFade = transitionStep as ScreenFade;
            if (transitionStepScreenFade != null)
            {
                transitionStepScreenFade.Color = InConfig.Color;
                transitionStepScreenFade.Texture = InConfig.Texture;
            }
            base.SetupTransitionStepIn(transitionStep);
        }

        /// <summary>
        /// Add common values to the transitionStep for the out transition
        /// </summary>
        /// <param name="transitionStep"></param>
        public override void SetupTransitionStepOut(TransitionStep transitionStep)
        {
            var transitionStepScreenFade = transitionStep as ScreenFade;
            if (transitionStepScreenFade != null)
            {
                transitionStepScreenFade.Color = OutConfig.Color;
                transitionStepScreenFade.Texture = OutConfig.Texture;
                transitionStepScreenFade.SceneChangeMode = OutConfig.SceneChangeMode;
                transitionStepScreenFade.SceneToLoad = OutConfig.SceneToLoad;
            }
            base.SetupTransitionStepOut(transitionStep);
        }

        #endregion TransitionBase Overrides

        #region Transition specific settings

        [System.Serializable]
        public class InSettings
        {
            [Tooltip("Optional overlay texture to use.")]
            public Texture2D Texture;
            [Tooltip("Tint color.")]
            public Color Color = Color.black;
            [Tooltip("Skip running this if there is already a cross transition in progress. Useful for e.g. your entry scene where on first run you enter directly (running this transition), but might later cross transition to from another scene and so not want this transition to run.")]
            public bool SkipOnCrossTransition = true;
        }

        [System.Serializable]
        public class OutSettings
        {
            [Tooltip("Optional overlay texture to use.")]
            public Texture2D Texture;
            [Tooltip("Tint color.")]
            public Color Color = Color.black;
            [Tooltip("Whether and how to transition to a new scene.")]
            public TransitionStepScreen.SceneChangeModeType SceneChangeMode;
            [Tooltip("If transitioning to a new scene then the name of the scene to transition to.")]
            public string SceneToLoad;
        }

        #endregion Transition specific settings
    }
}
