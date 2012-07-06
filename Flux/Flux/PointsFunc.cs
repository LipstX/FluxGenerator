using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Flux
{
    public partial class MainWindow
    {
        private void CreatePoint(MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this);
            schema.PointFromScreen(p);
            var myImage3 = new Image();
            var bi3 = new BitmapImage();

            if (_pointName.StartsWith("Start"))
            {
                _pointName = "Start" + _nbStart.ToString(CultureInfo.InvariantCulture);
                _nbStart++;
            }
            else if (_pointName.StartsWith("Check"))
            {
                _pointName = "Checkpoint" + _nbCp.ToString(CultureInfo.InvariantCulture);
                _nbCp++;
            }
            else
            {
                _pointName = "End" + _nbEnd.ToString(CultureInfo.InvariantCulture);
                _nbEnd++;
            }

            bi3.BeginInit();
            bi3.UriSource = new Uri(_clickImagePath, UriKind.Relative);
            bi3.EndInit();

            myImage3.Stretch = Stretch.Fill;
            myImage3.IsManipulationEnabled = true;
            myImage3.Source = bi3;
            myImage3.MinWidth = 30;
            myImage3.MinHeight = 30;
            myImage3.Width = 30;
            myImage3.Height = 30;
            myImage3.Name = _pointName;
            myImage3.MouseLeave += MyimgMouseLeave;
            myImage3.MouseDown += MyimgMouseDown;
            myImage3.MouseEnter += MyimgMouseEnter;

            var oMargin = new Thickness(p.X - 30, p.Y - 40, 0, 0);
            myImage3.Margin = oMargin;
            mainCanva.Children.Add(myImage3);
            _listImg.Add(myImage3);
            PointList.Items.Add(_pointName);
        }

        private void MyimgMouseLeave(object sender, MouseEventArgs e)
        {
            var bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(_oldImg, UriKind.Absolute);
            bi3.EndInit();
            ((Image)sender).Source = bi3;
        }

        private string _oldImg;

        void MyimgMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            var bi3 = new BitmapImage();
            bi3.BeginInit();
            _oldImg = ((Image)sender).Source.ToString();
            bi3.UriSource = new Uri("/Images/Selected.png", UriKind.Relative);
            bi3.EndInit();
            ((Image)sender).Source = bi3;
        }

        private void ChangeImage(object sender, SelectionChangedEventArgs e)
        {
            if (Start.IsSelected)
            {
                _pointName = "Start" + _nbStart.ToString(CultureInfo.InvariantCulture);
                _clickImagePath = "/Images/Start.gif";
            }
            else if (Checkpoint.IsSelected)
            {
                _pointName = "Checkpoint" + _nbCp.ToString(CultureInfo.InvariantCulture);
                _clickImagePath = "/Images/checkpoint.png";
            }
            else
            {
                _pointName = "End" + _nbEnd.ToString(CultureInfo.InvariantCulture);
                _clickImagePath = "/Images/End.gif";
            }
        }

        private string _oldpath;
        private bool _isset;

        private void PointListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isset)
            {
                foreach (var it in _listImg)
                {
                    if (it.Name == _oldImg)
                    {
                        var bi3 = new BitmapImage();
                        bi3.BeginInit();
                        bi3.UriSource = new Uri(_oldpath, UriKind.Absolute);
                        bi3.EndInit();
                        it.Source = bi3;
                    }
                }
            }
            string selectedItem = null;
            if (PointList.SelectedItem != null)
                selectedItem = PointList.SelectedItem.ToString();
            foreach (var it in _listImg)
            {
                if (it.Name == selectedItem)
                {
                    var bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri("/Images/Selected.png", UriKind.Relative);
                    bi3.EndInit();
                    _oldpath = it.Source.ToString();
                    _oldImg = selectedItem;
                    it.Source = bi3;
                }
            }
            _isset = true;
        }

        private void DeletePoint(object sender, KeyEventArgs e)
        {
            string selectitem = null;
            if (PointList.SelectedItem != null)
                selectitem = PointList.SelectedItem.ToString();
            if (e.Key == Key.Delete)
            {
                for (int index = 0; index < _listImg.Count; index++)
                {
                    var it = _listImg[index];
                    if (it.Name == selectitem)
                    {
                        mainCanva.Children.Remove(it);
                        _listImg.Remove(it);
                        if (selectitem != null) PointList.Items.Remove(selectitem);
                    }
                }
            }
        }
    }
}
