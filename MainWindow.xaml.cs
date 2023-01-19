using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;

namespace GrafikaZadanie2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int x = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Otworz_tekstowy(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PBM files (*.pbm)|*.pbm|PGM files (*.pgm)|*.pgm|PPM files (*.ppm)|*.ppm";
            if (openFileDialog.ShowDialog() == true)
            {
                using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                {
                   string type = sr.ReadLine();
                    if(type == "P1")
                    {
                        
                        string size = sr.ReadLine();
                        string[] splitedSize = size.Split(' ');
                        int x = int.Parse(splitedSize[0]);
                        int y = int.Parse(splitedSize[1]);
                        Bitmap bitmap = new Bitmap(x, y);
                        for(int i = 0; i < y; i++)
                        {
                            string line = sr.ReadLine();
                            string[] splitedLine = line.Split(' ');
                            for(int j=0; j<x; j++)
                            {
                                if (splitedLine[j] == "1")
                                {
                                    bitmap.SetPixel(j, i, System.Drawing.Color.Black);
                                }
                                else
                                {
                                    bitmap.SetPixel(j, i, System.Drawing.Color.White);
                                }
                            }
                        }
                        sourceBitmapImage.Source = Convert(bitmap);
                    }
                    else if(type == "P2")
                    {
                        string size = sr.ReadLine();
                        string[] splitedSize = size.Split(' ');
                        int x = int.Parse(splitedSize[0]);
                        int y = int.Parse(splitedSize[1]);
                        Bitmap bitmap = new Bitmap(x, y);
                        float color = float.Parse(sr.ReadLine());
                        for (int i = 0; i < y; i++)
                        {
                            string line = sr.ReadLine();
                            string[] splitedLine = line.Split(' ');
                            List<string> splitedLineWithNoSpaces = new List<string>();
                            foreach(string s in splitedLine)
                            {
                                if(s != "")
                                {
                                    splitedLineWithNoSpaces.Add(s);
                                }
                            }
                            for (int j = 0; j < x; j++)
                            {
                                float c = float.Parse(splitedLineWithNoSpaces[j]);
                                float cc = (c / color) * 255;
                                System.Drawing.Color ccc = System.Drawing.Color.FromArgb((int)cc, (int)cc, (int)cc);
                                bitmap.SetPixel(j, i, ccc);
                            }
                        }
                        sourceBitmapImage.Source = Convert(bitmap);
                    }
                    else if(type == "P3")
                    {
                        string size = sr.ReadLine();
                        string[] splitedSize = size.Split(' ');
                        int x = int.Parse(splitedSize[0]);
                        int y = int.Parse(splitedSize[1]);
                        Bitmap bitmap = new Bitmap(x, y);
                        float color = float.Parse(sr.ReadLine());

                        for (int i = 0; i < y; i++)
                        {
                            string line = sr.ReadLine();
                            string[] splitedLine = line.Split(' ');
                            List<string> splitedLineWithNoSpaces = new List<string>();
                            foreach (string s in splitedLine)
                            {
                                if (s != "")
                                {
                                    splitedLineWithNoSpaces.Add(s);
                                }
                            }
                            for (int j = 0; j < 3*x; j+=3)
                            {
                                float[] colors = new float[3];
                                colors[0] = float.Parse(splitedLineWithNoSpaces[j]);
                                colors[1] = float.Parse(splitedLineWithNoSpaces[j+1]);
                                colors[2] = float.Parse(splitedLineWithNoSpaces[j+2]);
                                System.Drawing.Color ccc = System.Drawing.Color.FromArgb(255, (int)(colors[0]), (int)(colors[1]), (int)(colors[2]));
                                bitmap.SetPixel(j/3, i, ccc);
                            }
                        }
                        sourceBitmapImage.Source = Convert(bitmap);
                    }
                }
            }
        }

        private void Otworz_binarny(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PBM files (*.pbm)|*.pbm|PGM files (*.pgm)|*.pgm|PPM files (*.ppm)|*.ppm";
            if (openFileDialog.ShowDialog() == true)
            {
                 using (var stream = File.Open(openFileDialog.FileName, FileMode.Open))
                 {
                    using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                    {
                        char type1 = reader.ReadChar();
                        char type2 = reader.ReadChar();
                        reader.ReadChar();
                        if (type1 == 'P' && type2 == '4')
                        {
                            byte checker = 0;
                            string value = "";
                            List<string> values = new List<string>();
                            while(checker != 10)
                            {
                                checker = reader.ReadByte();
                                if(checker == 32 || checker == 10)
                                {
                                    values.Add(value);
                                    value = "";
                                }
                                else if(checker != 32 && checker != 10)
                                {
                                    value += System.Convert.ToChar(checker);
                                }

                            }
                            int xx = System.Convert.ToInt32(values[0]);
                            int yy = System.Convert.ToInt32(values[1]);
                            Bitmap bitmap = new Bitmap(xx, yy);
                            short[] data = new short[xx * yy];
                            for(int i = 0; i< xx * yy; i++)
                            {
                                byte b = reader.ReadByte();
                                data[i] = b;
                            }
                            int x = 0;
                            int y = 0;
                            for (int i = 0; i < xx * yy; i++)
                            {
                                if (data[i] == 1)
                                {
                                    bitmap.SetPixel(x, y, System.Drawing.Color.Black);
                                }
                                else
                                {
                                    bitmap.SetPixel(x, y, System.Drawing.Color.White);
                                }
                                x += 1;

                                if (x >= bitmap.Width)
                                {
                                    x = 0;
                                    y += 1;
                                }
                            }
                            sourceBitmapImage.Source = Convert(bitmap);
                        }
                        else if(type1 == 'P' && type2 == '5')
                        {
                            byte checker = 0;
                            string value = "";
                            List<string> values = new List<string>();
                            while (checker != 10)
                            {
                                checker = reader.ReadByte();
                                if (checker == 32 || checker == 10)
                                {
                                    values.Add(value);
                                    value = "";
                                }
                                else if (checker != 32 && checker != 10)
                                {
                                    value += System.Convert.ToChar(checker);
                                }

                            }
                            int xx = System.Convert.ToInt32(values[0]);
                            int yy = System.Convert.ToInt32(values[1]);
                            Bitmap bitmap = new Bitmap(xx, yy);
                            checker = 0;
                            while (checker != 10)
                            {
                                checker = reader.ReadByte();
                                if (checker == 32 || checker == 10)
                                {
                                    values.Add(value);
                                    value = "";
                                }
                                else if (checker != 32 && checker != 10)
                                {
                                    value += System.Convert.ToChar(checker);
                                }
                            }
                            double color = System.Convert.ToDouble(values[2]);
                            short[] data = new short[xx * yy];
                            for(int i = 0; i < xx * yy; i++)
                            {
                                byte b = reader.ReadByte();
                                data[i] = b;
                            }
                            int x = 0;
                            int y = 0;
                            for(int i =0; i< xx * yy; i++)
                            {
                                double c = System.Convert.ToDouble(data[i]);
                                double cc = (c / color) * 255;
                                System.Drawing.Color ccc = System.Drawing.Color.FromArgb((int)cc, (int)cc, (int)cc);
                                bitmap.SetPixel(x, y, ccc);
                                x += 1;

                                if (x >= bitmap.Width)
                                {
                                    x = 0;
                                    y += 1;
                                }
                            }
                            sourceBitmapImage.Source = Convert(bitmap);
                        }
                        else if(type1 == 'P' && type2 == '6')
                        {
                            byte checker = 0;
                            string value = "";
                            List<string> values = new List<string>();
                            while (checker != 10)
                            {
                                checker = reader.ReadByte();
                                if (checker == 32 || checker == 10)
                                {
                                    values.Add(value);
                                    value = "";
                                }
                                else if (checker != 32 && checker != 10)
                                {
                                    value += System.Convert.ToChar(checker);
                                }

                            }
                            int xx = System.Convert.ToInt32(values[0]);
                            int yy = System.Convert.ToInt32(values[1]);
                            Bitmap bitmap = new Bitmap(xx, yy);
                            checker = 0;
                            while (checker != 10)
                            {
                                checker = reader.ReadByte();
                                if (checker == 32 || checker == 10)
                                {
                                    values.Add(value);
                                    value = "";
                                }
                                else if (checker != 32 && checker != 10)
                                {
                                    value += System.Convert.ToChar(checker);
                                }
                            }
                            double color = System.Convert.ToDouble(values[2]);
                            short[] data = new short[xx * yy * 3];
                            for (int i = 0; i < xx * yy * 3; i++)
                            {
                                byte b = reader.ReadByte();
                                data[i] = b;
                            }
                            int x = 0;
                            int y = 0;
                            for (int i = 0; i < xx * yy * 3; i+=3)
                            {
                                int[] colors = new int[3];
                                colors[0] = System.Convert.ToInt32(data[i]);
                                colors[1] = System.Convert.ToInt32(data[i + 1]);
                                colors[2] = System.Convert.ToInt32(data[i + 2]);
                                System.Drawing.Color ccc = System.Drawing.Color.FromArgb(255, colors[0], colors[1], colors[2]);
                                bitmap.SetPixel(x, y, ccc);
                                x += 1;

                                if (x >= bitmap.Width)
                                {
                                    x = 0;
                                    y += 1;
                                }
                            }
                            sourceBitmapImage.Source = Convert(bitmap);
                        }
                    }
                 }
            }
        }

        private void Zapisz_tekstowy(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PBM files (*.pbm)|*.pbm|PGM files (*.pgm)|*.pgm|PPM files (*.ppm)|*.ppm";
            if (saveFileDialog.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                {
                    string[] extensionFile = saveFileDialog.FileName.Split('.');
                    if(extensionFile[1] == "pbm")
                    {
                        sw.WriteLine("P1");
                        BitmapImage bitmapImage = (BitmapImage)sourceBitmapImage.Source;
                        Bitmap bitmap = new Bitmap(bitmapImage.StreamSource);
                        sw.WriteLine(bitmap.Width + " " + bitmap.Height);
                        for(int i = 0; i < bitmap.Height; i++)
                        {
                            string line = "";
                            for(int j = 0; j < bitmap.Width; j++)
                            {
                                System.Drawing.Color pixel = bitmap.GetPixel(j, i);
                                line += pixel.R >= 127 ? "0 " : "1 ";
                            }
                            sw.WriteLine(line);
                        }
                        
                    }
                    else if(extensionFile[1] == "pgm")
                    {
                        sw.WriteLine("P2");
                        BitmapImage bitmapImage = (BitmapImage)sourceBitmapImage.Source;
                        Bitmap bitmap = new Bitmap(bitmapImage.StreamSource);
                        sw.WriteLine(bitmap.Width + " " + bitmap.Height);
                        sw.WriteLine("15");
                        for (int i = 0; i < bitmap.Height; i++)
                        {
                            string line = "";
                            for (int j = 0; j < bitmap.Width; j++)
                            {
                                System.Drawing.Color pixel = bitmap.GetPixel(j, i);
                                int color = (int)((pixel.R / 255.0) * 15);
                                if(color > 9 || line == "")
                                {
                                    line += color + " ";
                                }
                                else
                                {
                                    line += " " + color + " ";
                                }
                                
                            }
                            sw.WriteLine(line);
                        }
                    }
                    else if(extensionFile[1] == "ppm")
                    {
                        sw.WriteLine("P3");
                        BitmapImage bitmapImage = (BitmapImage)sourceBitmapImage.Source;
                        Bitmap bitmap = new Bitmap(bitmapImage.StreamSource);
                        sw.WriteLine(bitmap.Width + " " + bitmap.Height);
                        sw.WriteLine("255");
                        for (int i = 0; i < bitmap.Height; i++)
                        {
                            string line = "";
                            for (int j = 0; j < bitmap.Width; j++)
                            {
                                System.Drawing.Color pixel = bitmap.GetPixel(j, i);
                                if(pixel.R >= 0 && pixel.R <= 9)
                                {
                                    if (pixel.G >= 0 && pixel.G <= 9)
                                    {
                                        if (pixel.B >= 0 && pixel.B <= 9)
                                        {
                                            line += "  " + pixel.R.ToString() + "   " + pixel.G.ToString() + "   " + pixel.B.ToString() + "   ";
                                        }
                                        else if (pixel.B >= 10 && pixel.B <= 99)
                                        {
                                            line += "  " + pixel.R.ToString() + "   " + pixel.G.ToString() + "  " + pixel.B.ToString() + "   ";
                                        }
                                        else
                                        {
                                            line += "  " + pixel.R.ToString() + "   " + pixel.G.ToString() + " " + pixel.B.ToString() + "   ";
                                        }
                                    }
                                    else if (pixel.G >= 10 && pixel.G <= 99)
                                    {
                                        if (pixel.B >= 0 && pixel.B <= 9)
                                        {
                                            line += "  " + pixel.R.ToString() + "  " + pixel.G.ToString() + "   " + pixel.B.ToString() + "   ";
                                        }
                                        else if (pixel.B >= 10 && pixel.B <= 99)
                                        {
                                            line += "  " + pixel.R.ToString() + "  " + pixel.G.ToString() + "  " + pixel.B.ToString() + "   ";
                                        }
                                        else
                                        {
                                            line += "  " + pixel.R.ToString() + "  " + pixel.G.ToString() + " " + pixel.B.ToString() + "   ";
                                        }
                                    }
                                    else
                                    {
                                        if (pixel.B >= 0 && pixel.B <= 9)
                                        {
                                            line += "  " + pixel.R.ToString() + " " + pixel.G.ToString() + "   " + pixel.B.ToString() + "   ";
                                        }
                                        else if (pixel.B >= 10 && pixel.B <= 99)
                                        {
                                            line += "  " + pixel.R.ToString() + " " + pixel.G.ToString() + "  " + pixel.B.ToString() + "   ";
                                        }
                                        else
                                        {
                                            line += "  " + pixel.R.ToString() + " " + pixel.G.ToString() + " " + pixel.B.ToString() + "   ";
                                        }
                                    }
                                }
                                else if (pixel.R >= 10 && pixel.R <= 99)
                                {
                                    if (pixel.G >= 0 && pixel.G <= 9)
                                    {
                                        if (pixel.B >= 0 && pixel.B <= 9)
                                        {
                                            line += " " + pixel.R.ToString() + "   " + pixel.G.ToString() + "   " + pixel.B.ToString() + "   ";
                                        }
                                        else if (pixel.B >= 10 && pixel.B <= 99)
                                        {
                                            line += " " + pixel.R.ToString() + "   " + pixel.G.ToString() + "  " + pixel.B.ToString() + "   ";
                                        }
                                        else
                                        {
                                            line += " " + pixel.R.ToString() + "   " + pixel.G.ToString() + " " + pixel.B.ToString() + "   ";
                                        }
                                    }
                                    else if (pixel.G >= 10 && pixel.G <= 99)
                                    {
                                        if (pixel.B >= 0 && pixel.B <= 9)
                                        {
                                            line += " " + pixel.R.ToString() + "  " + pixel.G.ToString() + "   " + pixel.B.ToString() + "   ";
                                        }
                                        else if (pixel.B >= 10 && pixel.B <= 99)
                                        {
                                            line += " " + pixel.R.ToString() + "  " + pixel.G.ToString() + "  " + pixel.B.ToString() + "   ";
                                        }
                                        else
                                        {
                                            line += " " + pixel.R.ToString() + "  " + pixel.G.ToString() + " " + pixel.B.ToString() + "   ";
                                        }
                                    }
                                    else
                                    {
                                        if (pixel.B >= 0 && pixel.B <= 9)
                                        {
                                            line += " " + pixel.R.ToString() + " " + pixel.G.ToString() + "   " + pixel.B.ToString() + "   ";
                                        }
                                        else if (pixel.B >= 10 && pixel.B <= 99)
                                        {
                                            line += " " + pixel.R.ToString() + " " + pixel.G.ToString() + "  " + pixel.B.ToString() + "   ";
                                        }
                                        else
                                        {
                                            line += " " + pixel.R.ToString() + " " + pixel.G.ToString() + " " + pixel.B.ToString() + "   ";
                                        }
                                    }
                                }
                                else
                                {
                                    if (pixel.G >= 0 && pixel.G <= 9)
                                    {
                                        if (pixel.B >= 0 && pixel.B <= 9)
                                        {
                                            line += pixel.R.ToString() + "   " + pixel.G.ToString() + "   " + pixel.B.ToString() + "   ";
                                        }
                                        else if (pixel.B >= 10 && pixel.B <= 99)
                                        {
                                            line += pixel.R.ToString() + "   " + pixel.G.ToString() + "  " + pixel.B.ToString() + "   ";
                                        }
                                        else
                                        {
                                            line += pixel.R.ToString() + "   " + pixel.G.ToString() + " " + pixel.B.ToString() + "   ";
                                        }
                                    }
                                    else if (pixel.G >= 10 && pixel.G <= 99)
                                    {
                                        if (pixel.B >= 0 && pixel.B <= 9)
                                        {
                                            line += pixel.R.ToString() + "  " + pixel.G.ToString() + "   " + pixel.B.ToString() + "   ";
                                        }
                                        else if (pixel.B >= 10 && pixel.B <= 99)
                                        {
                                            line += pixel.R.ToString() + "  " + pixel.G.ToString() + "  " + pixel.B.ToString() + "   ";
                                        }
                                        else
                                        {
                                            line += pixel.R.ToString() + "  " + pixel.G.ToString() + " " + pixel.B.ToString() + "   ";
                                        }
                                    }
                                    else
                                    {
                                        if (pixel.B >= 0 && pixel.B <= 9)
                                        {
                                            line += pixel.R.ToString() + " " + pixel.G.ToString() + "   " + pixel.B.ToString() + "   ";
                                        }
                                        else if (pixel.B >= 10 && pixel.B <= 99)
                                        {
                                            line += pixel.R.ToString() + " " + pixel.G.ToString() + "  " + pixel.B.ToString() + "   ";
                                        }
                                        else
                                        {
                                            line += pixel.R.ToString() + " " + pixel.G.ToString() + " " + pixel.B.ToString() + "   ";
                                        }
                                    }
                                }
                            }
                            sw.WriteLine(line);
                        }
                    }
                }
            }

        }
        private void Zapisz_binarny(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PBM files (*.pbm)|*.pbm|PGM files (*.pgm)|*.pgm|PPM files (*.ppm)|*.ppm";
            if (saveFileDialog.ShowDialog() == true)
            {
                using (var stream = File.Open(saveFileDialog.FileName, FileMode.Create))
                {
                    using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
                    {
                        string[] extensionFile = saveFileDialog.FileName.Split('.');
                        if(extensionFile[1] == "pbm")
                        {
                            writer.Write('P');
                            writer.Write('4');
                            writer.Write((byte)10);
                            BitmapImage bitmapImage = (BitmapImage)sourceBitmapImage.Source;
                            Bitmap bitmap = new Bitmap(bitmapImage.StreamSource);
                            string width = System.Convert.ToString(bitmap.Width);
                            for(int i = 0; i < width.Length; i++)
                            {
                                writer.Write(width[i]);
                            }
                            writer.Write(' ');
                            string height = System.Convert.ToString(bitmap.Height);
                            for (int i = 0; i < height.Length; i++)
                            {
                                writer.Write(height[i]);
                            }
                            writer.Write((byte)10);
                            byte[] data = new byte[bitmap.Height * bitmap.Width];
                            int xx = 0;
                            int yy = 0;
                            for (int i = 0; i < bitmap.Height * bitmap.Width; i++)
                            {
                                int line;
                                    System.Drawing.Color pixel = bitmap.GetPixel(xx, yy);
                                    line = pixel.R >= 127 ? 0 : 1;
                                    xx++;
                                    if(xx >= bitmap.Width)
                                    {
                                        xx = 0;
                                        yy++;
                                    }
                                data[i] = System.Convert.ToByte(line);
                            }
                            writer.Write(data);
                        }
                        else if (extensionFile[1] == "pgm")
                        {
                            writer.Write('P');
                            writer.Write('5');
                            writer.Write((byte)10);
                            BitmapImage bitmapImage = (BitmapImage)sourceBitmapImage.Source;
                            Bitmap bitmap = new Bitmap(bitmapImage.StreamSource);
                            string width = System.Convert.ToString(bitmap.Width);
                            for (int i = 0; i < width.Length; i++)
                            {
                                writer.Write(width[i]);
                            }
                            writer.Write(' ');
                            string height = System.Convert.ToString(bitmap.Height);
                            for (int i = 0; i < height.Length; i++)
                            {
                                writer.Write(height[i]);
                            }
                            writer.Write((byte)10);
                            writer.Write('1');
                            writer.Write('5');
                            writer.Write((byte)10);
                            byte[] data = new byte[bitmap.Height * bitmap.Width];
                            int xx = 0;
                            int yy = 0;
                            for (int i = 0; i < bitmap.Height * bitmap.Width; i++)
                            {
                                    System.Drawing.Color pixel = bitmap.GetPixel(xx, yy);
                                    int color = (int)((pixel.R / 255.0) * 15);
                                    xx++;
                                    if (xx >= bitmap.Width)
                                    {
                                        xx = 0;
                                        yy++;
                                        if (yy >= bitmap.Height)
                                        {
                                            break;
                                        }
                                    }
                                data[i] = System.Convert.ToByte(color);
                            }
                            writer.Write(data);
                        }
                        else if (extensionFile[1] == "ppm")
                        {
                            writer.Write('P');
                            writer.Write('6');
                            writer.Write((byte)10);
                            BitmapImage bitmapImage = (BitmapImage)sourceBitmapImage.Source;
                            Bitmap bitmap = new Bitmap(bitmapImage.StreamSource);
                            string width = System.Convert.ToString(bitmap.Width);
                            for (int i = 0; i < width.Length; i++)
                            {
                                writer.Write(width[i]);
                            }
                            writer.Write(' ');
                            string height = System.Convert.ToString(bitmap.Height);
                            for (int i = 0; i < height.Length; i++)
                            {
                                writer.Write(height[i]);
                            }
                            writer.Write((byte)10);
                            writer.Write('2');
                            writer.Write('5');
                            writer.Write('5');
                            writer.Write((byte)10);
                            byte[] data = new byte[bitmap.Height * bitmap.Width * 3];
                            int xx = 0;
                            int yy = 0;
                            for (int i = 0; i < bitmap.Height * bitmap.Width * 3; i+= 3)
                            {
                                System.Drawing.Color pixel = bitmap.GetPixel(xx, yy);
                                data[i] = System.Convert.ToByte(pixel.R);
                                data[i+1] = System.Convert.ToByte(pixel.G);
                                data[i+2] = System.Convert.ToByte(pixel.B);
                                xx++;
                                if (xx >= bitmap.Width)
                                {
                                    xx = 0;
                                    yy++;
                                    if (yy >= bitmap.Height)
                                    {
                                        break;
                                    }
                                }
                                
                            }
                            writer.Write(data);
                        }
                        
                    }
                }
            }

        }

        public BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            src.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
    }
}
