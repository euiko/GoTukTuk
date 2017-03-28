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
using System.Linq;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps.AbstractClasses;
using UnityEngine;
using UnityEngine.UI;

namespace FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps
{
    /// <summary>
    /// Transition step for fading a canvas group, image or text.
    /// </summary>
    public class Fade : TransitionStepFloat {

        CanvasGroup[] _canvasGroups = new CanvasGroup[0];
        Image[] _images = new Image[0];
        Text[] _texts = new Text[0];
        SpriteRenderer[] _spriteRenderers = new SpriteRenderer[0];

        bool _hasComponentReferences;

        #region Constructors

        public Fade(UnityEngine.GameObject target,
            float startTransparency = 0,
            float endTransparency = 1,
            float delay = 0,
            float duration = 0.5f,
            TransitionModeType transitionMode = TransitionModeType.Specified,
            TransitionHelper.TweenType tweenType = TransitionHelper.TweenType.linear,
            AnimationCurve animationCurve = null,
            Action onStart = null,
            Action<float> onUpdate = null,
            Action onComplete = null) :
                base(target, startValue: startTransparency, endValue: endTransparency, 
                    delay: delay, duration: duration, transitionMode: transitionMode, tweenType: tweenType,
                    animationCurve: animationCurve, onStart: onStart,onUpdate: onUpdate, onComplete: onComplete)
        {
        }

        #endregion Constructors

        #region TransitionStepValue Overrides

        /// <summary>
        /// Get the current transparency level.
        /// </summary>
        /// <returns></returns>
        public override float GetCurrent()
        {
            if (!_hasComponentReferences)
                SetupComponentReferences();

            if (_canvasGroups.Length > 0)
                return _canvasGroups[0].alpha;
            if (_images.Length > 0)
                return _images[0].color.a;
            if (_texts.Length > 0)
                return _texts[0].color.a;
            if (_spriteRenderers.Length > 0)
                return _spriteRenderers[0].color.a;

            return 1;
        }

        /// <summary>
        /// Set the current transparency level
        /// </summary>
        /// <param name="transparency"></param>
        public override void SetCurrent(float transparency)
        {
            if (!_hasComponentReferences)
                SetupComponentReferences();
            foreach (var canvas in _canvasGroups)
                canvas.alpha = transparency;
            foreach (var image in _images)
                image.color = new Color(image.color.r, image.color.g, image.color.b, transparency);
            foreach (var text in _texts)
                text.color = new Color(text.color.r, text.color.g, text.color.b, transparency);
            foreach (var spriteRenderer in _spriteRenderers)
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, transparency);
        }

        #endregion TransitionStepValue Overrides

        /// <summary>
        /// Get component references
        /// </summary>
        void SetupComponentReferences()
        {
            _canvasGroups = new CanvasGroup[0];
            _images = new Image[0];
            _texts = new Text[0];
            _spriteRenderers = new SpriteRenderer[0];
            // get the components to work on target
            var canvasGroup = Target.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                _canvasGroups = _canvasGroups.Concat(Enumerable.Repeat(canvasGroup, 1)).ToArray();
            }
            else
            {
                var image = Target.GetComponent<Image>();
                if (image != null)
                    _images = _images.Concat(Enumerable.Repeat(image, 1)).ToArray();

                var text = Target.GetComponent<Text>();
                if (text != null)
                    _texts = _texts.Concat(Enumerable.Repeat(text, 1)).ToArray();
            }
            var spriteRenderer = Target.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                _spriteRenderers = _spriteRenderers.Concat(Enumerable.Repeat(spriteRenderer, 1)).ToArray();

            _hasComponentReferences = true;
        }
    }

    #region TransitionStep extensions

    public static class FadeExtensions
    {
        /// <summary>
        /// Fade extension method for TransitionStep
        /// </summary>
        /// <typeparam name="T">interface type</typeparam>
        /// <param name="TransitionStep"></param>
        /// <returns></returns>
        public static Fade Fade(this TransitionStep transitionStep,
            float startTransparency,
            float endTransparency,
            float delay = 0,
            float duration = 0.5f,
            TransitionStep.TransitionModeType transitionMode = TransitionStep.TransitionModeType.Specified,
            TransitionHelper.TweenType tweenType = TransitionHelper.TweenType.linear,
            AnimationCurve animationCurve = null,
            bool runAtStart = false,
            Action onStart = null,
            Action<float> onUpdate = null,
            Action onComplete = null)
        {
            var newTransitionStep = new Fade(transitionStep.Target,
                startTransparency,
                endTransparency,
                delay,
                duration,
                transitionMode,
                tweenType,
                animationCurve,
                onStart,
                onUpdate,
                onComplete);
            if (runAtStart)
                transitionStep.AddOnStartTransitionStep(newTransitionStep);
            else
                transitionStep.AddOnCompleteTransitionStep(newTransitionStep);
            newTransitionStep.Parent = transitionStep;
            return newTransitionStep;
        }

        /// <summary>
        /// Fade extension method for TransitionStep
        /// </summary>
        /// <typeparam name="T">interface type</typeparam>
        /// <param name="TransitionStep"></param>
        /// <returns></returns>
        public static Fade FadeToOriginal(this TransitionStep transitionStep,
            float startTransparency,
            float delay = 0,
            float duration = 0.5f,
            TransitionHelper.TweenType tweenType = TransitionHelper.TweenType.linear,
            AnimationCurve animationCurve = null,
            bool runAtStart = false,
            Action onStart = null,
            Action<float> onUpdate = null,
            Action onComplete = null)
        {
            var newTransitionStep = new Fade(transitionStep.Target,
                startTransparency,
                0,
                delay,
                duration,
                TransitionStep.TransitionModeType.ToOriginal,
                tweenType,
                animationCurve,
                onStart,
                onUpdate,
                onComplete);
            if (runAtStart)
                transitionStep.AddOnStartTransitionStep(newTransitionStep);
            else
                transitionStep.AddOnCompleteTransitionStep(newTransitionStep);
            newTransitionStep.Parent = transitionStep;
            newTransitionStep.EndValue = newTransitionStep.OriginalValue;
            return newTransitionStep;
        }

        /// <summary>
        /// Fade extension method for TransitionStep
        /// </summary>
        /// <typeparam name="T">interface type</typeparam>
        /// <param name="TransitionStep"></param>
        /// <returns></returns>
        public static Fade FadeFromOriginal(this TransitionStep transitionStep,
            float endTransparency,
            float delay = 0,
            float duration = 0.5f,
            TransitionHelper.TweenType tweenType = TransitionHelper.TweenType.linear,
            AnimationCurve animationCurve = null,
            bool runAtStart = false,
            Action onStart = null,
            Action<float> onUpdate = null,
            Action onComplete = null)
        {
            var newTransitionStep = new Fade(transitionStep.Target,
                0,
                endTransparency,
                delay,
                duration,
                TransitionStep.TransitionModeType.FromCurrent,
                tweenType,
                animationCurve,
                onStart,
                onUpdate,
                onComplete);
            if (runAtStart)
                transitionStep.AddOnStartTransitionStep(newTransitionStep);
            else
                transitionStep.AddOnCompleteTransitionStep(newTransitionStep);
            newTransitionStep.Parent = transitionStep;
            newTransitionStep.StartValue = newTransitionStep.OriginalValue;
            return newTransitionStep;
        }
    }
    #endregion TransitionStep extensions
}
