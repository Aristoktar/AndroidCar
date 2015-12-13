
namespace Aristov.Communication.RT {
	interface IImage {
		byte[] ByteData {
			get;
			set;
		}
		MediaType MediaType {
			get;
			set;
		}
	}
}
