#
# a cmake implementation of the Wrap DotNet command
#

MACRO(VTK_WRAP_DOTNET TARGET SRC_LIST_NAME SOURCES)
  IF(NOT VTK_WRAP_DOTNET_EXE)
    MESSAGE(SEND_ERROR "VTK_WRAP_DOTNET_EXE not specified when calling VTK_WRAP_DOTNET")
  ENDIF(NOT VTK_WRAP_DOTNET_EXE)
  IF(NOT VTK_WRAP_DOTNET_CLASSKITS)
    MESSAGE(SEND_ERROR "VTK_WRAP_DOTNET_CLASSKITS not specified when calling VTK_WRAP_DOTNET")
  ENDIF(NOT VTK_WRAP_DOTNET_CLASSKITS)
  # for new cmake use the new custom commands
  IF("${CMAKE_MAJOR_VERSION}.${CMAKE_MINOR_VERSION}" GREATER 1.6)

    # Initialize the custom target counter.
    IF(VTK_WRAP_DOTNET_NEED_CUSTOM_TARGETS)
      SET(VTK_WRAP_DOTNET_CUSTOM_COUNT "")
      SET(VTK_WRAP_DOTNET_CUSTOM_NAME ${TARGET})
      SET(VTK_WRAP_DOTNET_CUSTOM_LIST)
    ENDIF(VTK_WRAP_DOTNET_NEED_CUSTOM_TARGETS)

    SET(DOTNET_NO_WRAP
      )
    
    # For each class
    FOREACH(FILE ${SOURCES})
      # should we wrap the file?
      GET_SOURCE_FILE_PROPERTY(TMP_WRAP_EXCLUDE ${FILE} WRAP_EXCLUDE)

      FOREACH(NO_WRAP_FILE ${DOTNET_NO_WRAP})
        STRING(COMPARE EQUAL ${FILE} ${NO_WRAP_FILE} PRE_EXCLUDE)
        IF(${PRE_EXCLUDE})
          SET(TMP_WRAP_EXCLUDE 1)
        ENDIF(${PRE_EXCLUDE})
      ENDFOREACH(NO_WRAP_FILE ${DOTNET_NO_WRAP})
      
      # if we should wrap it
      IF (NOT TMP_WRAP_EXCLUDE)
        
        # what is the filename without the extension
        GET_FILENAME_COMPONENT(TMP_FILENAME ${FILE} NAME_WE)
        
        # the input file might be full path so handle that
        GET_FILENAME_COMPONENT(TMP_FILEPATH ${FILE} PATH)
        
        # compute the input filename
        IF (TMP_FILEPATH)
          SET(TMP_INPUT ${TMP_FILEPATH}/${TMP_FILENAME}.h) 
        ELSE (TMP_FILEPATH)
          SET(TMP_INPUT ${CMAKE_CURRENT_SOURCE_DIR}/${TMP_FILENAME}.h)
        ENDIF (TMP_FILEPATH)
        
        # is it abstract?
        GET_SOURCE_FILE_PROPERTY(TMP_ABSTRACT ${FILE} ABSTRACT)
        IF (TMP_ABSTRACT)
          SET(TMP_CONCRETE 0)
        ELSE (TMP_ABSTRACT)
          SET(TMP_CONCRETE 1)
        ENDIF (TMP_ABSTRACT)

        # add the info to the init file
        SET(VTK_WRAPPER_INIT_DATA
          "${VTK_WRAPPER_INIT_DATA}\n${TMP_FILENAME} vtk${KIT}")
        
        # new source file is nameDotNet.cxx, add to resulting list
        SET(${SRC_LIST_NAME} ${${SRC_LIST_NAME}} 
          ${TMP_FILENAME}DotNet.cxx)

        # add custom command to output
        ADD_CUSTOM_COMMAND(
          OUTPUT ${CMAKE_CURRENT_BINARY_DIR}/${TMP_FILENAME}DotNet.cxx
          DEPENDS ${VTK_WRAP_DOTNET_EXE} ${VTK_WRAP_HINTS} ${TMP_INPUT}
          COMMAND ${VTK_WRAP_DOTNET_EXE}
          ARGS ${TMP_INPUT} ${VTK_WRAP_HINTS} ${TMP_CONCRETE} 
          ${CMAKE_CURRENT_BINARY_DIR}/${TMP_FILENAME}DotNet.cxx
          COMMENT "DotNet Wrappings"
          )

        # Add this output to a custom target if needed.
        IF(VTK_WRAP_DOTNET_NEED_CUSTOM_TARGETS)
          SET(VTK_WRAP_DOTNET_CUSTOM_LIST ${VTK_WRAP_DOTNET_CUSTOM_LIST}
            ${CMAKE_CURRENT_BINARY_DIR}/${TMP_FILENAME}DotNet.cxx)
          SET(VTK_WRAP_DOTNET_CUSTOM_COUNT ${VTK_WRAP_DOTNET_CUSTOM_COUNT}x)
          IF(VTK_WRAP_DOTNET_CUSTOM_COUNT MATCHES "^${VTK_WRAP_DOTNET_CUSTOM_LIMIT}$")
            SET(VTK_WRAP_DOTNET_CUSTOM_NAME ${VTK_WRAP_DOTNET_CUSTOM_NAME}Hack)
            ADD_CUSTOM_TARGET(${VTK_WRAP_DOTNET_CUSTOM_NAME} DEPENDS ${VTK_WRAP_DOTNET_CUSTOM_LIST})
            SET(KIT_DOTNET_DEPS ${VTK_WRAP_DOTNET_CUSTOM_NAME})
            SET(VTK_WRAP_DOTNET_CUSTOM_LIST)
            SET(VTK_WRAP_DOTNET_CUSTOM_COUNT)
          ENDIF(VTK_WRAP_DOTNET_CUSTOM_COUNT MATCHES "^${VTK_WRAP_DOTNET_CUSTOM_LIMIT}$")
        ENDIF(VTK_WRAP_DOTNET_NEED_CUSTOM_TARGETS)
      ENDIF (NOT TMP_WRAP_EXCLUDE)
    ENDFOREACH(FILE)
    
    # finish the data file for the init file        
    CONFIGURE_FILE(
      ${VTK_CMAKE_DIR}/vtkWrapperInit.data.in 
      ${CMAKE_CURRENT_BINARY_DIR}/${TARGET}Init.data
      COPY_ONLY
      IMMEDIATE
      )
  
    FILE(APPEND "${VTK_WRAP_DOTNET_CLASSKITS}" "${VTK_WRAPPER_INIT_DATA}")
  
  ELSE("${CMAKE_MAJOR_VERSION}.${CMAKE_MINOR_VERSION}" GREATER 1.6)
    MESSAGE(SEND_ERROR "VTK_WRAP_DOTNET cannot be used with CMake version older than 1.6.")    
  ENDIF("${CMAKE_MAJOR_VERSION}.${CMAKE_MINOR_VERSION}" GREATER 1.6)  
ENDMACRO(VTK_WRAP_DOTNET)

