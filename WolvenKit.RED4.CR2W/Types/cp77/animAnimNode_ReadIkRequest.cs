using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class animAnimNode_ReadIkRequest : animAnimNode_OnePoseInput
	{
		[Ordinal(12)] [RED("ikChain")] public CName IkChain { get; set; }
		[Ordinal(13)] [RED("outTransform")] public animTransformIndex OutTransform { get; set; }

		public animAnimNode_ReadIkRequest(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
