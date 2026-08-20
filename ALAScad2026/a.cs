using HelixToolkit.Geometry;
using HelixToolkit.Maths;
using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ALAScad2026
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}



using System;
using System.Windows;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;

namespace BimViewerApp
{
    public partial class MainWindow : Window
    {
        // Viewport camera properties
        private OrthographicCamera3D camera;

        public MainWindow()
        {
            InitializeComponent();
            InitializeEngine();
        }

        private void InitializeEngine()
        {
            // Set up a standard orthographic engineering camera
            camera = new OrthographicCamera3D()
            {
                Position = new System.Windows.Media.Media3D.Point3D(30, 30, 30),
                LookDirection = new System.Windows.Media.Media3D.Vector3D(-30, -30, -30),
                UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 1, 0),
                Width = 40
            };

            view3D.Camera = camera;

            // Assign the rendering hardware controller context
            view3D.EffectsManager = new DefaultEffectsManager();
        }

        private void OnAddBeamClick(object sender, RoutedEventArgs e)
        {
            // 1. Build a basic box mesh structure (Representing a basic beam segment)
            var meshBuilder = new MeshBuilder();
            // Create a box at center(0,2,0) with structural length x=12, height y=2, width z=1
            meshBuilder.AddBox(new Vector3(0, 2, 0), 12, 2, 1);

            var beamMesh = meshBuilder.ToMeshGeometry3D();

            // 2. Define the Material Profile (Tekla-Style Glossy Painted Steel)
            var steelMaterial = new PhongMaterial()
            {
                AmbientColor = new Color4(0.1f, 0.2f, 0.4f, 1.0f),
                DiffuseColor = new Color4(0.2f, 0.5f, 0.8f, 1.0f), // Industrial Steel Blue
                SpecularColor = new Color4(1.0f, 1.0f, 1.0f, 1.0f),
                SpecularShininess = 32f
            };

            // 3. Package the geometry data into a high-performance render model
            var beamModel = new MeshGeometryModel3D()
            {
                Geometry = beamMesh,
                Material = steelMaterial,
                CullMode = SharpDX.Direct3D11.CullMode.Back // Performance optimization: don't render inner hidden faces
            };

            // 4. Inject object dynamically into the live viewport scene graph
            BimGroupModel.Children.Add(beamModel);
        }
    }
}
