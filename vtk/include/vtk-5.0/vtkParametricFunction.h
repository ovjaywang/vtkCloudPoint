/*=========================================================================

  Program:   Visualization Toolkit
  Module:    $RCSfile: vtkParametricFunction.h,v $

  Copyright (c) Ken Martin, Will Schroeder, Bill Lorensen 
  All rights reserved.
  See Copyright.txt or http://www.kitware.com/Copyright.htm for details.

     This software is distributed WITHOUT ANY WARRANTY; without even 
     the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR 
     PURPOSE.  See the above copyright notice for more information.

=========================================================================*/
// .NAME vtkParametricFunction - abstract interface for parametric functions
// .SECTION Description
// vtkParametricFunction is an abstract interface for functions
// defined by parametric mapping i.e. f(u,v,w)->(x,y,z) where 
// u_min <= u < u_max, v_min <= v < v_max, w_min <= w < w_max. (For
// notational convenience, we will write f(u)->x and assume that
// u means (u,v,w) and x means (x,y,z).)
//
// The interface contains the pure virtual function, Evaluate(), that
// generates a point and the derivatives at that point which are then used to
// construct the surface. A second pure virtual function, EvaluateScalar(),
// can be used to generate a scalar for the surface. Finally, the
// GetDimension() virtual function is used to differentiate 1D, 2D, and 3D
// parametric functions. Since this abstract class defines a pure virtual
// API, its subclasses must implement the pure virtual functions
// GetDimension(), Evaluate() and EvaluateScalar().
//
// This class has also methods for defining a range of parametric values (u,v,w).
// 
// .SECTION Thanks
// Andrew Maclean a.maclean@cas.edu.au for creating and contributing the
// class.
//
// .SECTION See Also
// vtkParametricFunctionSource - tessellates a parametric function
//
// Implementations of derived classes implementing non-orentable surfaces:
// vtkParametricBoy vtkParametricCrossCap vtkParametricFigure8Klein
// vtkParametricKlein vtkParametricMobius vtkParametricRoman
//
// Implementations of derived classes implementing orientable surfaces:
// vtkParametricConicSpiral vtkParametricDini vtkParametricEllipsoid 
// vtkParametricEnneper vtkParametricRandomHills vtkParametricSuperEllipsoid 
// vtkParametricSuperToroid vtkParametricTorus 
//
#ifndef __vtkParametricFunction_h
#define __vtkParametricFunction_h

#include "vtkObject.h"

class VTK_COMMON_EXPORT vtkParametricFunction : public vtkObject
{
public:
  vtkTypeRevisionMacro(vtkParametricFunction, vtkObject);
  void PrintSelf(ostream& os, vtkIndent indent);

  // Description
  // Return the dimension of parametric space. Depending on the dimension,
  // then the (u,v,w) parameters and associated information (e.g., derivates)
  // have meaning. For example, if the dimension of the function is one, then
  // u[0] and Duvw[0...2] have meaning.
  // This is a pure virtual function that must be instantiated in 
  // a derived class. 
  virtual int GetDimension() = 0;

  // Description:
  // Performs the mapping \$f(uvw)->(Pt,Duvw)\$f.
  // This is a pure virtual function that must be instantiated in 
  // a derived class. 
  //
  // uvw are the parameters, with u corresponding to uvw[0],
  // v to uvw[1] and w to uvw[2] respectively. Pt is the returned Cartesian point, 
  // Duvw are the derivatives of this point with respect to u, v and w.  
  // Note that the first three values in Duvw are Du, the next three are Dv, 
  // and the final three are Dw. Du Dv Dw are the partial derivatives of the 
  // function at the point Pt with respect to u, v and w respectively.
  virtual void Evaluate(double uvw[3], double Pt[3], double Duvw[9]) = 0;

  // Description:
  // Calculate a user defined scalar using one or all of uvw, Pt, Duvw.
  // This is a pure virtual function that must be instantiated in 
  // a derived class. 
  //
  // uvw are the parameters with Pt being the the cartesian point, 
  // Duvw are the derivatives of this point with respect to u, v, and w.
  // Pt, Duvw are obtained from Evaluate().
  virtual double EvaluateScalar(double uvw[3], double Pt[3], double Duvw[9]) = 0;

  // Description:
  // Set/Get the minimum u-value.
  vtkSetMacro(MinimumU,double);
  vtkGetMacro(MinimumU,double);

  // Description:
  // Set/Get the maximum u-value.
  vtkSetMacro(MaximumU,double);
  vtkGetMacro(MaximumU,double);

  // Description:
  // Set/Get the minimum v-value.
  vtkSetMacro(MinimumV,double);
  vtkGetMacro(MinimumV,double);

  // Description:
  // Set/Get the maximum v-value.
  vtkSetMacro(MaximumV,double);
  vtkGetMacro(MaximumV,double);

  // Description:
  // Set/Get the minimum w-value.
  vtkSetMacro(MinimumW,double);
  vtkGetMacro(MinimumW,double);

  // Description:
  // Set/Get the maximum w-value.
  vtkSetMacro(MaximumW,double);
  vtkGetMacro(MaximumW,double);

  // Description:
  // Set/Get the flag which joins the first triangle strip to the last one.
  vtkSetMacro(JoinU,int);
  vtkGetMacro(JoinU,int);
  vtkBooleanMacro(JoinU,int);

  // Description:
  // Set/Get the flag which joins the the ends of the triangle strips.
  vtkSetMacro(JoinV,int);
  vtkGetMacro(JoinV,int);
  vtkBooleanMacro(JoinV,int);

  // Description:
  // Set/Get the flag which joins the first triangle strip to 
  // the last one with a twist.
  // JoinU must also be set if this is set. Used when building
  // some non-orientable surfaces.
  vtkSetMacro(TwistU,int);
  vtkGetMacro(TwistU,int);
  vtkBooleanMacro(TwistU,int);

  // Description:
  // Set/Get the flag which joins the ends of the 
  // triangle strips with a twist.
  // JoinV must also be set if this is set. Used when building
  // some non-orientable surfaces.
  vtkSetMacro(TwistV,int);
  vtkGetMacro(TwistV,int);
  vtkBooleanMacro(TwistV,int);

  // Description:
  // Set/Get the flag which determines the ordering of the the 
  // vertices forming the triangle strips. The ordering of the 
  // points being inserted into the triangle strip is important 
  // because it determines the direction of the normals for the 
  // lighting. If set, the ordering is clockwise, otherwise the
  // ordering is anti-clockwise. Default is true (i.e. clockwise
  // ordering).
  vtkSetMacro(ClockwiseOrdering,int);
  vtkGetMacro(ClockwiseOrdering,int);
  vtkBooleanMacro(ClockwiseOrdering,int);

  // Description:
  // Set/Get the flag which determines whether derivatives are available 
  // from the parametric function (i.e., whether the Evaluate() method 
  // returns valid derivatives).
  vtkSetMacro(DerivativesAvailable,int);
  vtkGetMacro(DerivativesAvailable,int);
  vtkBooleanMacro(DerivativesAvailable,int);

protected:
  vtkParametricFunction();
  virtual ~vtkParametricFunction();

  // Variables
  double MinimumU;
  double MaximumU;
  double MinimumV;
  double MaximumV;
  double MinimumW;
  double MaximumW;

  int JoinU;
  int JoinV;
  int JoinW;

  int TwistU;
  int TwistV;
  int TwistW;

  int ClockwiseOrdering;
  
  int DerivativesAvailable;

private:
  vtkParametricFunction(const vtkParametricFunction&);  // Not implemented.
  void operator=(const vtkParametricFunction&);  // Not implemented.

};

#endif
