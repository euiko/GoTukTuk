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
using UnityEngine;

namespace FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps.AbstractClasses
{
    /// <summary>
    /// An abstract class for a transition step that is based upon a value of some sort
    /// </summary>
    public abstract class TransitionStepValue<T> : TransitionStep where T : struct  {
        public T StartValue { get; set; }
        public T EndValue { get; set; }

        public T Value { get; set; }

        public T OriginalValue { get; set; }

        #region Constructors

        public TransitionStepValue(GameObject target = null, float delay = 0, float duration = 0.5F, TransitionModeType transitionMode = TransitionModeType.Specified, TransitionHelper.TweenType tweenType = TransitionHelper.TweenType.linear, AnimationCurve animationCurve = null, CoordinateSpaceType coordinateSpace = CoordinateSpaceType.Global, Action onStart = null, Action<float> onUpdate = null, Action onComplete = null) : 
            base(target, delay, duration, transitionMode, tweenType, animationCurve, coordinateSpace, onStart, onUpdate, onComplete)
        {
        }

        #endregion Constructors

        /// <summary>
        /// Get the current value. Override this in a subclasses to provide a value specific to that item
        /// </summary>
        /// <returns></returns>
        public virtual T GetCurrent()
        {
            return default(T);
        }


        /// <summary>
        /// Set the current value. Override this in a subclasses to set a value specific to that item
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetCurrent(T value)
        {
        }
    }
}
