using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Aristov.Communication.RT
{
   public class Packet
   {
	   public static int TypeOffset = 0;
	   public static int TimeStampOffset = 4;
	   public static int TokenOffset = 12;
	   public static int DataOffset = 16;
	  
	   public static int DataSize = 1000;
	   



	   private static readonly int _headerLength = sizeof ( Int32 )*2;
	   private byte[] _data;
	   private PacketType _packetType;

	   public static byte[] Create ( byte[] bytes , PacketType type, int token )
	   {
		   var res = new byte[DataOffset + DataSize];
		   BitConverter.GetBytes((int) type).CopyTo(res,TypeOffset);
		   BitConverter.GetBytes(token).CopyTo(res,TokenOffset);
		   BitConverter.GetBytes(DateTime.Now.Ticks).CopyTo(res,TimeStampOffset);
		   bytes.CopyTo(res,DataOffset);
		   return res;
	   }

	

	   public Packet(byte[] bytes, PacketType type)
	   {

	   }

	   public Packet(byte[] bytes,bool isPacketIncomin)
	   {
		   if (!isPacketIncomin)
		   {
			   _packetType = PacketType.ImageRecive;
			   _data = bytes;
		   }
		   else
		   {
			   int currOffset = 0;
			   _packetType = (PacketType)BitConverter.ToInt32 ( bytes , currOffset );
			   currOffset += sizeof ( Int32 );
			  // var dataLength


		   }
	   }

	   public byte[] GetSendData()
	   {
		   var header = new byte[_headerLength];
		   int currOffset = 0;
		   BitConverter.GetBytes ( (Int32) _packetType ).CopyTo ( header , currOffset );
		   currOffset += sizeof ( Int32 );
		   BitConverter.GetBytes ( _data.Length).CopyTo ( header , currOffset );
		   currOffset += sizeof ( Int32 );
		   
		   var sendData = new byte[_headerLength + _data.Length];
		   var data = header.Concat(_data).ToArray();
		   return data;
	   }
	   
   }
}
