using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ProyectoFinalArqHard
{
    class ColorRGB
    {

        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }


        

    }

    static class Extensions
    {
        public static Rectangle ToRect(this Size size)
        {
            return new Rectangle(new Point(), size);
        }
    }

    class Program
    {



        

        private static ColorRGB[,] GetValuesWithLockBits48and24(Bitmap bitmap)
        {
            ColorRGB[,] rgb = new ColorRGB[bitmap.Height, bitmap.Width];
            BitmapData bitmapData = bitmap.LockBits(bitmap.Size.ToRect(), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            try
            {
                unsafe
                {
                    byte* ppixelRow = (byte*)bitmapData.Scan0;

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        byte* ppixelData = (byte*)ppixelRow;


                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // components are stored in BGR order, i.e. red component last
                            ColorRGB color = new ColorRGB();

                            color.Red = ppixelData[2];
                            color.Green = ppixelData[1];
                            color.Blue = ppixelData[0];
                            rgb[y, x] = color;

                            ppixelData += 3;
                        }

                        ppixelRow += bitmapData.Stride;
                    }
                }

                return rgb;
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

 

        private static ColorRGB[,] GetValuesWithLockBits16(Bitmap bitmap)
        {
            ColorRGB[,] rgb = new ColorRGB[bitmap.Height, bitmap.Width];
            BitmapData bitmapData = bitmap.LockBits(bitmap.Size.ToRect(), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            try
            {
                unsafe
                {
                    byte* ppixelRow = (byte*)bitmapData.Scan0;

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        byte* ppixelData = (byte*)ppixelRow;


                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // components are stored in BGR order, i.e. red component last
                            ColorRGB color = new ColorRGB();
                           
                            color.Red = ppixelData[2];
                            color.Green = ppixelData[1];
                            color.Blue = ppixelData[0];
                            rgb[y, x] = color;

                            ppixelData += 1;
                        }

                        ppixelRow += bitmapData.Stride;
                    }
                }

                return rgb;
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
        public static ColorRGB[,] getRGBMatrix(String ubicacion, PixelFormat depth)
        {
            using (Bitmap original = new Bitmap(ubicacion))
            using (Bitmap a = new Bitmap(original.Width, original.Height, depth))
            {
                using (Graphics gr = Graphics.FromImage(a))
                {
                    gr.DrawImage(original, a.Size.ToRect());
                }


                //Stopwatch

                if(depth == PixelFormat.Format16bppRgb565)
                {
                    return GetValuesWithLockBits16(a);
                }
                else
                {
                    return GetValuesWithLockBits48and24(a);
                }

               

            }
        }
        static void version1(String ubicacion, PixelFormat depth, int prof, int n)
        {
            //StreamWriter sw = new StreamWriter(@"C:\Users\juanm\OneDrive\Documentos\ICESI\7mo\Arquitectura de Computadores\Proyecto\time\Tiempo1.txt");

            ColorRGB[,] c = getRGBMatrix(ubicacion, depth);
            Console.WriteLine("rgb " + c.Length);
            long tiempo = 0;
            Stopwatch timeA = new Stopwatch();


          
                timeA.Restart();
                timeA.Start();
                for (int i = 0; i < c.GetLength(0); i++)
                {
                    for (int j = 0; j < c.GetLength(1); j++)
                    {
                       // Console.WriteLine(c[i, j].Red + " " + c[i, j].Green + " " + c[i, j].Blue);
                        c[i, j].Red = (n - c[i, j].Red);
                        c[i, j].Green = (n - c[i, j].Green);
                        c[i, j].Blue = (n - c[i, j].Blue);
                        //Console.WriteLine(c[i, j].Red + " " + c[i, j].Green + " " + c[i, j].Blue);
                    }
                }
                timeA.Stop();
            



            tiempo = (long)(timeA.Elapsed.TotalMilliseconds * 1000000); //*1000000 ns; *1000 us
            Console.WriteLine("Version 1: " + tiempo);

            //a.Save("./inv/version1/" + prof + "/" + size + ".bmp");
        }

        static void version2(String ubicacion, PixelFormat depth, int prof, int n)
        {
            //StreamWriter sw = new StreamWriter(@"C:\Users\juanm\OneDrive\Documentos\ICESI\7mo\Arquitectura de Computadores\Proyecto\time\Tiempo1.txt");

            ColorRGB[,] c = getRGBMatrix(ubicacion, depth);
            Console.WriteLine("rgb " + c.Length);
            long tiempo = 0;
            Stopwatch timeA = new Stopwatch();



            timeA.Restart();
            timeA.Start();
            for (int i = 0; i < c.GetLength(0); i++)
            {
                for (int j = 0; j < c.GetLength(1); j++)
                {
                  
                    c[i, j].Red = (n - c[i, j].Red);
                    
                   
                }
            }
            for (int i = 0; i < c.GetLength(0); i++)
            {
                for (int j = 0; j < c.GetLength(1); j++)
                {

                  
                    c[i, j].Green = (n - c[i, j].Green);
                    

                }
            }



            for (int i = 0; i < c.GetLength(0); i++)
            {
                for (int j = 0; j < c.GetLength(1); j++)
                {

                    c[i, j].Blue = (n - c[i, j].Blue);

                }
            }
            timeA.Stop();




            tiempo = (long)(timeA.Elapsed.TotalMilliseconds * 1000000); //*1000000 ns; *1000 us
            Console.WriteLine("Version 2: " + tiempo);

        }

        static void version3(String ubicacion, PixelFormat depth, int prof, int n)
        {
            //StreamWriter sw = new StreamWriter(@"C:\Users\juanm\OneDrive\Documentos\ICESI\7mo\Arquitectura de Computadores\Proyecto\time\Tiempo1.txt");

            ColorRGB[,] c = getRGBMatrix(ubicacion, depth);
            Console.WriteLine("rgb " + c.Length);
            long tiempo = 0;
            Stopwatch timeA = new Stopwatch();



            timeA.Restart();
            timeA.Start();
            for (int j = 0; j < c.GetLength(0); j++)
            {
                for (int i = 0; i < c.GetLength(1); i++)
                {
                    // Console.WriteLine(c[i, j].Red + " " + c[i, j].Green + " " + c[i, j].Blue);
                    c[i, j].Red = (n - c[i, j].Red);
                    c[i, j].Green = (n - c[i, j].Green);
                    c[i, j].Blue = (n - c[i, j].Blue);
                    //Console.WriteLine(c[i, j].Red + " " + c[i, j].Green + " " + c[i, j].Blue);
                }
            }
            timeA.Stop();




            tiempo = (long)(timeA.Elapsed.TotalMilliseconds * 1000000); //*1000000 ns; *1000 us
            Console.WriteLine("Version 3: " + tiempo);

            //a.Save("./inv/version1/" + prof + "/" + size + ".bmp");
        }

        static void version4(String ubicacion, PixelFormat depth, int prof, int n)
        {
            //StreamWriter sw = new StreamWriter(@"C:\Users\juanm\OneDrive\Documentos\ICESI\7mo\Arquitectura de Computadores\Proyecto\time\Tiempo1.txt");

            ColorRGB[,] c = getRGBMatrix(ubicacion, depth);
            Console.WriteLine("rgb " + c.Length);
            long tiempo = 0;
            Stopwatch timeA = new Stopwatch();



            timeA.Restart();
            timeA.Start();
            for (int i = 0; i < c.GetLength(0); i++)
            {
                for (int j = 0; j < c.GetLength(1); j++)
                {
                   
                    c[i, j].Red = (n - c[i, j].Red);
                   
                }
            }
            for (int i = c.GetLength(0) - 1; i > 0; i--)
            {
                for (int j = c.GetLength(1) - 1; j > 0; j--)
                {
                    
                    c[i, j].Green = (n - c[i, j].Green);
                    c[i, j].Blue = (n - c[i, j].Blue);
                   
                }
            }
            timeA.Stop();




            tiempo = (long)(timeA.Elapsed.TotalMilliseconds * 1000000); //*1000000 ns; *1000 us
            Console.WriteLine("Version 4: " + tiempo);

            //a.Save("./inv/version1/" + prof + "/" + size + ".bmp");
        }

        static void version5(String ubicacion, PixelFormat depth, int prof, int n)
        {
            //StreamWriter sw = new StreamWriter(@"C:\Users\juanm\OneDrive\Documentos\ICESI\7mo\Arquitectura de Computadores\Proyecto\time\Tiempo1.txt");

            ColorRGB[,] c = getRGBMatrix(ubicacion, depth);
            Console.WriteLine("rgb " + c.Length);
            long tiempo = 0;
            Stopwatch timeA = new Stopwatch();



            timeA.Restart();
            timeA.Start();
            for (int i = 0; i < c.GetLength(0)-1; i += 2)
            {
                for (int j = 0; j < c.GetLength(1)-1; j += 2)
                {
                    c[i, j].Red = n -  c[i, j].Red;
                    c[i + 1, j].Red = n - c[i + 1, j].Red;
                    c[i, j + 1].Red = n - c[i, j + 1].Red;
                    c[i + 1, j + 1].Red = n - c[i + 1, j + 1].Red;
                   
                    c[i, j].Green = n - c[i, j].Green;
                    c[i + 1, j].Green = n - c[i + 1, j].Green;
                    c[i, j + 1].Green = n - c[i, j + 1].Green;
                    c[i + 1, j + 1].Green = n - c[i + 1, j + 1].Green;
                  

                    c[i, j].Blue = n - c[i, j].Blue;
                    c[i + 1, j].Blue = n - c[i + 1, j].Blue;
                    c[i, j + 1].Blue = n - c[i, j + 1].Blue;
                    c[i + 1, j + 1].Blue = n - c[i + 1, j + 1].Blue;
                }
            }
            timeA.Stop();




            tiempo = (long)(timeA.Elapsed.TotalMilliseconds * 1000000); //*1000000 ns; *1000 us
            Console.WriteLine("Version 5: " + tiempo);

            //a.Save("./inv/version1/" + prof + "/" + size + ".bmp");
        }







        static void Main(string[] args)
        {
            bool salir = true;
            while (salir)
            {


                Console.WriteLine("Version (1,2,3,4,5)");
                String ver = Console.ReadLine();
                int version = int.Parse(ver);
                Console.WriteLine("size (64,160,512,1500)");
                String si = Console.ReadLine();
                int size = int.Parse(si);
                Console.WriteLine("depth (16,24,48)");
                String dep = Console.ReadLine();
                int depth = int.Parse(dep);

                String imagen6424 = "./Imagenes/tif/64/24.tif";
                String imagen16024 = "./Imagenes/tif/160/24.tif";
                String imagen51224 = "./Imagenes/tif/512/24.tif";
                String imagen150024 = "./Imagenes/tif/1500/24.tif";

                String imagen6448 = "./Imagenes/tif/64/48.tif";
                String imagen16048 = "./Imagenes/tif/160/48.tif";
                String imagen51248 = "./Imagenes/tif/512/48.tif";
                String imagen150048 = "./Imagenes/tif/1500/48.tif";

                String imagen6416 = "./Imagenes/tif/64/16.tif";
                String imagen16016 = "./Imagenes/tif/160/16.tif";
                String imagen51216 = "./Imagenes/tif/512/16.tif";
                String imagen150016 = "./Imagenes/tif/1500/16.tif";

                for (int i = 0; i < 1; i++)
                {
                    try
                    {

                        switch (version)
                        {
                            case 1:
                                if (size == 64)
                                {
                                    if (depth == 16)
                                    {

                                        version1(imagen6416, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version1(imagen6448, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version1(imagen6424, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }
                                if (size == 160)
                                {
                                    if (depth == 16)
                                    {

                                        version1(imagen16016, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version1(imagen16048, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version1(imagen16024, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }
                                if (size == 512)
                                {
                                    if (depth == 16)
                                    {

                                        version1(imagen51216, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version1(imagen51248, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version1(imagen51224, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }
                                if (size == 1500)
                                {
                                    if (depth == 16)
                                    {

                                        version1(imagen150016, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version1(imagen150048, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version1(imagen150024, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }
                                break;
                            case 2:
                                if (size == 64)
                                {
                                    if (depth == 16)
                                    {

                                        version2(imagen6416, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version2(imagen6448, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version2(imagen6424, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }
                                if (size == 160)
                                {
                                    if (depth == 16)
                                    {

                                        version2(imagen16016, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version2(imagen16048, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version2(imagen16024, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }
                                if (size == 512)
                                {
                                    if (depth == 16)
                                    {

                                        version2(imagen51216, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version2(imagen51248, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version2(imagen51224, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }
                                if (size == 1500)
                                {
                                    if (depth == 16)
                                    {

                                        version2(imagen150016, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version2(imagen150048, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version2(imagen150024, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }
                                break;
                            case 3:
                                if (size == 64)
                                {
                                    if (depth == 16)
                                    {

                                        version3(imagen6416, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version3(imagen6448, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version3(imagen6424, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }
                                if (size == 160)
                                {
                                    if (depth == 16)
                                    {

                                        version3(imagen16016, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version3(imagen16048, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version3(imagen16024, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }
                                if (size == 512)
                                {
                                    if (depth == 16)
                                    {

                                        version3(imagen51216, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version3(imagen51248, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version3(imagen51224, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }
                                if (size == 1500)
                                {
                                    if (depth == 16)
                                    {

                                        version3(imagen150016, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version3(imagen150048, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version3(imagen150024, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }
                                break;
                            case 4:
                                if (size == 64)
                                {
                                    if (depth == 16)
                                    {

                                        version4(imagen6416, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version4(imagen6448, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version4(imagen6424, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }
                                if (size == 160)
                                {
                                    if (depth == 16)
                                    {

                                        version4(imagen16016, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version4(imagen16048, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version4(imagen16024, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }
                                if (size == 512)
                                {
                                    if (depth == 16)
                                    {

                                        version4(imagen51216, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version4(imagen51248, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version4(imagen51224, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }
                                if (size == 1500)
                                {
                                    if (depth == 16)
                                    {

                                        version4(imagen150016, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version4(imagen150048, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version4(imagen150024, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }break;
                            case 5:
                                if (size == 64)
                                {
                                    if (depth == 16)
                                    {

                                        version5(imagen6416, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version5(imagen6448, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version5(imagen6424, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }
                                if (size == 160)
                                {
                                    if (depth == 16)
                                    {

                                        version5(imagen16016, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version5(imagen16048, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version5(imagen16024, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }
                                if (size == 512)
                                {
                                    if (depth == 16)
                                    {

                                        version5(imagen51216, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version5(imagen51248, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version5(imagen51224, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }
                                if (size == 1500)
                                {
                                    if (depth == 16)
                                    {

                                        version5(imagen150016, PixelFormat.Format16bppRgb555, depth, 31);
                                    }
                                    if (depth == 48)
                                    {
                                        version5(imagen150048, PixelFormat.Format48bppRgb, depth, 65535);
                                    }
                                    if (depth == 24)
                                    {
                                        version5(imagen150024, PixelFormat.Format24bppRgb, depth, 255);
                                    }

                                }break;
                            default:
                                salir = false;
                                break;
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine(e.StackTrace);
                    }
                }
            }
        }
    }
}
