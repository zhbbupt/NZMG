﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication2
{
    class LB_TO_XY
    {
                //Define the params test
        private const double p = 3.1415926;
        private double latitudeOrigin;//原点纬度（度）
        private double longitudeOrigin;//原点经度（度）
        private double latitude;//纬度（弧度）
        private double longtitude;//经度（弧度）
        //private double param_w;//参数w
        private const double N_EXC = 6023150;//北伪偏移
        private const double E_EXC = 2510000;//东伪偏移

        private double SemiMajorAxis;//椭圆半长轴

        // private double latitude_distance;//等距纬度差值
        // private double longtitude_distance;//经线差值
        private double[] A = new double[10] { 0.6399175073, -0.1358797613, 0.063294409, -0.02526853, 0.0117879, -0.0055161, 0.0026906, -0.001333, 0.00067, -0.00034 };
        private double[,] B = new double[6, 2];
        private void init()
        {
                B[0,0] = 0.7557853228;
                B[0,1] = 0;
                B[1,0] = 0.249204646;
                B[1,1] = 0.003371507;
                B[2,0] = -0.001541739;
                B[2,1] = 0.041058560;
                B[3,0] = -0.10162907;
                B[3,1] = 0.01727609;
                B[4,0] = -0.26623489;
                B[4,1] = -0.36249218;
                B[5,0] = -0.6870983;
                B[5,1] = -1.1651967;
                           C[0, 0] = 1.3231270439;
                C[0, 1] = 0;
                C[1, 0] = -0.577245789;
                C[1, 1] = -0.007809598;
                C[2, 0] = 0.508307513;
                C[2, 1] = -0.112208952;
                C[3, 0] = -0.15094762;
                C[3, 1] = 0.18200602;
                C[4, 0] = 1.01418179;
                C[4, 1] = 1.64497696;
                C[5, 0] = 1.9660549;
                C[5, 1] = 2.5127645;
        }
        private double[,] C = new double[6, 2];
        private double[] D = new double[9] { 1.5627014243, 0.5185406398, -0.03333098, -0.1052906, -0.0368594, 0.007317, 0.01220, 0.00394, -0.0013 };

        private double X, Y;

        private struct complex  //复数结构体
        {
            public double real;
            public double imag;
        }

        public double[] NZMG_LB_TO_XY(double m_B0, double m_L0, double m_B, double m_L, double SemiAxis)
        {
            latitudeOrigin = m_B0 * 3600;
            longitudeOrigin = m_L0 * (p / 180);
            latitude =  m_B * 3600;//度数单位由 度 化为 秒 ;
            longtitude = m_L * (p / 180);//度数单位由 度 化为 弧度;
            SemiMajorAxis = SemiAxis;
            double[] result=new double[2];
            init();
            result = TransformBLToXY();
            return result;
            //X = m_X;
            //Y = m_Y;
        }


        //private double[,] get_C
        //{
        //    get
        //    {
        //        return C;
        //    }
        //    set
        //    {
        //        C[0, 0] = 1.3231270439;
        //        C[0, 1] = 0;
        //        C[1, 0] = -0.577245789;
        //        C[1, 1] = -0.007809598;
        //        C[2, 0] = 0.508307513;
        //        C[2, 1] = -0.112208952;
        //        C[3, 0] = -0.15094762;
        //        C[3, 1] = 0.18200602;
        //        C[4, 0] = 1.01418179;
        //        C[4, 1] = 1.64497696;
        //        C[5, 0] = 1.9660549;
        //        C[5, 1] = 2.5127645;
        //    }
        //}


        private void Cal_param_w(double latitude_distance, out double param_w)
        {
            int i = 1;
            param_w = 0;

            while (i <= 10)
            {
                param_w = param_w + A[i - 1] * Math.Pow(latitude_distance * Math.Pow(10, -5), i);
                i++;
            }

        }


        private void Cal_longtitude_distance(double longtitude, out double longtitude_distance)
        {
            longtitude_distance = (longtitude - longitudeOrigin); //单位需要转化为弧度
        }


        private void Cal_latitude_distance(double latitude, out double latitude_distance)
        {
            latitude_distance = latitude - latitudeOrigin; //单位需要转化为秒
        }


        private void complex_add(out complex tmp, complex A, complex B)  //复数加法
        {
            tmp.real = A.real + B.real;
            tmp.imag = A.imag + B.imag;

        }
        private void complex_multiplication(out complex tmp, complex A, complex B)//复数乘法
        {
            tmp.real = A.real * B.real - A.imag * B.imag;
            tmp.imag = A.real * B.imag + A.imag * B.real;

        }

        private void complex_division(out complex tmp, complex A, complex B)//复数除法
        {
            tmp.real = (A.real * B.real - +A.imag * B.imag) / (B.real * B.real + B.imag * B.imag);
            tmp.imag = (A.imag * B.real + A.real * B.imag) / (B.real * B.real + B.imag * B.imag);

        }


        protected double[] TransformBLToXY()
        {
            double latitude_distance;//纬度差值
            double longtitude_distance;//经线差值
            double param_w; //等距纬度相应的差值
            //double N, E; //NZMG上的坐标
            Cal_latitude_distance(latitude, out latitude_distance);
            Cal_param_w(latitude_distance, out param_w);
            Cal_longtitude_distance(longtitude, out longtitude_distance);
            complex Zeta, tmp_B, tmp_Zeta, Z, tmp_Z;
            Zeta.real = param_w;
            Zeta.imag = longtitude_distance;
            tmp_Zeta = Zeta;
            Z.real = 0;
            Z.imag = 0;
            tmp_Z.real = 0;
            tmp_Z.imag = 0;
            
            for (int i = 0; i <= 5; i++)
            {
                tmp_B.real = B[i, 0];
                tmp_B.imag = B[i, 1];
                complex_multiplication(out tmp_Z, tmp_B, tmp_Zeta);
                complex_add(out Z, Z, tmp_Z);
                complex_multiplication(out tmp_Zeta, tmp_Zeta, Zeta);
            }
            double m_X, m_Y;
            m_X = N_EXC + Z.real * SemiMajorAxis;
            m_Y = E_EXC + Z.imag * SemiMajorAxis;
            double[] result=new double[2];
            result[0] = m_X;
            result[1] = m_Y;
            return result;
        }
    }
}
