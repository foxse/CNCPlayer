using System.Globalization;

namespace GCode.Core
{
    public class Point3D
    {
        public Point3D()
        {
        }

        public Point3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public static Point3D Parse(string text)
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
                            x = float.Parse(val, System.Globalization.NumberStyles.Any, NumberFormatInfo.InvariantInfo);
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

            return new Point3D(x, y, z);
        }

        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }
    }
}
