using Caliburn.Micro;
using Microsoft.Win32;
using System.Windows;
using System;
using GCode.Core;
using System.IO;
using GCodePlayer3D.ViewModels;
using System.Timers;
using System.Collections.Generic;

namespace GCodePlayer3D
{
    public class ShellViewModel : Conductor<object>
    {
        private Timer _timer;

        private Point3D _step = new Point3D();

        public Point3D HeadPosition { get; set; } = new Point3D();

        public bool CanStart => Commands != null && Commands.Count > 0;

        public ShellViewModel()
        {
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                DrawNext();
            });
        }

        private float _headSpeed = 1f;
        public float HeadSpeed
        {
            get { return _headSpeed; }
            set { _headSpeed = value * (ViewScale / 100f); }
        }

        private int _viewScale = 100;

        public int ViewScale
        {
            get => _viewScale;
            set
            {
                _viewScale = value;
                NotifyOfPropertyChange(() => ViewScale);
            }
        }

        private int _thickness = 1;

        public int Thickness
        {
            get { return _thickness; }
            set
            {
                _thickness = value;
                NotifyOfPropertyChange(() => Thickness);
            }
        }

        public bool ShowLineNumbers { get; set; } = true;
        public bool ShowTravelLines { get; set; }

        private bool showGrid = true;
        public bool ShowGrid { 
            get => showGrid;
            set { 
                showGrid = value;
                NotifyOfPropertyChange(() => ShowGrid);
            } 
        }

        private int _commandIndex;

        public int CommandIndex
        {
            get => _commandIndex;
            set
            {
                _commandIndex = value;
                if (_commandIndex > 0 && _commandIndex < Commands.Count)
                    CurrentCommand = Commands[_commandIndex];
                else
                    CurrentCommand = null;

                if (CurrentCommand != null)
                    ActivateItemAsync(new GCommandViewModel(CurrentCommand));

                NotifyOfPropertyChange(() => CommandIndex);
            }
        }

        public GCommand SelectedCommand { get; set; }
        public GCommand CurrentCommand { get; set; }

        public BindableCollection<GCommand> Commands { get; set; }
        public BindableCollection<string> GCode { get; set; }

        private System.Windows.Media.Media3D.Point3DCollection points;

        public System.Windows.Media.Media3D.Point3DCollection Points
        {
            get
            {
                return this.points;
            }

            set
            {
                points = value;
                NotifyOfPropertyChange("Points");
            }
        }

        public async void LoadCodeFromFile()
        {
            Commands = new BindableCollection<GCommand>();

            var ofd = new OpenFileDialog();
            var dialogResult = ofd.ShowDialog();
            if (dialogResult == true)
            {
                try
                {
                    GCode = new BindableCollection<string>(await File.ReadAllLinesAsync(ofd.FileName));

                    BaseParser.DropCounters();

                    foreach (var line in GCode)
                    {
                        var command = BaseParser.Parse(line, ViewScale / 100f, ShowLineNumbers);
                        Commands.Add(command);
                    }

                    HeadPosition = new Point3D();
                    CurrentCommand = null;
                    CommandIndex = -1;

                    NotifyOfPropertyChange(() => GCode);
                    NotifyOfPropertyChange(() => CanStart);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        public void StartSequence()
        {


            if (_commandIndex < 0 || _commandIndex >= Commands.Count)
            {
                CommandIndex = 0;
                Points = null;
            }

            if (Points == null || _commandIndex == 0)
            {
                Points = new System.Windows.Media.Media3D.Point3DCollection();
                HeadPosition = new Point3D();
                CurrentCommand = null;

                NotifyOfPropertyChange(() => HeadPosition);
            }

            if (_timer != null && _timer.Enabled)
            {
                StopTimer();
                return;
            }



            StartTimer();
        }

        private void DrawNext()
        {
            if (_commandIndex > Commands.Count - 1)
            {
                StopTimer();
                return;
            }

            if (Commands[_commandIndex] == null)
            {
                CommandIndex++;
                return;
            }

            if (CurrentCommand == null)
            {
                CurrentCommand = Commands[_commandIndex];
            }

            if (CurrentCommand.CurrentPos == null || CurrentCommand.TargetPos == null)
            {
                lock (CurrentCommand)
                {
                    _step.X = CurrentCommand.DestinationX == null ? 0 :
                        HeadPosition.X > CurrentCommand.DestinationX.Value ? -HeadSpeed : HeadSpeed;
                    _step.Y = CurrentCommand.DestinationY == null ? 0 :
                        HeadPosition.Y > CurrentCommand.DestinationY ? -HeadSpeed : HeadSpeed;
                    _step.Z = CurrentCommand.DestinationZ == null ? 0 :
                        HeadPosition.Z > CurrentCommand.DestinationZ ? -HeadSpeed : HeadSpeed;

                    CurrentCommand.CurrentPos = new Point3D(HeadPosition.X, HeadPosition.Y, HeadPosition.Z);
                    CurrentCommand.TargetPos = new Point3D(CurrentCommand.DestinationX.HasValue ? CurrentCommand.DestinationX.Value : HeadPosition.X,
                        CurrentCommand.DestinationY.HasValue ? CurrentCommand.DestinationY.Value : HeadPosition.Y,
                        CurrentCommand.DestinationZ.HasValue ? CurrentCommand.DestinationZ.Value : HeadPosition.Z);

                    if (CurrentCommand.CommandType == GCommandType.G03)
                    {
                        CurrentCommand.Vertices = CreateArcBetweenPoins(CurrentCommand.CurrentPos.X, CurrentCommand.CurrentPos.Y, CurrentCommand.TargetPos.X, CurrentCommand.TargetPos.Y, CurrentCommand.ArcRadius, true, true, true);
                    }
                    else if (CurrentCommand.CommandType == GCommandType.G02)
                    {
                        CurrentCommand.Vertices = CreateArcBetweenPoins(CurrentCommand.CurrentPos.X, CurrentCommand.CurrentPos.Y, CurrentCommand.TargetPos.X, CurrentCommand.TargetPos.Y, CurrentCommand.ArcRadius, false, true, false);
                    }

                    UpdateCurrent();
                }
            }
            else
            {
                lock (CurrentCommand)
                {
                    UpdateCurrent();
                }
            }
        }

        private void UpdateCurrent()
        {
            if (CurrentCommand.CommandType == GCommandType.G03 && CurrentCommand.Vertices.Length > 1)
            {
                if (CurrentCommand.VertexIndex < CurrentCommand.Vertices.Length - 1)
                {
                    var v1 = CurrentCommand.Vertices[CurrentCommand.VertexIndex];
                    var v2 = CurrentCommand.Vertices[CurrentCommand.VertexIndex + 1];
                    var z = CurrentCommand.CurrentPos.Z;

                    //App.Current.Dispatcher.Invoke(() =>
                    //{
                    lock (Points)
                    {
                        Points.Add(new System.Windows.Media.Media3D.Point3D(v1.X, v1.Y, z));
                        Points.Add(new System.Windows.Media.Media3D.Point3D(v2.X, v2.Y, z));
                    }
                    //});

                    CurrentCommand.CurrentPos.X = v2.X;
                    CurrentCommand.CurrentPos.Y = v2.Y;
                    CurrentCommand.CurrentPos.Z = CurrentCommand.TargetPos.Z;

                    CurrentCommand.VertexIndex += 1;
                }
                else
                {
                    CurrentCommand.VertexIndex = 0;

                    CurrentCommand.CurrentPos.X = CurrentCommand.TargetPos.X;
                    CurrentCommand.CurrentPos.Y = CurrentCommand.TargetPos.Y;
                    CurrentCommand.CurrentPos.Z = CurrentCommand.TargetPos.Z;
                }
            }
            else if (CurrentCommand.CommandType == GCommandType.G02)
            {
                if (CurrentCommand.VertexIndex < CurrentCommand.Vertices.Length - 1)
                {
                    var v1 = CurrentCommand.Vertices[CurrentCommand.VertexIndex];
                    var v2 = CurrentCommand.Vertices[CurrentCommand.VertexIndex + 1];
                    var z = CurrentCommand.CurrentPos.Z;

                    //App.Current.Dispatcher.Invoke(() =>
                    //{
                    lock (Points)
                    {
                        Points.Add(new System.Windows.Media.Media3D.Point3D(v1.X, v1.Y, z));
                        Points.Add(new System.Windows.Media.Media3D.Point3D(v2.X, v2.Y, z));
                    }
                    // });

                    CurrentCommand.CurrentPos.X = v2.X;
                    CurrentCommand.CurrentPos.Y = v2.Y;
                    CurrentCommand.CurrentPos.Z = CurrentCommand.TargetPos.Z;

                    CurrentCommand.VertexIndex += 1;
                }
                else
                {
                    CurrentCommand.VertexIndex = 0;

                    CurrentCommand.CurrentPos.X = CurrentCommand.TargetPos.X;
                    CurrentCommand.CurrentPos.Y = CurrentCommand.TargetPos.Y;
                    CurrentCommand.CurrentPos.Z = CurrentCommand.TargetPos.Z;
                }
            }
            else
            {
                if (_step.X > 0)
                {
                    CurrentCommand.CurrentPos.X = CurrentCommand.CurrentPos.X + _step.X >
                    CurrentCommand.TargetPos.X ? CurrentCommand.TargetPos.X : CurrentCommand.CurrentPos.X + _step.X;
                }

                if (_step.X < 0)
                {
                    CurrentCommand.CurrentPos.X = CurrentCommand.CurrentPos.X + _step.X <
                        CurrentCommand.TargetPos.X ? CurrentCommand.TargetPos.X : CurrentCommand.CurrentPos.X + _step.X;

                }

                if (_step.Y > 0)
                {
                    CurrentCommand.CurrentPos.Y = CurrentCommand.CurrentPos.Y + _step.Y >
                        CurrentCommand.TargetPos.Y ? CurrentCommand.TargetPos.Y : CurrentCommand.CurrentPos.Y + _step.Y;

                }

                if (_step.Y < 0)
                {
                    CurrentCommand.CurrentPos.Y = CurrentCommand.CurrentPos.Y + _step.Y <
                        CurrentCommand.TargetPos.Y ? CurrentCommand.TargetPos.Y : CurrentCommand.CurrentPos.Y + _step.Y;

                }

                if (_step.Z > 0)
                {
                    CurrentCommand.CurrentPos.Z = CurrentCommand.CurrentPos.Z + _step.Z >
                        CurrentCommand.TargetPos.Z ? CurrentCommand.TargetPos.Z : CurrentCommand.CurrentPos.Z + _step.Z;
                }
                if (_step.Z < 0)
                {
                    CurrentCommand.CurrentPos.Z = CurrentCommand.CurrentPos.Z + _step.Z <
                        CurrentCommand.TargetPos.Z ? CurrentCommand.TargetPos.Z : CurrentCommand.CurrentPos.Z + _step.Z;
                }
            }

            if (CurrentCommand.CommandType == GCommandType.G01 || CurrentCommand.CommandType == GCommandType.G1)
            {
                var hx = HeadPosition.X;
                var hy = HeadPosition.Y;
                var hz = HeadPosition.Z;

                var x = CurrentCommand.CurrentPos.X;
                var y = CurrentCommand.CurrentPos.Y;
                var z = CurrentCommand.CurrentPos.Z;

                if (CurrentCommand.DestinationZ != null && CurrentCommand.DestinationX == null && CurrentCommand.DestinationY == null)
                {
                    //ToDo: CutInPath
                    //App.Current.Dispatcher.Invoke(() =>
                    //{
                    lock (Points)
                    {
                        Points.Add(new System.Windows.Media.Media3D.Point3D(hx, hy, hz));
                        Points.Add(new System.Windows.Media.Media3D.Point3D(x, y, z));
                    }
                    //});
                }
                else
                {
                    //ToDo: CutPath
                    //App.Current.Dispatcher.Invoke(() =>
                    //{
                    lock (Points)
                    {
                        Points.Add(new System.Windows.Media.Media3D.Point3D(hx, hy, hz));
                        Points.Add(new System.Windows.Media.Media3D.Point3D(x, y, z));
                    }
                    //});
                }
            }
            else if (ShowTravelLines && CurrentCommand.CommandType == GCommandType.G0)
            {
                var hx = HeadPosition.X;
                var hy = HeadPosition.Y;
                var hz = HeadPosition.Z;

                var x = CurrentCommand.CurrentPos.X;
                var y = CurrentCommand.CurrentPos.Y;
                var z = CurrentCommand.CurrentPos.Z;

                //ToDo: ToolMovePath
                //App.Current.Dispatcher.Invoke(() =>
                //{
                lock (Points)
                {
                    Points.Add(new System.Windows.Media.Media3D.Point3D(hx, hy, hz));
                    Points.Add(new System.Windows.Media.Media3D.Point3D(x, y, z));
                }
                //});
            }

            if (CurrentCommand == null || CurrentCommand.CurrentPos == null)
                return;

            HeadPosition.X = CurrentCommand.CurrentPos.X;
            HeadPosition.Y = CurrentCommand.CurrentPos.Y;
            HeadPosition.Z = CurrentCommand.CurrentPos.Z;

            NotifyOfPropertyChange(() => HeadPosition);

            if (CurrentCommand.CurrentPos.X == CurrentCommand.TargetPos.X &&
                CurrentCommand.CurrentPos.Y == CurrentCommand.TargetPos.Y
                && CurrentCommand.CurrentPos.Z == CurrentCommand.TargetPos.Z)
            {
                CurrentCommand = null;
                CommandIndex++;
            }
        }

        Point3D[] CreateArcBetweenPoins(
    float x1, float y1, float x2, float y2, float radius, bool arcDirection, bool useBiggerArc, bool reverce)
        {
            // distance between points
            float distance = (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
            // halfway point
            float xAverage = (x1 + x2) / 2.0f;
            float yAverage = (y1 + y2) / 2.0f;
            // circle center
            float xCenter = (float)Math.Sqrt(radius * radius - distance * distance / 4.0) * (y1 - y2) / distance;
            float yCenter = (float)Math.Sqrt(radius * radius - distance * distance / 4.0) * (x2 - x1) / distance;
            xCenter = xAverage + (arcDirection ? xCenter : -xCenter);
            yCenter = yAverage + (arcDirection ? yCenter : -yCenter);
            // angles
            float angle1 = (float)Math.Atan2(x1 - xCenter, y1 - yCenter);
            float angle2 = (float)Math.Atan2(x2 - xCenter, y2 - yCenter);
            // create the arc
            return CreateArc(angle1, angle2, radius, xCenter, yCenter, useBiggerArc, reverce);
        }

        Point3D[] CreateArc(float angle1, float angle2, float radius, float x, float y, bool useBiggerArc, bool reverce)
        {
            // Prepare angles
            angle1 = NormalizeAngleToSmallestPositive(angle1);
            angle2 = NormalizeAngleToSmallestPositive(angle2);
            if (angle1 > angle2)
            {
                float buffer = angle1;
                angle1 = angle2;
                angle2 = buffer;
            }
            if (useBiggerArc && angle2 - angle1 > (float)Math.PI)
            {
                angle1 += (float)Math.PI * 2;
            }

            var ARC_VERTEX_COUNT = /*10 **/ ViewScale;
            List<Point3D> virtices = new List<Point3D>(ARC_VERTEX_COUNT);
            for (int i = 0; i < ARC_VERTEX_COUNT; i++)
            {
                virtices.Add(
                    new Point3D((float)Math.Sin((float)i / (ARC_VERTEX_COUNT - 1) * (angle2 - angle1) + angle1) * radius + x,
                    (float)Math.Cos((float)i / (ARC_VERTEX_COUNT - 1) * (angle2 - angle1) + angle1) * radius + y,
                    HeadPosition.Z));
            }

            if (reverce)
                virtices.Reverse();

            return virtices.ToArray();
        }

        float NormalizeAngleToSmallestPositive(float angle)
        {
            while (angle < 0.0) { angle += (float)Math.PI * 2; }
            while (angle >= Math.PI * 2) { angle -= (float)Math.PI * 2; }
            return angle;
        }

        public void OnClose()
        {
            StopTimer();
        }

        private void StopTimer()
        {
            if (_timer != null)
            {
                lock (_timer)
                {
                    _timer.Stop();
                    _timer.Dispose();
                    _timer = null;
                }
            }
        }

        private void StartTimer()
        {
            if (_timer == null)
            {
                _timer = new Timer(10 * HeadSpeed);
                _timer.Elapsed += _timer_Elapsed;
                _timer.Start();
            }
        }
    }
}