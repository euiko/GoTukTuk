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

namespace FlipWebApps.BeautifulTransitions.Scripts.Transitions.Components.Camera.AbstractClasses
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(UnityEngine.Camera))]
    public abstract class TransitionCameraBase : TransitionBase
    {
        public bool SkipIdleRendering;

        protected RenderTexture CrossTransitionRenderTexture;
        protected UnityEngine.Camera CrossTransitionTarget;

        /// <summary>
        /// Cross Transition over to the specified camera.
        /// </summary>
        /// This will use the target camera to perform the cross fade and for things like position etc. Note that the target
        /// camera will only be temporarily activated. Once the transition is done the the source camera will be physically 
        /// transformed to match the position, location and rotation of the target camera. This is to avoid issues with audio
        /// and other factors. If you need different functionality then let us know.
        /// <param name="target"></param>
        public void CrossTransition(UnityEngine.Camera target)
        {
            CrossTransitionTarget = target;
            TransitionOut();
        }

        #region TransitionBase Overrides

        public override void TransitionOutStart()
        {
            if (CrossTransitionTarget != null)
            {
                CrossTransitionRenderTexture = new RenderTexture(UnityEngine.Screen.width, UnityEngine.Screen.height, 24);
                CrossTransitionTarget.gameObject.SetActive(true);
                CrossTransitionTarget.targetTexture = CrossTransitionRenderTexture;
            }
            base.TransitionOutStart();
        }

        public override void TransitionOutComplete()
        {
            if (CrossTransitionTarget != null)
            {
                CrossTransitionTarget.gameObject.SetActive(false);
                CrossTransitionTarget.targetTexture = null;
                Destroy(CrossTransitionRenderTexture);
                CrossTransitionRenderTexture = null;
                transform.position = CrossTransitionTarget.transform.position;
                transform.rotation = CrossTransitionTarget.transform.rotation;
                transform.localScale = CrossTransitionTarget.transform.localScale;
                CrossTransitionTarget = null;

                var transitionStepFloat = CurrentTransitionStep as TransitionStepFloat;
                if (transitionStepFloat != null)
                    transitionStepFloat.Value = 0;
            }
            base.TransitionOutComplete();
        }

        /// <summary>
        /// Get an instance of the current transition item
        /// </summary>
        /// <returns></returns>
        public override TransitionStep CreateTransitionStep()
        {
            return new TransitionStepFloat();
        }


        /// <summary>
        /// Add common values to the transitionStep for the in transition
        /// </summary>
        /// <param name="transitionStep"></param>
        public override void SetupTransitionStepIn(TransitionStep transitionStep)
        {
            var transitionStepScreenWipe = transitionStep as TransitionStepFloat;
            if (transitionStepScreenWipe != null)
            {
                transitionStepScreenWipe.StartValue = 1;
                transitionStepScreenWipe.EndValue = 0;
            }
            base.SetupTransitionStepIn(transitionStep);
        }


        /// <summary>
        /// Add common values to the transitionStep for the out transition
        /// </summary>
        /// <param name="transitionStep"></param>
        public override void SetupTransitionStepOut(TransitionStep transitionStep)
        {
            var transitionStepScreenWipe = transitionStep as TransitionStepFloat;
            if (transitionStepScreenWipe != null)
            {
                transitionStepScreenWipe.StartValue = 0;
                transitionStepScreenWipe.EndValue = 1;
            }
            base.SetupTransitionStepOut(transitionStep);
        }

        #endregion TransitionBase Overrides
    }
}
