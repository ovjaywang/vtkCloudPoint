/*=========================================================================

  Program:   Visualization Toolkit
  Module:    $RCSfile: vtkImageConstantPad.h,v $

  Copyright (c) Ken Martin, Will Schroeder, Bill Lorensen
  All rights reserved.
  See Copyright.txt or http://www.kitware.com/Copyright.htm for details.

     This software is distributed WITHOUT ANY WARRANTY; without even
     the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
     PURPOSE.  See the above copyright notice for more information.

=========================================================================*/
// .NAME vtkImageConstantPad - Makes image larger by padding with constant.
// .SECTION Description
// vtkImageConstantPad changes the image extent of its input.
// Any pixels outside of the original image extent are filled with
// a constant value.

// .SECTION See Also
// vtkImageWrapPad vtkImageMirrorPad

#ifndef __vtkImageConstantPad_h
#define __vtkImageConstantPad_h


#include "vtkImagePadFilter.h"

class VTK_IMAGING_EXPORT vtkImageConstantPad : public vtkImagePadFilter
{
public:
  static vtkImageConstantPad *New();
  vtkTypeRevisionMacro(vtkImageConstantPad,vtkImagePadFilter);

  void PrintSelf(ostream& os, vtkIndent indent);

  // Description:
  // Set/Get the pad value.
  vtkSetMacro(Constant, double);
  vtkGetMacro(Constant, double);
  
  
protected:
  vtkImageConstantPad();
  ~vtkImageConstantPad() {};

  double Constant;

  void ThreadedRequestData (vtkInformation* request,
                            vtkInformationVector** inputVector,
                            vtkInformationVector* outputVector,
                            vtkImageData ***inData, vtkImageData **outData,
                            int ext[6], int id);
private:
  vtkImageConstantPad(const vtkImageConstantPad&);  // Not implemented.
  void operator=(const vtkImageConstantPad&);  // Not implemented.
};

#endif



