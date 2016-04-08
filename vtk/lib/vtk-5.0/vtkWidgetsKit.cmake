# Directory containing class headers.
SET(VTK_WIDGETS_HEADER_DIR "${VTK_INSTALL_PREFIX}/include/vtk-5.0")

# Classes in vtkWidgets.
SET(VTK_WIDGETS_CLASSES
  "vtk3DWidget"
  "vtkBoxWidget"
  "vtkImagePlaneWidget"
  "vtkImageTracerWidget"
  "vtkImplicitPlaneWidget"
  "vtkLineWidget"
  "vtkOrientationMarkerWidget"
  "vtkPlaneWidget"
  "vtkPointWidget"
  "vtkPolyDataSourceWidget"
  "vtkScalarBarWidget"
  "vtkSphereWidget"
  "vtkSplineWidget"
  "vtkXYPlotWidget")

# Abstract classes in vtkWidgets.
SET(VTK_WIDGETS_CLASSES_ABSTRACT
  "vtk3DWidget"
  "vtkPolyDataSourceWidget")

# Wrap-exclude classes in vtkWidgets.
SET(VTK_WIDGETS_CLASSES_WRAP_EXCLUDE)

# Set convenient variables to test each class.
FOREACH(class ${VTK_WIDGETS_CLASSES})
  SET(VTK_CLASS_EXISTS_${class} 1)
ENDFOREACH(class)
FOREACH(class ${VTK_WIDGETS_CLASSES_ABSTRACT})
  SET(VTK_CLASS_ABSTRACT_${class} 1)
ENDFOREACH(class)
FOREACH(class ${VTK_WIDGETS_CLASSES_WRAP_EXCLUDE})
  SET(VTK_CLASS_WRAP_EXCLUDE_${class} 1)
ENDFOREACH(class)
