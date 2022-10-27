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
                            for (int j = 0; j < x; j+=3)
                            {
                                float[] colors = new float[3];
                                colors[0] = float.Parse(splitedLineWithNoSpaces[j]);
                                colors[1] = float.Parse(splitedLineWithNoSpaces[j+1]);
                                colors[2] = float.Parse(splitedLineWithNoSpaces[j+2]);
                                System.Drawing.Color ccc = System.Drawing.Color.FromArgb((int)(colors[0]/color), (int)(colors[1] / color), (int)(colors[2] / color));
                                bitmap.SetPixel(j, i, ccc);
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
