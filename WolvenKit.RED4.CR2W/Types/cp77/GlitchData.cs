using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class GlitchData : CVariable
	{
		[Ordinal(0)] [RED("intensity")] public CFloat Intensity { get; set; }
		[Ordinal(1)] [RED("state")] public CEnum<EGlitchState> State { get; set; }

		public GlitchData(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
