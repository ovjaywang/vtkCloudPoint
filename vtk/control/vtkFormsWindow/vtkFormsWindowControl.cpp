// vtkFormsWindow.cpp : main project file.
#include <strstream>
#include <Windows.h> // Just for MK_CONTROL
#include "vtkWin32OpenGLRenderWindow.h"
#include "vtkWin32RenderWindowInteractor.h"
#include "vtkInteractorStyleSwitch.h"

// These includes are for the splash screen.
#include "vtkAlgorithmOutput.h"
#include "vtkAlgorithm.h"
#include "vtkRenderer.h"
#include "vtkConeSource.h"
#include "vtkPolyDataMapper.h"
#include "vtkActor.h"
#include "vtkProperty.h"

#include "vtkFormsWindowControl.h"

using namespace System::Windows::Forms;
#ifdef TRACE
using namespace System::Diagnostics;
#endif // TRACE



namespace vtk
  {  
  vtkFormsWindowControl::vtkFormsWindowControl(void)
    {
#ifdef TRACE
    m_traceSource = gcnew System::Diagnostics::TraceSource("vtkFormsWindowControl");
    m_traceSource->TraceEvent(TraceEventType::Information,0,"Begin constructor");
#ifdef VTK_CONTROL_SOURCE_SWITCH
    SourceSwitch^ sourceSwitch = gcnew SourceSwitch("vtkFormsWindowSwitch", "Critical");
    m_traceSource->Switch = sourceSwitch;
    m_traceSource->Switch->Level = SourceLevels::All;
#endif // VTK_CONTROL_SOURCE_SWITCH
#endif // TRACE
    ControlStyles style = ControlStyles::AllPaintingInWmPaint |
      ControlStyles::Opaque | ControlStyles::ResizeRedraw;
    this->SetStyle(style, true);
    this->pvtkWin32OpenGLRW = 0;

    InitializeComponent();


    // create a default vtk window
    vtkWin32OpenGLRenderWindow^ win = gcnew vtkWin32OpenGLRenderWindow();
    this->SetRenderWindow(win);
    delete win;
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,1,"End constructor");
#endif // TRACE
    }


  vtkFormsWindowControl::~vtkFormsWindowControl()
    {
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,0,"Begin destructor");
#endif // TRACE

    if (components)
      {
      delete components;
      } 

#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,1,"End destructor");
#endif // TRACE
    }


  //! give an instance of a vtk render window to the mfc window
  void vtkFormsWindowControl::SetRenderWindow(vtkWin32OpenGLRenderWindow^ win)
    {
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,0,"begin SetRenderWindow");
#endif // TRACE

    if(this->pvtkWin32OpenGLRW)
      {
      if(this->pvtkWin32OpenGLRW->GetMapped())
        this->pvtkWin32OpenGLRW->Finalize();
      this->pvtkWin32OpenGLRW->UnRegister(NULL);
      this->pvtkWin32OpenGLRW = 0;
      }

    if ( nullptr == win )
      {
      this->pvtkWin32OpenGLRW = 0;
      return;
      }

    this->pvtkWin32OpenGLRW = ::vtkWin32OpenGLRenderWindow::SafeDownCast(
      static_cast<::vtkObjectBase*>(win->GetNativePointer().ToPointer()));

    if(this->pvtkWin32OpenGLRW)
      {
      this->pvtkWin32OpenGLRW->Register(NULL);
      ::vtkWin32RenderWindowInteractor* iren = ::vtkWin32RenderWindowInteractor::New();
      iren->SetRenderWindow(this->pvtkWin32OpenGLRW);

      ::vtkInteractorStyleSwitch* style = ::vtkInteractorStyleSwitch::New();
      iren->SetInteractorStyle( style );
      //style->SetCurrentStyleToTrackballCamera();
      style->Delete();

      // The control must wait to initialize the interactor until it has
      // been given a parent window. Until then, initializing the interactor
      // will always fail.

      // release our hold on interactor
      iren->Delete();
      }
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,1,"end SetRenderWindow");
#endif // TRACE
    }


  void vtkFormsWindowControl::InitializeInteractor()
    {
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,0, "begin InitializeInteractor");
#endif // TRACE
    ::vtkWin32RenderWindowInteractor* iren = ::vtkWin32RenderWindowInteractor::SafeDownCast(
      this->pvtkWin32OpenGLRW->GetInteractor());
    if ( 0 == iren )
      {
      throw gcnew System::ApplicationException("RenderWindow interactor must by of type Win32");
      }
    iren->SetInstallMessageProc(0);

    // setup the parent window
    this->pvtkWin32OpenGLRW->SetWindowId(this->Handle.ToPointer());
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,0, "window id {0}", this->Handle);
#endif // TRACE
    if ( nullptr != this->Parent )
      {
      this->pvtkWin32OpenGLRW->SetParentId(this->Parent->Handle.ToPointer());
#ifdef TRACE
      m_traceSource->TraceEvent(TraceEventType::Information,0, "parent id {0}",
        this->Parent->Handle);
#endif // TRACE
      }
    else
      {
      throw gcnew System::ApplicationException(
        "Cannot set render window until control has a parent.");
      }
    iren->Initialize();

    // update size
    if (iren->GetInitialized())
      iren->UpdateSize(this->Width, this->Height);

#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,1,"end InitializeInteractor");
#endif // TRACE
    }


  //! get the render window
  vtkWin32OpenGLRenderWindow^ vtkFormsWindowControl::GetRenderWindow()
    {
    return gcnew vtkWin32OpenGLRenderWindow(IntPtr(this->pvtkWin32OpenGLRW), false);
    }



  //! get the interactor
  vtkRenderWindowInteractor^ vtkFormsWindowControl::GetInteractor()
    {
    if(!this->pvtkWin32OpenGLRW)
      return nullptr;
    ::vtkRenderWindowInteractor* rwi = this->pvtkWin32OpenGLRW->GetInteractor();
    if ( 0 == rwi ) return nullptr;
    return gcnew vtkRenderWindowInteractor(IntPtr(rwi), false);
    }


  //! get the render window
  ::vtkWin32OpenGLRenderWindow* vtkFormsWindowControl::GetRenderWindowNative()
    {
    return this->pvtkWin32OpenGLRW;
    }



  //! get the interactor
  ::vtkRenderWindowInteractor* vtkFormsWindowControl::GetInteractorNative()
    {
    if(!this->pvtkWin32OpenGLRW)
      return 0;
    return this->pvtkWin32OpenGLRW->GetInteractor();
    }


  void vtkFormsWindowControl::OnPaint( System::Windows::Forms::PaintEventArgs^ ea )
    {
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,0,"begin OnPaint");
#endif // TRACE
    __super::OnPaint(ea);

    if (this->GetInteractorNative())
      {
      if (this->GetInteractorNative()->GetInitialized())
        {
        this->GetInteractorNative()->Render();
        }
      else
        {
        this->InitializeInteractor();

        if ( nullptr != this->Site )
          {
#ifdef TRACE
          m_traceSource->TraceEvent(TraceEventType::Information,0,"control has site");
#endif // TRACE
          if ( this->Site->DesignMode )
            {
#ifdef TRACE
            m_traceSource->TraceEvent(TraceEventType::Information,0,"control in design mode");
#endif // TRACE
            this->ShowSplashScreen();
            }
          }

        if ( this->GetInteractor()->GetInitialized() )
          {
          this->GetInteractorNative()->Render();
          }
        }
      }
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,0,"end OnPaint");
#endif // TRACE
    }



  void vtkFormsWindowControl::OnPrint( System::Windows::Forms::PaintEventArgs^ ea )
    {
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,0,"OnPrint begin");
#endif // TRACE
    __super::OnPrint(ea);

    /*
    // Obtain the size of the printer page in pixels.
    HDC hdc = static_cast<HDC>(ea->Graphics->GetHdc().ToPointer());
    int capsX = ::GetDeviceCaps(hdc, HORZRES);
    int capsY = ::GetDeviceCaps(hdc, VERTRES);
    int cxPage = ea->Graphics->ClipBounds.Width;
    int cyPage = ea->Graphics->ClipBounds.Height;
    int pageScale = ea->Graphics->PageScale;
    System::Drawing::GraphicsUnit unit = ea->Graphics->PageUnit;
    int dpix = ea->Graphics->DpiX;
    #ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,0,
    "OnPrint w {0} h {1} scale {2} unit {3} dpix {4} caps {5} {6}",
    cxPage, cyPage, pageScale, unit, dpix, capsX, capsY);
    #endif // TRACE


    // Get the size of the window in pixels.
    int *size = this->pvtkWin32OpenGLRW->GetSize();
    int cxWindow = size[0];
    int cyWindow = size[1];
    float fx = float(cxPage) / float(cxWindow);
    float fy = float(cyPage) / float(cyWindow);
    float scale = (fx < fy) ? fx : fy;
    int x = int(scale * float(cxWindow));
    int y = int(scale * float(cyWindow));
    this->pvtkWin32OpenGLRW->SetupMemoryRendering(cxWindow, cyWindow, hdc);
    this->pvtkWin32OpenGLRW->Render();
    HDC memDC = this->pvtkWin32OpenGLRW->GetMemoryDC();
    StretchBlt(hdc,0,0,x,y,memDC,0,0,cxWindow,cyWindow,SRCCOPY);
    this->pvtkWin32OpenGLRW->ResumeScreenRendering();
    ea->Graphics->ReleaseHdc();
    */
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,1, "OnPrint end");
#endif // TRACE
    }


  void vtkFormsWindowControl::OnResize( System::EventArgs^ ea)
    {
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,0,
      String::Format("OnResize begin w {0} h {1}", this->Width, this->Height));
#endif // TRACE
    if (this->GetInteractorNative() && this->GetInteractorNative()->GetInitialized())
      this->GetInteractorNative()->UpdateSize(this->Width, this->Height);
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,1,"OnResize end");
#endif // TRACE
    }

  void vtkFormsWindowControl::OnDoubleClick( System::EventArgs^ ea )
    {
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,0,"OnDoubleClick begin");
#endif // TRACE
    unsigned int nFlags = 0;
    if ( (this->ModifierKeys & Keys::Control) == Keys::Control )
      nFlags |= MK_CONTROL;
    if ( (this->ModifierKeys & Keys::Shift) == Keys::Shift )
      nFlags |= MK_SHIFT;

    int x = this->MousePosition.X;
    int y = this->MousePosition.Y;

    if ( (this->MouseButtons & System::Windows::Forms::MouseButtons::Left) ==
      System::Windows::Forms::MouseButtons::Left )
      {
      static_cast<::vtkWin32RenderWindowInteractor*>(this->GetInteractorNative())->
        OnLButtonDown((HWND) this->Handle.ToPointer(), nFlags, x, y, 1);
      }
    else if ( (this->MouseButtons & System::Windows::Forms::MouseButtons::Middle) ==
      System::Windows::Forms::MouseButtons::Middle)
      {
      static_cast<::vtkWin32RenderWindowInteractor*>(this->GetInteractorNative())->
        OnMButtonDown((HWND) this->Handle.ToPointer(), nFlags, x, y, 1);
      }
    else if ( (this->MouseButtons & System::Windows::Forms::MouseButtons::Right) ==
      System::Windows::Forms::MouseButtons::Right)
      {
      static_cast<::vtkWin32RenderWindowInteractor*>(this->GetInteractorNative())->
        OnRButtonDown((HWND) this->Handle.ToPointer(), nFlags, x, y, 1);
      }

    __super::OnDoubleClick(ea);
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,1,"OnDoubleClick end");
#endif // TRACE
    }


  void vtkFormsWindowControl::DestroyHandle()
    {
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,0,"OnDestroyHandle begin");
#endif // TRACE
    __super::DestroyHandle();
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,1,"OnDestroyHandle end");
#endif // TRACE
    }


  void vtkFormsWindowControl::OnEnabledChanged(System::EventArgs^ e)
    {
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,0,"OnEnabledChanged begin");
#endif // TRACE
    __super::OnEnabledChanged(e);
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,1,"OnEnabledChanged end");
#endif // TRACE
    }

  void vtkFormsWindowControl::OnHandleDestroyed(System::EventArgs^ e)
    {
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,0,"OnHandleDestroyed begin");
#endif // TRACE

    // Remove the render window before the window handle is destroyed
    // because the render window needs access to SetWGLCurrent().
    this->SetRenderWindow(nullptr);

    __super::OnHandleDestroyed(e);
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,1,"OnHandleDestroyed end");
#endif // TRACE
    }

  void vtkFormsWindowControl::OnMouseDown( System::Windows::Forms::MouseEventArgs^ ea )
    {
    this->Focus(); // Being focused tells Form you will receive key events.
    unsigned int nFlags = 0;
    if ( (this->ModifierKeys & Keys::Control) == Keys::Control )
      nFlags |= MK_CONTROL;
    if ( (this->ModifierKeys & Keys::Shift) == Keys::Shift )
      nFlags |= MK_SHIFT;

    int x = ea->X;
    int y = ea->Y;

    if ( (ea->Button & System::Windows::Forms::MouseButtons::Left) ==
      System::Windows::Forms::MouseButtons::Left )
      {
      static_cast<::vtkWin32RenderWindowInteractor*>(this->GetInteractorNative())->
        OnLButtonDown((HWND) this->Handle.ToPointer(), nFlags, x, y);
      }
    else if ( (ea->Button & System::Windows::Forms::MouseButtons::Middle) ==
      System::Windows::Forms::MouseButtons::Middle)
      {
      static_cast<::vtkWin32RenderWindowInteractor*>(this->GetInteractorNative())->
        OnMButtonDown((HWND) this->Handle.ToPointer(), nFlags, x, y);
      }
    else if ( (ea->Button & System::Windows::Forms::MouseButtons::Right) ==
      System::Windows::Forms::MouseButtons::Right)
      {
      static_cast<::vtkWin32RenderWindowInteractor*>(this->GetInteractorNative())->
        OnRButtonDown((HWND) this->Handle.ToPointer(), nFlags, x, y);
      }

    __super::OnMouseDown(ea);
    }



  void vtkFormsWindowControl::OnMouseUp( System::Windows::Forms::MouseEventArgs^ ea )
    {
    unsigned int nFlags = 0;
    if ( (this->ModifierKeys & Keys::Control) == Keys::Control )
      nFlags |= MK_CONTROL;
    if ( (this->ModifierKeys & Keys::Shift) == Keys::Shift )
      nFlags |= MK_SHIFT;

    int x = ea->X;
    int y = ea->Y;

    if ( (ea->Button & System::Windows::Forms::MouseButtons::Left) ==
      System::Windows::Forms::MouseButtons::Left )
      {
      static_cast<::vtkWin32RenderWindowInteractor*>(this->GetInteractorNative())->
        OnLButtonUp((HWND) this->Handle.ToPointer(), nFlags, x, y);
      }
    else if ( (ea->Button & System::Windows::Forms::MouseButtons::Middle) ==
      System::Windows::Forms::MouseButtons::Middle)
      {
      static_cast<::vtkWin32RenderWindowInteractor*>(this->GetInteractorNative())->
        OnMButtonUp((HWND) this->Handle.ToPointer(), nFlags, x, y);
      }
    else if ( (ea->Button & System::Windows::Forms::MouseButtons::Right) ==
      System::Windows::Forms::MouseButtons::Right)
      {
      static_cast<::vtkWin32RenderWindowInteractor*>(this->GetInteractorNative())->
        OnRButtonUp((HWND) this->Handle.ToPointer(), nFlags, x, y);
      }

    __super::OnMouseUp(ea);
    }


  void vtkFormsWindowControl::OnMouseMove( System::Windows::Forms::MouseEventArgs^ ea )
    {
    unsigned int nFlags = 0;
    if ( (this->ModifierKeys & Keys::Control) == Keys::Control )
      nFlags |= MK_CONTROL;
    if ( (this->ModifierKeys & Keys::Shift) == Keys::Shift )
      nFlags |= MK_SHIFT;

    int x = ea->X;
    int y = ea->Y;

    ::vtkRenderWindowInteractor* rwi = this->GetInteractorNative();
    ::vtkWin32RenderWindowInteractor* win32rwi = ::vtkWin32RenderWindowInteractor::SafeDownCast(rwi);
    HWND hwnd = static_cast<HWND>(this->Handle.ToPointer());

#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,2,"OnMouseMove {4} 0x{0:x} {1} {2} {3}",
      this->Handle, nFlags, x, y, this->Name);
#endif // TRACE
    if ( 0 != win32rwi )
      {
      win32rwi->OnMouseMove(hwnd, nFlags, x, y);
      }
    else
      {
      throw gcnew System::ApplicationException("vtkFormsWindowControl::OnMouseMove null pointer.");
      }

    __super::OnMouseMove(ea);
    }


  void vtkFormsWindowControl::OnMouseWheel( System::Windows::Forms::MouseEventArgs^ ea )
    {
    unsigned int nFlags = 0;
    if ( (this->ModifierKeys & Keys::Control) == Keys::Control )
      nFlags |= MK_CONTROL;
    if ( (this->ModifierKeys & Keys::Shift) == Keys::Shift )
      nFlags |= MK_SHIFT;

    int x = ea->X;
    int y = ea->Y;

    if ( ea->Delta > 0 )
      {
      static_cast<::vtkWin32RenderWindowInteractor*>(this->GetInteractorNative())->
        OnMouseWheelForward((HWND) this->Handle.ToPointer(), nFlags, x, y);
      }
    else
      {
      static_cast<::vtkWin32RenderWindowInteractor*>(this->GetInteractorNative())->
        OnMouseWheelBackward((HWND) this->Handle.ToPointer(), nFlags, x, y);
      }

    __super::OnMouseWheel(ea);
    }



  void vtkFormsWindowControl::OnKeyPress( System::Windows::Forms::KeyPressEventArgs^ ea )
    {
    unsigned int nFlags = 0;
    if ( (this->ModifierKeys & Keys::Control) == Keys::Control )
      nFlags |= MK_CONTROL;
    if ( (this->ModifierKeys & Keys::Shift) == Keys::Shift )
      nFlags |= MK_SHIFT;

    unsigned int nChar = safe_cast<unsigned int>( ea->KeyChar );

    int rptCnt = 0;

    static_cast<::vtkWin32RenderWindowInteractor*>(this->GetInteractorNative())->
      OnChar((HWND) this->Handle.ToPointer(), nChar, rptCnt, nFlags);
    __super::OnKeyPress(ea);
    }

  void vtkFormsWindowControl::OnKeyDown( System::Windows::Forms::KeyEventArgs^ ea )
    {
    unsigned int nFlags = 0;
    if ( (ea->Modifiers & Keys::Control) == Keys::Control )
      nFlags |= MK_CONTROL;
    if ( (ea->Modifiers & Keys::Shift) == Keys::Shift )
      nFlags |= MK_SHIFT;

    unsigned int nChar = safe_cast<unsigned int>( ea->KeyCode );

    int rptCnt = 0;

    static_cast<::vtkWin32RenderWindowInteractor*>(this->GetInteractorNative())->
      OnKeyDown((HWND) this->Handle.ToPointer(), nChar, rptCnt, nFlags);
    __super::OnKeyDown(ea);
    }



  void vtkFormsWindowControl::OnKeyUp( System::Windows::Forms::KeyEventArgs^ ea )
    {
    unsigned int nFlags = 0;
    if ( (ea->Modifiers & Keys::Control) == Keys::Control )
      nFlags |= MK_CONTROL;
    if ( (ea->Modifiers & Keys::Shift) == Keys::Shift )
      nFlags |= MK_SHIFT;

    unsigned int nChar = safe_cast<unsigned int>( ea->KeyCode );

    int rptCnt = 0;

    static_cast<::vtkWin32RenderWindowInteractor*>(this->GetInteractorNative())->
      OnKeyUp((HWND) this->Handle.ToPointer(), nChar, rptCnt, nFlags);
    __super::OnKeyUp(ea);
    }


  void vtkFormsWindowControl::ShowSplashScreen()
    {
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,0,"ShowSplashScreen begin");
#endif // TRACE
    ::vtkConeSource* cone = ::vtkConeSource::New();
    cone->SetHeight( 3.0 );
    cone->SetRadius( 1.0 );
    cone->SetResolution( 10 );

    ::vtkPolyDataMapper* coneMapper = ::vtkPolyDataMapper::New();
    coneMapper->SetInputConnection( cone->GetOutputPort() );
    cone->Delete();

    ::vtkActor* coneActor = ::vtkActor::New();
    coneActor->SetMapper( coneMapper );
    coneMapper->Delete();
    coneActor->GetProperty()->SetColor(1,0,0);

    ::vtkRenderer* ren = ::vtkRenderer::New();
    if ( 0 != ren )
      {
      ren->AddActor( coneActor );
      ren->SetBackground( 0.1, 0.2, 0.4 );
      ::vtkRenderWindow* renWin = this->GetRenderWindowNative();
      renWin->AddRenderer(ren);
      ren->Delete();
      }
    else
      {
#ifdef TRACE
      m_traceSource->TraceEvent(TraceEventType::Warning,0,
        "could not get renderer in ShowCone");
#endif // TRACE
      }
    coneActor->Delete();
#ifdef TRACE
    m_traceSource->TraceEvent(TraceEventType::Information,0,"ShowSplashScreen end");
#endif // TRACE
    }

  void vtkFormsWindowControl::WndProc( System::Windows::Forms::Message% m ) 
    { 
    switch (m.Msg) 
      { 
      case (int)WM_TIMER: 
        {
        static_cast<::vtkWin32RenderWindowInteractor*>(this->GetInteractorNative())->
          OnTimer((HWND) this->Handle.ToPointer(),
          static_cast<int>(m.WParam));
        break; 
        } 
      default: 
        {     
        __super::WndProc(m); 
        break;   
        } 
      }   
    } 

  }
