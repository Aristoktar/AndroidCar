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
using Aristov.Communication.RT;
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

		void ButtonStart_Click ( object sender , EventArgs e ) {
			try
			{
				_server = new Server ( TextHost.Text );
				_server.Start ();
			}
			catch (Exception ex)
			{
				Log.Debug(LogTag, string.Format("Error: {0}",ex));
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
				Log.Debug(LogTag, "CameraOpend");
				_camera.SetPreviewDisplay(holder);
				_camera.SetPreviewCallback(this);
				_camera.StartPreview();

				
				//  camera.setDisplayOrientation(90);
			}
			catch ( Exception e )
			{
				Log.Error(LogTag,"camera open faild");
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
				if (_server != null)
				{
					if (countF%rate==0)
					{
						//TcpClient client = new TcpClient ( "aristov.me" , 123 );
						//var stream = client.GetStream ();
						Camera.Parameters parameters = camera.GetParameters();
						Rect rect = new Rect(0, 0, parameters.PreviewSize.Width, parameters.PreviewSize.Height);
						var jpegdata = ConvertYuvToJpeg(data, camera);
						//stream.Write ( jpegdata , 0 , jpegdata.Length );
						Log.Info(LogTag, "New frame sent:hash "+jpegdata.GetHashCode());
						_server.NewFrame(jpegdata);
						
					}
					countF++;
				}
			}
			catch (Exception ex)
			{

				Log.Error(LogTag,string.Format("Error while sending frame: {0}", ex));
			}

		}
		private byte[] ConvertYuvToJpeg ( byte[] yuvData , Android.Hardware.Camera camera ) {
			var cameraParameters = camera.GetParameters ();
			var width = cameraParameters.PreviewSize.Width;
			var height = cameraParameters.PreviewSize.Height;
			var yuv = new YuvImage ( yuvData , cameraParameters.PreviewFormat , width , height , null );
			var ms = new MemoryStream ();
			var quality = 1;   // adjust this as needed
			yuv.CompressToJpeg ( new Rect ( 0 , 0 , width , height ) , quality , ms );
			var jpegData = ms.ToArray ();

			return jpegData;
		}

		public static Bitmap bytesToBitmap ( byte[] imageBytes ) {
			Bitmap bitmap = BitmapFactory.DecodeByteArray ( imageBytes , 0 , imageBytes.Length );

			return bitmap;
		}
	}
}

