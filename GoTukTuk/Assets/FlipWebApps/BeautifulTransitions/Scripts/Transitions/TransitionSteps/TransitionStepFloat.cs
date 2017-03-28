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

namespace FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps
{
    /// <summary>
    /// A transition step that is based upon a float value
    /// </summary>
    public class TransitionStepFloat : TransitionStepValue<float>
    {

        #region Constructors

        public TransitionStepFloat(UnityEngine.GameObject target = null,
            float? startValue = null,
            float? endValue = null,
            float delay = 0,
            float duration = 0.5f,
            TransitionModeType transitionMode = TransitionModeType.Specified,
            TransitionHelper.TweenType tweenType = TransitionHelper.TweenType.linear,
            AnimationCurve animationCurve = null,
            CoordinateSpaceType coordinateSpace = CoordinateSpaceType.Global,
            Action onStart = null,
            Action<float> onUpdate = null,
            Action onComplete = null) :
            base(target, delay, duration, transitionMode, tweenType, animationCurve, coordinateSpace, onStart, onUpdate, onComplete)
        {
            StartValue = startValue.GetValueOrDefault();
            EndValue = endValue.GetValueOrDefault();
            OriginalValue = GetCurrent();
        }

        #endregion Constructors

        /// <summary>
        /// Set the start value for when progress is 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        TransitionStep SetStartValue(float value)
        {
            StartValue = value;
            return this;
        }


        /// <summary>
        /// Set the end value for when progress is 1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        TransitionStep SetEndValue(float value)
        {
            EndValue = value;
            return this;
        }

        #region TransitionStep Overrides

        /// <summary>
        /// Override for start to set values based upon the mode.
        /// </summary>
        public override void Start()
        {
            if (TransitionMode == TransitionModeType.FromCurrent)
                StartValue = GetCurrent();
            else if (TransitionMode == TransitionModeType.ToOriginal)
                EndValue = OriginalValue;

            base.Start();
        }


        /// <summary>
        /// Override this if you need to update based upon the progress (0..1)
        /// </summary>
        /// <param name="progress"></param>
        public override void ProgressUpdated(float progress)
        {
            Value = EaseValue(StartValue, EndValue, progress);
            SetCurrent(Value);
        }

        #endregion TransitionStep Overrides

    }
}
