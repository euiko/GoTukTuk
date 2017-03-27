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
using UnityEngine;

namespace FlipWebApps.BeautifulTransitions.Scripts.Transitions.TransitionSteps
{
    /// <summary>
    /// Global scheduler and singleton for behind the scenes management of transitions
    /// </summary>
    public class TransitionController : MonoBehaviour
    {
        #region General Properties

        public Texture2D ScreenSnapshot { get; set; }

        public bool IsInCrossTransition { get; set; }

        #endregion General Properties

        #region Shared Transition Specific References

        public ScreenWipeComponents SharedScreenWipeComponents { get; set; }
        public ScreenFadeComponents SharedScreenFadeComponents { get; set; }

        #endregion Shared Transition Specific References

        #region Auto instantiate singleton

        /// <summary>
        /// Singleton that auto creates a gameobject.
        /// </summary>
        public static TransitionController Instance {
            get
            {
                if (_instance == null)
                {
                    var singleton = new UnityEngine.GameObject("(Beautiful Transitions - Controller)");
                    _instance = singleton.AddComponent<TransitionController>();
                    _instance.Setup();

                    DontDestroyOnLoad(singleton);
                }
                return _instance;
            }
            private set { _instance = value; }
        }
        static TransitionController _instance;

        public static bool IsActive { get { return Instance != null; } }

        #endregion Auto instantiate singleton


        /// 
        /// Called on first access to the singleon
        /// 
        void Setup()
        {
            SharedScreenWipeComponents = new ScreenWipeComponents();
            SharedScreenFadeComponents = new ScreenFadeComponents();
        }


        #region Scene loading and changes

        /// <summary>
        /// Coroutine that will load a scene and wait unitl it has loaded.
        /// </summary>
        /// <param name="sceneToLoad"></param>
        /// <returns></returns>
        public IEnumerator LoadSceneAndWaitForLoad(string sceneToLoad)
        {
#if UNITY_5_4_OR_NEWER
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneFinishedLoading;
#endif
            TransitionHelper.LoadScene(sceneToLoad);
            while (!_newSceneLoaded)
                yield return null;
            _newSceneLoaded = false;
#if UNITY_5_4_OR_NEWER
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneFinishedLoading;
#endif
        }
        bool _newSceneLoaded;

        /// <summary>
        /// Unity 5.4+ scene changed callback
        /// </summary>
#if UNITY_5_4_OR_NEWER
        void OnSceneFinishedLoading(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            _newSceneLoaded = true;
        }

#else
        /// <summary>
        /// Unity 5.3 and lower monobehaviour schem loaded method. 
        /// </summary>
        /// <param name="level"></param>
        void OnLevelWasLoaded(int level)
        {
            _newSceneLoaded = true;
        }
#endif

        #endregion Scene loading and changes

        #region Screenshots

        /// <summary>
        /// Take a screenshot and save for later usage.
        /// </summary>
        /// Note: this will in turn start a coroutine so you need to wait for the next frame before the image is available through the ScreenSnapshot property
        public void TakeScreenshot()
        {
            StartCoroutine(TakeScreenshotCoroutine());
        }


        /// <summary>
        /// Need to run as a coroutine as we have to wait until the output buffer is ready for reading.
        /// </summary>
        public IEnumerator TakeScreenshotCoroutine()
        {
            yield return new WaitForEndOfFrame();

            ScreenSnapshot = TransitionHelper.TakeScreenshot();
        }
#endregion Screenshots
    }
}

