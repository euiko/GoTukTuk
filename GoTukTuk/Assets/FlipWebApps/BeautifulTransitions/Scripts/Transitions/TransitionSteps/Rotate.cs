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

// The above copyright notice and this permission notice must not be reRotated from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//----------------------------------------------

using System;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps.AbstractClasses;
using UnityEngine;
using UnityEngine.Assertions;

namespace FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps
{
    /// <summary>
    /// Transition step for rotating a gameobject.
    /// </summary>
    public class Rotate : TransitionStepVector3 {

        #region Constructors

        public Rotate(UnityEngine.GameObject target,
            Vector3? startRotation = null,
            Vector3? endRotation = null,
            float delay = 0,
            float duration = 0.5f,
            TransitionModeType transitionMode = TransitionModeType.Specified,
            TransitionHelper.TweenType tweenType = TransitionHelper.TweenType.linear,
            AnimationCurve animationCurve = null,
            TransitionStep.CoordinateSpaceType coordinateSpace = TransitionStep.CoordinateSpaceType.Global,
            Action onStart = null,
            Action<float> onUpdate = null,
            Action onComplete = null) :
                base(target, startRotation, endRotation, delay: delay, duration: duration, transitionMode: transitionMode, tweenType: tweenType,
                animationCurve: animationCurve, coordinateSpace: coordinateSpace, onStart: onStart, onUpdate: onUpdate, onComplete: onComplete)
        {
            Assert.AreNotEqual(CoordinateSpaceType.AnchoredPosition, CoordinateSpace, "AnchoredPosition is not supported for Rotate. Please change");
        }

        #endregion Constructors

        #region TransitionStepValue Overrides

        /// <summary>
        /// Get the current rotation based upon the current CoordinateMode
        /// </summary>
        /// <returns></returns>
        public override Vector3 GetCurrent()
        {
            if (CoordinateSpace == CoordinateSpaceType.Global)
                return Target.transform.eulerAngles;
            else //if (CoordinateMode == CoordinateModeType.Local)
                return Target.transform.localEulerAngles;
        }

        /// <summary>
        /// Set the current rotation based upon the current CoordinateMode
        /// </summary>
        /// <returns></returns>
        public override void SetCurrent(Vector3 rotation)
        {
            if (CoordinateSpace == CoordinateSpaceType.Global)
                Target.transform.eulerAngles = rotation;
            else //if (CoordinateMode == CoordinateModeType.Local)
                Target.transform.localEulerAngles = rotation;
        }

        #endregion TransitionStepValue Overrides

    }


    #region TransitionStep extensions

    public static class RotateExtensions
    {
        /// <summary>
        /// Rotate extension method for TransitionStep
        /// </summary>
        /// <typeparam name="T">interface type</typeparam>
        /// <param name="TransitionStep"></param>
        /// <returns></returns>
        public static Rotate Rotate(this TransitionStep transitionStep,
            Vector3 startRotation,
            Vector3 endRotation,
            float delay = 0,
            float duration = 0.5f,
            TransitionStep.TransitionModeType transitionMode = TransitionStep.TransitionModeType.Specified,
            TransitionHelper.TweenType tweenType = TransitionHelper.TweenType.linear,
            AnimationCurve animationCurve = null,
            TransitionStep.CoordinateSpaceType coordinateMode = TransitionStep.CoordinateSpaceType.Global,
            bool runAtStart = false,
            Action onStart = null,
            Action<float> onUpdate = null,
            Action onComplete = null)
        {
            var newTransitionStep = new Rotate(transitionStep.Target,
                startRotation,
                endRotation,
                delay,
                duration,
                transitionMode,
                tweenType,
                animationCurve,
                coordinateMode,
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
        /// Rotate extension method for TransitionStep
        /// </summary>
        /// <typeparam name="T">interface type</typeparam>
        /// <param name="TransitionStep"></param>
        /// <returns></returns>
        public static Rotate RotateToOriginal(this TransitionStep transitionStep,
            Vector3 startRotation,
            float delay = 0,
            float duration = 0.5f,
            TransitionHelper.TweenType tweenType = TransitionHelper.TweenType.linear,
            AnimationCurve animationCurve = null,
            TransitionStep.CoordinateSpaceType coordinateMode = TransitionStep.CoordinateSpaceType.Global,
            bool runAtStart = false,
            Action onStart = null,
            Action<float> onUpdate = null,
            Action onComplete = null)
        {
            var newTransitionStep = new Rotate(transitionStep.Target,
                startRotation,
                Vector3.zero,
                delay,
                duration,
                TransitionStep.TransitionModeType.ToOriginal,
                tweenType,
                animationCurve,
                coordinateMode,
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
        /// Rotate extension method for TransitionStep
        /// </summary>
        /// <typeparam name="T">interface type</typeparam>
        /// <param name="TransitionStep"></param>
        /// <returns></returns>
        public static Rotate RotateFromOriginal(this TransitionStep transitionStep,
            Vector3 endRotation,
            float delay = 0,
            float duration = 0.5f,
            TransitionHelper.TweenType tweenType = TransitionHelper.TweenType.linear,
            AnimationCurve animationCurve = null,
            TransitionStep.CoordinateSpaceType coordinateMode = TransitionStep.CoordinateSpaceType.Global,
            bool runAtStart = false,
            Action onStart = null,
            Action<float> onUpdate = null,
            Action onComplete = null)
        {
            var newTransitionStep = new Rotate(transitionStep.Target,
                Vector3.zero,
                endRotation,
                delay,
                duration,
                TransitionStep.TransitionModeType.FromCurrent,
                tweenType,
                animationCurve,
                coordinateMode,
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
