using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.WP8;
using MobileCRM;
using System.Device.Location;
using Xamarin.Forms.Platform.WinPhone;
using MobileCRM.WindowsPhone;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Media;

[assembly: ExportRenderer(typeof(MiXMap), typeof(WindowsPhoneRenderer))]
namespace MobileCRM.WindowsPhone
{
    public class WindowsPhoneRenderer : MapRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Maps.Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                MapPolyline line = new MapPolyline();
                line.StrokeColor = Colors.Red;
                line.StrokeThickness = 15;
                
                //List<GeoCoordinate> listCoordinates = new List<GeoCoordinate>();
                line.Path.Add(new GeoCoordinate(37.7970564, -122.4034628));
                line.Path.Add(new GeoCoordinate(37.7970564, -122.6034628));
                line.Path.Add(new GeoCoordinate(37.7970564, -122.8034628));

                Microsoft.Phone.Maps.Controls.Map map = Control as Microsoft.Phone.Maps.Controls.Map;

                map.MapElements.Add(line);


                /*
                if (listCoordinates == null)
                {
                    // first time through:
                    listCoordinates = new List<GeoCoordinate>();
                    listCoordinates.Add(e.Position.Location);
                    lastCoordinate = e.Position.Location;
                    return;
                }
                else
                {
                    listCoordinates.Add(e.Position.Location);
                    DrawRoute(e.Position.Location);
                    lastCoordinate = e.Position.Location;
                }
                
                mapView = Control as MKMapView;

                Map myMap = e.NewElement as Map;

                mapView.OverlayRenderer = (m, o) =>
                {
                    if (lineRenderer == null)
                    {
                        lineRenderer = new MKPolylineRenderer(o as MKPolyline);
                        lineRenderer.StrokeColor = UIColor.Red;
                        lineRenderer.FillColor = UIColor.Red;
                    }
                    return lineRenderer;
                };

                var point1 = new CLLocationCoordinate2D(37.7970564, -122.4034628);
                var point2 = new CLLocationCoordinate2D(37.7970564, -122.6034628);
                var point3 = new CLLocationCoordinate2D(37.7970564, -122.8034628);

                lineOverlay = MKPolyline.FromCoordinates(new CLLocationCoordinate2D[] { point1, point2, point3 });
                mapView.AddOverlay(lineOverlay);
                */
            }
        }
    }
}
