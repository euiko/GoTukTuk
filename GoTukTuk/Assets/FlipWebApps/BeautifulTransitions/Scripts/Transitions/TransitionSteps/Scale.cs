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

// The above copyright notice and this permission notice must not be reScaled from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//----------------------------------------------

using System;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps.AbstractClasses;
using UnityEngine;

namespace FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps
{
    /// <summary>
    /// Transition step for scaling a gameobject.
    /// </summary>
    public class Scale : TransitionStepVector3 {

        #region Constructors

        public Scale(UnityEngine.GameObject target,
            Vector3? startScale = null,
            Vector3? endScale = null,
            float delay = 0,
            float duration = 0.5f,
            TransitionModeType transitionMode = TransitionModeType.Specified,
            TransitionHelper.TweenType tweenType = TransitionHelper.TweenType.linear,
            AnimationCurve animationCurve = null,
            Action onStart = null,
            Action<float> onUpdate = null,
            Action onComplete = null) :
                base(target, startScale, endScale, delay: delay, duration: duration, transitionMode: transitionMode, tweenType: tweenType,
                animationCurve: animationCurve, onStart: onStart, onUpdate: onUpdate, onComplete: onComplete)
        {
        }

        #endregion Constructors

        #region TransitionStepValue Overrides

        /// <summary>
        /// Get the current scale
        /// </summary>
        /// <returns></returns>
        public override Vector3 GetCurrent()
        {
            return Target.transform.localScale;
        }

        /// <summary>
        /// Set the current scale
        /// </summary>
        /// <returns></returns>
        public override void SetCurrent(Vector3 scale)
        {
            Target.transform.localScale = scale;
        }

        #endregion TransitionStepValue Overrides

    }

    #region TransitionStep extensions

    public static class ScaleExtensions
    {
        /// <summary>
        /// Scale extension method for TransitionStep
        /// </summary>
        /// <typeparam name="T">interface type</typeparam>
        /// <param name="TransitionStep"></param>
        /// <returns></returns>
        public static Scale Scale(this TransitionStep transitionStep,
            Vector3 startScale,
            Vector3 endScale,
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
            var newTransitionStep = new Scale(transitionStep.Target,
                startScale,
                endScale,
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
        /// Scale extension method for TransitionStep
        /// </summary>
        /// <typeparam name="T">interface type</typeparam>
        /// <param name="TransitionStep"></param>
        /// <returns></returns>
        public static Scale ScaleToOriginal(this TransitionStep transitionStep,
            Vector3 startScale,
            float delay = 0,
            float duration = 0.5f,
            TransitionHelper.TweenType tweenType = TransitionHelper.TweenType.linear,
            AnimationCurve animationCurve = null,
            bool runAtStart = false,
            Action onStart = null,
            Action<float> onUpdate = null,
            Action onComplete = null)
        {
            var newTransitionStep = new Scale(transitionStep.Target,
                startScale,
                Vector3.zero,
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
        /// Scale extension method for TransitionStep
        /// </summary>
        /// <typeparam name="T">interface type</typeparam>
        /// <param name="TransitionStep"></param>
        /// <returns></returns>
        public static Scale ScaleFromOriginal(this TransitionStep transitionStep,
            Vector3 endScale,
            float delay = 0,
            float duration = 0.5f,
            TransitionHelper.TweenType tweenType = TransitionHelper.TweenType.linear,
            AnimationCurve animationCurve = null,
            bool runAtStart = false,
            Action onStart = null,
            Action<float> onUpdate = null,
            Action onComplete = null)
        {
            var newTransitionStep = new Scale(transitionStep.Target,
                Vector3.zero,
                endScale,
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
