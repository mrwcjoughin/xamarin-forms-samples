using NUnit.Framework;
using System;
using Xamarin.UITest;
using System.Linq;
using Xamarin.UITest.Queries;
using Xamarin.UITest.Android;
using System.Threading;

namespace WorkingWithWebviewTests
{
	[TestFixture ()]
	public class Test
	{
		IApp app;
		string buttonClass;
		string formsTextViewClass;
		bool isAndroid;

		[SetUp]
		public void SetUp ()
		{
//			app = ConfigureApp.Android.ApkFile ("/Users/dylankelly/Documents/xamarin-forms-samples/WorkingWithWebview/Android/bin/Release/WorkingWithWebview.Android-Signed.apk")
//				.ApiKey ("024b0d715a7e9c22388450cf0069cb19").StartApp ();
			app = ConfigureApp.iOS.AppBundle ("/Users/dylankelly/Documents/xamarin-forms-samples/WorkingWithWebview/iOS/bin/iPhoneSimulator/Release/WorkingWithWebviewiOS.app")
				.ApiKey ("024b0d715a7e9c22388450cf0069cb19")
				.StartApp ();
			isAndroid = app.GetType () == typeof(AndroidApp);
			buttonClass = isAndroid ? "Button" : "UIButtonLabel";
			formsTextViewClass = isAndroid ? "FormsTextView" : "UILabel";
		}

		[TearDown]
		public void TearDown(){
			app = null;
		}

		[Test ()]
		public void LocalWebView ()
		{
			ConfirmNavigationBar ();

			AssertBodyTextContentContainsString ("\nXamarin.Forms\nWelcome to WebView.\n\n");

		}

		[Test ()]
		public void BaseUrlWebView ()
		{
			ConfirmNavigationBar ();

			if(isAndroid)ScrollHorizontalUntilValueFound ("BaseUrl");
			app.Tap (c => c.Text ("BaseUrl"));
			AssertBodyTextContentContainsString ("\nXamarin.Forms\nThe CSS and image are loaded " +
			"from local files!\n\nnext page\n\n");

			AppWebResult[] result = app.WaitForElement (c => c.Css ("A"));
			foreach (AppWebResult r in result) {
				if (r.TextContent.Equals ("next page")) {
					app.TapCoordinates (r.Rect.CenterX, r.Rect.CenterY);
					break;
				}
			}

			Thread.Sleep (500);

			if (isAndroid)
				Assert.IsTrue (app.WaitForElement (c => c.Css ("P")) [0].TextContent.Equals ("This is a local Android Html page"));
			else
				Assert.IsTrue (app.WaitForElement (c => c.Css ("P")) [0].TextContent.Equals ("This is a local iOS Html page"));
		}

		[Test ()]
		public void WebPageWebView ()
		{
			ConfirmNavigationBar ();

			if(isAndroid)ScrollHorizontalUntilValueFound ("Web Page");
			app.Tap (c => c.Text ("Web Page"));

			Assert.IsTrue (app.WaitForElement (c => c.Css ("#top-container")).Any (), 
				"Could not find css id: 'top-container'.");
				
		}

		[Test ()]
		public void ExternalWebView ()
		{
			ConfirmNavigationBar ();

			if(isAndroid)ScrollHorizontalUntilValueFound ("External");
			app.Tap (c => c.Text ("External"));

			Assert.IsTrue (app.WaitForElement (c => c.ClassFull (formsTextViewClass)).ElementAt (0).Text.Equals (
				"These buttons leave the current app and open the built-in web browser app for the platform"));

			Assert.IsTrue (app.WaitForElement (c => c.ClassFull (buttonClass)).ElementAt (0).Text.Equals (
				"Open location using built-in Web Browser app"));

			Assert.IsTrue (app.WaitForElement (c => c.ClassFull (buttonClass)).ElementAt (1).Text.Equals (
				"Make call using built-in Phone app"));

		}

		void AssertBodyTextContentContainsString (string text)
		{
			Assert.AreEqual (app.WaitForElement (c => c.Css ("BODY")) [0].TextContent, text);
		}

		void ConfirmNavigationBar ()
		{
			Assert.IsTrue (app.WaitForElement (c => c.Text ("Local")).Any (), "Could not find 'Local' navigation element.");
			Assert.IsTrue (app.WaitForElement (c => c.Text ("BaseUrl")).Any (), "Could not find 'BaseUrl' navigation element.");
		}

		private void ScrollHorizontalUntilValueFound (string strMenuItem)
		{
			float y = app.WaitForElement (c => c.Text ("Local")).ElementAt (0).Rect.CenterY;
			float x = app.Query (c => c.Text ("Local")).ElementAt (0).Rect.X;
			while (!app.Query (c => c.Text (strMenuItem)).Any ()) {
				app.DragCoordinates (x + 100f, y, x, y);
			}
		}
	}
}

