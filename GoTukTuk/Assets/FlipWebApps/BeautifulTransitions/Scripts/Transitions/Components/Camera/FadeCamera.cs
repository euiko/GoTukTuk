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

using FlipWebApps.BeautifulTransitions.Scripts.Transitions.Components.Camera.AbstractClasses;
using FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps;
using UnityEngine;

namespace FlipWebApps.BeautifulTransitions.Scripts.Transitions.Components.Camera
{
    [ExecuteInEditMode]
    [AddComponentMenu("Beautiful Transitions/Camera/Fade")]
    [HelpURL("http://www.flipwebapps.com/beautiful-transitions/")]
    public class FadeCamera : TransitionCameraBase
    {
        [Header("Fade Specific")]
        public InSettings InConfig;
        public OutSettings OutConfig;

        Material _material;

        public void Awake()
        {
            var shader = Shader.Find("Hidden/FlipWebApps/BeautifulTransitions/FadeCamera");
            if (shader != null && shader.isSupported)
                _material = new Material(shader);
            else
                Debug.Log("FadeCamera: Shader is not found or supported on this platform.");
        }


        // Postprocess the image
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            var transitionStepFloat = CurrentTransitionStep as TransitionStepFloat;
            if (transitionStepFloat != null && _material != null && !Mathf.Approximately(transitionStepFloat.Value, 0))
            {
                if (CrossTransitionTarget != null)
                {
                    _material.SetTexture("_OverlayTex", CrossTransitionRenderTexture);
                }
                else
                {
                    _material.SetTexture("_OverlayTex",
                        TransitionMode == TransitionModeType.In ? InConfig.Texture : OutConfig.Texture);
                }

                _material.SetColor("_Color", TransitionMode == TransitionModeType.In ? InConfig.Color : OutConfig.Color);
                _material.SetFloat("_Amount", transitionStepFloat.Value);

                Graphics.Blit(source, destination, _material);
            }
            else
            {
                Graphics.Blit(source, destination);
            }
        }

        #region Transition specific settings

        [System.Serializable]
        public class InSettings
        {
            [Tooltip("Optional overlay texture to use.")]
            public Texture2D Texture;
            [Tooltip("Tint color.")]
            public Color Color;
        }

        [System.Serializable]
        public class OutSettings
        {
            [Tooltip("Optional overlay texture to use.")]
            public Texture2D Texture;
            [Tooltip("Tint color.")]
            public Color Color;
        }

        #endregion Transition specific settings
    }
}
