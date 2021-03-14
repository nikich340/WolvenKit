using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class ArcadeMachineControllerPS : ScriptableDeviceComponentPS
	{
		[Ordinal(103)] [RED("gameVideosPaths")] public CArray<redResourceReferenceScriptToken> GameVideosPaths { get; set; }

		public ArcadeMachineControllerPS(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
