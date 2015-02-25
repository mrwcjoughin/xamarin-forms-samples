using NUnit.Framework;
using System;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using System.Linq;
using System.Collections.Generic;

namespace WorkingWithGesturesTest
{
	[TestFixture ()]
	public class Test
	{

		IApp app;
		string imageRendererClass;
		string frameRendererClass;

		[SetUp]
		public void SetUp ()
		{
			app = ConfigureApp.Android.ApkFile ("/Users/dylankelly/Documents/xamarin-forms-samples/WorkingWithGestures/Android/bin/Release/WorkingWithGestures.Android-Signed.apk")
							.ApiKey ("024b0d715a7e9c22388450cf0069cb19").StartApp ();
//			app = ConfigureApp.iOS.AppBundle ("/Users/dylankelly/Documents/xamarin-forms-samples/WorkingWithGestures/iOS/bin/iPhoneSimulator/Release/WorkingWithGesturesiOS.app")
//				.ApiKey ("024b0d715a7e9c22388450cf0069cb19")
//				.StartApp ();

			imageRendererClass = app.GetType () == typeof(AndroidApp) ? "ImageRenderer" : "Xamarin_Forms_Platform_iOS_ImageRenderer";
			frameRendererClass = app.GetType () == typeof(AndroidApp) ? "FrameRenderer" : "Xamarin_Forms_Platform_iOS_FrameRenderer";
		}

		[Test ()]
		public void TapImage ()
		{
			ConfirmNavigationBar ();
			Assert.IsTrue (app.Query (c => c.Text ("tap the photo!")).Any (), "Could not find 'tap the photo!'");

			TapClassAndAssertTapCount (imageRendererClass);
		}

		[Test ()]
		public void TapFrame ()
		{
			ConfirmNavigationBar ();
			app.Tap (c => c.Text ("Frame"));
			Assert.IsTrue (app.WaitForElement (c => c.Text ("Tap Inside Frame")).Any (), "Could not find 'Tap Inside Frame'");

			TapClassAndAssertTapCount (frameRendererClass);
		}

		[Test ()]
		public void TapInXaml_Image ()
		{
			NavigateToInXaml ();
			TapClassAndAssertTapCount (imageRendererClass);
		}

		[Test ()]
		public void TapInXaml_Frame ()
		{
			NavigateToInXaml ();
			TapClassAndAssertTapCount (frameRendererClass);
		}

		void NavigateToInXaml ()
		{
			ConfirmNavigationBar ();
			app.Tap (c => c.Text ("In Xaml"));
			Assert.IsTrue (app.WaitForElement (c => c.Text ("Tap inside frame (or on the monkey!)")).Any ()
				, "Tap inside frame (or on the monkey!)");
		}

		void ConfirmNavigationBar ()
		{
			Assert.IsTrue (app.WaitForElement (c => c.Text ("Image")).Any (), "Could not find 'Image' navigation element.");
			Assert.IsTrue (app.WaitForElement (c => c.Text ("Frame")).Any (), "Could not find 'Frame' navigation element.");
			Assert.IsTrue (app.WaitForElement (c => c.Text ("In Xaml")).Any (), "Could not find 'In Xaml' navigation element.");
		}

		void TapClassAndAssertTapCount (string classString)
		{
			Assert.IsTrue (app.WaitForElement (c => c.Class (classString)).Any (), "Could not find class " + classString + ".");

			app.Tap (c => c.Class (classString));
			Assert.IsTrue (app.WaitForElement (c => c.Text ("1 tap so far!")).Any (), "Could not find '1 tap so far!'");

			app.Tap (c => c.Class (classString));
			Assert.IsTrue (app.WaitForElement (c => c.Text ("2 taps so far!")).Any (), "Could not find '2 taps so far!'");
		}
	}

}

