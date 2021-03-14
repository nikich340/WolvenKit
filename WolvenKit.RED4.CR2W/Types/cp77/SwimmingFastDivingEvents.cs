using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class SwimmingFastDivingEvents : LocomotionSwimmingEvents
	{
		[Ordinal(0)] [RED("lapsedTime")] public CFloat LapsedTime { get; set; }

		public SwimmingFastDivingEvents(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
