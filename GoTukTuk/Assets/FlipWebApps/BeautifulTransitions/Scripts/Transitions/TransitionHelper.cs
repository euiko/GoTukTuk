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

using FlipWebApps.BeautifulTransitions.Scripts.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.Components;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps;
using UnityEngine;
using UnityEngine.Assertions;

namespace FlipWebApps.BeautifulTransitions.Scripts.Transitions
{
    /// <summary>
    /// Contains various helper methods and functions for processing and handling transitions
    /// </summary>
    [HelpURL("http://www.flipwebapps.com/beautiful-transitions/")]
    public class TransitionHelper {
        #region Tween related
        /// <summary>
        /// The tween types that we have available
        /// </summary>
        public enum TweenType
        {
            none,
            easeInQuad,
            easeOutQuad,
            easeInOutQuad,
            easeInCubic,
            easeOutCubic,
            easeInOutCubic,
            easeInQuart,
            easeOutQuart,
            easeInOutQuart,
            easeInQuint,
            easeOutQuint,
            easeInOutQuint,
            easeInSine,
            easeOutSine,
            easeInOutSine,
            easeInExpo,
            easeOutExpo,
            easeInOutExpo,
            easeInCirc,
            easeOutCirc,
            easeInOutCirc,
            linear,
            spring,
            easeInBounce,
            easeOutBounce,
            easeInOutBounce,
            easeInBack,
            easeOutBack,
            easeInOutBack,
            easeInElastic,
            easeOutElastic,
            easeInOutElastic,
            AnimationCurve = 999
        }


        /// <summary>
        /// Get an instance of the given tween function
        /// </summary>
        /// <param name="progressMode"></param>
        /// <returns></returns>
        public static TweenMethods.TweenFunction GetTweenFunction(TweenType progressMode)
        {
            TweenMethods.TweenFunction tweenFunction = null;
            switch (progressMode)
            {
                case TweenType.easeInQuad:
                    tweenFunction = TweenMethods.easeInQuad;
                    break;
                case TweenType.easeOutQuad:
                    tweenFunction = TweenMethods.easeOutQuad;
                    break;
                case TweenType.easeInOutQuad:
                    tweenFunction = TweenMethods.easeInOutQuad;
                    break;
                case TweenType.easeInCubic:
                    tweenFunction = TweenMethods.easeInCubic;
                    break;
                case TweenType.easeOutCubic:
                    tweenFunction = TweenMethods.easeOutCubic;
                    break;
                case TweenType.easeInOutCubic:
                    tweenFunction = TweenMethods.easeInOutCubic;
                    break;
                case TweenType.easeInQuart:
                    tweenFunction = TweenMethods.easeInQuart;
                    break;
                case TweenType.easeOutQuart:
                    tweenFunction = TweenMethods.easeOutQuart;
                    break;
                case TweenType.easeInOutQuart:
                    tweenFunction = TweenMethods.easeInOutQuart;
                    break;
                case TweenType.easeInQuint:
                    tweenFunction = TweenMethods.easeInQuint;
                    break;
                case TweenType.easeOutQuint:
                    tweenFunction = TweenMethods.easeOutQuint;
                    break;
                case TweenType.easeInOutQuint:
                    tweenFunction = TweenMethods.easeInOutQuint;
                    break;
                case TweenType.easeInSine:
                    tweenFunction = TweenMethods.easeInSine;
                    break;
                case TweenType.easeOutSine:
                    tweenFunction = TweenMethods.easeOutSine;
                    break;
                case TweenType.easeInOutSine:
                    tweenFunction = TweenMethods.easeInOutSine;
                    break;
                case TweenType.easeInExpo:
                    tweenFunction = TweenMethods.easeInExpo;
                    break;
                case TweenType.easeOutExpo:
                    tweenFunction = TweenMethods.easeOutExpo;
                    break;
                case TweenType.easeInOutExpo:
                    tweenFunction = TweenMethods.easeInOutExpo;
                    break;
                case TweenType.easeInCirc:
                    tweenFunction = TweenMethods.easeInCirc;
                    break;
                case TweenType.easeOutCirc:
                    tweenFunction = TweenMethods.easeOutCirc;
                    break;
                case TweenType.easeInOutCirc:
                    tweenFunction = TweenMethods.easeInOutCirc;
                    break;
                case TweenType.linear:
                    tweenFunction = TweenMethods.linear;
                    break;
                case TweenType.spring:
                    tweenFunction = TweenMethods.spring;
                    break;
                /* GFX47 MOD START */
                /*case TransitionHelper.TweenType.bounce:
			ease = new EasingFunction(bounce);
			break;*/
                case TweenType.easeInBounce:
                    tweenFunction = TweenMethods.easeInBounce;
                    break;
                case TweenType.easeOutBounce:
                    tweenFunction = TweenMethods.easeOutBounce;
                    break;
                case TweenType.easeInOutBounce:
                    tweenFunction = TweenMethods.easeInOutBounce;
                    break;
                /* GFX47 MOD END */
                case TweenType.easeInBack:
                    tweenFunction = TweenMethods.easeInBack;
                    break;
                case TweenType.easeOutBack:
                    tweenFunction = TweenMethods.easeOutBack;
                    break;
                case TweenType.easeInOutBack:
                    tweenFunction = TweenMethods.easeInOutBack;
                    break;
                /* GFX47 MOD START */
                /*case TransitionHelper.TweenType.elastic:
			ease = new EasingFunction(elastic);
			break;*/
                case TweenType.easeInElastic:
                    tweenFunction = TweenMethods.easeInElastic;
                    break;
                case TweenType.easeOutElastic:
                    tweenFunction = TweenMethods.easeOutElastic;
                    break;
                case TweenType.easeInOutElastic:
                    tweenFunction = TweenMethods.easeInOutElastic;
                    break;
                    /* GFX47 MOD END */
            }
            return tweenFunction;
        }

        #endregion Tween related

        #region Transition related

        /// <summary>
        /// Returns whether the specified gameobject contains any transitions.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static bool ContainsTransition(UnityEngine.GameObject gameObject)
        {
            var transitionBases = gameObject.GetComponents<TransitionBase>();
            return transitionBases.Length != 0;
        }


        /// <summary>
        /// Find and transition in any transitions contained on or as a child of the specified gameobject. Depending on 
        /// the TransitionChildren and MustTriggerDirect configuration, any further children of these transitions 
        /// will also be triggered.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="onComplete"></param>
        /// <returns>Returns a list of transitions that were triggered.</returns>
        public static List<TransitionBase> TransitionIn(UnityEngine.GameObject gameObject, Action onComplete = null)
        {
            var transitionBases = TransitionIn(gameObject, false);
            if (onComplete != null && transitionBases.Count > 0)
                TransitionController.Instance.StartCoroutine(CallActionAfterDelay(GetTransitionInTime(transitionBases), onComplete));
            return transitionBases;
        }


        static List<TransitionBase> TransitionIn(UnityEngine.GameObject gameObject, bool isRecursiveCall) {
            var transitionBases = gameObject.GetComponents<TransitionBase>();
            var transitionList = new List<TransitionBase>();
            var callRecursive = false;

            // transition in transition items.
            foreach (var transitionBase in transitionBases) {
                // if first invoked on this gameobject, or don't need to trigger direct transition direct.
                if (transitionBase.isActiveAndEnabled && (isRecursiveCall == false || !transitionBase.TransitionInConfig.MustTriggerDirect)) {  
                    transitionBase.TransitionIn();
                    transitionList.Add(transitionBase);
                    // if we should transition children then set recursive flag
                    if (transitionBase.TransitionInConfig.TransitionChildren)                           
                        callRecursive = true;
                }
            }

            // if no transition items, or recursive call then process all child gameobjects
            if (transitionBases.Length == 0 || callRecursive)
            {
                for (var i = 0; i < gameObject.transform.childCount; i++)
                {
                    var transform = gameObject.transform.GetChild(i);
                    transitionList.AddRange(TransitionIn(transform.gameObject, true));
                }
            }

            return transitionList;
        }


        /// <summary>
        /// Find and transition out any transitions contained on or as a child of the specified gameobject. Depending on 
        /// the TransitionChildren and MustTriggerDirect configuration, any further children of these transitions 
        /// will also be triggered.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="onComplete"></param>
        /// <returns>Returns a list of transitions that were triggered.</returns>
        public static List<TransitionBase> TransitionOut(UnityEngine.GameObject gameObject, Action onComplete = null)
        {
            var transitionBases = TransitionOut(gameObject, false);
            if (onComplete != null && transitionBases.Count > 0)
                TransitionController.Instance.StartCoroutine(CallActionAfterDelay(GetTransitionOutTime(transitionBases), onComplete));
            return transitionBases;
        }


        static List<TransitionBase> TransitionOut(UnityEngine.GameObject gameObject, bool isRecursiveCall) {
            var transitionBases = gameObject.GetComponents<TransitionBase>();
            var transitionList = new List<TransitionBase>();
            var callRecursive = false;

            // transition out transition items.
            foreach (var transitionBase in transitionBases)
            {
                // if first invoked on this gameobject, or don't need to trigger direct transition direct.
                if (transitionBase.isActiveAndEnabled && (isRecursiveCall == false || !transitionBase.TransitionOutConfig.MustTriggerDirect))
                {
                    transitionBase.TransitionOut();
                    transitionList.Add(transitionBase);
                    // if we should transition children then set recursive flag
                    if (transitionBase.TransitionOutConfig.TransitionChildren)
                        callRecursive = true;
                }
            }

            // if no transition items, or recursive call then process all child gameobjects
            if (transitionBases.Length == 0 || callRecursive)
            {
                for (var i = 0; i < gameObject.transform.childCount; i++)
                {
                    var transform = gameObject.transform.GetChild(i);
                    transitionList.AddRange(TransitionOut(transform.gameObject, true));
                }
            }

            return transitionList;
        }


        /// <summary>
        /// Get the amount of time that the specified transitions will take for a transition in.
        /// </summary>
        /// <param name="transitionBases"></param>
        /// <returns></returns>
        public static float GetTransitionInTime(List<TransitionBase> transitionBases)
        {
            float transitionTime = 0;
            foreach (var transitionBase in transitionBases)
                transitionTime = Mathf.Max(transitionTime, transitionBase.TransitionInConfig.Delay + transitionBase.TransitionInConfig.Duration);
            return transitionTime;
        }


        /// <summary>
        /// Get the amount of time that the specified transitions will take for a transition out.
        /// </summary>
        /// <param name="transitionBases"></param>
        /// <returns></returns>
        public static float GetTransitionOutTime(List<TransitionBase> transitionBases)
        {
            float transitionTime = 0;
            foreach (var transitionBase in transitionBases)
                transitionTime = Mathf.Max(transitionTime, transitionBase.TransitionOutConfig.Delay + transitionBase.TransitionOutConfig.Duration);
            return transitionTime;
        }

        #endregion Transition related

        #region General Helper Functions

        /// <summary>
        /// Coroutine to call a specified action after a delay.
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerator CallActionAfterDelay(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action();
        }


        /// <summary>
        /// Get a screenshot of the current display
        /// </summary>
        /// <returns></returns>
        public static Texture2D TakeScreenshot()
        {
            var screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false, false);
            screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
            screenshot.Apply();

            return screenshot;
        }


        /// <summary>
        /// Load the specified scene (compatible with Unity 5+)
        /// </summary>
        /// <param name="sceneName"></param>
        public static void LoadScene(string sceneName)
        {
            Assert.IsFalse(string.IsNullOrEmpty(sceneName), "SceneName must be specified.");
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			Application.LoadLevel(sceneName);
#else
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
#endif
        }
        #endregion General Helper Functions


        //void SetActiveAnimated(GameObject gameObject, bool active)
        //{
        //    var animator = gameObject.GetComponent<Animator>();
        //    if (animator != null)
        //    {
        //        animator.SetBool("Active", active);
        //    }
        //    gameObject.SetActive(active);
        //}


        //void SetActiveImmediate(GameObject gameObject, bool active)
        //{
        //    var animator = gameObject.GetComponent<Animator>();
        //    var layerIndex = animator.GetLayerIndex("Active");
        //    if (animator != null)
        //    {
        //        animator.Play(active ? "Active" : "Inactive", layerIndex, 1);
        //    }
        //    gameObject.SetActive(active);
        //}
    }
}
