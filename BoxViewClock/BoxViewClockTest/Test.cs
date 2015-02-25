using NUnit.Framework;
using System;
using Xamarin.UITest;
using Xamarin.UITest.Android;

namespace BoxViewClockTest
{
	[TestFixture ()]
	public class Test
	{
		IApp app;

		string boxRendererClass;

		[SetUp]
		public void SetUp(){
			app = ConfigureApp.Android.ApkFile ("/Users/dylankelly/Documents/xamarin-forms-samples/BoxViewClock/BoxViewClock/BoxViewClock.Android/bin/Release/BoxViewClock.Android-Signed.apk")
				.ApiKey ("024b0d715a7e9c22388450cf0069cb19").StartApp ();
//			app = ConfigureApp.iOS.AppBundle ("/Users/dylankelly/Documents/xamarin-forms-samples/BoxViewClock/BoxViewClock/BoxViewClock.iOS/bin/iPhoneSimulator/Release/FormsTemplateiOS.app")
//				.ApiKey ("024b0d715a7e9c22388450cf0069cb19")
//				.StartApp ();
			boxRendererClass = app.GetType() == typeof(AndroidApp) ? "BoxRenderer" : "Xamarin_Forms_Platform_iOS_BoxRenderer";
		}

		// Could probably check to see that second, minute, and hour hands are moving
		// but there is no easy way given that they do not have IDs
		[Test ()]
		public void CheckForBoxRenderers ()
		{

			Assert.AreEqual (app.WaitForElement (c => c.Class (boxRendererClass)).Length, 63);

		}
	}
}

