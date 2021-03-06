/*=========================================================================

  Program:   Visualization Toolkit
  Module:    $RCSfile: vtkExtentSplitter.h,v $

  Copyright (c) Ken Martin, Will Schroeder, Bill Lorensen
  All rights reserved.
  See Copyright.txt or http://www.kitware.com/Copyright.htm for details.

     This software is distributed WITHOUT ANY WARRANTY; without even
     the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
     PURPOSE.  See the above copyright notice for more information.

=========================================================================*/
// .NAME vtkExtentSplitter - Split an extent across other extents.
// .SECTION Description
// vtkExtentSplitter splits each input extent into non-overlapping
// sub-extents that are completely contained within other "source
// extents".  A source extent corresponds to some resource providing
// an extent.  Each source extent has an integer identifier, integer
// priority, and an extent.  The input extents are split into
// sub-extents according to priority, availability, and amount of
// overlap of the source extents.  This can be used by parallel data
// readers to read as few piece files as possible.

#ifndef __vtkExtentSplitter_h
#define __vtkExtentSplitter_h

#include "vtkObject.h"

class vtkExtentSplitterInternals;

class VTK_COMMON_EXPORT vtkExtentSplitter : public vtkObject
{
public:
  vtkTypeRevisionMacro(vtkExtentSplitter,vtkObject);
  void PrintSelf(ostream& os, vtkIndent indent);  
  static vtkExtentSplitter *New();
  
  // Description:
  // Add/Remove a source providing the given extent.  Sources with
  // higher priority numbers are favored.  Source id numbers and
  // priorities must be non-negative.
  void AddExtentSource(int id, int priority, int x0, int x1,
                       int y0, int y1, int z0, int z1);
  void AddExtentSource(int id, int priority, int* extent);
  void RemoveExtentSource(int id);
  void RemoveAllExtentSources();
  
  // Description:
  // Add an extent to the queue of extents to be split among the
  // available sources.
  void AddExtent(int x0, int x1, int y0, int y1, int z0, int z1);
  void AddExtent(int* extent);
  
  // Description:
  // Split the extents currently in the queue among the available
  // sources.  The queue is empty when this returns.  Returns 1 if all
  // extents could be read.  Returns 0 if any portion of any extent
  // was not available through any source.
  int ComputeSubExtents();
  
  // Description:
  // Get the number of sub-extents into which the original set of
  // extents have been split across the available sources.  Valid
  // after a call to ComputeSubExtents.
  int GetNumberOfSubExtents();
  
  // Description:
  // Get the sub-extent associated with the given index.  Use
  // GetSubExtentSource to get the id of the source from which this
  // sub-extent should be read.  Valid after a call to
  // ComputeSubExtents.
  int* GetSubExtent(int index);
  void GetSubExtent(int index, int* extent);
  
  // Description:
  // Get the id of the source from which the sub-extent associated
  // with the given index should be read.  Returns -1 if no source
  // provides the sub-extent.
  int GetSubExtentSource(int index);  
  
  // Description:
  // Get/Set whether "point mode" is on.  In point mode, sub-extents
  // are generated to ensure every point in the update request is
  // read, but not necessarily every cell.  This can be used when
  // point data are stored in a planar slice per piece with no cell
  // data.  The default is OFF.
  vtkGetMacro(PointMode, int);
  vtkSetMacro(PointMode, int);
  vtkBooleanMacro(PointMode, int);
  
protected:
  vtkExtentSplitter();
  ~vtkExtentSplitter();
  
  // Internal utility methods.
  void SplitExtent(int* extent, int* subextent);
  int IntersectExtents(const int* extent1, const int* extent2, int* result);
  int Min(int a, int b);
  int Max(int a, int b);
  
  // Internal implementation data.
  vtkExtentSplitterInternals* Internal;
  
  // On if reading only all points (but not always all cells) is
  // necessary.  Used for reading volumes of planar slices storing
  // only point data.
  int PointMode;
  
private:
  vtkExtentSplitter(const vtkExtentSplitter&);  // Not implemented.
  void operator=(const vtkExtentSplitter&);  // Not implemented.
};

#endif
