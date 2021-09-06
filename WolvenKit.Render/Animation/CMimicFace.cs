using System.Collections.Generic;
using WolvenKit.Render.Animation;

namespace WolvenKit.Render
{
    public class CMimicFace
    {
        public CSkeleton mimicSkeleton;
        public CSkeleton floatTrackSkeleton;
        public List<MimicPose> mimicPoses = new List<MimicPose>();
        public List<int> mapping = new List<int>();
        SMimicTrackPose mimicTrackPoses;
        SMimicTrackPose mimicFilterPoses;
        public List<Vector> normalBlendAreas = new List<Vector>();
        int neckIndex;
        int headIndex;
    }

    public class MimicPose
    {
        public string name;
        public List<Bone> bones;
        public float duration;
        public uint numFrames;
        public float dt;
    }
    public class SMimicTrackPose
    {
    }
}