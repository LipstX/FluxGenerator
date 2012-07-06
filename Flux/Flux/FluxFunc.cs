using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Flux
{
    public partial class MainWindow
    {
        private void MyimgMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_first)
            {
                _firstPos = e.GetPosition(this);
                _first = false;
            }
            else
            {
                var secondPos = e.GetPosition(this);
                var points = new PointCollection();
                var point1 = new Point(_firstPos.X - 15, _firstPos.Y -25);
                var x2 = (Convert.ToInt32(fluxSize.Text) / 10);
                var point2 = new Point(secondPos.X - x2, secondPos.Y -25);
                points.Add(point1);
                points.Add(point2);
                var myLine = new Polyline
                {
                    Name = "Flux" + _nbFLux.ToString(CultureInfo.InvariantCulture),
                    IsManipulationEnabled = true,
                    Stroke = Brushes.Red,
                    StrokeThickness = Convert.ToInt32(fluxSize.Text) / 10,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Triangle,
                    Points = points
                };
                mainCanva.Children.Add(myLine);
                _first = true;
                _listFlux.Add(myLine);
                comboBox2.Items.Add(myLine.Name);
                _nbFLux++;
            }
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
