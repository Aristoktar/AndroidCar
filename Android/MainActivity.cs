using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Aristov.Common;
using Aristov.Common.Android;
using Aristov.Communication.RT;
using Aristov.Communication.RT.Queued;
using Java.IO;
using Java.Lang;
using Camera = Android.Hardware.Camera;
using Exception = System.Exception;
using Thread = System.Threading.Thread;


namespace Android {
	[Activity ( Label = "Android" , MainLauncher = true , Icon = "@drawable/icon" )]
	public class MainActivity : Activity , ISurfaceHolderCallback,Camera.IPreviewCallback
	{
		static Camera _camera;
		private readonly string LogTag = "MyCarApp";
		int count = 1;
		
		Thread thread;
		NetworkStream stre;
		SurfaceView surfaceView;
		ISurfaceHolder surfaceHolder;
		LinearLayout layout;
		Server _server;
		TextureView _textureView;
		int frames = 0;

		Button ButtonStart;
		Button ButtonStop;
		EditText TextHost;

		VideoServer _videoServer;
		ILogger Logger;

		public MainActivity()
		{
			LoggerFactory.Setup ( typeof ( AndroidLogger ) );
			Logger = LoggerFactory.Create ();
		}

		protected override void OnCreate ( Bundle bundle ) {
			base.OnCreate ( bundle );
			SetContentView ( Resource.Layout.Main );

			surfaceView = FindViewById<SurfaceView> ( Resource.Id.surfaceCamera );
			surfaceHolder = surfaceView.Holder;
			surfaceHolder.AddCallback ( this );
			ButtonStart = FindViewById<Button> ( Resource.Id.ButtonStart );
			ButtonStop= FindViewById<Button> ( Resource.Id.ButtonStop );
			TextHost = FindViewById<EditText> ( Resource.Id.TextHost );
			
			ButtonStart.Click += ButtonStart_Click;
			
		}

		void ButtonStart_Click ( object sender , EventArgs e )
		{
			try
			{
				var host = GetString ( Resource.String.InitHost ).Split ( ':' );
				_videoServer = new VideoServer ( host[0] , int.Parse ( host[1] ) );
			}
			catch (Exception ex)
			{
				Logger.Error("Error: {0}", ex);
			}
			
		}

		

		public void SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height)
		{
		}

		
		public void SurfaceCreated(ISurfaceHolder holder)
		{
			try {
				_camera = Camera.Open();
				_camera.SetDisplayOrientation(90);
				_camera.SetPreviewDisplay(holder);
				_camera.SetPreviewCallback(this);
				_camera.StartPreview();
				Logger.Info("Camera opened");
				//  camera.setDisplayOrientation(90);
			}
			catch ( Exception e )
			{
				Logger.Error("camera open faild");
			}
		}


		public void SurfaceDestroyed(ISurfaceHolder holder)
		{
		}

		int rate = 15;
		int countF = 0;
		public void OnPreviewFrame(byte[] data, Camera camera)
		{

			try {
				if (_videoServer != null)
				{
					if (countF%rate==0)
					{
						Camera.Parameters parameters = camera.GetParameters();
						Rect rect = new Rect(0, 0, parameters.PreviewSize.Width, parameters.PreviewSize.Height);
						var jpegdata = ConvertYuvToJpeg(data, camera);
						_videoServer.NewFrame(jpegdata);
					}
					countF++;
				}
			}
			catch (Exception ex)
			{

				Logger.Error("Error while sending frame: {0}", ex);
			}

		}
		private byte[] ConvertYuvToJpeg ( byte[] vuvData , Android.Hardware.Camera camera ) {
			var cameraParameters = camera.GetParameters ();
			var width = cameraParameters.PreviewSize.Width;
			var height = cameraParameters.PreviewSize.Height;
			var yuv = new YuvImage ( vuvData , cameraParameters.PreviewFormat , width , height , null );
			var ms = new MemoryStream ();
			var quality = 1;   // adjust this as needed
			yuv.CompressToJpeg ( new Rect ( 0 , 0 , width , height ) , quality , ms );
			var jpegData = ms.ToArray ();

			return jpegData;
		}

		public static Bitmap BytesToBitmap ( byte[] imageBytes ) {
			Bitmap bitmap = BitmapFactory.DecodeByteArray ( imageBytes , 0 , imageBytes.Length );
			return bitmap;
		}
	}
}

