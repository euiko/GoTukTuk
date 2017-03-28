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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlipWebApps.BeautifulTransitions.Scripts.Shake
{
    /// <summary>
    /// Shake helper functions that you can use for triggering and running your own shakes.
    /// </summary>
    /// Note: this is a work in progress. Improvements are consider movement by other scripts. improved shake algorithm.
    [HelpURL("http://www.flipwebapps.com/beautiful-transitions/shake/")]
    public class ShakeHelper
    {
        static readonly List<int> ActiveShakes = new List<int>(2);


        /// <summary>
        /// Shake the specified transform using the passed duration, range and decay start.
        /// 
        /// Note: this will use the caller object to start a coroutine as when 
        /// deactivating a display item we need to wait for the animation to 
        /// complete before deactivating the gameobject.
        /// </summary>
        /// <param name="caller">A MonoBehaviour used to start the shake coroutine. If calling from your own component you can usually just pass 'this'.</param>
        /// <param name="transform">The transform that you want to shake.</param>
        /// <param name="duration">The duration to shake the camera for.</param>
        /// <param name="range">The shake movement range from the origin. Set any dimension to 0 to stop movement along that axis.</param>
        /// <param name="decayStart">The offset relative to duration after which to start decaying (slowing down) the movement in the range 0 to 1.</param>
        public static void Shake(MonoBehaviour caller, Transform transform, float duration, Vector3 range, float decayStart = 1f)
        {
            caller.StartCoroutine(ShakeCoroutine(transform, duration, range, decayStart));
        }


        /// <summary>
        /// Coroutine called to shake the specified transform.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="duration"></param>
        /// <param name="range"></param>
        /// <param name="decayStart"></param>
        /// <returns></returns>
        static IEnumerator ShakeCoroutine(Transform transform, float duration, Vector3 range, float decayStart = 1f)
        {
            // if we run multiple shakes then we can record the wrong originalPositino. For now just disallow multiple shakes on the same object
            if (!ActiveShakes.Contains(transform.GetInstanceID()))
            {
                ActiveShakes.Add(transform.GetInstanceID());

                var originalPosition = transform.localPosition;

                var randomAngle = Random.Range(0, 361);

                var decay = 0f;
                var decayFactor = Mathf.Approximately(decayStart, 1) ? 0 : 1/(1 - decayStart);

                var elapsedTime = 0f;
                while (elapsedTime < duration)
                {
                    if (transform.gameObject != null)
                    {
                        var percentComplete = elapsedTime/duration;

                        if (percentComplete >= decayStart)
                        {
                            decay = 1 - decayFactor + decayFactor*percentComplete;
                            // decay = Mathf.Clamp(v, 0.0f, 1.0f); // should be no need to clamp due to above conditional test!
                        }

                        // perlin movement
                        //var smoothRandom = SmoothRandom.GetVector3(speed);
                        //var target = originalPosition + Vector3.Scale(smoothRandom, range * decay);
                        //speed *= -1;
                        //range *= -1; //= new Vector3(range.x * -1, range.y * -1, range.z * -1);

                        // angle based
                        randomAngle += 180 + Random.Range(-60, 60); // mirror angle and add some varience.
                        var sinAngle = Mathf.Sin(randomAngle);
                        var cosAngle = Mathf.Cos(randomAngle);
                        var offset = new Vector3(cosAngle*sinAngle*range.x,
                            sinAngle*sinAngle*range.y,
                            cosAngle*range.z);
                        var target = originalPosition + (offset*(1 - decay));

                        transform.localPosition = target;
                    }
                    yield return null;
                    elapsedTime += Time.deltaTime;
                }

                // set back to original position - TODO consider movement from other scripts.
                transform.localPosition = originalPosition;
                ActiveShakes.Remove(transform.GetInstanceID());
            }
        }
    }
}