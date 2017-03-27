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
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.Components.GameObject.AbstractClasses;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps.AbstractClasses;
using UnityEngine;

namespace FlipWebApps.BeautifulTransitions.Scripts.Transitions.Components.GameObject
{
    [AddComponentMenu("Beautiful Transitions/GameObject + UI/Move")]
    [HelpURL("http://www.flipwebapps.com/beautiful-transitions/")]
    public class TransitionMove : TransitionGameObjectBase
    {
        public enum MoveModeType
        {
            Global,
            Local,
            AnchoredPosition
        };

        public enum MoveType
        {
            FixedPosition,
            Delta
        };

        [Header("Move Specific")]
        public MoveModeType MoveMode = MoveModeType.Global;

        public InSettings InConfig;
        public OutSettings OutConfig;

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
            return new Move(Target, coordinateSpace: ConvertMoveMode());
        }

        /// <summary>
        /// Add common values to the transitionStep for the in transition
        /// </summary>
        /// <param name="transitionStep"></param>
        public override void SetupTransitionStepIn(TransitionStep transitionStep)
        {
            var transitionStepMove = transitionStep as Move;
            if (transitionStepMove != null)
            {
                transitionStepMove.StartValue = InConfig.StartPositionType == MoveType.FixedPosition
                    ? InConfig.StartPosition
                    : _originalPosition + InConfig.StartPosition; ;
                transitionStepMove.EndValue = _originalPosition;
                transitionStepMove.CoordinateSpace = ConvertMoveMode();
            }
            base.SetupTransitionStepIn(transitionStep);
        }

        /// <summary>
        /// Add common values to the transitionStep for the out transition
        /// </summary>
        /// <param name="transitionStep"></param>
        public override void SetupTransitionStepOut(TransitionStep transitionStep)
        {
            var transitionStepMove = transitionStep as Move;
            if (transitionStepMove != null)
            {
                transitionStepMove.StartValue = transitionStepMove.GetCurrent();
                transitionStepMove.EndValue = OutConfig.EndPositionType == MoveType.FixedPosition
                    ? OutConfig.EndPosition
                    : _originalPosition + OutConfig.EndPosition;
                transitionStepMove.CoordinateSpace = ConvertMoveMode();
            }
            base.SetupTransitionStepOut(transitionStep);
        }

        #endregion TransitionBase Overrides

        /// <summary>
        /// Convert custom move mode to standard one.
        /// </summary>
        /// <returns></returns>
        TransitionStep.CoordinateSpaceType ConvertMoveMode()
        {
            if (MoveMode == MoveModeType.Global)
                return TransitionStep.CoordinateSpaceType.Global;
            if (MoveMode == MoveModeType.Local)
                return TransitionStep.CoordinateSpaceType.Local;
            // else if (MoveMode == MoveModeType.AnchoredPosition)
            return TransitionStep.CoordinateSpaceType.AnchoredPosition;
        }

        #region Transition specific settings

        [Serializable]
        public class InSettings
        {
            [Tooltip("Movement type.")]
            public MoveType StartPositionType;
            [Tooltip("Starting position (end at the GameObjects initial position).")]
            public Vector3 StartPosition = new Vector3(0, 0, 0);
        }

        [Serializable]
        public class OutSettings
        {
            [Tooltip("Movement type.")]
            public MoveType EndPositionType;
            [Tooltip("End position (end at the GameObjects current position).")]
            public Vector3 EndPosition = new Vector3(0, 0, 0);
        }

        #endregion Transition specific settings

    }
}
