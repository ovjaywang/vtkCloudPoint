# Directory containing class headers.
SET(VTK_PARALLEL_HEADER_DIR "${VTK_INSTALL_PREFIX}/include/vtk-5.0")

# Classes in vtkParallel.
SET(VTK_PARALLEL_CLASSES
  "vtkDuplicatePolyData"
  "vtkBranchExtentTranslator"
  "vtkCollectPolyData"
  "vtkCommunicator"
  "vtkCompositer"
  "vtkCompositeRenderManager"
  "vtkCompressCompositer"
  "vtkCutMaterial"
  "vtkDistributedDataFilter"
  "vtkDistributedStreamTracer"
  "vtkDummyController"
  "vtkEnSightWriter"
  "vtkExtractCTHPart"
  "vtkExtractPolyDataPiece"
  "vtkExtractUnstructuredGridPiece"
  "vtkExtractUserDefinedPiece"
  "vtkPKdTree"
  "vtkMemoryLimitImageDataStreamer"
  "vtkMultiProcessController"
  "vtkParallelRenderManager"
  "vtkPassThroughFilter"
  "vtkPCellDataToPointData"
  "vtkPChacoReader"
  "vtkPDataSetReader"
  "vtkPDataSetWriter"
  "vtkPImageWriter"
  "vtkPLinearExtrusionFilter"
  "vtkPOPReader"
  "vtkPOutlineFilter"
  "vtkPOutlineCornerFilter"
  "vtkPPolyDataNormals"
  "vtkPProbeFilter"
  "vtkPSphereSource"
  "vtkPStreamTracer"
  "vtkParallelFactory"
  "vtkPieceScalars"
  "vtkPipelineSize"
  "vtkProcessIdScalars"
  "vtkRTAnalyticSource"
  "vtkRectilinearGridOutlineFilter"
  "vtkSocketCommunicator"
  "vtkSocketController"
  "vtkSubGroup"
  "vtkTransmitPolyDataPiece"
  "vtkTransmitUnstructuredGridPiece"
  "vtkTreeCompositer"
  "vtkExodusIIWriter")

# Abstract classes in vtkParallel.
SET(VTK_PARALLEL_CLASSES_ABSTRACT
  "vtkCommunicator"
  "vtkMultiProcessController"
  "vtkParallelRenderManager"
  "vtkPStreamTracer")

# Wrap-exclude classes in vtkParallel.
SET(VTK_PARALLEL_CLASSES_WRAP_EXCLUDE)

# Set convenient variables to test each class.
FOREACH(class ${VTK_PARALLEL_CLASSES})
  SET(VTK_CLASS_EXISTS_${class} 1)
ENDFOREACH(class)
FOREACH(class ${VTK_PARALLEL_CLASSES_ABSTRACT})
  SET(VTK_CLASS_ABSTRACT_${class} 1)
ENDFOREACH(class)
FOREACH(class ${VTK_PARALLEL_CLASSES_WRAP_EXCLUDE})
  SET(VTK_CLASS_WRAP_EXCLUDE_${class} 1)
ENDFOREACH(class)
