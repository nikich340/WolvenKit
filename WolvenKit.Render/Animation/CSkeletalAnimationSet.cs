using IrrlichtLime.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolvenKit.CR2W.Types;

namespace WolvenKit.Render.Animation
{

    /// <summary>
    /// For json serializer
    /// </summary>
    class CSkeletalAnimationSet
    {
        //string requiredSfxTag;
        public List<CSkeletalAnimationSetEntry> animations = new List<CSkeletalAnimationSetEntry>();
        //public List<CExtAnimEventsFile> extAnimEvents = new List<CExtAnimEventsFile>();
        //string skeleton; //handle:CSkeleton
        //public List<ICompressedPose> compressedPoses = new List<ICompressedPose>();
        //string Streaming option; //enum? //SAnimationBufferStreamingOption
        //uint "Number of non-streamable bones";
    }


    public class CSkeletalAnimationSetEntry
    {
        public CSkeletalAnimation animation;
        public List<CVariable> entries = new List<CVariable>();
    }

    public abstract class IAnimationBuffer { };

    public class CSkeletalAnimation
    {
        public string name;
        //string streamingType;
        //bool hasBundingBox;
        //Box boundingBox;
        //uint id;
        public IMotionExtraction motionExtraction;
        //int compressedPose;
        public IAnimationBuffer animBuffer;
        public float framesPerSecond;
        public float duration;
    }

    class CAnimationBufferMultipart : IAnimationBuffer
    {
        public uint numFrames;
        public uint numBones;
        public uint numTracks;
        public List<uint> firstFrames = new List<uint>();
        public List<CAnimationBufferBitwiseCompressed> parts = new List<CAnimationBufferBitwiseCompressed>();
    }

    class CAnimationBufferBitwiseCompressed : IAnimationBuffer
    {
        public uint version;
        public List<Bone> bones;
        //public List<Bone> data;
        //public List<Bone> fallbackData;
        public List<Track> tracks;
        //uint deferredData;
        //string orientationCompressionMethod;
        public float duration;
        public uint numFrames;
        public float dt;
        //string streamingOption;
        //uint nonStreamableBones;
        bool hasRefIKBones;
    }

    public class Track
    {
        public string trackName = "????";
        public sbyte compression = 0;
        public float dt = 0;
        public uint dataAddr = 0;
        public uint dataAddrFallback = 0;
        public ushort numFrames = 0;
        public List<float> trackFrames = new List<float>();
        public int index;
    }

    public class Bone
    {
        public int index = 0;
        public string BoneName = "????";
        public float position_dt = 0;
        public uint position_dataAddr = 0;
        public uint position_dataAddrFallback = 0;
        public ushort position_numFrames = 0;
        public List<Vector> positionFrames = new List<Vector>();
        public float rotation_dt = 0;
        public uint rotation_dataAddr = 0;
        public uint rotation_dataAddrFallback = 0;
        public ushort rotation_numFrames = 0;
        public List<Quaternion> rotationFrames = new List<Quaternion>();
        public float scale_dt = 0;
        public uint scale_dataAddr = 0;
        public uint scale_dataAddrFallback = 0;
        public ushort scale_numFrames = 0;
        public List<Vector> scaleFrames = new List<Vector>();
    }

}
