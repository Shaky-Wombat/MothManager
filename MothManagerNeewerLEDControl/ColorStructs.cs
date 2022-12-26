﻿using System;
using Windows.Storage.Search;

namespace MothManagerNeewerLEDControl
{
    public static class ColorStruct
    {
        public struct RGB: IComparable<RGB>
        {
            private float _r;
            private float _g;
            private float _b;
            
            public float R
            {
                get => _r;
                set => _r = value;
            }
            
            public float G
            {
                get => _g;
                set => _g = value;
            }
            
            public float B
            {
                get => _b;
                set => _b = value;
            }

            public RGB(float r, float g, float b)
            {
                _r = r;
                _g = g;
                _b = b;
            }

            public static RGB FromKelvin(int kelvin)
            {
                var temperature = kelvin / 100.0f;

                if (temperature <= 66)
                {
                    return new RGB(
                        1f,
                        Math.Clamp((int)(99.4708025861f * Math.Log(temperature) - 161.1195681661f), 0, 255) / 255f,
                        temperature <= 19 ? 0f : Math.Clamp((int)(138.5177312231f * Math.Log(temperature - 10f) - 305.0447927307f), 0, 255) / 255f
                    );
                }
                else
                {
                    return new RGB(
                        Math.Clamp((int)(329.698727446f * Math.Pow(temperature - 60f, -0.1332047592f)), 0, 255) / 255f,
                        Math.Clamp((int)(288.1221695283f * Math.Pow(temperature - 60f, -0.0755148492f)), 0, 255) / 255f,
                        1f
                    );
                }
            }

            public int CompareTo(RGB other)
            {
                var rComparison = _r.CompareTo(other._r);
                if (rComparison != 0) return rComparison;
                var gComparison = _g.CompareTo(other._g);
                if (gComparison != 0) return gComparison;
                return _b.CompareTo(other._b);
            }
            
            public static implicit operator HSV(RGB rgb) => RgbToHSV(rgb);
            public static explicit operator RGB(HSV hsv) => HsvToRgb(hsv);
        }

        public struct HSV : IComparable<HSV>
        {
            private float _h;
            private float _s;
            private float _v;
            
            public float H
            {
                get => _h;
                set => _h = value;
            }
            
            public float S
            {
                get => _s;
                set => _s = value;
            }
            
            public float V
            {
                get => _v;
                set => _v = value;
            }

            public HSV(float h, float s, float v)
            {
                _h = h;
                _s = s;
                _v = v;
            }

            public void MakeValid()
            {
                _h.Wrap(1f);
                _s.Saturate();
                _v.Saturate();
            }

            public int CompareTo(HSV other)
            {
                var hComparison = _h.CompareTo(other._h);
                if (hComparison != 0) return hComparison;
                var sComparison = _s.CompareTo(other._s);
                if (sComparison != 0) return sComparison;
                return _v.CompareTo(other._v);
            }
            
            public static implicit operator RGB(HSV hsv) => HsvToRgb(hsv);
            public static explicit operator HSV(RGB rgb) => RgbToHSV(rgb);
        }

        // Based on https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Math/Color.cs
        public static HSV RgbToHSV(RGB rgb)
        {
            // when blue is highest valued
            if ((rgb.B > rgb.G) && (rgb.B > rgb.R))
            {
                return RgbToHsvHelper(4f, rgb.B, rgb.R, rgb.G);
            }
            
            //when green is highest valued
            if (rgb.B > rgb.R)
            {
                return RgbToHsvHelper(2f, rgb.G, rgb.B, rgb.R);
            }
            
            //when red is highest valued
            return RgbToHsvHelper(0f, rgb.R, rgb.G, rgb.B);
        }
        
        public static RGB HsvToRgb(HSV hsv)
        {
            if (hsv.S == 0f || hsv.V == 0f)
            {
                return new RGB(hsv.V, hsv.V, hsv.V);
            }

            var hToFloor = hsv.H * 6f;
            var temp = (int)Math.Floor(hToFloor);
            var t = hToFloor - temp;
            var value1 = hsv.V * (1f - hsv.S);
            var value2 = hsv.V * (1f - hsv.S * t);
            var value3 = hsv.V * (1f - hsv.S * (1f - t));

            switch (temp)
            {
                case 0:
                    return new RGB(hsv.V.Saturate(), value3.Saturate(), value1.Saturate());
                
                case 1:
                    return new RGB(value2.Saturate(), hsv.V.Saturate(), value1.Saturate());
                    
                case 2:
                    return new RGB(value1.Saturate(), hsv.V.Saturate(), value3.Saturate());
                
                case 3:
                    return new RGB(value1.Saturate(), value2.Saturate(), hsv.V.Saturate());
                
                case 4:
                    return new RGB(value3.Saturate(), value1.Saturate(), hsv.V.Saturate());
                
                case 5: 
                    return new RGB(hsv.V.Saturate(), value1.Saturate(), value2.Saturate());
                
                case 6:
                    return new RGB(hsv.V.Saturate(), value3.Saturate(), value1.Saturate());
                
                case -1:
                    return new RGB(hsv.V.Saturate(), value1.Saturate(), value2.Saturate());
                
                default:
                    return new RGB(0f, 0f, 0f);
            }
        }
        
        private static HSV RgbToHsvHelper(float offset, float dominantColor, float color1, float color2)
        {
            var hsv = new HSV();
            
            hsv.V = dominantColor;
            //we need to find out which is the minimum color
            if (hsv.V != 0)
            {
                //we check which color is smallest
                float small = 0;
                if (color1 > color2) small = color2;
                else small = color1;

                float diff = hsv.V - small;

                //if the two values are not the same, we compute the like this
                if (diff != 0)
                {
                    //S = max-min/max
                    hsv.S = diff / hsv.V;
                    //H = hue is offset by X, and is the difference between the two smallest colors
                    hsv.H = offset + ((color1 - color2) / diff);
                }
                else
                {
                    //S = 0 when the difference is zero
                    hsv.S = 0;
                    //H = 4 + (R-G) hue is offset by 4 when blue, and is the difference between the two smallest colors
                    hsv.H = offset + (color1 - color2);
                }

                hsv.H /= 6;

                //conversion values
                if (hsv.H < 0)
                    hsv.H += 1.0f;
            }
            else
            {
                hsv.S = 0;
                hsv.H = 0;
            }

            return hsv;
        }
        
        private static float Saturate(this float value)
        {
            return Math.Clamp(value, 0f, 1f);
        }

        private static float Wrap(this ref float value, float max)
        {
            return value >= 0 ? value % max : (value % max + max) % max;
        }
    }
}