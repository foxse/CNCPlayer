using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

using SharpGL;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Cameras;
using SharpGL.SceneGraph.Collections;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Lighting;
using SharpGL.SceneGraph.Effects;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Quadrics;
using SharpGL.Enumerations;
using SharpGL.SceneGraph.Assets;
using System.IO;
using System.Linq;

namespace SceneSample
{
    public partial class FormSceneSample : Form
    {
        List<string> _gCode;

        int _commandIndex = 0;
        public int CommandIndex
        {
            get{ return _commandIndex; }
            set
            {
                _commandIndex = value;
            }
        }

        private float headSpeed = 0.1f;
        public float HeadSpeed
        {
            get{ return headSpeed / ViewScale; }
            set{ headSpeed = value * ViewScale; }
        }

        public float ViewScale { get; set; } = 1 / 100f;

        public Point HeadPosition = new Point();
        Point _step = new Point();

        List<GCommand> _commands = new List<GCommand>();

        Material red = new Material
        {
            Diffuse = Color.Red
        };

        Material yellow = new Material
        {
            Diffuse = Color.Yellow
        };

        Material green = new Material
        {
            Diffuse = Color.Green
        };

        Material orange = new Material
        {
            Diffuse = Color.Orange
        };

        Polygon CutPath;
        Polygon ToolMovePath;
        Polygon CutInPath;

        //private OpenGL _gl;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormSceneSample"/> class.
        /// </summary>
        public FormSceneSample()
        {
            InitializeComponent();

            sceneControl.MouseDown += new MouseEventHandler(FormSceneSample_MouseDown);
            sceneControl.MouseMove += new MouseEventHandler(FormSceneSample_MouseMove);
            sceneControl.MouseUp += new MouseEventHandler(sceneControl1_MouseUp);
            sceneControl.MouseWheel += new MouseEventHandler(sceneControl1_MouseWheel);

            CreateScene();
        }

        private void CreateScene()
        {
            var root = sceneControl.Scene.SceneContainer.Children;
            root.Clear();

            var axes = new Axies();
            var grid = new Grid();

            var lti = new LinearTransformationEffect();
            lti.LinearTransformation.TranslateX = 10f;
            lti.LinearTransformation.TranslateY = 10f;
            lti.LinearTransformation.ScaleX = 1f;
            lti.LinearTransformation.ScaleY = 1f;
            lti.LinearTransformation.ScaleZ = 1f;

            //grid.Effects.Add(lti);
            grid.Effects.Add(arcBallEffect);

            var axisLti = new LinearTransformationEffect();
            axisLti.LinearTransformation.ScaleX = .1f;
            axisLti.LinearTransformation.ScaleY = .1f;
            axisLti.LinearTransformation.ScaleZ = .1f;

            axes.Effects.Add(axisLti);
            axes.Effects.Add(arcBallEffect);

            sceneControl.Scene.RenderBoundingVolumes = false;

            transformationEffect.LinearTransformation.ScaleX = ViewScale * 10;
            transformationEffect.LinearTransformation.ScaleY = ViewScale * 10;
            transformationEffect.LinearTransformation.ScaleZ = ViewScale * 10;

            sceneControl.Scene.SceneContainer.AddEffect(transformationEffect);

            root.Add(grid);
            root.Add(axes);

            Light light = new Light()
            {
                On = true,
                Position = new Vertex(0, 0, 10),
                GLCode = OpenGL.GL_LIGHT0
            };

            sceneControl.Scene.SceneContainer.AddChild(light);

            transformationEffect.LinearTransformation.ScaleX = 1f;
            transformationEffect.LinearTransformation.ScaleY = 1f;
            transformationEffect.LinearTransformation.ScaleZ = 1f;
        }

        private void LoadCodeFromFile()
        {
            var ofd = new OpenFileDialog();
            var dialogResult = ofd.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                try
                {
                    _gCode = File.ReadAllLines(ofd.FileName).ToList();
                    
                    GCodeParser.DropCounters();
                    
                    foreach (var line in _gCode)
                    {
                        var command = GCodeParser.Parse(line, ViewScale, checkBoxLineNumbers.Checked);
                        
                        _commands.Add(command);
                    }

                    listBox1.DataSource = _gCode;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void sceneControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            var scale = (e.Delta > 0 ? 1 : transformationEffect.LinearTransformation.ScaleX > 1 ? - 1 : 0) * ViewScale * 10;
            
            transformationEffect.LinearTransformation.ScaleX += scale;
            transformationEffect.LinearTransformation.ScaleY += scale;
            transformationEffect.LinearTransformation.ScaleZ += scale;
        }

        void sceneControl1_MouseUp(object sender, MouseEventArgs e)
        {
            arcBallEffect.ArcBall.MouseUp(e.X, e.Y);
        }

        private void StartSequence()
        {
            if (CutPath != null)
                sceneControl.Scene.SceneContainer.RemoveChild(CutPath);

            CutPath = new Polygon();
            CutPath.Material = green;
            CutPath.AddEffect(arcBallEffect);

            sceneControl.Scene.SceneContainer.AddChild(CutPath);

            if (ToolMovePath != null)
                sceneControl.Scene.SceneContainer.RemoveChild(ToolMovePath);

            ToolMovePath = new Polygon();
            ToolMovePath.Material = yellow;
            ToolMovePath.AddEffect(arcBallEffect);

            sceneControl.Scene.SceneContainer.AddChild(ToolMovePath);

            if (CutInPath != null)
                sceneControl.Scene.SceneContainer.RemoveChild(CutInPath);

            CutInPath = new Polygon();
            CutInPath.Material = red;
            CutInPath.AddEffect(arcBallEffect);

            sceneControl.Scene.SceneContainer.AddChild(CutInPath);

            HeadPosition = new Point();
            CurrentCommand = null;

            timer.Start();
        }

        int prevMouseX;
        int prevMouseY;

        void FormSceneSample_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                arcBallEffect.ArcBall.MouseMove(e.X, e.Y);
                //arcBallEffect.ArcBall.
                //var deltaX = e.X - prevMouseX;
                //var deltaY = e.Y - prevMouseY;
                //transformationEffect.LinearTransformation.RotateZ += deltaX;
                //transformationEffect.LinearTransformation.RotateY -= deltaY;
                //prevMouseX = e.X;
                //prevMouseY = e.Y;
            }
            else if (e.Button == MouseButtons.Middle)
            {
                var deltaX = (e.X - prevMouseX) / 2;
                var deltaY = (e.Y - prevMouseY) / 2;
                transformationEffect.LinearTransformation.TranslateX += deltaX;
                transformationEffect.LinearTransformation.TranslateY -= deltaY;
                prevMouseX = e.X;
                prevMouseY = e.Y;
            }
        }

        void FormSceneSample_MouseDown(object sender, MouseEventArgs e)
        {
            arcBallEffect.ArcBall.SetBounds(sceneControl.Width, sceneControl.Height);
            arcBallEffect.ArcBall.MouseDown(e.X, e.Y);

            prevMouseX = e.X;
            prevMouseY = e.Y;
        }
        private ArcBallEffect arcBallEffect = new ArcBallEffect();
        private LinearTransformationEffect transformationEffect = new LinearTransformationEffect();

        /// <summary>
        /// Adds the element to tree.
        /// </summary>
        /// <param name="sceneElement">The scene element.</param>
        /// <param name="nodes">The nodes.</param>
        private void AddElementToTree(SceneElement sceneElement, TreeNodeCollection nodes)
        {
            //  Add the element.
            TreeNode newNode = new TreeNode() 
            { 
                Text = sceneElement.Name, 
                Tag = sceneElement 
            };
            nodes.Add(newNode);

            //  Add each child.
            foreach (var element in sceneElement.Children)
                AddElementToTree(element, newNode.Nodes);
        }

        /// <summary>
        /// Handles the AfterSelect event of the treeView1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeViewEventArgs"/> instance containing the event data.</param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectedSceneElement = e.Node.Tag as SceneElement;
        }

        /// <summary>
        /// Called when [selected scene element changed].
        /// </summary>
        private void OnSelectedSceneElementChanged()
        {
            propertyGrid1.SelectedObject = SelectedSceneElement;
        }

        /// <summary>
        /// The selected scene element.
        /// </summary>
        private SceneElement selectedSceneElement = null;

        /// <summary>
        /// Gets or sets the selected scene element.
        /// </summary>
        /// <value>
        /// The selected scene element.
        /// </value>
        public SceneElement SelectedSceneElement
        {
            get { return selectedSceneElement; }
            set
            {
                selectedSceneElement = value;
                OnSelectedSceneElementChanged();
            }
        }

        private void sceneControl1_OpenGLInitialized(object sender, EventArgs e)
        {
            //OpenGL gl = this.sceneControl1.OpenGL;
            //_gl.ClearColor(0f, 0f, 0f, 0f);
        }

        private void sceneControl1_OpenGLDraw(object sender, RenderEventArgs args)
        {
            //_gl.LoadIdentity();

            //_gl.Rotate(transformationEffect.LinearTransformation.RotateX, transformationEffect.LinearTransformation.RotateY,
            //    transformationEffect.LinearTransformation.RotateZ);
            //_gl.Translate(transformationEffect.LinearTransformation.TranslateX, transformationEffect.LinearTransformation.TranslateY, transformationEffect.LinearTransformation.TranslateZ);
            //_gl.Scale(transformationEffect.LinearTransformation.ScaleX, transformationEffect.LinearTransformation.ScaleY, transformationEffect.LinearTransformation.ScaleZ);

            //_gl.PolygonMode(FaceMode.FrontAndBack, PolygonMode.Filled);
            //_gl.LineWidth(1.5f);
            //_gl.Color(1f, 0f, 0f, 0.5f);
            //_gl.Disable(3553u);

            //_gl.PushMatrix();
            //_gl.Begin(2u);

            //_gl.Color(0f, 1f, 0f, 0.5f);
            //_gl.Vertex(1, 1, 0);
            //_gl.Vertex(2, 2, 0);
            //_gl.End();
            //_gl.PopMatrix();
        }

        public GCommand CurrentCommand { get; set; }

        private void timer_Tick(object sender, EventArgs e)
        {
            DrawNext();
        }

        private void DrawNext()
        {
            if (_commandIndex > listBox1.Items.Count - 1)
            {
                timer.Stop();
                return;
            }

            listBox1.SelectedIndex = _commandIndex;

            if (_commands[_commandIndex] == null)
            {
                _commandIndex++;
                return;
            }

            if (CurrentCommand == null)
            {
                CurrentCommand = _commands[_commandIndex];

                _step.X = CurrentCommand.DestinationX == null ? 0 :
                    HeadPosition.X > CurrentCommand.DestinationX.Value ? -headSpeed : headSpeed;
                _step.Y = CurrentCommand.DestinationY == null ? 0 :
                    HeadPosition.Y > CurrentCommand.DestinationY ? -headSpeed : headSpeed;
                _step.Z = CurrentCommand.DestinationZ == null ? 0 :
                    HeadPosition.Z > CurrentCommand.DestinationZ ? -headSpeed : headSpeed;

                CurrentCommand.CurrentPos = new Point(HeadPosition.X, HeadPosition.Y, HeadPosition.Z);
                CurrentCommand.TargetPos = new Point(CurrentCommand.DestinationX.HasValue ? CurrentCommand.DestinationX.Value : HeadPosition.X,
                    CurrentCommand.DestinationY.HasValue ? CurrentCommand.DestinationY.Value : HeadPosition.Y,
                    CurrentCommand.DestinationZ.HasValue ? CurrentCommand.DestinationZ.Value : HeadPosition.Z);

                if (CurrentCommand.Command == CommandType.G03)
                {
                    CurrentCommand.Vertices = CreateArcBetweenPoins(CurrentCommand.CurrentPos.X, CurrentCommand.CurrentPos.Y, CurrentCommand.TargetPos.X, CurrentCommand.TargetPos.Y, CurrentCommand.ArcRadius, true, true, true);
                }
                else if (CurrentCommand.Command == CommandType.G02)
                {
                    CurrentCommand.Vertices = CreateArcBetweenPoins(CurrentCommand.CurrentPos.X, CurrentCommand.CurrentPos.Y, CurrentCommand.TargetPos.X, CurrentCommand.TargetPos.Y, CurrentCommand.ArcRadius, false, true, false);
                }

                propertyGrid1.SelectedObject = CurrentCommand;
            }

            if (CurrentCommand.Command == CommandType.G03 && CurrentCommand.Vertices.Length > 1)
            {
                if (CurrentCommand.VertexIndex < CurrentCommand.Vertices.Length - 1)
                {
                    var v1 = CurrentCommand.Vertices[CurrentCommand.VertexIndex];
                    var v2 = CurrentCommand.Vertices[CurrentCommand.VertexIndex + 1];

                    CutPath.AddFaceFromVertexData(new Vertex[] {
                        new Vertex(v1.X, v1.Y, CurrentCommand.CurrentPos.Z),
                        new Vertex(v2.X, v2.Y, CurrentCommand.CurrentPos.Z)
                    });

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
            else if (CurrentCommand.Command == CommandType.G02)
            {
                if (CurrentCommand.VertexIndex < CurrentCommand.Vertices.Length - 1)
                {
                    var v1 = CurrentCommand.Vertices[CurrentCommand.VertexIndex];
                    var v2 = CurrentCommand.Vertices[CurrentCommand.VertexIndex + 1];

                    CutPath.AddFaceFromVertexData(new Vertex[] {
                        new Vertex(v1.X, v1.Y, CurrentCommand.CurrentPos.Z),
                        new Vertex(v2.X, v2.Y, CurrentCommand.CurrentPos.Z)
                    });

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

            if (CurrentCommand.Command == CommandType.G01 || CurrentCommand.Command == CommandType.G1)
            {
                if (CurrentCommand.DestinationZ != null && CurrentCommand.DestinationX == null && CurrentCommand.DestinationY == null)
                {
                    CutInPath.AddFaceFromVertexData(new Vertex[] {
                    new Vertex(HeadPosition.X, HeadPosition.Y, HeadPosition.Z),
                    new Vertex(CurrentCommand.CurrentPos.X, CurrentCommand.CurrentPos.Y, CurrentCommand.CurrentPos.Z)
                });
                }
                else
                {
                    CutPath.AddFaceFromVertexData(new Vertex[] {
                        new Vertex(HeadPosition.X, HeadPosition.Y, HeadPosition.Z),
                        new Vertex(CurrentCommand.CurrentPos.X, CurrentCommand.CurrentPos.Y, CurrentCommand.CurrentPos.Z)
                    });
                }
            }
            else if (CurrentCommand.Command == CommandType.G0)
            {
                ToolMovePath.AddFaceFromVertexData(new Vertex[] {
                    new Vertex(HeadPosition.X, HeadPosition.Y, HeadPosition.Z),
                    new Vertex(CurrentCommand.CurrentPos.X, CurrentCommand.CurrentPos.Y, CurrentCommand.CurrentPos.Z)
                });
            }

            HeadPosition.X = CurrentCommand.CurrentPos.X;
            HeadPosition.Y = CurrentCommand.CurrentPos.Y;
            HeadPosition.Z = CurrentCommand.CurrentPos.Z;

            if (CurrentCommand.CurrentPos.X == CurrentCommand.TargetPos.X &&
                CurrentCommand.CurrentPos.Y == CurrentCommand.TargetPos.Y
                && CurrentCommand.CurrentPos.Z == CurrentCommand.TargetPos.Z)
            {
                CurrentCommand = null;
                _commandIndex++;
            }
        }

        float NormalizeAngleToSmallestPositive(float angle)
        {
            while (angle < 0.0) { angle += (float)Math.PI * 2; }
            while (angle >= Math.PI * 2) { angle -= (float)Math.PI * 2; }
            return angle;
        }

        Vertex[] CreateArcBetweenPoins(
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

        Vertex[] CreateArc(float angle1, float angle2, float radius, float x, float y, bool useBiggerArc, bool reverce)
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

            var ARC_VERTEX_COUNT = 10;
            List<Vertex> virtices = new List<Vertex>(ARC_VERTEX_COUNT);
            for (int i = 0; i < ARC_VERTEX_COUNT; i++)
            {
                virtices.Add(
                    new Vertex((float)Math.Sin((float)i / (ARC_VERTEX_COUNT - 1) * (angle2 - angle1) + angle1) * radius + x,
                    (float)Math.Cos((float)i / (ARC_VERTEX_COUNT - 1) * (angle2 - angle1) + angle1) * radius + y, 
                    HeadPosition.Z));
            }

            if (reverce)
                virtices.Reverse();

            return virtices.ToArray();
        }

        Point Rotate(float cx, float cy, float angle, Point p)
        {
            float s = (float)Math.Sin(angle);
            float c = (float)Math.Cos(angle);

            // translate point back to origin:
            p.X -= cx;
            p.Y -= cy;

            // rotate point
            float xnew = p.X * c - p.Y * s;
            float ynew = p.X * s + p.Y * c;

            // translate point back:
            p.X = xnew + cx;
            p.Y = ynew + cy;
            return p;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadCodeFromFile();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartSequence();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommandIndex = listBox1.SelectedIndex;

            if (_commands.Count > _commandIndex)
            {
                propertyGrid1.SelectedObject = _commands[_commandIndex];
            }
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            
            StartSequence();
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            LoadCodeFromFile();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Enabled = !timer.Enabled;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartSequence();
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = !timer.Enabled;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("GCode Player");
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            timer.Stop();
            CommandIndex = 0;
            HeadPosition = new Point();
        }

        private void freeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ToolMovePath.IsEnabled = freeCheckBox.Checked;
        }

        private void cutInCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CutInPath.IsEnabled = cutInCheckBox.Checked;
        }

        private void cutCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CutPath.IsEnabled = cutCheckBox.Checked;
        }
    }
}