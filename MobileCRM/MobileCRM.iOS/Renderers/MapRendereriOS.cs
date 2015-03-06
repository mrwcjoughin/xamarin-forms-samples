using System;
using CoreLocation;
using MapKit;
using MobileCRM;
using MobileCRM.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;

[assembly: ExportRenderer (typeof (MiXMap), typeof (MapRendereriOS))]
namespace MobileCRM.iOS
{
	public class MapRendereriOS : MapRenderer
    {
		MKMapView mapView;
		MKPolyline lineOverlay;
		MKPolylineRenderer lineRenderer;

		protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement == null) {

				mapView = Control as MKMapView;

				Map myMap = e.NewElement as Map;

				mapView.OverlayRenderer = (m, o) => {
					if(lineRenderer == null) {
						lineRenderer = new MKPolylineRenderer(o as MKPolyline);
						lineRenderer.StrokeColor = UIColor.Red;
						lineRenderer.FillColor = UIColor.Red;
					}
					return lineRenderer;
				};
				
				var point1 = new CLLocationCoordinate2D(37.7970564,-122.4034628);
				var point2 = new CLLocationCoordinate2D(37.7970564,-122.6034628);
				var point3 = new CLLocationCoordinate2D(37.7970564,-122.8034628);
				
				lineOverlay = MKPolyline.FromCoordinates(new CLLocationCoordinate2D[] {point1, point2, point3});
				mapView.AddOverlay (lineOverlay);
			}
		}
    }
}

