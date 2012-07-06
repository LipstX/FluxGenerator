using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Flux
{
    public partial class MainWindow
    {
        private  PointCollection _points;

        private void MyimgMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_first && ((Image)sender).Name.Contains("Start"))
            {
                _points = new PointCollection();
                _firstPos = e.GetPosition(this);
                var point1 = new Point(_firstPos.X - 5, _firstPos.Y -25);
                _points.Add(point1);
                _first = false;
            }
            else
            {
                if (((Image)sender).Name.Contains("Check"))
                {
                    var nextPos = e.GetPosition(this);
                    var x2 = (Convert.ToInt32(fluxSize.Text)/10);
                    var nextp = new Point(nextPos.X - x2 -5, nextPos.Y - 25);
                    _points.Add(nextp);
                }
                else if (((Image)sender).Name.Contains("End"))
                {
                    var lastPos = e.GetPosition(this);
                    var x2 = (Convert.ToInt32(fluxSize.Text) / 10);
                    var lastp = new Point(lastPos.X - x2-15, lastPos.Y - 25);
                    _points.Add(lastp);
                    var myLine = new Polyline
                                     {
                                         Name = "Flux" + _nbFLux.ToString(CultureInfo.InvariantCulture),
                                         IsManipulationEnabled = true,
                                         Stroke = Brushes.Gold,
                                         StrokeThickness = Convert.ToInt32(fluxSize.Text)/10,
                                         StrokeStartLineCap = PenLineCap.Flat,
                                         StrokeEndLineCap = PenLineCap.Triangle,
                                         Points = _points,
                                     };
                    myLine.MouseEnter += DoToolTip;
                    mainCanva.Children.Add(myLine);
                    _first = true;
                    _listFlux.Add(myLine);
                    comboBox2.Items.Add(myLine.Name);
                    _nbFLux++;
                }
            }
        }

        private void DoToolTip(object sender, MouseEventArgs e)
        {
            ToolTip = ((Polyline)sender).Name + " = " + (((Polyline)sender).StrokeThickness * 10).ToString(CultureInfo.InvariantCulture);
        }

        private void DeleteFlux(object sender, KeyEventArgs e)
        {
            string selectitem = null;
            if (comboBox2.SelectedItem != null)
                selectitem = comboBox2.SelectedItem.ToString();
            if (e.Key == Key.Delete)
            {
                for (int index = 0; index < _listFlux.Count; index++)
                {
                    var it = _listFlux[index];
                    if (it.Name == selectitem)
                    {
                        mainCanva.Children.Remove(it);
                        _listFlux.Remove(it);
                        if (selectitem != null) comboBox2.Items.Remove(selectitem);
                    }
                }
            }
        }
    }
}
