using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using WolvenKit.CR2W;
using WolvenKit.CR2W.Types;
using Newtonsoft.Json;
using WolvenKit.Bundles;
using WolvenKit.CR2W.Types.Utils;

namespace WolvenKit.Render.Animation
{
    class CCutsceneTemplate: CSkeletalAnimationSet
    {
        [JsonIgnore]
        public List<SCutsceneActorDef> SCutsceneActorDefsNat = new List<SCutsceneActorDef>();
        public CVariable SCutsceneActorDefs;
        public CVariable point;
        public CVariable burnedAudioTrackName;
        public CVariable entToHideTags;
        public CVariable lastLevelLoaded;
        public CVariable effects;

        public CVariable requiredSfxTag;
        public CVariable reverbName;
        public CVariable resourcesToPreloadManuallyPaths;
        public CVariable fadeBefore;
        public CVariable fadeAfter;
        public CVariable cameraBlendInTime;
        public CVariable cameraBlendOutTime;
        public CVariable blackscreenWhenLoading;
        public CVariable checkActorsPosition;
        public CVariable usedInFiles;
        public CVariable streamable;
        public CVariable banksDependency;
        //public CVariable animevents;
        public CBufferUInt32<CVectorWrapper> animevents;

    }

    class SCutsceneActorDef
    {
        public SCutsceneActorDef()
        {
            //<property Name="name" Type="String" />
            //<property Name="tag" Type="TagList" />
            //<property Name="voiceTag" Type="CName" />
            //<property Name="template" Type="soft:CEntityTemplate" />
            //<property Name="appearance" Type="CName" />
            //<property Name="type" Type="ECutsceneActorType" />
            //<property Name="finalPosition" Type="TagList" />
            //<property Name="killMe" Type="Bool" />
            //<property Name="useMimic" Type="Bool" />
            //<property Name="animationAtFinalPosition" Type="CName" />
        }

        public string name { get; internal set; }
        public string template { get; internal set; }
        public bool useMimic { get; internal set; }
        public string type { get; internal set; }
        public string voiceTag { get; internal set; }

        public List<string> tag = new List<string>();

        [JsonIgnore]
        public Rig rig;
        [JsonIgnore]
        internal CSkeleton face_rig;
        //public EntityGenerator templateData  { get; internal set; }
    }
}