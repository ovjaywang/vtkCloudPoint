// This sample demonstrates the vtkFormsWindowControl.
// It adds two instances of the control to the sides of a
// splitter which can be resized by the user.
// The Visual Studio designer would generate most of this
// code for you.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


namespace ControlSample
{
    public class SampleForm : Form
    {
        vtk.vtkBoxWidget m_boxWidget;

        public SampleForm()
        {
            InitializeComponent();

            this.AddConeToWindow(this.vtkFormsWindowControl1.GetRenderWindow());
            this.AddFlamingoToWindow(this.vtkFormsWindowControl2.GetRenderWindow());
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.vtkFormsWindowControl1 = new vtk.vtkFormsWindowControl();
            this.vtkFormsWindowControl2 = new vtk.vtkFormsWindowControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.vtkFormsWindowControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.vtkFormsWindowControl2);
            this.splitContainer1.Size = new System.Drawing.Size(292, 266);
            this.splitContainer1.SplitterDistance = 97;
            this.splitContainer1.TabIndex = 0;
            // 
            // vtkFormsWindowControl1
            // 
            this.vtkFormsWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vtkFormsWindowControl1.Location = new System.Drawing.Point(0, 0);
            this.vtkFormsWindowControl1.Name = "vtkFormsWindowControl1";
            this.vtkFormsWindowControl1.Size = new System.Drawing.Size(97, 266);
            this.vtkFormsWindowControl1.TabIndex = 0;
            this.vtkFormsWindowControl1.Text = "vtkFormsWindowControl1";
            // 
            // vtkFormsWindowControl2
            // 
            this.vtkFormsWindowControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vtkFormsWindowControl2.Location = new System.Drawing.Point(0, 0);
            this.vtkFormsWindowControl2.Name = "vtkFormsWindowControl2";
            this.vtkFormsWindowControl2.Size = new System.Drawing.Size(191, 266);
            this.vtkFormsWindowControl2.TabIndex = 0;
            this.vtkFormsWindowControl2.Text = "vtkFormsWindowControl2";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(499, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ControlSampleForm";
            this.Text = "Sample Form Using VTK";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }


        private System.Windows.Forms.SplitContainer splitContainer1;
        private vtk.vtkFormsWindowControl vtkFormsWindowControl1;
        private vtk.vtkFormsWindowControl vtkFormsWindowControl2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        static void myCallback(vtk.vtkObject caller, uint eventId,
            object clientData, IntPtr callData)
        {
            System.Diagnostics.Debug.WriteLine("Callback has been called.");
            vtk.vtkBoxWidget boxWidget = vtk.vtkBoxWidget.SafeDownCast(caller);
            if (null != boxWidget)
            {
                using (vtk.vtkTransform t = new vtk.vtkTransform())
                {
                    boxWidget.GetTransform(t);
                    boxWidget.GetProp3D().SetUserTransform(t);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Caller is not a box widget.");
            }
        }


        static void PrintCameraPosition(vtk.vtkObject caller, uint eventId,
                                            object clientData, IntPtr callData)
        {
            System.Diagnostics.Debug.WriteLine("Callback has been called.");
            vtk.vtkRenderer ren = vtk.vtkRenderer.SafeDownCast(caller);
            if (ren != null)
            {
                double[] position = ren.GetActiveCamera().GetPosition();
                Console.WriteLine(String.Format("{0}, {1}, {2}",

                    position[0], position[1], position[2]));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Caller is not a renderer.");
            }
        }

        void AddConeToWindow(vtk.vtkRenderWindow renWin)
        {
            // 
            // Next we create an instance of vtkConeSource and set some of its
            // properties. The instance of vtkConeSource "cone" is part of a visualization
            // pipeline (it is a source process object); it produces data (output type is
            // vtkPolyData) which other filters may process.
            //
            vtk.vtkConeSource cone = new vtk.vtkConeSource();
            cone.SetHeight(3.0f);
            cone.SetRadius(1.0f);
            cone.SetResolution(10);

            // 
            // In this example we terminate the pipeline with a mapper process object.
            // (Intermediate filters such as vtkShrinkPolyData could be inserted in
            // between the source and the mapper.)  We create an instance of
            // vtkPolyDataMapper to map the polygonal data into graphics primitives. We
            // connect the output of the cone souece to the input of this mapper.
            //
            vtk.vtkPolyDataMapper coneMapper = new vtk.vtkPolyDataMapper();
            coneMapper.SetInput(cone.GetOutput());

            // 
            // Create an actor to represent the cone. The actor orchestrates rendering of
            // the mapper's graphics primitives. An actor also refers to properties via a
            // vtkProperty instance, and includes an internal transformation matrix. We
            // set this actor's mapper to be coneMapper which we created above.
            //
            vtk.vtkActor coneActor = new vtk.vtkActor();
            coneActor.SetMapper(coneMapper);

            //
            // Create the Renderer and assign actors to it. A renderer is like a
            // viewport. It is part or all of a window on the screen and it is
            // responsible for drawing the actors it has.  We also set the background
            // color here
            //
            vtk.vtkRenderer ren1 = new vtk.vtkRenderer();
            ren1.AddActor(coneActor);
            ren1.SetBackground(0.1f, 0.2f, 0.4f);

            //
            // Finally we create the render window which will show up on the screen
            // We put our renderer into the render window using AddRenderer. We also
            // set the size to be 300 pixels by 300
            //
            renWin.AddRenderer(ren1);

            vtk.vtkRenderWindowInteractor iren = renWin.GetInteractor();

            {
                m_boxWidget = new vtk.vtkBoxWidget();
                m_boxWidget.SetInteractor(iren);
                m_boxWidget.SetPlaceFactor(1.25f);

                m_boxWidget.SetProp3D(coneActor);
                m_boxWidget.PlaceWidget();

                m_boxWidget.AddObserver((uint)vtk.EventIds.InteractionEvent,
                    new vtk.vtkDotNetCallback(myCallback));

                m_boxWidget.On();
            }
        }

        void AddFlamingoToWindow(vtk.vtkRenderWindow renWin)
        {
            // This example demonstrates the use of vtk3DSImporter.
            // vtk3DSImporter is used to load 3D Studio files.  Unlike writers,
            // importers can load scenes (data as well as lights, cameras, actors
            // etc.). Importers will either generate an instance of vtkRenderWindow
            // and/or vtkRenderer or will use the ones you specify.
            string VTK_DATA_ROOT = vtk.vtkDotNetUtil.vtkGetDataRoot();


            // Create the importer and read a file
            vtk.vtk3DSImporter importer = new vtk.vtk3DSImporter();
            importer.ComputeNormalsOn();
            importer.SetFileName(VTK_DATA_ROOT + "/Data/iflamigm.3ds");
            importer.Read();

            // Here we let the importer create a renderer and a render window for
            // us. We could have also create and assigned those ourselves like so:
            importer.SetRenderWindow(renWin);

            // Assign an interactor.
            // We have to ask the importer for it's render window.

            // Set some properties on the renderer.
            // We have to ask the importer for it's renderer.
            vtk.vtkRenderer ren = importer.GetRenderer();
            renWin.AddRenderer(ren);
            ren.SetBackground(0.1, 0.2, 0.4);

            // Position the camera:
            // change view up to +z
            vtk.vtkCamera camera = ren.GetActiveCamera();
            camera.SetPosition(0, 1, 0);
            camera.SetFocalPoint(0, 0, 0);
            camera.SetViewUp(0, 0, 1);
            // let the renderer compute good position and focal point
            ren.ResetCamera();
            camera.Dolly(1.4);
            ren.ResetCameraClippingRange();
        }
    }

    static class ControlSample
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SampleForm());
        }
    }
}