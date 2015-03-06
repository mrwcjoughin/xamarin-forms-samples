using System;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using MobileCRM;
using MobileCRM.Android;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer (typeof (MiXMap), typeof (MapRendererAndriod))]
namespace MobileCRM.Android
{
	public class MapRendererAndriod : MapRenderer
    {
		MapView mapView;
		GoogleMap map;

		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Xamarin.Forms.View> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement == null) {

				mapView = Control as MapView;
				map = mapView.Map;

				if (map != null) {
					Xamarin.Forms.Maps.Map myMap = e.NewElement as Xamarin.Forms.Maps.Map;

					PolylineOptions line = new PolylineOptions ();
					line.InvokeWidth (4);
					line.InvokeColor (global::Android.Graphics.Color.Red);
					// Add the points of the polyline
					LatLng latLng = new LatLng (37.7970564, -122.4034628);
					line.Add (latLng);
					latLng = new LatLng (37.7970564, -122.6034628);
					line.Add (latLng);
					latLng = new LatLng (37.7970564, -122.8034628);
					line.Add (latLng);
					// Add the polyline to the map
					map.AddPolyline (line);
				}
			}
		}
    }
}

