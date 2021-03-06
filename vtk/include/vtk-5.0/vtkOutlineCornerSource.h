/*=========================================================================

  Program:   Visualization Toolkit
  Module:    $RCSfile: vtkOutlineCornerSource.h,v $

  Copyright (c) Ken Martin, Will Schroeder, Bill Lorensen
  All rights reserved.
  See Copyright.txt or http://www.kitware.com/Copyright.htm for details.

     This software is distributed WITHOUT ANY WARRANTY; without even
     the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
     PURPOSE.  See the above copyright notice for more information.

=========================================================================*/
// .NAME vtkOutlineCornerSource - create wireframe outline corners around bounding box
// .SECTION Description
// vtkOutlineCornerSource creates wireframe outline corners around a user-specified 
// bounding box.

#ifndef __vtkOutlineCornerSource_h
#define __vtkOutlineCornerSource_h

#include "vtkOutlineSource.h"

class VTK_GRAPHICS_EXPORT vtkOutlineCornerSource : public vtkOutlineSource
{
public:
  vtkTypeRevisionMacro(vtkOutlineCornerSource,vtkOutlineSource);
  void PrintSelf(ostream& os, vtkIndent indent);

  // Description:
  // Construct outline corner source with default corner factor = 0.2
  static vtkOutlineCornerSource *New();

  // Description:
  // Set/Get the factor that controls the relative size of the corners
  // to the length of the corresponding bounds
  vtkSetClampMacro(CornerFactor, double, 0.001, 0.5);
  vtkGetMacro(CornerFactor, double);

protected:
  vtkOutlineCornerSource();
  ~vtkOutlineCornerSource() {};

  int RequestData(vtkInformation *, vtkInformationVector **, vtkInformationVector *);

  double CornerFactor;
private:
  vtkOutlineCornerSource(const vtkOutlineCornerSource&);  // Not implemented.
  void operator=(const vtkOutlineCornerSource&);  // Not implemented.
};

#endif
