using NUnit.Framework;
using System;
using Xamarin.UITest;
using Xamarin.UITest.Android;

namespace UsingDependencyServiceTest
{
	[TestFixture ()]
	public class Test
	{
		IApp app;

		string buttonClass;

		[SetUp]
		public void SetUp ()
		{
//			app = ConfigureApp.Android.ApkFile ("/Users/dylankelly/Documents/xamarin-forms-samples/UsingDependencyService/Android/bin/Release/UsingDependencyService.Android-Signed.apk")
//				.ApiKey ("024b0d715a7e9c22388450cf0069cb19").StartApp ();
			app = ConfigureApp.iOS.AppBundle ("/Users/dylankelly/Documents/xamarin-forms-samples/UsingDependencyService/iOS/bin/iPhoneSimulator/Release/UsingDependencyServiceiOS.app")
				.ApiKey ("024b0d715a7e9c22388450cf0069cb19")
				.StartApp ();
			buttonClass = app.GetType () == typeof(AndroidApp) ? "Button" : "UIButton";
		}

		[Test ()]
		public void CheckForButtonContent ()
		{
			app.WaitForElement (c => c.Class (buttonClass));

			app.Tap (c => c.Text ("Hello, Forms !"));

			Assert.Greater (app.Query (c => c.Text ("Hello, Forms !")).Length, 0, "Could not find 'Hello, Forms !'");
		}
	}
}

