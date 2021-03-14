using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class VendorJunkSellItem : IScriptable
	{
		[Ordinal(0)] [RED("item")] public wCHandle<gameItemData> Item { get; set; }
		[Ordinal(1)] [RED("quantity")] public CInt32 Quantity { get; set; }

		public VendorJunkSellItem(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
