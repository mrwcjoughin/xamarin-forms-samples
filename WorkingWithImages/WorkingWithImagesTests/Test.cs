using NUnit.Framework;
using System;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.Queries;
using System.Linq;
using Xamarin.UITest.iOS;

namespace WorkingWithImagesTests
{
	[TestFixture ()]
	public class Test
	{
		IApp app;
		string formsTextViewClass, formsImageViewClass;
		bool isAndroid;

		[SetUp]
		public void SetUp ()
		{
			app = ConfigureApp.Android.ApkFile ("/Users/dylankelly/Documents/xamarin-forms-samples/WorkingWithImages/WorkingWithImages.Android/bin/Release/com.YourCompany.WorkingWithImages-Signed.apk")
				.ApiKey ("024b0d715a7e9c22388450cf0069cb19").StartApp ();
//			app = ConfigureApp.iOS.AppBundle ("/Users/dylankelly/Documents/xamarin-forms-samples/WorkingWithImages/WorkingWithImages.iOS/bin/iPhoneSimulator/Release/FormsTemplateiOS.app")
//				.ApiKey ("024b0d715a7e9c22388450cf0069cb19")
//				.StartApp ();
			isAndroid = app.GetType () == typeof(AndroidApp);
			formsTextViewClass = isAndroid ? "FormsTextView" : "UILabel";
			formsImageViewClass = isAndroid ? "FormsImageView" : "UIImageView";
		}

		[TearDown]
		public void TearDown ()
		{
			app = null;
		}

		[Test ()]
		public void LocalImage ()
		{
			ConfirmNavigationBar ();

			ConfirmTwoTextViewOneImageView ("Image FileSource Xaml", 
				"'waterfront.jpg' is referenced in Xaml. " +
				"On iOS and Android multiple resolutions are supplied and resolved at runtime.");
		}

		[Test ()]
		public void DownloadedImage ()
		{
			ConfirmNavigationBar ();
			ScrollHorizontalUntilValueFound ("Downloaded");
			app.Tap (c => c.Text ("Downloaded"));

			ConfirmTwoTextViewOneImageView ("Image UriSource Xaml", 
				"example-app.png gets downloaded from xamarin.com");
		}

		[Test ()]
		public void EmbeddedImage ()
		{
			ConfirmNavigationBar ();
			ScrollHorizontalUntilValueFound ("Embedded");
			app.Tap (c => c.Text ("Embedded"));

			ConfirmTwoTextViewOneImageView ("Image Resource Xaml", 
				"WorkingWithImages.beach.jpg embedded resource");
		}

		void ConfirmTwoTextViewOneImageView (string textOne, string textTwo)
		{
			AppResult[] results = app.WaitForElement (c => c.Class (formsTextViewClass));
			AssertAppResultTextEqualsString (results.ElementAt (0), textOne);
			AssertAppResultTextEqualsString (results.ElementAt (1), textTwo);

			AssertFormsImageExists ();
		}

		void AssertFormsImageExists ()
		{
			Assert.IsTrue (app.WaitForElement (c => c.Class (formsImageViewClass)).Any ());
		}

		void AssertAppResultTextEqualsString (AppResult r, string s)
		{
			Assert.IsTrue (r.Text.Equals (s), string.Format ("AppResult.Text({0}) != {1}", r.Text, s));
		}

		void ConfirmNavigationBar ()
		{
			Assert.IsTrue (app.WaitForElement (c => c.Text ("Local")).Any (), "Could not find 'Local' navigation element.");
		}

		const string ANDROID_FIRST_MENU_ITEM = "Local";

		void ScrollHorizontalUntilValueFound (string strMenuItem)
		{
			float y = app.WaitForElement (c => c.Text (ANDROID_FIRST_MENU_ITEM)).ElementAt (0).Rect.CenterY + 5f;
			float x = app.Query (c => c.Text (ANDROID_FIRST_MENU_ITEM)).ElementAt (0).Rect.X;
			while (!app.Query (c => c.Text (strMenuItem)).Any ()) {
				app.DragCoordinates (x + 100f, y, x, y);
			}
		}
	}
}

