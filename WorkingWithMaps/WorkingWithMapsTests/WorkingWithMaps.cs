using NUnit.Framework;
using System;
using Xamarin.UITest;
using System.Linq;
using Xamarin.UITest.Queries;
using Xamarin.UITest.Android;
using System.Threading;
using Xamarin.UITest.iOS;

namespace WorkingWithMapsTests
{
	[TestFixture ()]
	public class Test
	{
		IApp app;
		string mapRendererClass, buttonClass, formsTextViewClass;
		bool isAndroid;

		[SetUp]
		public void SetUp ()
		{
			app = ConfigureApp.Android.ApkFile ("/Users/dylankelly/Documents/xamarin-forms-samples/WorkingWithMaps/Android/bin/Release/com.xamarin.workingwithmaps-Signed.apk")
				.ApiKey ("024b0d715a7e9c22388450cf0069cb19").StartApp ();
//			app = ConfigureApp.iOS.AppBundle ("/Users/dylankelly/Documents/xamarin-forms-samples/WorkingWithMaps/iOS/bin/iPhoneSimulator/Release/WorkingWithMapsiOS.app")
//				.ApiKey ("024b0d715a7e9c22388450cf0069cb19")
//				.StartApp ();
			isAndroid = app.GetType () == typeof(AndroidApp);
			mapRendererClass = isAndroid ? "MapRenderer" : "Xamarin_Forms_Platform_iOS_MapRenderer";
			buttonClass = isAndroid ? "Button" : "UIButton";
			formsTextViewClass = isAndroid ? "FormsTextView" : "UILabel";
		}

		[TearDown]
		public void TearDown(){
			app = null;
		}

		[Test ()]
		public void MapZoom(){
			ConfirmNavigationBar ();
			bool hasGooglePlayServices = CheckForGooglePlayServices ();

			app.WaitForElement(c => c.Class(mapRendererClass));
			if (hasGooglePlayServices || !isAndroid) {
				CheckForMapZoomButtons (0);
			} else {
				CheckForMapZoomButtons (1);
			}

			DragSliderFullWidth ();
		}

		[Test ()]
		public void Pins(){
			ConfirmNavigationBar ();
			bool hasGooglePlayServices = CheckForGooglePlayServices ();
			ScrollHorizontalUntilValueFound ("Pins");
			app.Tap(c => c.Text("Pins"));

			app.WaitForElement(c => c.Class(mapRendererClass));
			if (hasGooglePlayServices || !isAndroid) {
				CheckForPinsButtons (0);
			} else {
				CheckForPinsButtons (1);
			}

		}

		[Test ()]
		public void Geocode(){
			ConfirmNavigationBar ();
			bool hasGooglePlayServices = CheckForGooglePlayServices ();

			ScrollHorizontalUntilValueFound ("Geocode");
			app.Tap(c => c.Text("Geocode"));

			AssertGeocodeButtons ();
			if (hasGooglePlayServices || !isAndroid) {
				app.Tap (c => c.Button("Geocode '394 Pacific Ave'"));
				app.Tap (c => c.Button("Reverse geocode '37.808, -122.432'"));
				Thread.Sleep (1000);
				AssertAppResultTextEqualsString (
					app.Query(c => c.Class(formsTextViewClass)).ElementAt(0),
					"37.7976785, -122.4018163\n11 Marina Blvd\nSan Francisco, " +
					"CA 94123\nUSA\nMarina District\nSan Francisco, CA\nUSA\nSan " +
					"Francisco, CA\nUSA\nSan Francisco, CA 94123\nUSA\nSan Francisco " +
					"County\nCA\nUSA\n");
			}
		}

		[Test ()]
		public void MapApp(){
			ConfirmNavigationBar ();

			ScrollHorizontalUntilValueFound ("Map App");
			app.Tap(c => c.Text("Map App"));

			AssertAppResultTextEqualsString (
				app.WaitForElement (c => c.Class (formsTextViewClass)).ElementAt (0), 
				"These buttons leave the current app and open the built-in Maps app for the platform");

			AssertAppResultTextEqualsString (
				app.WaitForElement (c => c.Class (buttonClass)).ElementAt (0), 
				"Open location using built-in Maps app");

			AssertAppResultTextEqualsString (
				app.WaitForElement (c => c.Class (buttonClass)).ElementAt (1), 
				"Get directions using built-in Maps app");
		}

		bool CheckForGooglePlayServices(){
			return !(app.WaitForElement (c => c.Class (buttonClass))
				.ElementAt(0).Text.Equals("Get Google Play services"));
		}

		void CheckForMapZoomButtons(int k){
			AppResult[] results = app.WaitForElement (c => c.Class (buttonClass));
			AssertAppResultTextEqualsString(results.ElementAt(k),"Street");
			AssertAppResultTextEqualsString(results.ElementAt(k+1),"Hybrid");
			AssertAppResultTextEqualsString(results.ElementAt(k+2),"Satellite");
		}

		void CheckForPinsButtons(int k){
			AppResult[] results = app.WaitForElement (c => c.Class (buttonClass));
			AssertAppResultTextEqualsString(results.ElementAt(k),"Add more pins");
			AssertAppResultTextEqualsString(results.ElementAt(k+1),"Re-center");
		}

		void AssertAppResultTextEqualsString(AppResult r, string s){
			Assert.IsTrue(r.Text.Equals(s), string.Format("AppResult.Text({0}) != {1}", r.Text, s));
		}

		void ConfirmNavigationBar ()
		{
			Assert.IsTrue (app.WaitForElement (c => c.Text ("Map/Zoom")).Any (), "Could not find 'Map/Zoom' navigation element.");
			Assert.IsTrue (app.WaitForElement (c => c.Text ("Pins")).Any (), "Could not find 'Pins' navigation element.");
		}

		void ScrollHorizontalUntilValueFound (string strMenuItem)
		{
			float y = app.WaitForElement (c => c.Text ("Map/Zoom")).ElementAt (0).Rect.CenterY;
			float x = app.Query (c => c.Text ("Map/Zoom")).ElementAt (0).Rect.X;
			while (!app.Query (c => c.Text (strMenuItem)).Any ()) {
				app.DragCoordinates (x + 100f, y, x, y);
			}
		}

		void AssertGeocodeButtons(){
			AppResult[] results = app.WaitForElement (c => c.Class (buttonClass));
			AssertAppResultTextEqualsString(results.ElementAt(0),"Geocode '394 Pacific Ave'");
			AssertAppResultTextEqualsString(results.ElementAt(1),"Reverse geocode '37.808, -122.432'");
		}

		void DragSliderFullWidth()
		{
			if (!isAndroid) {

				var sliderGrip = app.Query (c => c.ClassFull ("UIImageView")).
					Where (image => image.Rect.Height == 31.0f && image.Rect.Height == 31.0f).
					Select (image => image).First ();

				((iOSApp)app).FlickCoordinates (sliderGrip.Rect.CenterX, sliderGrip.Rect.CenterY, 
					sliderGrip.Rect.CenterX + 100, sliderGrip.Rect.CenterY);

			} else {
				var seekBarRect = app.Query (c => c.Class ("SeekBar")) [0].Rect;
				((AndroidApp)app).DragCoordinates (seekBarRect.X, seekBarRect.CenterY, 
					(seekBarRect.X + seekBarRect.Width), seekBarRect.CenterY);
			}
		}
	}
}

