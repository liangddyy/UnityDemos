/*************************************************************************
 *  Copyright (C), 2017-2018, Mogoson tech. Co., Ltd.
 *  FileName: BezierCurve.cs
 *  Author: Mogoson   Version: 1.0   Date: 1/7/2017
 *  Version Description:
 *    Internal develop version,mainly to achieve its function.
 *  File Description:
 *    Ignore.
 *  Class List:
 *    <ID>           <name>             <description>
 *     1.
 *  Function List:
 *    <class ID>     <name>             <description>
 *     1.
 *  History:
 *    <ID>    <author>      <time>      <version>      <description>
 *     1.     Mogoson     1/7/2017       1.0        Build this file.
 *************************************************************************/

namespace Developer.Math.Curve
{
    using UnityEngine;

    /// <summary>
    /// Bezier Linear Curve.
    /// </summary>
    public struct BezierLinearCurve
    {
        #region Property and Field
        /// <summary>
        /// First Point of curve.
        /// </summary>
        public Vector3 p0;

        /// <summary>
        /// Second Point of curve.
        /// </summary>
        public Vector3 p1;
        #endregion

        #region Public Method
        /// <summary>
        /// Constructor.
        /// </summary>
        public BezierLinearCurve(Vector3 p0, Vector3 p1)
        {
            this.p0 = p0;
            this.p1 = p1;
        }//B...()_end

        /// <summary>
        /// Get curve point base on t.
        /// </summary>
        /// <param name="t">t is in the range (0~1).</param>
        /// <returns></returns>
        public Vector3 GetPoint(float t)
        {
            return (1 - t) * p0 + t * p1;
        }//GetPoint()_end
        #endregion
    }//struct_end

    /// <summary>
    /// Bezier Quadratic Curve.
    /// </summary>
    public struct BezierQuadraticCurve
    {
        #region Property and Field
        /// <summary>
        /// First Point of curve.
        /// </summary>
        public Vector3 p0;

        /// <summary>
        /// Second Point of curve.
        /// </summary>
        public Vector3 p1;

        /// <summary>
        /// Third Point of curve.
        /// </summary>
        public Vector3 p2;
        #endregion

        #region Public Method
        /// <summary>
        /// Constructor.
        /// </summary>
        public BezierQuadraticCurve(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
        }//B...()_end

        /// <summary>
        /// Get curve point base on t.
        /// </summary>
        /// <param name="t">t is in the range (0~1).</param>
        /// <returns></returns>
        public Vector3 GetPoint(float t)
        {
            return Mathf.Pow(1 - t, 2) * p0 + 2 * t * (1 - t) * p1 + Mathf.Pow(t, 2) * p2;
        }//GetPoint()_end
        #endregion
    }//struct_end

    /// <summary>
    /// Bezier Cubic Curve.
    /// </summary>
    public struct BezierCubicCurve
    {
        #region Property and Field
        /// <summary>
        /// First Point of curve.
        /// </summary>
        public Vector3 p0;

        /// <summary>
        /// Second Point of curve.
        /// </summary>
        public Vector3 p1;

        /// <summary>
        /// Third Point of curve.
        /// </summary>
        public Vector3 p2;

        /// <summary>
        /// Fourth Point of curve.
        /// </summary>
        public Vector3 p3;
        #endregion

        #region Public Method
        /// <summary>
        /// Constructor.
        /// </summary>
        public BezierCubicCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }//B...()_end

        /// <summary>
        /// Get curve point base on t.
        /// </summary>
        /// <param name="t">t is in the range (0~1).</param>
        /// <returns></returns>
        public Vector3 GetPoint(float t)
        {
            return Mathf.Pow(1 - t, 3) * p0 + 3 * t * Mathf.Pow(1 - t, 2) * p1 +
                3 * (1 - t) * Mathf.Pow(t, 2) * p2 + Mathf.Pow(t, 3) * p3;
        }//GetPoint()_end
        #endregion
    }//struct_end
} 
