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
	   private static readonly int _headerLength = sizeof ( Int32 )*2;
	   private byte[] _data;
	   private PacketType _packetType;

	   public Packet(byte[] bytes,bool isPacketIncomin)
	   {
		   if (isPacketIncomin)
		   {
			   _packetType = PacketType.Image;
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
