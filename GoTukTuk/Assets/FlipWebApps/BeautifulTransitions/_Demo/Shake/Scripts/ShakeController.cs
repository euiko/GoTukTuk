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

using FlipWebApps.BeautifulTransitions.Scripts.Shake.Components;
using UnityEngine;
using UnityEngine.UI;

namespace FlipWebApps.BeautifulTransitions._Demo.Shake.Scripts
{
    public class ShakeController : MonoBehaviour
    {
        public Text DurationText;
        public Slider DurationSlider;
        public Text DecayStartText;
        public Slider DecayStartSlider;
        public InputField XInput;
        public InputField YInput;
        public InputField ZInput;

        void Start ()
        {
            DurationSlider.value = ShakeCamera.Instance.Duration;
            DecayStartSlider.value = ShakeCamera.Instance.DecayStart;
            XInput.text = ShakeCamera.Instance.Range.x.ToString();
            YInput.text = ShakeCamera.Instance.Range.y.ToString();
            ZInput.text = ShakeCamera.Instance.Range.z.ToString();
        }

        void Update ()
        {
            DurationText.text = string.Format("Duration ({0})", DurationSlider.value);
            DecayStartText.text = string.Format("Decay Start ({0})", DecayStartSlider.value);
        }

        public void Shake()
        {
            // Here we call Shake with out new values. We could just call Shake() without any parameters to use
            // the values configured on the component.
            ShakeCamera.Instance.Shake(DurationSlider.value, 
                new Vector3(float.Parse(XInput.text), float.Parse(YInput.text), float.Parse(ZInput.text)), 
                DecayStartSlider.value);
        }
    }
}
