# Directory containing class headers.
SET(VTK_HYBRID_HEADER_DIR "${VTK_INSTALL_PREFIX}/include/vtk-5.0")

# Classes in vtkHybrid.
SET(VTK_HYBRID_CLASSES
  "vtk3DSImporter"
  "vtkArcPlotter"
  "vtkAnnotatedCubeActor"
  "vtkAxesActor"
  "vtkCaptionActor2D"
  "vtkCornerAnnotation"
  "vtkCubeAxesActor2D"
  "vtkDepthSortPolyData"
  "vtkEarthSource"
  "vtkFacetReader"
  "vtkGreedyTerrainDecimation"
  "vtkGridTransform"
  "vtkImageToPolyDataFilter"
  "vtkImplicitModeller"
  "vtkIterativeClosestPointTransform"
  "vtkLandmarkTransform"
  "vtkLegendBoxActor"
  "vtkPCAAnalysisFilter"
  "vtkPolyDataToImageStencil"
  "vtkProcrustesAlignmentFilter"
  "vtkProjectedTerrainPath"
  "vtkRIBExporter"
  "vtkRIBLight"
  "vtkRIBProperty"
  "vtkRenderLargeImage"
  "vtkThinPlateSplineTransform"
  "vtkTransformToGrid"
  "vtkVRMLImporter"
  "vtkVectorText"
  "vtkVideoSource"
  "vtkWeightedTransformFilter"
  "vtkXYPlotActor"
  "vtkPExodusReader"
  "vtkExodusReader"
  "vtkDSPFilterDefinition"
  "vtkExodusModel"
  "vtkDSPFilterGroup"
  "vtkWin32VideoSource")

# Abstract classes in vtkHybrid.
SET(VTK_HYBRID_CLASSES_ABSTRACT)

# Wrap-exclude classes in vtkHybrid.
SET(VTK_HYBRID_CLASSES_WRAP_EXCLUDE)

# Set convenient variables to test each class.
FOREACH(class ${VTK_HYBRID_CLASSES})
  SET(VTK_CLASS_EXISTS_${class} 1)
ENDFOREACH(class)
FOREACH(class ${VTK_HYBRID_CLASSES_ABSTRACT})
  SET(VTK_CLASS_ABSTRACT_${class} 1)
ENDFOREACH(class)
FOREACH(class ${VTK_HYBRID_CLASSES_WRAP_EXCLUDE})
  SET(VTK_CLASS_WRAP_EXCLUDE_${class} 1)
ENDFOREACH(class)
