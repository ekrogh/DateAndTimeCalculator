﻿using Acr.UserDialogs;
using Foundation;
using MobileCoreServices;
using pdfCalcProj.FileHandlers;
using pdfCalcProj.MessageThings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using static System.IO.File;

[assembly: Xamarin.Forms.Dependency(typeof(pdfCalcProj.iOS.FileHandlers.HandleFiles_iOS))]
namespace pdfCalcProj.iOS.FileHandlers
{
	class HandleFiles_iOS : IUIDocumentPickerDelegate, IHandleFiles
	{

		public IntPtr Handle => throw new NotImplementedException();

		#pragma warning disable 1998
		public async Task SelectFilesToReadFrom(string[] filetypes)
		{
			var myAllowedUTIsList = new List<string>();

			foreach (string filetype in filetypes)
			{
				switch (filetype)
				{
					case "pdf":
						{
							myAllowedUTIsList.Add(UTType.PDF);
							break;
						}
					case "Directory":
						{
							myAllowedUTIsList.Add(UTType.Directory);
							break;
						}
				}
			}
			string[] allowedFiles = myAllowedUTIsList.ToArray();
			//HERFRA

			try
			{
				// Display the picker
				var picker = new UIDocumentPickerViewController(allowedFiles, UIDocumentPickerMode.Open)
				{
					AllowsMultipleSelection = true,
				};
				picker.DidPickDocument += (sndr, pArgs) =>
				{
					SelectFilesResultMessageArgs args = new SelectFilesResultMessageArgs
					{
						TheSelectedFilesInfo = new List<SelectedFileInfo>(),

						DidPick = true
					};

					SelectedFileInfo urlHere = new SelectedFileInfo();

					// IMPORTANT! You must lock the security scope before you can
					// access this file
					var securityEnabled = pArgs.Url.StartAccessingSecurityScopedResource();

					// Open the document
					urlHere.TheStream = new FileStream(pArgs.Url.Path, FileMode.Open, FileAccess.Read);

					// IMPORTANT! You must release the security lock established
					// above.
					pArgs.Url.StopAccessingSecurityScopedResource();

					args.TheSelectedFilesInfo.Add(urlHere);

					// Fire the message
					MessagingCenter.Send<App, SelectFilesResultMessageArgs>((App)Xamarin.Forms.Application.Current, MessengerKeys.FilesToReadFromSelected, args);
				};
				picker.DidPickDocumentAtUrls += (sndr, pArgs) =>
				{
					SelectFilesResultMessageArgs args = new SelectFilesResultMessageArgs
					{
						TheSelectedFilesInfo = new List<SelectedFileInfo>(),

						DidPick = true
					};

					SelectedFileInfo urlHere = new SelectedFileInfo();

					foreach (NSUrl url in pArgs.Urls)
					{
						// IMPORTANT! You must lock the security scope before you can
						// access this file
						var securityEnabled = url.StartAccessingSecurityScopedResource();

						// Open the document
						urlHere.TheStream = new FileStream(url.Path, FileMode.Open, FileAccess.Read);

						// IMPORTANT! You must release the security lock established
						// above.
						url.StopAccessingSecurityScopedResource();

						args.TheSelectedFilesInfo.Add(urlHere);
					}

					// Fire the message
					MessagingCenter.Send<App, SelectFilesResultMessageArgs>((App)Xamarin.Forms.Application.Current, MessengerKeys.FilesToReadFromSelected, args);
				};
				picker.WasCancelled += (sndr, pArgs) =>
				{
					SelectFilesResultMessageArgs args = new SelectFilesResultMessageArgs
					{
						DidPick = false
					};

					// Fire the message
					MessagingCenter.Send<App, SelectFilesResultMessageArgs>((App)Xamarin.Forms.Application.Current, MessengerKeys.FilesToReadFromSelected, args);
				};

				UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(picker, true, null);

			}
			catch (Exception ex)
			{
				var xcptn = ex.ToString();
			}

		}
#pragma warning restore 1998

		public async Task SelectFilesToSaveTo(string[] filetypes, string mesgKey)
		{
			SelectFilesResultMessageArgs args = new SelectFilesResultMessageArgs
			{
				TheSelectedFilesInfo = new List<SelectedFileInfo>(),

				DidPick = false // In case try catches an exception
			};
			try
			{
				var r = await UserDialogs.Instance.PromptAsync(new PromptConfig
				{
					Title = "Enter file name",
					Message = "(Just file name. No Path!). \".txt\" will be added)",
					Placeholder = "filename",
					OnTextChanged = AreArgsValid(),
				});


				if (r.Ok)
				{
					SelectedFileInfo urlHere = new SelectedFileInfo
					{

						// Open the document
						TheStream =
						new FileStream(Path.Combine(FileSystem.CacheDirectory, r.Text + ".txt"),
									   FileMode.OpenOrCreate,
									   FileAccess.Write)
					};

					args.DidPick = true;

					args.TheSelectedFilesInfo = new List<SelectedFileInfo>
					{
						urlHere
					};
				}
				else
				{
					args.DidPick = false;
				}

				// Fire the message
				MessagingCenter.Send((App)Xamarin.Forms.Application.Current,
									 mesgKey,
									 args);

			}
			catch (Exception)
			{
			}
		}

		private static Action<PromptTextChangedArgs> AreArgsValid()
		{
			return args =>
			{
				args.IsValid =
					!(args.Value.Contains("/", StringComparison.CurrentCultureIgnoreCase)
					|| args.Value.Contains("\\", StringComparison.CurrentCultureIgnoreCase))
					&& (args.Value.IndexOfAny(Path.GetInvalidFileNameChars()) == ((int)(-1)))
					&& (args.Value.Length != 0);
			};
		}


		#region For IUIDocumentPickerDelegate Support
		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~HandleFiles_iOS()
		// {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
		#endregion

		#pragma warning disable 1998
		public async Task<bool> PathExists(string PathName)
		{
			return Directory.Exists(PathName);
		}
#pragma warning restore 1998

#pragma warning disable 1998
		public async Task<bool> FileExists(string FilePathAndName)
		{
			return Exists(FilePathAndName);
		}
#pragma warning restore 1998

		public async Task<string> ReadFromTextFile(System.IO.Stream TheTextFileStream)
		{
			try
			{
				byte[] bfr = new byte[TheTextFileStream.Length];
				await TheTextFileStream.ReadAsync(bfr, 0, (int)TheTextFileStream.Length);
				if (BitConverter.IsLittleEndian)
				{
					return Encoding.Unicode.GetString(bfr);
				}
				else
				{
					return Encoding.BigEndianUnicode.GetString(bfr);
				}
			}
			catch (Exception)
			{
			}

			return null;
		}

#pragma warning disable 1998
		public async Task<string> ReadFromTextFile(string FilePathAndName)
		{
			return ReadAllText(FilePathAndName);
		}
#pragma warning restore 1998

		public async Task<bool> SaveToTextFile(System.IO.Stream TheTextFileStream, string TheText)
		{
			try
			{
				byte[] outbfr;
				if (BitConverter.IsLittleEndian)
				{
					outbfr = Encoding.Unicode.GetBytes(TheText);
				}
				else
				{
					outbfr = Encoding.BigEndianUnicode.GetBytes(TheText);
				}
				await TheTextFileStream.WriteAsync(outbfr, 0, outbfr.Length);
				await TheTextFileStream.FlushAsync(); //Write all

				await Share.RequestAsync(new ShareFileRequest
				{
					Title = "Save file",
					File = new ShareFile((TheTextFileStream as FileStream).Name)
				});

				return true;
			}
			catch (Exception)
			{
			}

			return false;
		}

		public async Task<byte[]> ReadAllBytesFromFile(Stream TheByteFileStream)
		{
			try
			{
				byte[] bfr = new byte[TheByteFileStream.Length];
				await TheByteFileStream.ReadAsync(bfr, 0, (int)TheByteFileStream.Length);
				return bfr;
			}
			catch (Exception)
			{
			}

			return null;
		}

#pragma warning disable 1998
		public async Task<bool> SaveBytesToFile(string thePathAndFile, byte[] ByteBuffer)
		{
			WriteAllBytes(thePathAndFile, ByteBuffer);
			return true;
		}
#pragma warning restore 1998

		public string GetCurrentDirectory()
		{
			return Directory.GetCurrentDirectory();
		}

		public void DidPickDocument(UIDocumentPickerViewController controller, NSUrl url)
		{
			throw new NotImplementedException();
		}
	}
}