using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class LocomotionTakedownEvents : LocomotionEventsTransition
	{
		[Ordinal(0)] [RED("stateMachineInitData")] public wCHandle<LocomotionTakedownInitData> StateMachineInitData { get; set; }

		public LocomotionTakedownEvents(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
