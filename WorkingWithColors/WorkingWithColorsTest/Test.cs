using NUnit.Framework;
using System;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using System.Collections;
using System.Collections.Generic;

namespace WorkingWithColorsTest
{
	[TestFixture ()]
	public class Test
	{
		IApp app;

		const int LABEL_RENDERER_COUNT = 12;

		string labelRendererClass;

		[SetUp]
		public void SetUp ()
		{
			app = ConfigureApp.Android.ApkFile ("/Users/dylankelly/Documents/xamarin-forms-samples/WorkingWithColors/Android/bin/Release/WorkingWithColors.Android-Signed.apk")
				.ApiKey ("024b0d715a7e9c22388450cf0069cb19").StartApp ();
//			app = ConfigureApp.iOS.AppBundle ("/Users/dylankelly/Documents/xamarin-forms-samples/WorkingWithColors/iOS/bin/iPhoneSimulator/Release/WorkingWithColorsiOS.app")
//				.ApiKey ("024b0d715a7e9c22388450cf0069cb19")
//				.StartApp ();
			labelRendererClass = app.GetType () == typeof(AndroidApp) ? "LabelRenderer" : "Xamarin_Forms_Platform_iOS_LabelRenderer";
		}

		[Test ()]
		public void CheckForLabels ()
		{
			var labelText = new List<string>(){"Color Demo", "Red", "Orange", "Yellow", "Green",
				"Blue", "Indigo", "Violet", "Transparent", "Default", "Accent"};

			for (int i = 0; i < labelText.Count; i++) {
				app.WaitForElement (c => c.Text (labelText [i]), 
					"Timed out waiting for text " + labelText [i] + ".");
			}
				
		}

		[Test ()]
		public void CheckForLabelRenderers ()
		{

			Assert.AreEqual (app.WaitForElement (c => c.Class (labelRendererClass)).Length, 
				LABEL_RENDERER_COUNT, "Could not find " + LABEL_RENDERER_COUNT + " label renderers.");

		}
	}
}

