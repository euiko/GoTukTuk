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

using System;
using System.Collections;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps.AbstractClasses;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps
{
    /// <summary>
    /// A screen transition step base class
    /// </summary>
    public class TransitionStepScreen : TransitionStepFloat {
        public enum SceneChangeModeType { None, CrossTransition, End }

        protected RawImage SiblingRawImage { get; set; }

        /// <summary>
        /// Whether and how to transition to a new scene.
        /// </summary>
        public SceneChangeModeType SceneChangeMode { get; set; }

        /// <summary>
        /// If transitioning to a new scene then the name of the scene to transition to.
        /// </summary>
        public string SceneToLoad { get; set; }

        ///
        /// Skip running this if there is already a cross transition in progress. Useful for e.g. your entry 
        /// scene where on first run you enter directly (running this transition), but might later cross 
        /// transition to from another scene and so not want this transition to run.")]
        ///
        public bool SkipOnCrossTransition { get; set; }

        #region Constructors

        public TransitionStepScreen(UnityEngine.GameObject target,
            SceneChangeModeType sceneChangeMode = SceneChangeModeType.None,
            string sceneToLoad = null,
            bool skipOnCrossTransition = true,
            float delay = 0,
            float duration = 0.5f,
            TransitionHelper.TweenType tweenType = TransitionHelper.TweenType.linear,
            AnimationCurve animationCurve = null,
            Action onStart = null,
            Action<float> onUpdate = null,
            TransitionStep onCompleteItem = null,
            Action onComplete = null,
            Action<object> onCompleteWithData = null,
            object onCompleteData = null) :
                base(target, delay: delay, duration: duration, tweenType: tweenType,
                animationCurve: animationCurve, onStart: onStart,onUpdate: onUpdate, onComplete: onComplete)
        {
            SceneChangeMode = sceneChangeMode;
            SceneToLoad = sceneToLoad;
            SkipOnCrossTransition = skipOnCrossTransition;
            SetupComponents();
        }

        #endregion Constructors


        #region TransitionStep Overrides

        /// <summary>
        /// Coroutine to handle the transition delay.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator TransitionLoop()
        {
            // early exit if skipping when in cross transition
            if (SkipOnCrossTransition && TransitionController.Instance.IsInCrossTransition) yield break;

            // if delay and duration are both zero then just set to end state, otherwise set to start and transition
            if (Mathf.Approximately(Delay + Duration, 0))
            {
                SetProgressToEnd();
                TransitionStarted();
            }
            else
            {
                TransitionStarted();

                // delay
                if (!Mathf.Approximately(Delay, 0)) yield return new WaitForSeconds(Delay);

                if (SceneChangeMode == SceneChangeModeType.CrossTransition)
                {
                    TransitionController.Instance.IsInCrossTransition = true;
                    yield return
                        TransitionController.Instance.StartCoroutine(
                            TransitionController.Instance.TakeScreenshotCoroutine());
                    SetTransitionDisplayedState(true);
                    StartValue = 1;
                    EndValue = 0;
                    SetProgressToStart();
                    yield return
                        TransitionController.Instance.StartCoroutine(
                            TransitionController.Instance.LoadSceneAndWaitForLoad(SceneToLoad));
                }
                else
                {
                    SetTransitionDisplayedState(true);
                    SetProgressToStart();
                }

                // calculate normalised multiplication factor and avoid / by 0 error.
                var normalisedFactor = Mathf.Approximately(Duration, 0) ? float.MaxValue : (1/Duration);

                // repeat while progress is less than one.
                while (Progress < 1 && !IsStopped)
                {
                    // update progress if not paused
                    if (!IsPaused)
                    {
                        Progress += normalisedFactor*Time.deltaTime;
                        SetProgress(Progress);
                    }

                    yield return 0;
                }
            }

            // if we completed and weren't stopped
            if (Mathf.Approximately(Progress, 1) && !IsStopped)
            {
                TransitionCompleted();
            }
        }


        protected override void TransitionCompleted()
        {
            // Only disable the image if we have transitioned in.
            if (Mathf.Approximately(EndValue, 0))
                SetTransitionDisplayedState(false);
            base.TransitionCompleted();

            if (SceneChangeMode == SceneChangeModeType.End)
            {
                Assert.IsFalse(string.IsNullOrEmpty(SceneToLoad), "When setting SceneChangedMode to End you must enter the name of a scene to load.");
                TransitionHelper.LoadScene(SceneToLoad);
            }
            if (SceneChangeMode == SceneChangeModeType.CrossTransition)
            {
                TransitionController.Instance.IsInCrossTransition = false;
            }

        }

        #endregion TransitionStep Overrides

        /// <summary>
        /// Setup any component or references
        /// </summary>
        protected virtual void SetupComponents()
        {
            // see if we have our own RawImage, if not then we use the global one.
            SiblingRawImage = Target.GetComponent<RawImage>();
            if (SiblingRawImage != null)
            {
                SiblingRawImage.enabled = false;
            }
        }


        protected virtual void SetTransitionDisplayedState(bool isDisplayed)
        {
            if (SiblingRawImage != null)
                SiblingRawImage.enabled = isDisplayed;
        }
    }
}
