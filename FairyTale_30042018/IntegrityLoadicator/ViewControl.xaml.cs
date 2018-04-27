using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Drawing;
//using System.Reflection;

#region CADLIB
using WW.Cad.Base;
using WW.Cad.Drawing;
using WW.Drawing;
using WW.Cad.Drawing.GDI;
using WW.Cad.IO;
using WW.Cad.Model;
using WW.Cad.Model.Entities;
using WW.Math;
using WW.Cad.Drawing.Wpf;
using WW.Cad.Model.Objects;
using WW.Math.Geometry;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
#endregion 

namespace ZebecLoadMaster
{
    /// <summary>
    /// Interaction logic for ViewControl.xaml
    /// </summary>
    public partial class ViewControl : UserControl
    {
        private Bounds3D bounds;
        DxfModel model;
        private WpfWireframeGraphics3DUsingDrawingVisual wpfGraphics;
        private WireframeGraphics2Cache graphicsCache;
        private GraphicsConfig graphicsConfig;
        private Vector3D translation;
        private Vector3D translationAtMouseClick;
        private double scaling = 1d;
        private Point2D mouseDownLocation;
        private bool mouseDown;
        //private Point _initialPoint;
        Assembly assembly = Assembly.GetExecutingAssembly();
        private System.Windows.Point startPt;
        private int wid;
        private int hei;
        private System.Windows.Point lastLoc;
        private double CanvasLeft, CanvasTop;
        ////private bool drag = false;
        System.Windows.Shapes.Rectangle myRgbRectangle = new System.Windows.Shapes.Rectangle();
        System.Windows.Shapes.Rectangle myRgbRectangle2 = new System.Windows.Shapes.Rectangle();

        public ViewControl()
        {
            InitializeComponent();
            
        }
        public DxfModel Model
        {
            get
            {
                return model;
            }
            set
            {
                model = value;
                if (model != null)
                {
                    DxfLayout paperSpaceLayout = model.ActiveLayout;
                    if (model.Header.ShowModelSpace)
                    {
                        paperSpaceLayout = null;
                    }

                    #region calculate the model's bounds to determine a proper dots per inch

                    // The dots per inch value is important because it determines the eventual pen thickness.
                    graphicsConfig = (GraphicsConfig)GraphicsConfig.WhiteBackgroundCorrectForBackColor.Clone();
                    //GraphicsConfig.
                    BoundsCalculator boundsCalculator = new BoundsCalculator();
                    if (model.ActiveLayout == null || model.Header.ShowModelSpace)
                    {
                        boundsCalculator.GetBounds(model);
                    }
                    else
                    {
                        boundsCalculator.GetBounds(model, model.ActiveLayout);
                    }
                    bounds = boundsCalculator.Bounds;
                    Vector3D delta = bounds.Delta;
                    System.Windows.Size estimatedCanvasSize = new System.Windows.Size(200d, 200d);
                    double estimatedScale = Math.Min(estimatedCanvasSize.Width / delta.X, estimatedCanvasSize.Height / delta.Y);
                    graphicsConfig.DotsPerInch =30d / estimatedScale;
                    BoundsCalculator boundsCalculator1 = new BoundsCalculator();
                    boundsCalculator1.GetBounds(model, model.Entities[20]);
                    #endregion

                    graphicsCache = new WireframeGraphics2Cache(false, false);
                    graphicsCache.Config = graphicsConfig;
                    if (model.ActiveLayout == null || model.Header.ShowModelSpace)
                    {
                        graphicsCache.CreateDrawables(model, Matrix4D.Identity);
                       
                    }
                    else
                    {
                        graphicsCache.CreateDrawables(model, model.ActiveLayout);
                    }

                    wpfGraphics = new WpfWireframeGraphics3DUsingDrawingVisual();
                    wpfGraphics.Config = graphicsConfig;
                   
                    canvas.Children.Add(wpfGraphics.Canvas);
                  
                    UpdateWpfGraphics();
                    //CoordinatesCollector coordinatesCollector = new CoordinatesCollector();
                    //DrawContext.Wireframe drawContext =
                    //    new DrawContext.Wireframe.ModelSpace(
                    //        model,
                    //        GraphicsConfig.BlackBackground,
                    //        Matrix4D.Identity
                    //    );
                    //model.Draw(drawContext, coordinatesCollector);

                    canvas.SizeChanged += canvas_SizeChanged;
                    //int cou=canvas.Children.Count;
                    //SaveAsBitmap();

                    Bitmap bmp = CreateOneUnitToOnePixelBitmap(model, Matrix4D.Identity, graphicsConfig, SmoothingMode.Default);
                    bmp.Save(System.Windows.Forms.Application.StartupPath + "\\Images\\Img.png", ImageFormat.Png);
                }
            }
        }

       private static Bitmap CreateOneUnitToOnePixelBitmap(
      DxfModel model,
      Matrix4D transform,
      GraphicsConfig graphicsConfig,
      SmoothingMode smoothingMode)
        {
            // first calculate the size of the model
            //BoundsCalculator boundsCalculator = new BoundsCalculator(graphicsConfig);
            //boundsCalculator.GetBounds(model, transform);
            //Bounds3D bounds = boundsCalculator.Bounds;
            //Vector3D delta = bounds.Delta;
            //// now determine image size from this
            //// Note: Have to add 2 extra pixels on each side, otherwise Windows will not render
            //// the graphics on the outer edges. Also there seems to be always an empty unused line 
            //// around the bitmap, but that's not a problem, just a minor waste of space.
            //const int margin = 2;
            //int width = (int)System.Math.Ceiling(delta.X) + 2 * margin;
            //int height = (int)System.Math.Ceiling(delta.Y) + 2 * margin;
            //// Now move the model so it is centered in the coordinate ranges
            //// margin &lt;= x &lt;= width+margin and margin &lt;= y &lt;= height+margin
            //// Be careful: in DXF y points up, but in Bitmap it points down!
            //Matrix4D to2DTransform = DxfUtil.GetScaleTransform(
            //  bounds.Corner1,
            //  bounds.Corner2,
            //  new Point3D(bounds.Corner1.X, bounds.Corner2.Y, 0d),
            //  new Point3D(0d, delta.Y, 0d),
            //  new Point3D(delta.X, 0d, 0d),
            //  new Point3D(margin, margin, 0d)
            //) * transform;
            // now use standard method to create bitmap
            System.Drawing.Size maxSize = new System.Drawing.Size(1000, 600);
            return   ImageExporter.CreateAutoSizedBitmap(model, Matrix4D.Identity,
                                GraphicsConfig.WhiteBackgroundCorrectForBackColor,
                                SmoothingMode.HighQuality,
                                maxSize);
        
        }
        private void UpdateWpfGraphics()
        {
            wpfGraphics.DrawingVisuals.Clear();
            IWireframeGraphicsFactory2 graphicsFactory = wpfGraphics.CreateGraphicsFactory();
            foreach (IWireframeDrawable2 drawable in graphicsCache.Drawables)
            {
                drawable.Draw(graphicsFactory);
              
            }
        }
        /// <summary>
        /// Update the canvas RenderTransform.
        /// </summary>
        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateRenderTransform();
        }

        private void Canvas_LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mouseDown = true;
            translationAtMouseClick = translation;
            System.Windows.Point p = e.GetPosition(dockPanel);
            mouseDownLocation = new Point2D(p.X, p.Y);
            canvas.CaptureMouse();


            Cursor = Cursors.Hand;
            startPt = e.GetPosition(canvas);
            wid = (int)myRgbRectangle.Width;
            hei = (int)myRgbRectangle.Height;
            lastLoc = new System.Windows.Point(Canvas.GetLeft(myRgbRectangle), Canvas.GetTop(myRgbRectangle));
            Mouse.Capture((IInputElement)sender);
        }
       
        /// <summary>
        /// Pan if the left mouse button is down.
        /// </summary>
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                //Point p = e.GetPosition(dockPanel);
                //Point2D mouseLocation = new Point2D(p.X, p.Y);
                //Vector2D delta = mouseLocation - mouseDownLocation;
                //translation =
                //    translationAtMouseClick +
                //        new Vector3D(
                //            delta.X * 2d / canvasProfile.ActualWidth,
                //            -delta.Y * 2d / canvasProfile.ActualHeight,
                //            0d);
                //UpdateRenderTransform();
                ////Point position = e.MouseDevice.GetPosition(myRgbRectangle);
                ////myRgbRectangle.SetValue(Canvas.LeftProperty,
                ////                                 Math.Min(position.X, _initialPoint.Y));
                ////myRgbRectangle.SetValue(Canvas.TopProperty,
                ////                                 Math.Min(position.Y, _initialPoint.X));

                ////myRgbRectangle.SetValue(Canvas.
                var newX = (startPt.X + (e.GetPosition(canvas).X - startPt.X));
                var newY = (startPt.Y + (e.GetPosition(canvas).Y - startPt.Y));
                System.Windows.Point offset = new System.Windows.Point((startPt.X - lastLoc.X), (startPt.Y - lastLoc.Y));
                CanvasTop = newY - offset.Y;
                CanvasLeft = newX - offset.X;
                myRgbRectangle.SetValue(Canvas.TopProperty, CanvasTop);
                myRgbRectangle.SetValue(Canvas.LeftProperty, CanvasLeft);


            }
            if (!mouseDown)
                return;


        }

        private void Canvas_LeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
            canvas.ReleaseMouseCapture();
            //drag = false;
            Cursor = Cursors.Arrow;
            Mouse.Capture(null);
        }

        /// <summary>
        /// Zoom in/out.
        /// </summary>
        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int sign = Math.Sign(e.Delta);

            double newScaling = scaling;

            // wheel movement is forward 
            if (sign > 0)
            {
                newScaling *= 1.1d;
            }
            // wheel movement is backward 
            else if (sign < 0)
            {
                newScaling /= 1.1d;
            }
            System.Windows.Point p = e.GetPosition(dockPanel);

            // This is the post-zoom position.
            Point3D postZoomPosition =
                new Point3D(p.X / canvas.ActualWidth * 2d - 1d, -p.Y / canvas.ActualHeight * 2d + 1, 0d);

            // This is the pre-zoom position.
            Matrix4D zoomTranslation =
                Transformation4D.Translation((Vector3D)translation) *
                Transformation4D.Scaling(scaling);
            Point3D preZoomPosition = zoomTranslation.GetInverse().Transform(postZoomPosition);

            Matrix4D newZoomTranslation =
                Transformation4D.Translation((Vector3D)translation) *
                Transformation4D.Scaling(newScaling);
            Point3D uncorrectedPostZoomPosition = newZoomTranslation.Transform(preZoomPosition);

            translation += postZoomPosition - uncorrectedPostZoomPosition;
            scaling = newScaling;

            UpdateRenderTransform();
        }

        private void Canvas_LostMouseCapture(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void UpdateRenderTransform()
        {
            double canvasWidth = canvas.ActualWidth;
            double canvasHeight = canvas.ActualHeight;
            MatrixTransform baseTransform = DxfUtil.GetScaleWMMatrixTransform(
                (Point2D)bounds.Corner1,
                (Point2D)bounds.Corner2,
                (Point2D)bounds.Center,
                new Point2D(1d, canvasHeight),
                new Point2D(canvasWidth, 1d),
                new Point2D(0.5d * (canvasWidth + 1d), 0.5d * (canvasHeight + 1d))
                );

            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(baseTransform);
            transformGroup.Children.Add(new TranslateTransform()
            {
                X = -canvasWidth / 2d,
                Y = -canvasHeight / 2d
            });
            transformGroup.Children.Add(new ScaleTransform()
            {
                ScaleX = scaling,
                ScaleY = scaling
            });
            transformGroup.Children.Add(new TranslateTransform()
            {
                X = canvasWidth / 2d,
                Y = canvasHeight / 2d
            });
            transformGroup.Children.Add(new TranslateTransform()
            {
                X = translation.X * canvasWidth / 2d,
                Y = -translation.Y * canvasHeight / 2d
            });

            canvas.RenderTransform = transformGroup;
        }

        private void canvas_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
  
}
