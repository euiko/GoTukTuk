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
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps.AbstractClasses;
using UnityEngine;
using UnityEngine.UI;

namespace FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps
{
    /// <summary>
    /// Transition step for fading the screen.
    /// </summary>
    public class ScreenFade : TransitionStepScreen {
        public Texture2D Texture;
        public Color Color;

        readonly ScreenFadeComponents _screenFadeComponents;

        #region Constructors

        public ScreenFade(UnityEngine.GameObject target,
            Color? color = null,
            Texture2D texture = null,
            float delay = 0,
            float duration = 0.5f,
            TransitionHelper.TweenType tweenType = TransitionHelper.TweenType.linear,
            AnimationCurve animationCurve = null,
            Action onStart = null,
            Action<float> onUpdate = null,
            Action onComplete = null) :
                base(target, delay: delay, duration: duration, tweenType: tweenType,
                animationCurve: animationCurve, onStart: onStart,onUpdate: onUpdate, onComplete: onComplete)
        {
            _screenFadeComponents = new ScreenFadeComponents { PersistantAcrossScenes = true };

            Color = color.HasValue ? color.Value : Color.white;
            Texture = texture;
        }

        #endregion Constructors

        #region TransitionStep Overrides

        /// <summary>
        /// Call to start the transition
        /// </summary>
        public override void Start()
        {
            SetConfiguration(Texture, Color);
            base.Start();
        }

        /// <summary>
        /// Set the current transparency level
        /// </summary>
        /// <param name="position"></param>
        public override void SetCurrent(float progress)
        {
            // return if editor and no attached RawImage
            //#if UNITY_EDITOR
            //            if (!Application.isPlaying && RawImage == null) return;
            //#endif

            TargetComponents().FadeRawImage.color = new Color(
            TargetComponents().FadeRawImage.color.r,
            TargetComponents().FadeRawImage.color.g,
            TargetComponents().FadeRawImage.color.b, Value);
        }

        #endregion TransitionStep Overrides

        void SetConfiguration(Texture2D texture, Color color)
        {
            TargetComponents().FadeRawImage.texture = texture;
            TargetComponents().FadeRawImage.color = color;
        }


        protected override void SetTransitionDisplayedState(bool isDisplayed)
        {
            if (SiblingRawImage != null)
                base.SetTransitionDisplayedState(isDisplayed);
            else
                TargetComponents().BaseGameObject.SetActive(isDisplayed);

            // special cross transition handling
            if (SceneChangeMode == SceneChangeModeType.CrossTransition)
            {
                if (isDisplayed)
                    TargetComponents().FadeRawImage.texture = TransitionController.Instance.ScreenSnapshot;
                else
                {
                    TargetComponents().DeleteComponents();
                }
            }
        }

        ScreenFadeComponents TargetComponents()
        {
            return
                SceneChangeMode == SceneChangeModeType.CrossTransition ?
                    _screenFadeComponents : TransitionController.Instance.SharedScreenFadeComponents;
        }
    }


    #region Component Setup and References

    /// <summary>
    /// Used for holding all references to components relating to a screen fade.
    /// </summary>
    public class ScreenFadeComponents
    {
        /// <summary>
        /// Whether any created objects should be persisted across scenes.
        /// </summary>
        public bool PersistantAcrossScenes { get; set; }

        /// <summary>
        /// Gameobject to use for screen wipe - automatically setup if it doesn't exist
        /// </summary>
        public GameObject BaseGameObject
        {
            get
            {
                if (_baseGameObject == null)
                    CreateComponents();
                return _baseGameObject;
            }
            private set { _baseGameObject = value; }
        }
        GameObject _baseGameObject;

        /// <summary>
        /// Raw Image to use for screen wipe - automatically setup if it doesn't exist
        /// </summary>
        public RawImage FadeRawImage
        {
            get
            {
                if (_fadeRawImage == null)
                    CreateComponents();
                return _fadeRawImage;
            }
            set { _fadeRawImage = value; }
        }
        RawImage _fadeRawImage;


        /// <summary>
        /// Create components that are used by the screen wipe transition.
        /// </summary>
        void CreateComponents()
        {
            BaseGameObject = new UnityEngine.GameObject("(Beautiful Transitions - ScreenFade");
            if (PersistantAcrossScenes)
                BaseGameObject.transform.SetParent(TransitionController.Instance.gameObject.transform);
            BaseGameObject.SetActive(false);

            var myCanvas = BaseGameObject.AddComponent<Canvas>();
            myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            myCanvas.sortingOrder = 999;

            FadeRawImage = BaseGameObject.AddComponent<RawImage>();
        }


        /// <summary>
        /// Destroy any created components to support this transition.
        /// </summary>
        public void DeleteComponents()
        {
            if (BaseGameObject != null)
                GameObject.Destroy(BaseGameObject);
        }
    }

    #endregion Component Setup and References

    #region TransitionStep extensions

    public static class ScreenFadeExtensions
    {
        /// <summary>
        /// Fade extension method for TransitionStep
        /// </summary>
        /// <typeparam name="T">interface type</typeparam>
        /// <param name="TransitionStep"></param>
        /// <returns></returns>
        public static ScreenFade ScreenFade(this TransitionStep transitionStep,
            Color? color = null,
            Texture2D texture = null,
            float delay = 0,
            float duration = 0.5f,
            TransitionHelper.TweenType tweenType = TransitionHelper.TweenType.linear,
            AnimationCurve animationCurve = null,
            Action onStart = null,
            Action<float> onUpdate = null,
            Action onComplete = null)
        {
            var newTransitionStep = new ScreenFade(transitionStep.Target,
                color,
                texture,
                delay,
                duration,
                tweenType,
                animationCurve,
                onStart,
                onUpdate,
                onComplete);
            transitionStep.AddOnCompleteTransitionStep(newTransitionStep);
            newTransitionStep.Parent = transitionStep;
            return newTransitionStep;
        }
    }
    #endregion TransitionStep extensions
}
