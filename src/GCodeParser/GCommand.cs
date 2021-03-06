using System;

namespace GCode.Core
{
    public class GCommand
    {
        public GCommandType CommandType { get; set; }
        public float? DestinationX { get; set; }
        public float? DestinationY { get; set; }
        public float? DestinationZ { get; set; }
        public Point3D CurrentPos { get; set; }
        public Point3D TargetPos { get; set; }
        public int LineNumber { get; set; }
        public float ArcRadius { get; set; }
        public float Angle { get; set; }
        public float CurrentAngle { get; set; }
        public Point3D ArcCenter { get; set; }

        public Point3D[] Vertices { get; set; }


        public int VertexIndex { get; set; }

        public double GetArcCenterX(double x1, double y1, double x2, double y2, double radius)
        {
            double radsq = radius * radius;
            double q = Math.Sqrt(((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)));
            double x3 = (x1 + x2) / 2;

            return x3 + Math.Sqrt(radsq - ((q / 2) * (q / 2))) * ((y1 - y2) / q);
        }

        public double GetArcCenterY(double x1, double y1, double x2, double y2, double radius)
        {
            double radsq = radius * radius;
            double q = Math.Sqrt(((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)));

            double y3 = (y1 + y2) / 2;

            return y3 + Math.Sqrt(radsq - ((q / 2) * (q / 2))) * ((x2 - x1) / q);
        }

        public void SetArcCenter()
        {
            var arcCenterX = (float)GetArcCenterX(CurrentPos.X, CurrentPos.Y, TargetPos.X, TargetPos.Y, ArcRadius);
            var arcCenterY = (float)GetArcCenterY(CurrentPos.X, CurrentPos.Y, TargetPos.X, TargetPos.Y, ArcRadius);

            ArcCenter = new Point3D(arcCenterX, arcCenterY, CurrentPos.Z);
        }

        public void SetArcAngle()
        {
            //Angle = (float)(Math.Atan2(TargetPos.Y, TargetPos.X) - Math.Atan2(CurrentPos.Y, CurrentPos.X));
            Angle = GetAngleABC(CurrentPos, TargetPos, ArcCenter);
            if (Angle < -3.14159f)
                Angle = Angle + 3.14159f;
        }

        public void SetupArc()
        {
            SetArcCenter();
            SetArcAngle();
        }

        float GetAngleABC(Point3D a, Point3D b, Point3D c)
        {
            Point3D ab = new Point3D(b.X - a.X, b.Y - a.Y, 0);
            Point3D cb = new Point3D(b.X - c.X, b.Y - c.Y, 0);

            float dot = (ab.X * cb.X + ab.Y * cb.Y); // dot product
            float cross = (ab.X * cb.Y - ab.Y * cb.X); // cross product

            return (float)Math.Atan2(cross, dot) * 2;

            //return (int)Math.Floor(alpha * 180 / Math.PI + 0.5);
        }

        public override string ToString()
        {
            return $"({CurrentPos.X},{CurrentPos.Y}{CurrentPos.Z})";
        }
    }
}
