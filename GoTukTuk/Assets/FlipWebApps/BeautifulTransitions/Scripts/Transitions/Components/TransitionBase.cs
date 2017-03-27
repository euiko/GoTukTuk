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

using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps.AbstractClasses;
using UnityEngine;
using UnityEngine.Events;

namespace FlipWebApps.BeautifulTransitions.Scripts.Transitions.Components
{
    /// <summary>
    /// Abstract base class for all transition components.
    /// </summary>
    public abstract class TransitionBase : MonoBehaviour {
        [Tooltip("Whether to set up ready for transitioning in.")]
        public bool InitForTransitionIn = true;
        [Tooltip("Whether to automatically run the transition in effect in the OnEnable state.")]
        public bool AutoRun;
        [Tooltip("Whether to repeat initialisation and /or auto run in subsequent enabling of the gameitem.")]
        public bool RepeatWhenEnabled;

        public TransitionSettings TransitionInConfig;
        public TransitionSettings TransitionOutConfig;

        public enum TransitionModeType {None, In, Out}
        public TransitionModeType TransitionMode { get; set; }

        public TransitionStep CurrentTransitionStep { get; set; }

        bool _isInitialStateSet;                // Whether the initial state is set


        /// <summary>
        /// initialisation and default auto run when RepeatWhenEnabled is set and a gameobject is reenabled
        /// </summary>
        /// add this as on option.
        public virtual void OnEnable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif

            // the first run setup is called from Start so here we test if the initial state has been setup. If so then we have been 
            // disabled and reenabled so optionally transition in again if RepeatWhenEnabled is set.
            if (_isInitialStateSet && RepeatWhenEnabled)
                Setup();
        }


        /// <summary>
        /// initialisation and default auto run
        /// </summary>
        public virtual void Start()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif

            Setup();
        }


        /// <summary>
        /// Setup routine for initialising and auto running the transition.
        /// </summary>
        private void Setup()
        {
            if (InitForTransitionIn || AutoRun)
            {
                InitTransitionIn();
            }

            if (AutoRun)
            {
                TransitionIn();
            }
        }

        /// <summary>
        /// Initialise for a transition in.
        /// </summary>
        public virtual void InitTransitionIn()
        {
            SetupInitialStateOnce();
            TransitionMode = TransitionModeType.In;
            CurrentTransitionStep = CreateTransitionStepIn();
            CurrentTransitionStep.SetProgressToStart();
        }


        /// <summary>
        /// Transition in.
        /// </summary>
        public virtual void TransitionIn()
        {
            SetupInitialStateOnce();
            InitTransitionIn();
            CurrentTransitionStep.Start();
        }


        /// <summary>
        /// Initialise for a transition out.
        /// </summary>
        public virtual void InitTransitionOut()
        {
            SetupInitialStateOnce();
            TransitionMode = TransitionModeType.Out;
            CurrentTransitionStep = CreateTransitionStepOut();
            CurrentTransitionStep.SetProgressToStart();
        }


        /// <summary>
        /// Transition out.
        /// </summary>
        public virtual void TransitionOut()
        {
            SetupInitialStateOnce();
            InitTransitionOut();
            CurrentTransitionStep.Start();
        }

        /// <summary>
        /// Override this if you need to do any specific action when the value is updated
        /// </summary>
        /// <param name="amount"></param>
        public virtual void ValueUpdated(float amount)
        {
        }

        /// <summary>
        /// Setup any initial state - performing this one time only
        /// </summary>
        /// Called at the start of any call to InitTransitionIn, TransitionIn, InitTransitionOut or TransitionOut
        void SetupInitialStateOnce()
        {
            if (_isInitialStateSet) return;

            _isInitialStateSet = true;
            SetupInitialState();
        }

        /// <summary>
        /// Override this if you need to do gather any initial state
        /// </summary>
        /// We can not rely on gathering the initial state during the Start or Awake methods as if someone triggers the 
        /// transition through code then we risk that Awake / Start is called after a transition has already begun. This
        /// risks that e.g. for move the initial position is incorrectly recorded as the transition in start position.
        /// 
        /// This method will be called one time only just at the start of the first call to InitTransitionIn, 
        /// TransitionIn, InitTransitionOut or TransitionOut
        public virtual void SetupInitialState()
        {
        }

        #region Transition Callbacks

        /// <summary>
        /// Called when an in transition is started
        /// </summary>
        public virtual void TransitionInStart()
        {
            if (TransitionInConfig.OnTransitionStart != null)
                TransitionInConfig.OnTransitionStart.Invoke();
        }


        /// <summary>
        /// Called when an out transition is started
        /// </summary>
        public virtual void TransitionOutStart()
        {
            if (TransitionOutConfig.OnTransitionStart != null)
                TransitionOutConfig.OnTransitionStart.Invoke();
        }

        /// <summary>
        /// Called when an in transition has been completed (or interupted)
        /// </summary>
        public virtual void TransitionInComplete()
        {
            TransitionMode = TransitionModeType.Out;
            if (TransitionInConfig.OnTransitionComplete != null)
                TransitionInConfig.OnTransitionComplete.Invoke();
        }


        /// <summary>
        /// Called when an out transition has been completed (or interupted)
        /// </summary>
        public virtual void TransitionOutComplete()
        {
            if (TransitionOutConfig.OnTransitionComplete != null)
                TransitionOutConfig.OnTransitionComplete.Invoke();
        }

        #endregion transition callbacks

        #region Create transitionStep

        /// <summary>
        /// Create a transitionStep. Implement this to create the correct subclass of transitionStep
        /// </summary>
        /// <returns></returns>
        public abstract TransitionStep CreateTransitionStep();

        /// <summary>
        /// Create a transitionStep for transitioning in and populate with configured values
        /// </summary>
        /// <returns></returns>
        public virtual TransitionStep CreateTransitionStepIn()
        {
            var transitionStep = CurrentTransitionStep ?? CreateTransitionStep();
            SetupTransitionStepIn(transitionStep);
            return transitionStep;
        }

        /// <summary>
        /// Add common values to the transitionStep for the in transition
        /// </summary>
        /// <param name="transitionStep"></param>
        public virtual void SetupTransitionStepIn(TransitionStep transitionStep)
        {
            transitionStep.Delay = TransitionInConfig.Delay;
            transitionStep.Duration = TransitionInConfig.Duration;
            transitionStep.TweenType = TransitionInConfig.TransitionType;
            transitionStep.AnimationCurve = TransitionInConfig.AnimationCurve;
            transitionStep.OnStart = TransitionInStart;
            transitionStep.OnComplete = TransitionInComplete;
            transitionStep.OnUpdate = ValueUpdated;
        }

        /// <summary>
        /// Create a transitionStep for transitioning out and populate with configured values
        /// </summary>
        /// <returns></returns>
        public virtual TransitionStep CreateTransitionStepOut()
        {
            var transitionStep = CurrentTransitionStep ?? CreateTransitionStep();
            SetupTransitionStepOut(transitionStep);
            return transitionStep;
        }

        /// <summary>
        /// Add common values to the transitionStep for the out transition
        /// </summary>
        /// <param name="transitionStep"></param>
        public virtual void SetupTransitionStepOut(TransitionStep transitionStep)
        {
            transitionStep.Delay = TransitionOutConfig.Delay;
            transitionStep.Duration = TransitionOutConfig.Duration;
            transitionStep.TweenType = TransitionOutConfig.TransitionType;
            transitionStep.AnimationCurve = TransitionOutConfig.AnimationCurve;
            transitionStep.OnStart = TransitionOutStart;
            transitionStep.OnComplete = TransitionOutComplete;
            transitionStep.OnUpdate = ValueUpdated;
        }

        #endregion Create transitionStep

        /// <summary>
        /// Transition setting calss exposed through the editor
        /// </summary>
        [System.Serializable]
        public class TransitionSettings
        {
            [Tooltip("Whether the transition should auto run.\nFor in transitions this will happen when the gameobject is activated, for out transitions after the in transition is complete.")]
            public bool AutoRun = false;
            [Tooltip("Whether to automatically check and run transitions on child GameObjects.")]
            public bool TransitionChildren = false;
            [Tooltip("Whether this must be transitioned specifically. If not set it will run automatically when a parent transition is run that has the TransitionChildren property set.")]
            public bool MustTriggerDirect = false;
            [Tooltip("Time in seconds before this transition should be started.")]
            public float Delay;
            [Tooltip("How long this transition will / should run for.")]
            public float Duration = 0.3f;
            [Tooltip("How the transition should be run.")]
            public TransitionHelper.TweenType TransitionType = TransitionHelper.TweenType.linear;
            [Tooltip("A custom curve to show how the transition should be run.")]
            public AnimationCurve AnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            [Tooltip("Methods that should be called when the transition is started.")]
            public UnityEvent OnTransitionStart;
            [Tooltip("Methods that should be called when the transition has completed.")]
            public UnityEvent OnTransitionComplete;
        }
    }
}
