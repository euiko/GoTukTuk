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

using FlipWebApps.BeautifulTransitions.Scripts.Transitions.Components.GameObject.AbstractClasses;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps.AbstractClasses;
using UnityEngine;

namespace FlipWebApps.BeautifulTransitions.Scripts.Transitions.Components.GameObject
{
    [AddComponentMenu("Beautiful Transitions/GameObject + UI/Move Target")]
    [HelpURL("http://www.flipwebapps.com/beautiful-transitions/")]
    public class TransitionMoveTraget : TransitionGameObjectBase
    {
        [Header("Move Specific Settings")]
        public InSettings MoveInConfig;
        public OutSettings MoveOutConfig;

        Vector3 _originalPosition;

        #region TransitionBase Overrides

        /// <summary>
        /// Gather any initial state - See TrtansitionBase for further details
        /// </summary>
        public override void SetupInitialState()
        {
            _originalPosition = ((Move)CreateTransitionStep()).OriginalValue;
        }


        /// <summary>
        /// Get an instance of the current transition item
        /// </summary>
        /// <returns></returns>
        public override TransitionStep CreateTransitionStep()
        {
            return new Move(Target);
        }

        /// <summary>
        /// Add common values to the transitionStep for the in transition
        /// </summary>
        /// <param name="transitionStep"></param>
        public override void SetupTransitionStepIn(TransitionStep transitionStep)
        {
            var transitionStepMoveTarget = transitionStep as Move;
            if (transitionStepMoveTarget != null)
            {
                transitionStepMoveTarget.StartValue = new Vector3(
                    MoveInConfig.MoveX ? MoveInConfig.StartTarget.transform.position.x : _originalPosition.x,
                    MoveInConfig.MoveY ? MoveInConfig.StartTarget.transform.position.y : _originalPosition.y,
                    MoveInConfig.MoveZ ? MoveInConfig.StartTarget.transform.position.z : _originalPosition.z);
                transitionStepMoveTarget.EndValue = _originalPosition;
            }
            base.SetupTransitionStepIn(transitionStep);
        }

        /// <summary>
        /// Add common values to the transitionStep for the out transition
        /// </summary>
        /// <param name="transitionStep"></param>
        public override void SetupTransitionStepOut(TransitionStep transitionStep)
        {
            var transitionStepMoveTarget = transitionStep as Move;
            if (transitionStepMoveTarget != null)
            {
                transitionStepMoveTarget.StartValue = transitionStepMoveTarget.GetCurrent();
                transitionStepMoveTarget.EndValue = new Vector3(
                    MoveOutConfig.MoveX ? MoveOutConfig.EndTarget.transform.position.x : transitionStepMoveTarget.GetCurrent().x,
                    MoveOutConfig.MoveY ? MoveOutConfig.EndTarget.transform.position.y : transitionStepMoveTarget.GetCurrent().y,
                    MoveOutConfig.MoveZ ? MoveOutConfig.EndTarget.transform.position.z : transitionStepMoveTarget.GetCurrent().z);
            }
            base.SetupTransitionStepOut(transitionStep);
        }

        #endregion TransitionBase Overrides

        #region Transition specific settings

        [System.Serializable]
        public class InSettings
        {
            [Tooltip("GameObject used as a starting position (end at the GameObjects initial position).")]
            public UnityEngine.GameObject StartTarget;
            [Tooltip("Whether to move in the X direction. Clear this to keep the gameobjects original X position.")]
            public bool MoveX = true;
            [Tooltip("Whether to move in the Y direction. Clear this to keep the gameobjects original Y position.")]
            public bool MoveY = true;
            [Tooltip("Whether to move in the Z direction. Clear this to keep the gameobjects original Z position.")]
            public bool MoveZ = true;
        }

        [System.Serializable]
        public class OutSettings
        {
            [Tooltip("GameObject used as the ending position (starts at the GameObjects current position).")]
            public UnityEngine.GameObject EndTarget;
            [Tooltip("Whether to move in the X direction. Clear this to keep the gameobjects original X position.")]
            public bool MoveX = true;
            [Tooltip("Whether to move in the Y direction. Clear this to keep the gameobjects original Y position.")]
            public bool MoveY = true;
            [Tooltip("Whether to move in the Z direction. Clear this to keep the gameobjects original Z position.")]
            public bool MoveZ = true;
        }

        #endregion Transition specific settings
    }
}
