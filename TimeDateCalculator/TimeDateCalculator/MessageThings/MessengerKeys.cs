﻿namespace TimeDateCalculator.MessageThings
{
	public class MessengerKeys
	{
		// Orientation msgs.
		private const string PrivLandscapeOrientationRequest = "LandscapeOrientationRequest";
		private const string PrivPortraitOrientationRequest = "PortraitOrientationRequest";
		private const string PrivAllButUpsideDowntOrientationRequest = "AllButUpsideDowntOrientationRequest";

		public static string LandscapeOrientationRequest => PrivLandscapeOrientationRequest;
		public static string PortraitOrientationRequest => PrivPortraitOrientationRequest;
		public static string AllButUpsideDowntOrientationRequest => PrivAllButUpsideDowntOrientationRequest;

		// File handl. msgs.
		private const string fileToReadFromSelected = "FileToReadFromSelected";
		private const string fileToSaveToSelected = "FileToSaveToSelected";
		private const string fileToSaveRawTextToSelected = "FileToSaveRawTextToSelected";

		public static string FileToReadFromSelected => fileToReadFromSelected;
		public static string FileToSaveToSelected => fileToSaveToSelected;
		public static string FileToSaveRawTextToSelected => fileToSaveRawTextToSelected;

		// .ics Description entered
		private const string icsDescriptionEntered = "icsDescriptionEntered";
		public static string IcsDescriptionEntered => icsDescriptionEntered;

	}
}
