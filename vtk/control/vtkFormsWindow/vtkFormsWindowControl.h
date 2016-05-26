#pragma once

// vtkFormsWindowControl.h
// This class is a Windows Forms control that displays VTK actors.

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Windows::Forms;
using namespace System::Data;
using namespace System::Drawing;


class vtkWin32OpenGLRenderWindow;
class vtkRenderWindowInteractor;

#include "vtkConfigure.h"


namespace vtk {

  /// <summary>
  /// This Control encapsulates a VTK render window.
  /// </summary>
  ///
  /// WARNING: If you change the name of this class, you will need to change the
  ///          'Resource File Name' property for the managed resource compiler tool
  ///          associated with all .resx files this class depends on.  Otherwise,
  ///          the designers will not be able to interact properly with localized
  ///          resources associated with this form.
  public ref class vtkFormsWindowControl : public System::Windows::Forms::Control
  {

    //! the vtk window
    ::vtkWin32OpenGLRenderWindow* pvtkWin32OpenGLRW;
#ifdef TRACE
    System::Diagnostics::TraceSource^ m_traceSource;
#endif // TRACE
  public:
    vtkFormsWindowControl(void);

    /// <summary>Give an instance of a vtk render window to the control</summary>
	/// <param name="window">A pointer to a VTK render window.</param>
    virtual void SetRenderWindow(vtkWin32OpenGLRenderWindow^ window);
    /// <summary>Get a render window created by the control</summary>
	/// <returns>This method will return a non-null vtkRenderWindow.</returns>
    virtual vtkWin32OpenGLRenderWindow^ GetRenderWindow();
    /// <summary>Get a render window interactor</summary>
	/// <returns>A vtkInteractorStyleSwitch</returns>
    virtual vtkRenderWindowInteractor^ GetInteractor();

  protected:
    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    ~vtkFormsWindowControl();

    virtual void InitializeInteractor();

    virtual void OnPaint( System::Windows::Forms::PaintEventArgs^ ea ) override;
    virtual void OnPrint( System::Windows::Forms::PaintEventArgs^ ea ) override;
    virtual void OnResize( System::EventArgs^ ea) override;
    virtual void DestroyHandle() override;
    virtual void OnEnabledChanged(System::EventArgs^ e) override;
    virtual void OnHandleDestroyed(System::EventArgs^ e) override;

    virtual void OnDoubleClick( System::EventArgs^ ea ) override;
    virtual void OnMouseDown( System::Windows::Forms::MouseEventArgs^ ea ) override;
    virtual void OnMouseUp( System::Windows::Forms::MouseEventArgs^ ea ) override;
    virtual void OnMouseMove( System::Windows::Forms::MouseEventArgs^ ea ) override;
    virtual void OnMouseWheel( System::Windows::Forms::MouseEventArgs^ ea ) override;
    virtual void OnKeyPress( System::Windows::Forms::KeyPressEventArgs^ ea ) override;
    virtual void OnKeyDown( System::Windows::Forms::KeyEventArgs^ ea ) override;
    virtual void OnKeyUp( System::Windows::Forms::KeyEventArgs^ ea ) override;
    virtual void WndProc( System::Windows::Forms::Message% m ) override;

    void ShowSplashScreen();
    
  private:
    /// <summary>
    /// Required designer variable.
    /// </summary>
    System::ComponentModel::Container^ components;

    //! get the render window
    ::vtkWin32OpenGLRenderWindow* GetRenderWindowNative();
    //! get the interactor
    ::vtkRenderWindowInteractor* GetInteractorNative();

#pragma region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    void InitializeComponent(void)
    {
      // only for UserControl this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
    }
#pragma endregion
  };
}
