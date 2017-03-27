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

namespace FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps
{
    /// <summary>
    /// Transition step for triggering an animation.
    /// </summary>
    public class TriggerAnimation : TransitionStepFloat {
        public float Speed { get; set; }

        public Animator Animator { get; set; }

        public string Trigger { get; set; }
        public string TargetState { get; set; }

        #region Constructors

        public TriggerAnimation(UnityEngine.GameObject target,
            float speed = 1,
            float delay = 0,
            float duration = 0.5f,
            string trigger = "TransitionIn",
            string targetState = "TransitionOut",
            Action onStart = null,
            Action<float> onUpdate = null,
            Action onComplete = null) :
                base(target, delay: delay, duration: duration, onStart: onStart,onUpdate: onUpdate, onComplete: onComplete)
        {
            SetupComponentReferences();
            Animator.enabled = false;

            Speed = speed;
            Trigger = trigger;
            TargetState = targetState;
        }

        #endregion Constructors

        #region TransitionStep Overrides

        /// <summary>
        /// Custom Coroutine to handle the transition - needed due to additional yield statements in the main loop
        /// </summary>
        protected override IEnumerator TransitionLoop()
        {
            // if delay and duration are both zero then just set to end state, otherwise set to start and transition
            if (Mathf.Approximately(Delay + Duration, 0))
            {
                SetProgressToEnd();
                TransitionStarted();
            }
            else
            {
                SetProgressToStart();
                TransitionStarted();

                // delay
                if (!Mathf.Approximately(Delay, 0)) yield return new WaitForSeconds(Delay);

                Animator.enabled = true;
                Animator.SetTrigger(Trigger);
                Animator.speed = Speed;

                //TODO this assumes that we don't interrupt the transition or animation - can get problems otherwise.
                var stateReached = false;
                while (!stateReached)
                {
                    yield return new WaitForEndOfFrame();
                    if (!Animator.IsInTransition(0))
                        stateReached = Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 ||
                                       !Animator.GetCurrentAnimatorStateInfo(0).IsName(TargetState);
                }

                // if we completed and weren't stopped
                if (Mathf.Approximately(Progress, 1) && !IsStopped)
                {
                    TransitionCompleted();
                }
            }
        }

        #endregion TransitionStep Overrides

        /// <summary>
        /// Get component references
        /// </summary>
        void SetupComponentReferences()
        {
            Animator = Target.GetComponent<Animator>();
            Assert.IsNotNull(Animator, "Ensure that there is an Animator on the gameobject used by TransitionAnimation");
        }

    }

    #region TransitionStep extensions

    public static class AnimationExtensions
    {
        /// <summary>
        /// Animation extension method for TransitionStep
        /// </summary>
        /// <typeparam name="T">interface type</typeparam>
        /// <param name="TransitionStep"></param>
        /// <returns></returns>
        public static TriggerAnimation TriggerAnimation(this TransitionStep transitionStep, 
            float speed = 1,
            float delay = 0,
            float duration = 0.5f,
            string trigger = "TransitionIn",
            string targetState = "TransitionOut",
            bool runAtStart = false,
            Action onStart = null,
            Action<float> onUpdate = null,
            Action onComplete = null)
        {
            var newTransitionStep = new TriggerAnimation(transitionStep.Target, 
                speed,
                delay,
                duration,
                trigger,
                targetState,
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
    }

    #endregion TransitionStep extensions
}
