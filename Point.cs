using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SceneSample
{
    public class Point
    {
        public Point()
        {
        }

        public Point(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public static Point Parse(string text)
        {
            float x = 0, y = 0, z = 0;

            var arr = text.Trim().Split(' ');

            foreach (var item in arr)
            {
                var val = item.ToUpper();
                if (item.Length > 0)
                {
                    switch (item.ToUpper()[0])
                    {
                        case 'X':
                            val = item.Trim(new char[] { 'X', ' ' });
                            x = float.Parse(val, System.Globalization.NumberStyles.Any, NumberFormatInfo.InvariantInfo) ;
                            break;
                        case 'Y':
                            val = item.Trim(new char[] { 'Y', ' ' });
                            y = float.Parse(val, System.Globalization.NumberStyles.Any, NumberFormatInfo.InvariantInfo);
                            break;
                        case 'Z':
                            val = item.Trim(new char[] { 'Z', ' ' });
                            z = float.Parse(val, System.Globalization.NumberStyles.Any, NumberFormatInfo.InvariantInfo);
                            break;
                        default:
                            break;
                    }
                }
            }

            return new Point(x, y, z);
        }

        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }
    }
}
