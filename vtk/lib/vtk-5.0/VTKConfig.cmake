#-----------------------------------------------------------------------------
#
# VTKConfig.cmake - VTK CMake configuration file for external projects.
#
# This file is configured by VTK and used by the UseVTK.cmake module
# to load VTK's settings for an external project.

# Compute the installation prefix from VTK_DIR.
SET(VTK_INSTALL_PREFIX "${VTK_DIR}")
GET_FILENAME_COMPONENT(VTK_INSTALL_PREFIX "${VTK_INSTALL_PREFIX}" PATH)
GET_FILENAME_COMPONENT(VTK_INSTALL_PREFIX "${VTK_INSTALL_PREFIX}" PATH)

# The VTK include file directories.
SET(VTK_INCLUDE_DIRS "${VTK_INSTALL_PREFIX}/include/vtk-5.0;C:/dev/out/t2/build/Common")

# The VTK library directories.
SET(VTK_LIBRARY_DIRS "${VTK_INSTALL_PREFIX}/lib")

# The VTK runtime library directories.  Note that if
# VTK_CONFIGURATION_TYPES is set (see below) then these directories
# will be the parent directories under which there will be a directory
# of runtime binaries for each configuration type.
SET(VTK_RUNTIME_LIBRARY_DIRS "${VTK_INSTALL_PREFIX}/bin")

# The C and C++ flags added by VTK to the cmake-configured flags.
SET(VTK_REQUIRED_C_FLAGS "")
SET(VTK_REQUIRED_CXX_FLAGS "")
SET(VTK_REQUIRED_EXE_LINKER_FLAGS "")
SET(VTK_REQUIRED_SHARED_LINKER_FLAGS "")
SET(VTK_REQUIRED_MODULE_LINKER_FLAGS "")

# The VTK version number
SET(VTK_MAJOR_VERSION "5")
SET(VTK_MINOR_VERSION "0")
SET(VTK_BUILD_VERSION "1")

# The location of the UseVTK.cmake file.
SET(VTK_USE_FILE "${VTK_INSTALL_PREFIX}/lib/vtk-5.0/UseVTK.cmake")

# The build settings file.
SET(VTK_BUILD_SETTINGS_FILE "${VTK_INSTALL_PREFIX}/lib/vtk-5.0/VTKBuildSettings.cmake")

# The directory containing class list files for each kit.
SET(VTK_KITS_DIR "${VTK_INSTALL_PREFIX}/lib/vtk-5.0")

# The wrapping hints file.
SET(VTK_WRAP_HINTS "${VTK_INSTALL_PREFIX}/lib/vtk-5.0/hints")

# CMake extension module directory and macro file.
SET(VTK_LOAD_CMAKE_EXTENSIONS_MACRO "${VTK_INSTALL_PREFIX}/lib/vtk-5.0/CMake/vtkLoadCMakeExtensions.cmake")
SET(VTK_CMAKE_DIR "${VTK_INSTALL_PREFIX}/lib/vtk-5.0/CMake")
SET(VTK_CMAKE_EXTENSIONS_DIR "${VTK_INSTALL_PREFIX}/lib/vtk-5.0/CMake")

# The list of available kits.
SET(VTK_KITS "COMMON;FILTERING;IO;GRAPHICS;GENERIC_FILTERING;IMAGING;RENDERING;VOLUMERENDERING;HYBRID;WIDGETS;PARALLEL")

# The list of available languages.
SET(VTK_LANGUAGES "")

# VTK Configuration options.
SET(VTK_BUILD_SHARED_LIBS "ON")
SET(VTK_DEBUG_LEAKS "OFF")
SET(VTK_USE_TK "OFF")
SET(VTK_HAVE_VP1000 "")
SET(VTK_USE_MANGLED_MESA "OFF")
SET(VTK_MPIRUN_EXE "")
SET(VTK_MPI_MAX_NUMPROCS "")
SET(VTK_MPI_POSTFLAGS "")
SET(VTK_MPI_PREFLAGS "")
SET(VTK_MPI_CLIENT_PREFLAGS "")
SET(VTK_MPI_SERVER_PREFLAGS "")
SET(VTK_MPI_CLIENT_POSTFLAGS "")
SET(VTK_MPI_SERVER_POSTFLAGS "")
SET(VTK_OPENGL_HAS_OSMESA "OFF")
SET(VTK_USE_64BIT_IDS "OFF")
SET(VTK_USE_ANSI_STDLIB "ON")
SET(VTK_USE_CARBON "OFF")
SET(VTK_USE_COCOA "OFF")
SET(VTK_USE_GUISUPPORT "OFF")
SET(VTK_USE_MATROX_IMAGING "OFF")
SET(VTK_USE_MFC "")
SET(VTK_USE_QVTK "")
SET(VTK_USE_MPI "OFF")
SET(VTK_USE_PARALLEL "ON")
SET(VTK_USE_RENDERING "ON")
SET(VTK_USE_VIDEO_FOR_WINDOWS "ON")
SET(VTK_USE_GL2PS "OFF")
SET(VTK_USE_X "0")
SET(VTK_WRAP_JAVA "OFF")
SET(VTK_WRAP_PYTHON "OFF")
SET(VTK_WRAP_TCL "OFF")
SET(VTK_WRAP_DOTNET "ON")
SET(VTK_LEGACY_REMOVE "OFF")
SET(VTK_LEGACY_SILENT "OFF")

# The Hybrid and VolumeRendering kits are now switched with Rendering.
SET(VTK_USE_HYBRID "ON")
SET(VTK_USE_VOLUMERENDERING "ON")

# The Tcl/Tk configuration.
SET(VTK_TCL_TK_STATIC "")
SET(VTK_TCL_TK_COPY_SUPPORT_LIBRARY "")
SET(VTK_TCL_SUPPORT_LIBRARY_PATH "")
SET(VTK_TK_SUPPORT_LIBRARY_PATH "")
SET(VTK_TCL_TK_MACROS_MODULE "${VTK_INSTALL_PREFIX}/lib/vtk-5.0/CMake/vtkTclTkMacros.cmake")
SET(VTK_TCL_HOME "")
SET(VTK_WRAP_TCL_EXE "")
SET(VTK_WRAP_TCL_INIT_EXE "")
SET(VTK_TK_INTERNAL_DIR "")
SET(VTK_TK_RESOURCES_DIR "")

# The Java configuration.
SET(VTK_JAVA_JAR "")
SET(VTK_PARSE_JAVA_EXE "")
SET(VTK_WRAP_JAVA_EXE "")

# The Python configuration.
SET(VTK_WRAP_PYTHON_EXE "")
SET(VTK_WRAP_PYTHON_INIT_EXE "")

# The Doxygen configuration.
SET(VTK_DOXYGEN_HOME "${VTK_INSTALL_PREFIX}/lib/vtk-5.0/doxygen")

# The VTK test script locations.
SET(VTK_HEADER_TESTING_PY "${VTK_INSTALL_PREFIX}/lib/vtk-5.0/testing/HeaderTesting.py")
SET(VTK_FIND_STRING_TCL "${VTK_INSTALL_PREFIX}/lib/vtk-5.0/testing/FindString.tcl")
SET(VTK_PRINT_SELF_CHECK_TCL "${VTK_INSTALL_PREFIX}/lib/vtk-5.0/testing/PrintSelfCheck.tcl")
SET(VTK_RT_IMAGE_TEST_TCL "${VTK_INSTALL_PREFIX}/lib/vtk-5.0/testing/rtImageTest.tcl")
SET(VTK_PRT_IMAGE_TEST_TCL "${VTK_INSTALL_PREFIX}/lib/vtk-5.0/testing/prtImageTest.tcl")

# The names of utility libraries used by VTK.
SET(VTK_PNG_LIBRARIES      "vtkpng")
SET(VTK_ZLIB_LIBRARIES     "vtkzlib")
SET(VTK_JPEG_LIBRARIES     "vtkjpeg")
SET(VTK_TIFF_LIBRARIES     "vtktiff")
SET(VTK_EXPAT_LIBRARIES    "vtkexpat")
SET(VTK_FREETYPE_LIBRARIES "vtkfreetype")

# The VTK Qt configuration.
IF(VTK_USE_QVTK)
  INCLUDE(${VTK_DIR}/VTKConfigQt.cmake)
ENDIF(VTK_USE_QVTK)

# A VTK install tree always provides one build configuration.  A VTK
# build tree may provide either one or multiple build configurations
# depending on the CMake generator used.  Since VTK can be used either
# from a build tree or an install tree it is useful for outside
# projects to know the configurations available.  If this
# VTKConfig.cmake is in a VTK install tree VTK_CONFIGURATION_TYPES
# will be empty and VTK_BUILD_TYPE will be set to the value of
# CMAKE_BUILD_TYPE used to build VTK.  If VTKConfig.cmake is in a VTK
# build tree then VTK_CONFIGURATION_TYPES and VTK_BUILD_TYPE will have
# values matching CMAKE_CONFIGURATION_TYPES and CMAKE_BUILD_TYPE for
# that build tree (only one will ever be set).
SET(VTK_CONFIGURATION_TYPES )
SET(VTK_BUILD_TYPE MinSizeRel)

# The VTK library dependencies.
IF(NOT VTK_NO_LIBRARY_DEPENDS AND EXISTS "${VTK_INSTALL_PREFIX}/lib/vtk-5.0/VTKLibraryDepends.cmake")
  INCLUDE("${VTK_INSTALL_PREFIX}/lib/vtk-5.0/VTKLibraryDepends.cmake")
ENDIF(NOT VTK_NO_LIBRARY_DEPENDS AND EXISTS "${VTK_INSTALL_PREFIX}/lib/vtk-5.0/VTKLibraryDepends.cmake")

# The old, less clear name for VTK_RUNTIME_LIBRARY_DIRS.  Kept here
# for compatibility.
SET(VTK_RUNTIME_DIRS ${VTK_RUNTIME_LIBRARY_DIRS})


