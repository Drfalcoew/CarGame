using System;
using System.Collections.Generic;

using CoreGraphics;
using Foundation;
using SceneKit;
using UIKit;

namespace CarGame
{
    public partial class GameViewController : UIViewController {

        // Initializing Scene objects
        SCNView gameView;
        SCNScene gameScene;
        SCNNode cameraNode;
        SCNPyramid tree;
        SCNNode treeNode;
        SCNFloor floor;
        SCNNode floorNode;
        SCNBox userShape;
        SCNNode user;


        // Initializing View objects

        UIButton leftBtn;
        UIButton rightBtn;



        protected GameViewController(IntPtr handle) : base(handle) {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void DidReceiveMemoryWarning(){
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public void InitView() {
            gameView = View as SCNView;
            gameView.AutoenablesDefaultLighting = true;
        }

        public void InitScene() {
            gameScene = new SCNScene();
            gameView.Scene = gameScene;
            gameView.Playing = true;
        }

        public void InitCamera(){ // setting up camera
                                    
            gameView.AllowsCameraControl = true;
            float pi = (float)Math.PI;
            cameraNode = new SCNNode();
            cameraNode.Camera = new SCNCamera();
            cameraNode.Camera.FocalDistance = 20.0f;
            cameraNode.Position = new SCNVector3(0, 90, 0);
            cameraNode.EulerAngles = new SCNVector3(-pi/2, -pi/2, 0.0f);
            cameraNode.Camera.FieldOfView = 120;
            cameraNode.Camera.UsesOrthographicProjection = false;
            gameScene.RootNode.AddChildNode(cameraNode);
        }

        public void CreateTrees(){

            tree = SCNPyramid.Create(23, 60, 23);
            treeNode = SCNNode.FromGeometry(tree);
            treeNode.Position = new SCNVector3(0, 0, 0);
            treeNode.CastsShadow = true;
            gameScene.RootNode.AddChildNode(treeNode);

        }

        public void CreateLight(){
            float pi = (float)Math.PI;
            var light = SCNLight.Create();
            var lightNode = SCNNode.Create();

            light.LightType = SCNLightType.Omni;
            light.Color = UIColor.White;
            lightNode.Light = light;
            lightNode.Position = new SCNVector3(-90, 40, 60);
            gameScene.RootNode.AddChildNode(lightNode);

            var directionLight = SCNNode.Create();
            directionLight.Light = SCNLight.Create();
            directionLight.Light.LightType = SCNLightType.Directional;
            directionLight.Light.CastsShadow = true;
            directionLight.Light.ShadowMode = SCNShadowMode.Deferred;
            directionLight.Light.CategoryBitMask = (System.nuint)(-1);
            directionLight.Light.AutomaticallyAdjustsShadowProjection = true;
            directionLight.Light.MaximumShadowDistance = 50;
            directionLight.Position = new SCNVector3(-90, 40, 60);
            directionLight.Rotation = new SCNVector4(x: -1, y: 0, z: 0, w: pi / 2);
            directionLight.Light.ShadowColor = new UIColor(white: 0, alpha: 0.5f);

            gameScene.RootNode.AddChildNode(directionLight);
      
        }

        public void CreateFloor() {
            var floorMaterial = new SCNMaterial();
            floorMaterial.Diffuse.Contents = new UIImage("art.scnassets/Floor.jpg");


            floor = SCNFloor.Create();
            floor.Width = 200;
            floor.Length = 200;
            floor.FirstMaterial = floorMaterial;
            floor.Reflectivity = 0;
            floorNode = SCNNode.FromGeometry(floor);
            floorNode.Geometry.FirstMaterial.Diffuse.WrapS = SCNWrapMode.Repeat;
            floorNode.Geometry.FirstMaterial.Diffuse.WrapT = SCNWrapMode.Repeat;
            floorNode.Position = new SCNVector3(0, 0, 0);
            floorNode.CastsShadow = true;
            gameScene.RootNode.AddChildNode(floorNode);
        }


        public void CreateUser() {
            userShape = SCNBox.Create(8.0f, 7.0f, 15.0f, 0.005f);  
            user = SCNNode.FromGeometry(userShape);  // Creating a new node, user, that inherits the shape of the SCNBox, userShape
            user.Position = new SCNVector3(10, 4, -50);  
            user.PhysicsBody = SCNPhysicsBody.  // Trying to set a physics body for the user in order for us to give controls to it
            gameScene.RootNode.AddChildNode(user);
        }


        public void CreateButtons(){
            leftBtn = new UIButton(new CGRect(0.0, 0.0, View.Frame.Width / 2, View.Frame.Height));
            leftBtn.BackgroundColor = new UIColor(0.5f, 0.5f);
            leftBtn.AddTarget(ButtonEventHandler, UIControlEvent.TouchDown);

            rightBtn = new UIButton(new CGRect(View.Frame.Width / 2, 0.0, View.Frame.Width / 2, View.Frame.Height));
            rightBtn.BackgroundColor = new UIColor(43, 0, 0, 0.5f);
            rightBtn.AddTarget(ButtonEventHandler, UIControlEvent.TouchDown);


            View.AddSubview(rightBtn);
            View.AddSubview(leftBtn);
        }


        public void ButtonEventHandler(object sender, EventArgs e)
        {
            if (sender == leftBtn) {
                Console.WriteLine("LeftButton Touched");
            } else if (sender == rightBtn) {
                Console.WriteLine("RightButton Touched");
            }
        }
        public override void ViewDidLoad(){
            base.ViewDidLoad();


            InitView();
            InitScene();
            InitCamera();
            CreateTrees();
            CreateFloor();
            CreateLight();
            CreateUser();
            CreateButtons();
            // create a new scene

        }

        public override bool ShouldAutorotate(){
            return true;
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            Console.WriteLine(cameraNode.Camera.FieldOfView);
        }


        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(){
            return UIInterfaceOrientationMask.AllButUpsideDown;
        }
    }
}
