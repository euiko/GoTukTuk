// Copyright (c) 2011 Bob Berkebile (pixelplacment)
// Please direct any bugs/comments/suggestions to http://pixelplacement.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

/*
TERMS OF USE - EASING EQUATIONS
Open source under the BSD License.
Copyright (c)2001 Robert Penner
All rights reserved.
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
Neither the name of the author nor the names of contributors may be used to endorse or promote products derived from this software without specific prior written permission.
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

#region Namespaces

using FlipWebApps.BeautifulTransitions.Scripts.Transitions;
using UnityEngine;

#endregion

namespace FlipWebApps.BeautifulTransitions.Scripts.Helper
{
    public class TweenMethods : MonoBehaviour
    {
        public delegate float TweenFunction(float start, float end, float value);

        public static float linear(float start, float end, float value)
        {
            return Mathf.Lerp(start, end, value);
        }

        public static float clerp(float start, float end, float value)
        {
            float min = 0.0f;
            float max = 360.0f;
            float half = Mathf.Abs((max - min)*0.5f);
            float retval = 0.0f;
            float diff = 0.0f;
            if ((end - start) < -half)
            {
                diff = ((max - start) + end)*value;
                retval = start + diff;
            }
            else if ((end - start) > half)
            {
                diff = -((max - end) + start)*value;
                retval = start + diff;
            }
            else retval = start + (end - start)*value;
            return retval;
        }

        public static float spring(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value = (Mathf.Sin(value*Mathf.PI*(0.2f + 2.5f*value*value*value))*Mathf.Pow(1f - value, 2.2f) + value)*
                    (1f + (1.2f*(1f - value)));
            return start + (end - start)*value;
        }

        public static float easeInQuad(float start, float end, float value)
        {
            end -= start;
            return end*value*value + start;
        }

        public static float easeOutQuad(float start, float end, float value)
        {
            end -= start;
            return -end*value*(value - 2) + start;
        }

        public static float easeInOutQuad(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end*0.5f*value*value + start;
            value--;
            return -end*0.5f*(value*(value - 2) - 1) + start;
        }

        public static float easeInCubic(float start, float end, float value)
        {
            end -= start;
            return end*value*value*value + start;
        }

        public static float easeOutCubic(float start, float end, float value)
        {
            value--;
            end -= start;
            return end*(value*value*value + 1) + start;
        }

        public static float easeInOutCubic(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end*0.5f*value*value*value + start;
            value -= 2;
            return end*0.5f*(value*value*value + 2) + start;
        }

        public static float easeInQuart(float start, float end, float value)
        {
            end -= start;
            return end*value*value*value*value + start;
        }

        public static float easeOutQuart(float start, float end, float value)
        {
            value--;
            end -= start;
            return -end*(value*value*value*value - 1) + start;
        }

        public static float easeInOutQuart(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end*0.5f*value*value*value*value + start;
            value -= 2;
            return -end*0.5f*(value*value*value*value - 2) + start;
        }

        public static float easeInQuint(float start, float end, float value)
        {
            end -= start;
            return end*value*value*value*value*value + start;
        }

        public static float easeOutQuint(float start, float end, float value)
        {
            value--;
            end -= start;
            return end*(value*value*value*value*value + 1) + start;
        }

        public static float easeInOutQuint(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end*0.5f*value*value*value*value*value + start;
            value -= 2;
            return end*0.5f*(value*value*value*value*value + 2) + start;
        }

        public static float easeInSine(float start, float end, float value)
        {
            end -= start;
            return -end*Mathf.Cos(value*(Mathf.PI*0.5f)) + end + start;
        }

        public static float easeOutSine(float start, float end, float value)
        {
            end -= start;
            return end*Mathf.Sin(value*(Mathf.PI*0.5f)) + start;
        }

        public static float easeInOutSine(float start, float end, float value)
        {
            end -= start;
            return -end*0.5f*(Mathf.Cos(Mathf.PI*value) - 1) + start;
        }

        public static float easeInExpo(float start, float end, float value)
        {
            end -= start;
            return end*Mathf.Pow(2, 10*(value - 1)) + start;
        }

        public static float easeOutExpo(float start, float end, float value)
        {
            end -= start;
            return end*(-Mathf.Pow(2, -10*value) + 1) + start;
        }

        public static float easeInOutExpo(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end*0.5f*Mathf.Pow(2, 10*(value - 1)) + start;
            value--;
            return end*0.5f*(-Mathf.Pow(2, -10*value) + 2) + start;
        }

        public static float easeInCirc(float start, float end, float value)
        {
            end -= start;
            return -end*(Mathf.Sqrt(1 - value*value) - 1) + start;
        }

        public static float easeOutCirc(float start, float end, float value)
        {
            value--;
            end -= start;
            return end*Mathf.Sqrt(1 - value*value) + start;
        }

        public static float easeInOutCirc(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return -end*0.5f*(Mathf.Sqrt(1 - value*value) - 1) + start;
            value -= 2;
            return end*0.5f*(Mathf.Sqrt(1 - value*value) + 1) + start;
        }

        /* GFX47 MOD START */

        public static float easeInBounce(float start, float end, float value)
        {
            end -= start;
            float d = 1f;
            return end - easeOutBounce(0, end, d - value) + start;
        }

        /* GFX47 MOD END */

        /* GFX47 MOD START */
        //public static float bounce(float start, float end, float value){
        public static float easeOutBounce(float start, float end, float value)
        {
            value /= 1f;
            end -= start;
            if (value < (1/2.75f))
            {
                return end*(7.5625f*value*value) + start;
            }
            else if (value < (2/2.75f))
            {
                value -= (1.5f/2.75f);
                return end*(7.5625f*(value)*value + .75f) + start;
            }
            else if (value < (2.5/2.75))
            {
                value -= (2.25f/2.75f);
                return end*(7.5625f*(value)*value + .9375f) + start;
            }
            else
            {
                value -= (2.625f/2.75f);
                return end*(7.5625f*(value)*value + .984375f) + start;
            }
        }

        /* GFX47 MOD END */

        /* GFX47 MOD START */

        public static float easeInOutBounce(float start, float end, float value)
        {
            end -= start;
            float d = 1f;
            if (value < d*0.5f) return easeInBounce(0, end, value*2)*0.5f + start;
            else return easeOutBounce(0, end, value*2 - d)*0.5f + end*0.5f + start;
        }

        /* GFX47 MOD END */

        public static float easeInBack(float start, float end, float value)
        {
            end -= start;
            value /= 1;
            float s = 1.70158f;
            return end*(value)*value*((s + 1)*value - s) + start;
        }

        public static float easeOutBack(float start, float end, float value)
        {
            float s = 1.70158f;
            end -= start;
            value = (value) - 1;
            return end*((value)*value*((s + 1)*value + s) + 1) + start;
        }

        public static float easeInOutBack(float start, float end, float value)
        {
            float s = 1.70158f;
            end -= start;
            value /= .5f;
            if ((value) < 1)
            {
                s *= (1.525f);
                return end*0.5f*(value*value*(((s) + 1)*value - s)) + start;
            }
            value -= 2;
            s *= (1.525f);
            return end*0.5f*((value)*value*(((s) + 1)*value + s) + 2) + start;
        }

        public static float punch(float amplitude, float value)
        {
            float s = 9;
            if (value == 0)
            {
                return 0;
            }
            else if (value == 1)
            {
                return 0;
            }
            float period = 1*0.3f;
            s = period/(2*Mathf.PI)*Mathf.Asin(0);
            return (amplitude*Mathf.Pow(2, -10*value)*Mathf.Sin((value*1 - s)*(2*Mathf.PI)/period));
        }

        /* GFX47 MOD START */

        public static float easeInElastic(float start, float end, float value)
        {
            end -= start;

            float d = 1f;
            float p = d*.3f;
            float s = 0;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p/4;
            }
            else
            {
                s = p/(2*Mathf.PI)*Mathf.Asin(end/a);
            }

            return -(a*Mathf.Pow(2, 10*(value -= 1))*Mathf.Sin((value*d - s)*(2*Mathf.PI)/p)) + start;
        }

        /* GFX47 MOD END */

        /* GFX47 MOD START */
        //public static float elastic(float start, float end, float value){
        public static float easeOutElastic(float start, float end, float value)
        {
            /* GFX47 MOD END */
            //Thank you to rafael.marteleto for fixing this as a port over from Pedro's UnityTween
            end -= start;

            float d = 1f;
            float p = d*.3f;
            float s = 0;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p*0.25f;
            }
            else
            {
                s = p/(2*Mathf.PI)*Mathf.Asin(end/a);
            }

            return (a*Mathf.Pow(2, -10*value)*Mathf.Sin((value*d - s)*(2*Mathf.PI)/p) + end + start);
        }

        /* GFX47 MOD START */

        public static float easeInOutElastic(float start, float end, float value)
        {
            end -= start;

            float d = 1f;
            float p = d*.3f;
            float s = 0;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d*0.5f) == 2) return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p/4;
            }
            else
            {
                s = p/(2*Mathf.PI)*Mathf.Asin(end/a);
            }

            if (value < 1)
                return -0.5f*(a*Mathf.Pow(2, 10*(value -= 1))*Mathf.Sin((value*d - s)*(2*Mathf.PI)/p)) + start;
            return a*Mathf.Pow(2, -10*(value -= 1))*Mathf.Sin((value*d - s)*(2*Mathf.PI)/p)*0.5f + end + start;
        }

        /* GFX47 MOD END */
    }
}