using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TcpCatcher {
    public static class ConvertorUtil
    {

		/// <summary>
		/// Convert Image to byteArrey
		/// </summary>
		/// <param name="img"></param>
		/// <param name="format">provide null for default value of jpeg</param>
		/// <returns></returns>
	    public static byte[] ImageToBytes(Image img,ImageFormat format)
	    {

			MemoryStream ms = new MemoryStream ();
			img.Save ( ms , format??ImageFormat.Jpeg);
			return ms.ToArray ();
	    }

	    public static byte[] ImageToBytes(Image img) { return ImageToBytes(img, null); }

	    public static Image BytesToImage(byte[] bytes)
	    {
			MemoryStream ms = new MemoryStream ( bytes );
			Image img = Image.FromStream ( ms );
			return img;
	    }
		/// <summary>
		/// serialization
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static byte[] ObjectToBytes ( object obj ) {

			using ( var ms = new MemoryStream () ) {
				var bf = new BinaryFormatter ();
				bf.Serialize ( ms , obj );
				return  ms.ToArray ();
			}


		}

		/// <summary>
		/// deserialisation
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static T BytesToObject<T> ( byte[] bytes ) {
			using ( var ms = new MemoryStream ( bytes ) ) {
				var bf = new BinaryFormatter ();
				return (T) bf.Deserialize ( ms );
			}
		}
    }
}
