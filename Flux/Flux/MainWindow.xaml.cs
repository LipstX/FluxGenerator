using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Windows.Shapes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Point = System.Windows.Point;

namespace Flux
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            _clickImagePath = "/Images/Start.gif";
            _listImg = new List<Image>();
            _listFlux = new List<Polyline>();
            _nbStart = 1;
            _nbCp = 1;
            _nbEnd = 1;
            _nbFLux = 1;
            _first = true;
            _isset = false;
        }

        private readonly List<Image> _listImg;
        private readonly List<Polyline> _listFlux;
        private string _clickImagePath;
        private string _pointName;
        private int _nbFLux;
        private int _nbStart;
        private int _nbCp;
        private int _nbEnd;
        private bool _first;
        private Point _firstPos;

        private void LoadPicture(object sender, RoutedEventArgs e)
        {
            try
            {
                var files = new OpenFileDialog
                                {Filter = "Images|*.png;*.bmp;*.jpg", Title = "Choisissez une image à charger"};
                if (files.ShowDialog() == true)
                {
                    var bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri(files.FileName, UriKind.Absolute);
                    bi3.EndInit();
                    schema.Source = bi3;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Syntax problem in your XML file here : \n" + exc);
            }
        }

        private void ClickMouse(object sender, MouseButtonEventArgs e)
        {
            CreatePoint(e);
        }

        private void ChangePictureSize(object sender, SizeChangedEventArgs e)
        {
            mainCanva.Width = mainGrid.ActualWidth - 175;
            mainCanva.Height = mainGrid.ActualHeight;
            schema.Width = mainGrid.ActualWidth - 175;
            schema.Height = mainGrid.ActualHeight;
        }

        private void MakePdf(object sender, RoutedEventArgs e)
        {
            var saveFileDialog1 = new SaveFileDialog
                                      {
                                          Filter = Properties.Resources.MainWindow_Testit_PDF_Files___pdf,
                                          Title = Properties.Resources.MainWindow_Testit_Save_a_PDF_File
                                      };
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                var fs = (FileStream) saveFileDialog1.OpenFile();
                var nouveauDocument = new Document();
                fs.Close();
                string pic = saveFileDialog1.FileName.Substring(0, saveFileDialog1.FileName.IndexOf('.'));
                SavePicture(pic);
                try
                {
                    PdfWriter.GetInstance(nouveauDocument, new FileStream(saveFileDialog1.FileName, FileMode.Create));
                    nouveauDocument.Open();
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(pic + ".png");
                    nouveauDocument.Add(img);
                    iTextSharp.text.Image img2 = iTextSharp.text.Image.GetInstance("logo.bmp");
                    nouveauDocument.Add(img2);
                    File.Delete(pic + ".png");
                }
                catch (DocumentException de)
                {
                    Console.WriteLine(Properties.Resources.MainWindow_testit_error_ + de.Message);
                }
                catch (IOException ioe)
                {
                    Console.WriteLine(Properties.Resources.MainWindow_testit_error_ + ioe.Message);
                }
                nouveauDocument.Close();
            }
        }

        private void SavePicture(string fileName)
        {
            double width = Convert.ToDouble(mainCanva.GetValue(WidthProperty));
            double height = Convert.ToDouble(mainCanva.GetValue(HeightProperty));
            if (double.IsNaN(width) || double.IsNaN(height))
            {
                throw new FormatException("You need to indicate the Width and Height values of the UIElement.");
            }
            var render = new RenderTargetBitmap(Convert.ToInt32(width),
                                                Convert.ToInt32(mainCanva.GetValue(HeightProperty)), 96, 96,
                                                PixelFormats.Pbgra32);
            // Indicate which control to render in the image
            render.Render(mainCanva);
            using (var stream = new FileStream(fileName + ".png", FileMode.Create))
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(render));
                encoder.Save(stream);
            }
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string selectitem = null;
            if (comboBox2.SelectedItem != null)
                selectitem = comboBox2.SelectedItem.ToString();
            foreach (var it in _listFlux)
            {
                if (it.Name == selectitem)
                {
                    fluxSize.Text = (it.StrokeThickness*10).ToString(CultureInfo.InvariantCulture);
                    it.Stroke = Brushes.Aqua;
                }
                else
                    it.Stroke = Brushes.Gold;
            }
        }
    }
}

