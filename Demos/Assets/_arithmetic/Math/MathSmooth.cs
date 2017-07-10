/*************************************************************************
 *  Copyright (C), 2016-2017, Mogoson tech. Co., Ltd.
 *  FileName: MathSmooth.cs
 *  Author: Mogoson   Version: 1.0   Date: 6/21/2016
 *  Version Description:
 *    Internal develop version,mainly to achieve its function.
 *  File Description:
 *    Ignore.
 *  Class List:
 *    <ID>           <name>             <description>
 *     1.       LinearSmooth             Ignore.
 *     2.       QuadraticSmooth          Ignore.
 *     3.       CubicSmooth              Ignore.
 *  Function List:
 *    <class ID>     <name>             <description>
 *     1.       ThreePointSmooth           Ignore.
 *     1.       FivePointSmooth            Ignore.
 *     1.       SevenPointSmooth           Ignore.
 *     2.       FivePointSmooth            Ignore.
 *     2.       SevenPointSmooth           Ignore.
 *     3.       FivePointSmooth            Ignore.
 *     3.       SevenPointSmooth           Ignore.
 *  History:
 *    <ID>    <author>      <time>      <version>      <description>
 *     1.     Mogoson     6/21/2016       1.0        Build this file.
 *************************************************************************/

namespace Developer.Math.Smooth
{
    /// <summary>
    /// 线性拟合
    /// </summary>
    public static class LinearSmooth
    {
        #region Public Method
        /// <summary>
        /// 三点线性拟合
        /// </summary>
        /// <param name="source">数据源</param>
        /// <returns>拟合结果</returns>
        public static double[] ThreePointSmooth(double[] source)
        {
            var n = source.Length;
            var result = new double[n];
            if (n < 3)
            {
                for (int i = 0; i < n; i++)
                {
                    result[i] = source[i];
                }//for_end
            }
            else
            {
                result[0] = (5 * source[0] + 2 * source[1] - source[2]) / 6;
                for (int i = 1; i < n - 1; i++)
                {
                    result[i] = (source[i - 1] + source[i] + source[i + 1]) / 3;
                }//for_end
                result[n - 1] = (5 * source[n - 1] + 2 * source[n - 2] - source[n - 3]) / 6;
            }//else_end
            return result;
        }//Three...()_end

        /// <summary>
        /// 五点线性拟合
        /// </summary>
        /// <param name="source">数据源</param>
        /// <returns>拟合结果</returns>
        public static double[] FivePointSmooth(double[] source)
        {
            var n = source.Length;
            var result = new double[n];
            if (n < 5)
            {
                for (int i = 0; i < n; i++)
                {
                    result[i] = source[i];
                }//for_end
            }
            else
            {
                result[0] = (3 * source[0] + 2 * source[1] + source[2] - source[4]) / 5;
                result[1] = (4 * source[0] + 3 * source[1] + 2 * source[2] + source[3]) / 10;
                for (int i = 2; i < n - 2; i++)
                {
                    result[i] = (source[i - 2] + source[i - 1] + source[i] + source[i + 1] + source[i + 2]) / 5;
                }//for_end
                result[n - 2] = (4 * source[n - 1] + 3 * source[n - 2] + 2 * source[n - 3] + source[n - 4]) / 10;
                result[n - 1] = (3 * source[n - 1] + 2 * source[n - 2] + source[n - 3] - source[n - 5]) / 5;
            }//else_end
            return result;
        }//Five...()_end

        /// <summary>
        /// 七点线性拟合
        /// </summary>
        /// <param name="source">数据源</param>
        /// <returns>拟合结果</returns>
        public static double[] SevenPointSmooth(double[] source)
        {
            var n = source.Length;
            var result = new double[n];
            if (n < 7)
            {
                for (int i = 0; i < n; i++)
                {
                    result[i] = source[i];
                }//for_end
            }
            else
            {
                result[0] = (13 * source[0] + 10 * source[1] + 7 * source[2] + 4 * source[3] +
                    source[4] - 2 * source[5] - 5 * source[6]) / 28;

                result[1] = (5 * source[0] + 4 * source[1] + 3 * source[2] + 2 * source[3] +
                    source[4] - source[6]) / 14;

                result[2] = (7 * source[0] + 6 * source[1] + 5 * source[2] + 4 * source[3] +
                    3 * source[4] + 2 * source[5] + source[6]) / 28;

                for (int i = 3; i < n - 3; i++)
                {
                    result[i] = (source[i - 3] + source[i - 2] + source[i - 1] + source[i] +
                        source[i + 1] + source[i + 2] + source[i + 3]) / 7;
                }//for_end

                result[n - 3] = (7 * source[n - 1] + 6 * source[n - 2] + 5 * source[n - 3] +
                    4 * source[n - 4] + 3 * source[n - 5] + 2 * source[n - 6] + source[n - 7]) / 28;

                result[n - 2] = (5 * source[n - 1] + 4 * source[n - 2] + 3 * source[n - 3] +
                    2 * source[n - 4] + source[n - 5] - source[n - 7]) / 14;

                result[n - 1] = (13 * source[n - 1] + 10 * source[n - 2] + 7 * source[n - 3] +
                    4 * source[n - 4] + source[n - 5] - 2 * source[n - 6] - 5 * source[n - 7]) / 28;
            }//else_end
            return result;
        }//Seven...()_end
        #endregion
    } 

    /// <summary>
    /// 二次拟合
    /// </summary>
    public static class QuadraticSmooth
    {
        #region Public Method
        /// <summary>
        /// 五点二次拟合
        /// </summary>
        /// <param name="source">数据源</param>
        /// <returns>拟合结果</returns>
        public static double[] FivePointSmooth(double[] source)
        {
            var n = source.Length;
            var result = new double[n];
            if (n < 5)
            {
                for (int i = 0; i < n; i++)
                {
                    result[i] = source[i];
                }//for_end
            }
            else
            {
                result[0] = (31 * source[0] + 9 * source[1] - 3 * source[2] - 5 * source[3] + 3 * source[4]) / 35;
                result[1] = (9 * source[0] + 13 * source[1] + 12 * source[2] + 6 * source[3] - 5 * source[4]) / 35;
                for (int i = 2; i < n - 2; i++)
                {
                    result[i] = (-3 * (source[i - 2] + source[i + 2]) +
                              12 * (source[i - 1] + source[i + 1]) + 17 * source[i]) / 35;
                }//for_end
                result[n - 2] = (9 * source[n - 1] + 13 * source[n - 2] + 12 * source[n - 3] + 6 * source[n - 4] - 5 * source[n - 5]) / 35;
                result[n - 1] = (31 * source[n - 1] + 9 * source[n - 2] - 3 * source[n - 3] - 5 * source[n - 4] + 3 * source[n - 5]) / 35;
            }//else_end
            return result;
        }//Five...()_end

        /// <summary>
        /// 七点二次拟合
        /// </summary>
        /// <param name="source">数据源</param>
        /// <returns>拟合结果</returns>
        public static double[] SevenPointSmooth(double[] source)
        {
            var n = source.Length;
            var result = new double[n];
            if (n < 7)
            {
                for (int i = 0; i < n; i++)
                {
                    result[i] = source[i];
                }//for_end
            }
            else
            {
                result[0] = (32 * source[0] + 15 * source[1] + 3 * source[2] - 4 * source[3] - 
                    6 * source[4] - 3 * source[5] + 5 * source[6]) / 42;

                result[1] = (5 * source[0] + 4 * source[1] + 3 * source[2] + 2 * source[3] + 
                    source[4] - source[6]) / 14;

                result[2] = (source[0] + 3 * source[1] + 4 * source[2] + 4 * source[3] + 
                    3 * source[4] + source[5] - 2 * source[6]) / 14;
                
                for (int i = 3; i < n - 3; i++)
                {
                    result[i] = (-2 * (source[i - 3] + source[i + 3]) + 3 * (source[i - 2] + source[i + 2]) + 
                        6 * (source[i - 1] + source[i + 1]) + 7 * source[i]) / 21;
                }//for_end

                result[n - 3] = (source[n - 1] + 3 * source[n - 2] + 4 * source[n - 3] + 4 * source[n - 4] + 
                    3 * source[n - 5] + source[n - 6] - 2 * source[n - 7]) / 14;

                result[n - 2] = (5 * source[n - 1] + 4 * source[n - 2] + 3 * source[n - 3] + 2 * source[n - 4] +
                    source[n - 5] - source[n - 7]) / 14;

                result[n - 1] = (32 * source[n - 1] + 15 * source[n - 2] + 3 * source[n - 3] - 4 * source[n - 4] -
                    6 * source[n - 5] - 3 * source[n - 6] + 5 * source[n - 7]) / 42;
            }//else_end
            return result;
        }//Seven...()_end
        #endregion
    } 

    /// <summary>
    /// 三次拟合
    /// </summary>
    public static class CubicSmooth
    {
        #region Public Method
        /// <summary>
        /// 五点三次拟合
        /// </summary>
        /// <param name="source">数据源</param>
        /// <returns>拟合结果</returns>
        public static double[] FivePointSmooth(double[] source)
        {
            var n = source.Length;
            var result = new double[n];
            if (n < 5)
            {
                for (int i = 0; i < n; i++)
                {
                    result[i] = source[i];
                }//for_end
            }
            else
            {
                result[0] = (69 * source[0] + 4 * source[1] - 6 * source[2] + 
                    4 * source[3] - source[4]) / 70;

                result[1] = (2 * source[0] + 27 * source[1] + 12 * source[2] - 
                    8 * source[3] + 2 * source[4]) / 35;

                for (int i = 2; i < n - 2; i++)
                {
                    result[i] = (-3 * (source[i - 2] + source[i + 2]) + 12 * (source[i - 1] + 
                        source[i + 1]) + 17 * source[i]) / 35;
                }//for_end

                result[n - 2] = (2 * source[n - 5] - 8 * source[n - 4] + 12 * source[n - 3] + 
                    27 * source[n - 2] + 2 * source[n - 1]) / 35;

                result[n - 1] = (-source[n - 5] + 4 * source[n - 4] - 6 * source[n - 3] +
                    4 * source[n - 2] + 69 * source[n - 1]) / 70;
            }//else_end
            return result;
        }//Five...()_end

        /// <summary>
        /// 七点三次拟合
        /// </summary>
        /// <param name="source">数据源</param>
        /// <returns>拟合结果</returns>
        public static double[] SevenPointSmooth(double[] source)
        {
            var n = source.Length;
            var result = new double[n];
            if (n < 7)
            {
                for (int i = 0; i < n; i++)
                {
                    result[i] = source[i];
                }//for_end
            }
            else
            {
                result[0] = (39 * source[0] + 8 * source[1] - 4 * source[2] - 4 * source[3] + 
                    source[4] + 4 * source[5] - 2 * source[6]) / 42;

                result[1] = (8 * source[0] + 19 * source[1] + 16 * source[2] + 6 * source[3] - 
                    4 * source[4] - 7 * source[5] + 4 * source[6]) / 42;

                result[2] = (-4 * source[0] + 16 * source[1] + 19 * source[2] + 12 * source[3] + 
                    2 * source[4] - 4 * source[5] + source[6]) / 42;

                for (int i = 3; i <= n - 4; i++)
                {
                    result[i] = (-2 * (source[i - 3] + source[i + 3]) + 3 * (source[i - 2] + 
                        source[i + 2]) + 6 * (source[i - 1] + source[i + 1]) + 7 * source[i]) / 21;
                }//for_end

                result[n - 3] = (-4 * source[n - 1] + 16 * source[n - 2] + 19 * source[n - 3] + 
                    12 * source[n - 4] + 2 * source[n - 5] - 4 * source[n - 6] + source[n - 7]) / 42;

                result[n - 2] = (8 * source[n - 1] + 19 * source[n - 2] + 16 * source[n - 3] + 
                    6 * source[n - 4] - 4 * source[n - 5] - 7 * source[n - 6] + 4 * source[n - 7]) / 42;

                result[n - 1] = (39 * source[n - 1] + 8 * source[n - 2] - 4 * source[n - 3] - 
                    4 * source[n - 4] + source[n - 5] + 4 * source[n - 6] - 2 * source[n - 7]) / 42;
            }//else_end
            return result;
        }//Seven...()_end
        #endregion
    } 
} 
