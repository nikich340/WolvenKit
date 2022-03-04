using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using WolvenKit.CR2W;
using WolvenKit.CR2W.Types;
using Newtonsoft.Json;
using IrrlichtLime;
using IrrlichtLime.Core;
using IrrlichtLime.Scene;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using WolvenKit.Common.Wcc;

namespace WolvenKit.Render.Animation
{

    public class ExportAnimation
    {
        private uint ReadFloat24(BinaryReader file)
        {
            var pad = 0;
            var b1 = file.ReadByte();
            var b2 = file.ReadByte();
            var b3 = file.ReadByte();
            return
                ((uint)b3 << 24) | ((uint)b2 << 16) | ((uint)b1 << 8) | ((uint)pad);
        }

        public float ReadCompressFloat(BinaryReader file, int compression)
        {
            float val = 0;
            if (compression == 0)
            {
                val = file.ReadSingle();
            }
            else if (compression == 1) //24 bit single
            {
                var bits = ReadFloat24(file);
                val = BitConverter.ToSingle(BitConverter.GetBytes(bits), 0);
            }
            else if (compression == 2)
            {
                var bitsx = file.ReadUInt16() << 16;
                val = BitConverter.ToSingle(BitConverter.GetBytes(bitsx), 0);
            }
            return val;
        }

        bool IsBitSet(byte b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }
        CSkeletalAnimationSet SkeletalAnimationSet = new CSkeletalAnimationSet();
        Rig loadedRig;
        CR2WFile animFile;
        int selectedAnimIdx;

        public static List<KeyValuePair<string, int>> AnimationNames = new List<KeyValuePair<string, int>>();
        public ExportAnimation()
        {
        }
        /// <summary>
        /// Loads an w2anims file and sets a list of AnimationNames
        /// </summary>
        public void LoadData(CR2WFile animFile, Rig exportRig = null)
        {
            loadedRig = exportRig;
            AnimationNames.Clear();
            this.animFile = animFile;
            if (animFile != null)
                foreach (CPtr animationPtr in (animFile.chunks[0].GetVariableByName("animations") as CArray).array)
                {
                    var setEntry = animationPtr.Reference;
                    var animation = (setEntry.GetVariableByName("animation") as CPtr).Reference;
                    var name = animation.GetVariableByName("name");
                    //var chunkIdx = (chunk.GetVariableByName("animBuffer") as CPtr).Reference.ChunkIndex;
                    AnimationNames.Add(new KeyValuePair<string, int>((name as CName).Value, setEntry.ChunkIndex));
                }
            //SelectAnimation(animFile, 0);
        }

        public void LoadAllAnims()
        {
            SkeletalAnimationSet.animations.Clear();
            //use actor skeletons to load animation data for export
            foreach (CPtr animationPtr in (animFile.chunks[0].GetVariableByName("animations") as CArray).array)
            {
                SkeletalAnimationSet.animations.Add(LoadEntry(animationPtr.Reference, loadedRig.meshSkeleton));
            }
        }



        /// <summary>
        /// Sets the selected animation
        /// </summary>
        public void SelectAnimation(CR2WFile animFile, int selectedAnimIdx)
        {
            this.selectedAnimIdx = selectedAnimIdx;
            SkeletalAnimationSet.animations.Clear();
        }

        /// <summary>
        /// This will replace bone index as name with bone names in the rig
        /// </summary>
        public void Apply(Rig exportRig)
        {
            loadedRig = exportRig;
        }

        /// <summary>
        /// Create the animation data that will be exported
        /// </summary>
        private void ReadSelectedAnimation()
        {
            if (animFile != null)
                foreach (var chunk in animFile.chunks)
                {
                    if (chunk.Type == "CSkeletalAnimationSetEntry" && chunk.ChunkIndex == AnimationNames[selectedAnimIdx].Value)
                    {
                        SkeletalAnimationSet.animations.Add(LoadEntry(chunk, loadedRig.meshSkeleton));
                    }
                }
        }

        public CSkeletalAnimationSetEntry LoadEntry(CR2WExportWrapper chunk, CSkeleton meshSkeleton)
        {
            CSkeletalAnimationSetEntry SkeletalAnimationSetEntry = new CSkeletalAnimationSetEntry();
            var animation = (chunk.GetVariableByName("animation") as CPtr).Reference;
            var entries = (chunk.data as CR2W.Types.CSkeletalAnimationSetEntry).entries;
            SkeletalAnimationSetEntry.entries = entries;//getCExtAnimEvents(entries);
            CSkeletalAnimation SkeletalAnimation = new CSkeletalAnimation();

            if ((animation.GetVariableByName("motionExtraction") as CPtr) != null)
            {
                var motionExtraction = (animation.GetVariableByName("motionExtraction") as CPtr).Reference;
                CLineMotionExtraction2 motion = new CLineMotionExtraction2();
                motion.duration = (motionExtraction.GetVariableByName("duration") as CFloat)?.val ?? 1.0f;
                motion.flags = (motionExtraction.GetVariableByName("flags") as CUInt8)?.val ?? 0;
                foreach (CFloat frame in (motionExtraction.GetVariableByName("frames") as CArray).array)
                    motion.frames.Add(frame.val);
                CByteArray byteArray = motionExtraction.GetVariableByName("deltaTimes") as CByteArray;
                motion.deltaTimes = byteArray.Bytes;
                SkeletalAnimation.motionExtraction = motion;
            }
            SkeletalAnimation.name = (animation.GetVariableByName("name") as CName).Value;
            SkeletalAnimation.framesPerSecond = (animation.GetVariableByName("framesPerSecond") as CFloat).val;
            SkeletalAnimation.duration = (animation.GetVariableByName("duration") as CFloat)?.val ?? 1.0f;
            CR2WExportWrapper animBufferChunk = (animation.GetVariableByName("animBuffer") as CPtr).Reference;
            SkeletalAnimation.animBuffer = getBuffer(animBufferChunk, meshSkeleton, SkeletalAnimation.name);

            SkeletalAnimationSetEntry.animation = SkeletalAnimation;
            return SkeletalAnimationSetEntry;
        }

        //private List<CExtAnimEvent> getCExtAnimEvents(List<CVariable> entries)
        //{
        //    List <CExtAnimEvent> list = new List<CExtAnimEvent>(); 
        //    foreach (var entry in entries)
        //    {
        //        CVector entryVector = entry as CVector;

        //        CExtAnimEvent animEvent = new CExtAnimEvent();
        //        animEvent.TYPE = entry.Type;
        //        animEvent = getAnimEventValues(animEvent, entryVector);
        //        list.Add(animEvent);
        //    }
        //    return list;
        //}

        //private CExtAnimEvent getAnimEventValues(CExtAnimEvent animEvent, CVector entryVector)
        //{
        //    foreach (var v in entryVector.GetEditableVariables())
        //    {
        //        Type animEventType = typeof(CExtAnimEvent);

        //        switch (v.Type)
        //        {
        //            case "CName":
        //                animEventType.GetField(v.Name).SetValue(animEvent, (entryVector.GetVariableByName(v.Name) as CName)?.Value ?? null);
        //                break;
        //            case "StringAnsi":
        //                animEventType.GetField(v.Name).SetValue(animEvent, (entryVector.GetVariableByName(v.Name) as CStringAnsi)?.val.Replace("\x00", "") ?? null);
        //                break;
        //            case "Float":
        //                animEventType.GetField(v.Name).SetValue(animEvent, (entryVector.GetVariableByName(v.Name) as CFloat)?.val ?? null);
        //                break;
        //            case "Bool":
        //                animEventType.GetField(v.Name).SetValue(animEvent, (entryVector.GetVariableByName(v.Name) as CBool)?.val ?? null);
        //                break;
        //            case "array:2,0,StringAnsi":
        //                List<string> switchesToUpdate = new List<string>();
        //                foreach (CStringAnsi switches in (entryVector.GetVariableByName(v.Name) as CArray).array)
        //                    switchesToUpdate.Add(switches.val.Replace("\x00", ""));
        //                animEventType.GetField(v.Name).SetValue(animEvent, switchesToUpdate);
        //                break;
        //            case "SEnumVariant":
        //                SEnumVariant enumVariant = new SEnumVariant();

        //                foreach (CVariable e in (entryVector.GetVariableByName(v.Name) as CVector).GetEditableVariables())
        //                {
        //                    if (e.Type == "CName")
        //                        enumVariant.enumType = (e as CName).Value;
        //                    if (e.Type == "Int32")
        //                        enumVariant.enumValue = (e as CInt32).val;
        //                }
        //                animEventType.GetField(v.Name).SetValue(animEvent, enumVariant);
        //                break;
        //            case "SMultiValue":
        //                break;
        //            case "array:2,0,handle:IEdEntitySetupEffector":
        //                break;
        //            case "array:2,0,CFistfightOpponent":
        //                break;
        //            case "EGettingItem":
        //                break;
        //            case "EItemAction":
        //                break;
        //            case "EAnimEffectAction":
        //                break;
        //            case "EItemEffectAction":
        //                break;
        //            case "EItemLatentAction":
        //                break;
        //            case "EProjectileCastPosition":
        //                break;
        //            case "EDropAction":
        //                break;
        //            case "CPreAttackEventData":
        //                CPreAttackEventData data = new CPreAttackEventData();
        //                data = getPreAttackEventData(data, (entryVector.GetVariableByName(v.Name) as CVector));
        //                animEventType.GetField(v.Name).SetValue(animEvent, data);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    return animEvent;
        //}

        //private CPreAttackEventData getPreAttackEventData(CPreAttackEventData attackData, CVector entryVector)
        //{
        //    foreach (var v in entryVector.GetEditableVariables())
        //    {
        //        Type attackDataType = typeof(CPreAttackEventData);
        //        switch (v.Type)
        //        {
        //            case "CName":
        //                attackDataType.GetField(v.Name).SetValue(attackData, (entryVector.GetVariableByName(v.Name) as CName)?.Value ?? null);
        //                break;
        //            case "Float":
        //                attackDataType.GetField(v.Name).SetValue(attackData, (entryVector.GetVariableByName(v.Name) as CFloat)?.val ?? null);
        //                break;
        //            case "Bool":
        //                attackDataType.GetField(v.Name).SetValue(attackData, (entryVector.GetVariableByName(v.Name) as CBool)?.val ?? null);
        //                break;
        //            case "Int32":
        //                attackDataType.GetField(v.Name).SetValue(attackData, (entryVector.GetVariableByName(v.Name) as CInt32)?.val ?? null);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    return attackData;
        //}


        public IAnimationBuffer getBuffer(CR2WExportWrapper chunk, CSkeleton bufferSkel, String animName = "-")
        {
            if (chunk.Type == "CAnimationBufferMultipart")
            {
                CAnimationBufferMultipart animBuffer = new CAnimationBufferMultipart();
                animBuffer.numBones = (chunk.GetVariableByName("numBones") as CUInt32).val;
                animBuffer.numFrames = (chunk.GetVariableByName("numFrames") as CUInt32).val;
                if ((chunk.GetVariableByName("numTracks") as CUInt32) != null)
                    animBuffer.numTracks = (chunk.GetVariableByName("numTracks") as CUInt32).val | 0;
                foreach (CUInt32 frame in (chunk.GetVariableByName("firstFrames") as CArray).array)
                    animBuffer.firstFrames.Add(frame.val);
                foreach (CPtr Buffer in (chunk.GetVariableByName("parts") as CArray).array)
                {
                    var a_buffer = Buffer.Reference;
                    animBuffer.parts.Add(readBuffer(a_buffer, animFile, bufferSkel));
                }
                return animBuffer;
            }
            //if (chunk.Type == "CAnimationBufferBitwiseCompressed")
            else {
                return readBuffer(chunk, animFile, bufferSkel, animName);
            }
        }

        CAnimationBufferBitwiseCompressed readBuffer(CR2WExportWrapper chunk, CR2WFile animFile, CSkeleton bufferSkel, String animName = "-")
        {
            List<Bone> bones = new List<Bone>();
            //List<Track> tracks = new List<Track>();
            List<List<uint>> positionsKeyframes = new List<List<uint>>();
            List<List<uint>> orientKeyframes = new List<List<uint>>();
            List<List<uint>> scalesKeyframes = new List<List<uint>>();
            List<List<Vector3Df>> positions = new List<List<Vector3Df>>();
            List<List<Quaternion>> orientations = new List<List<Quaternion>>();
            List<List<Vector3Df>> orientationsEuler = new List<List<Vector3Df>>();
            List<List<Vector3Df>> scales = new List<List<Vector3Df>>();
            List<CVector> currentBones = new List<CVector>();
            //List<CVector> currentTracks = new List<CVector>();
            //List<List<float>> trackFrames = new List<List<float>>();

            CAnimationBufferBitwiseCompressed AnimationBuffer = new CAnimationBufferBitwiseCompressed();
            string dataAddrVar = "dataAddr";
            uint keyFrame = 0;
            byte[] data;
            //data = (chunk.GetVariableByName("fallbackData") as CByteArray).Bytes;

            uint numFrames = (chunk.GetVariableByName("numFrames") as CUInt32).val;
            float animDuration = (chunk.GetVariableByName("duration") as CFloat)?.val ?? 1.0f;

            AnimationBuffer.duration = animDuration;
            AnimationBuffer.numFrames = numFrames;
            AnimationBuffer.dt = (chunk.GetVariableByName("dt") as CFloat)?.val ?? 0.03333333f;

            var deferredData = chunk.GetVariableByName("deferredData") as CInt16;
            var streamingOption = (chunk.GetVariableByName("streamingOption") as CVariable);
            Console.WriteLine("deferredData is null: " + (deferredData != null) + ", val: " + deferredData.val);

            if (deferredData != null && deferredData.val != 0)
                if (streamingOption != null && streamingOption.ToString() == "ABSO_PartiallyStreamable")
                    data = ConvertAnimation.Combine((chunk.GetVariableByName("data") as CByteArray).Bytes,
                    File.ReadAllBytes(animFile.FileName + "." + deferredData.val + ".buffer"));
                else
                    data = File.ReadAllBytes(animFile.FileName + "." + deferredData.val + ".buffer");
            else
                data = (chunk.GetVariableByName("data") as CByteArray).Bytes;

            Console.WriteLine("Animation: " + animName + ", data bytes = " + data.Count());
            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader br = new BinaryReader(ms))
            {
                if (chunk.GetVariableByName("tracks") != null)
                {
                    AnimationBuffer.tracks = new List<Track>();
                    List<CVariable> fileTracks = (chunk.GetVariableByName("tracks") as CArray).array;
                    if (fileTracks.Count > 7)
                    {

                    }
                    for (int i = 0; i < fileTracks.Count; i++)
                    {
                        CVector trackFile = fileTracks[i] as CVector;
                        List<float> trackFrames = new List<float>();
                        CUInt32 stream_position = trackFile.GetVariableByName(dataAddrVar) as CUInt32;
                        //if (position != null)
                        //{
                        Track track = new Track();
                        int frameNumFrames = (trackFile.GetVariableByName("numFrames") as CUInt16).val;
                        track.index = i;
                        track.trackName = bufferSkel.tracks[i];
                        CInt8 compression = trackFile.GetVariableByName("compression") as CInt8;
                        CFloat dt = trackFile.GetVariableByName("dt") as CFloat;
                        CUInt32 dataAddr = trackFile.GetVariableByName("dataAddr") as CUInt32;
                        CUInt32 dataAddrFallback = trackFile.GetVariableByName("dataAddrFallback") as CUInt32;
                        CUInt16 numframes = trackFile.GetVariableByName("numFrames") as CUInt16;
                        if (compression != null)
                            track.compression = compression.val;
                        if (dt != null)
                            track.dt = dt.val;
                        if (dataAddr != null)
                            track.dataAddr = dataAddr.val;
                        if (dataAddrFallback != null)
                            track.dataAddrFallback = dataAddrFallback.val;
                        if (numframes != null)
                            track.numFrames = numframes.val;

                        if (stream_position != null)
                        {
                            br.BaseStream.Position = stream_position.val;
                            for (uint j = 0; j < frameNumFrames; j++)
                            {
                                track.trackFrames.Add(ReadCompressFloat(br, track.compression));
                            }
                        }
                        AnimationBuffer.tracks.Add(track);
                        //}
                    }
                }
                bool shownMsg = false;
                foreach (CVector bone in (chunk.GetVariableByName("bones") as CArray).array)
                {
                    List<uint> currkeyframe = new List<uint>();
                    List<Quaternion> currorient = new List<Quaternion>();
                    List<Vector3Df> currorientEuler = new List<Vector3Df>();
                    currentBones.Add(bone);
                    var addr = (bone.GetVariableByName("orientation") as CVector).GetVariableByName(dataAddrVar) as CUInt32;
                    if (addr != null)
                        br.BaseStream.Position = addr.val;
                    else
                        br.BaseStream.Position = 0;

                    int orientNumFrames = ((bone.GetVariableByName("orientation") as CVector).GetVariableByName("numFrames") as CUInt16).val;


                    string cm = chunk.GetVariableByName("orientationCompressionMethod")?.ToString() ?? "";
                    if (!shownMsg)
                    {
                        Console.WriteLine("compression = [" + cm + "]");
                        shownMsg = true;
                    }

                    if (cm == "ABOCM_PackIn48bitsW") { 
                        for (uint idx = 0; idx < orientNumFrames; idx++)
                        {
                            keyFrame = idx;
                            currkeyframe.Add(keyFrame);
                            byte[] odata = br.ReadBytes(6);
                            ulong bits = 0;
                            if (odata.Count() == 6)
                                bits = (ulong)odata[0] << 40 | (ulong)odata[1] << 32 | (ulong)odata[2] << 24 | (ulong)odata[3] << 16 | (ulong)odata[4] << 8 | odata[5];

                            ushort[] orients = new ushort[4];
                            float[] quart = new float[4];
                            orients[0] = (ushort)((bits & 0x0000FFF000000000) >> 36);
                            orients[1] = (ushort)((bits & 0x0000000FFF000000) >> 24);
                            orients[2] = (ushort)((bits & 0x0000000000FFF000) >> 12);
                            orients[3] = (ushort)((bits & 0x0000000000000FFF));

                            for (int i = 0; i < orients.Length; i++)
                            {
                                float fVal = (2047.0f - orients[i]) * (1 / 2048.0f);
                                quart[i] = fVal;
                            }
                            quart[3] = -quart[3];

                            Quaternion orientation = new Quaternion(quart[0], quart[1], quart[2], quart[3]).Normalize();
                            if (float.IsNaN(orientation.X) || float.IsNaN(orientation.Y) || float.IsNaN(orientation.Z) || float.IsNaN(orientation.W))
                            {
                                Console.WriteLine("NaN! orients: " + orients + ", quart: " + quart);
                            }
                            currorient.Add(orientation);
                            Vector3Df euler = orientation.ToEuler();
                            currorientEuler.Add(euler);
                            //Console.WriteLine("Euler : x=%f, y=%f, z=%f", euler.X, euler.Y, euler.Z);
                        }
                    }
                    else if (cm == "ABOCM_AsFloat_XYZSignedWInLastBit")
                    {
                        for (uint idx = 0; idx < orientNumFrames; idx++)
                        {
                            keyFrame = idx;
                            currkeyframe.Add(keyFrame);

                            float x = br.ReadSingle();
                            float y = br.ReadSingle();
                            float z = br.ReadSingle();

                            if (float.IsNaN(x))
                                x = 0.0f;
                            if (float.IsNaN(y))
                                y = 0.0f;
                            if (float.IsNaN(z))
                                z = 0.0f;

                            // Yeah, a value stored inside float..
                            // 1st bit doesn't affect float much, used here to store W sign
                            bool signW = (BitConverter.GetBytes(z).First() & 1) > 0;

                            float minScalar = Math.Min(x * x + y * y + z * z, 1.0f);
                            float w = (float)Math.Sqrt(1.0f - minScalar);
                            if (!signW)
                            {
                                w = -w;
                            }
                            //Console.WriteLine("X = " + x + ", Y = " + y + ", Z = " + z + ", w = " + w);

                            Quaternion orientation = new Quaternion(x, y, z, w).Normalize();
                            /*if (float.IsNaN(orientation.X) || float.IsNaN(orientation.Y) || float.IsNaN(orientation.Z) || float.IsNaN(orientation.W))
                            {
                                Console.WriteLine("NaN! orients: " + x + "," + y + "," + z + "," + w);
                            }*/
                            currorient.Add(orientation);
                            Vector3Df euler = orientation.ToEuler();
                            currorientEuler.Add(euler);
                        }
                    }
                    else //cutscenes are ABOCM_PackIn64bitsW
                    {
                        if (!shownMsg)
                        {
                            //MessageBox.Show("Used cm: [" + cm + "]", "Unknown CompressionMethod!");
                            shownMsg = true;
                        }
                        
                        for (uint idx = 0; idx < orientNumFrames; idx++)
                        {
                            keyFrame = idx;
                            //keyFrame += numFrames;
                            currkeyframe.Add(keyFrame);
                            ushort[] orients = new ushort[4];
                            float[] quart = new float[4];
                            orients[0] = br.ReadUInt16();
                            orients[1] = br.ReadUInt16();
                            orients[2] = br.ReadUInt16();
                            orients[3] = br.ReadUInt16();

                            for (int i = 0; i < orients.Length; i++)
                            {
                                float fVal = (32768.0f - orients[i]) * (1 / 32767.0f);
                                quart[i] = fVal;
                            }
                            quart[3] = -quart[3];


                            Quaternion orientation = new Quaternion(quart[0], quart[1], quart[2], quart[3]).Normalize();
                            currorient.Add(orientation);
                            Vector3Df euler = orientation.ToEuler();
                            if ( float.IsNaN(orientation.X) || float.IsNaN(orientation.Y) || float.IsNaN(orientation.Z) || float.IsNaN(orientation.W) )
                            {
                                Console.WriteLine("NaN! orients: " + orients + ", quart: " + quart);
                            }
                            currorientEuler.Add(euler);
                        }
                    }

                    orientKeyframes.Add(currkeyframe);
                    orientations.Add(currorient);
                    orientationsEuler.Add(currorientEuler);

                    // TODO: Refactor
                    List<Vector3Df> currposition = new List<Vector3Df>();
                    currkeyframe = new List<uint>();
                    int compression = 0;
                    var compr = (bone.GetVariableByName("position") as CVector).GetVariableByName("compression") as CInt8;
                    if (compr != null)
                        compression = compr.val;
                    addr = (bone.GetVariableByName("position") as CVector).GetVariableByName(dataAddrVar) as CUInt32;
                    if (addr != null)
                        br.BaseStream.Position = addr.val;
                    else
                        br.BaseStream.Position = 0;
                    var posNumFrames = ((bone.GetVariableByName("position") as CVector).GetVariableByName("numFrames") as CUInt16).val;
                    for (uint idx = 0; idx < posNumFrames; idx++)
                    {
                        keyFrame = idx;
                        //keyFrame += numFrames;
                        currkeyframe.Add(keyFrame);
                        var vec = new CVector3D();
                        vec.Read(br, compression);
                        Vector3Df pos = new Vector3Df(vec.x.val, vec.y.val, vec.z.val);
                        currposition.Add(pos);
                    }
                    positionsKeyframes.Add(currkeyframe);
                    positions.Add(currposition);

                    List<Vector3Df> currscale = new List<Vector3Df>();
                    currkeyframe = new List<uint>();
                    compression = 0;
                    compr = (bone.GetVariableByName("scale") as CVector).GetVariableByName("compression") as CInt8;
                    if (compr != null)
                        compression = compr.val;
                    addr = (bone.GetVariableByName("scale") as CVector).GetVariableByName(dataAddrVar) as CUInt32;
                    if (addr != null)
                        br.BaseStream.Position = addr.val;
                    else
                        br.BaseStream.Position = 0;
                    var scaleNumFrames = ((bone.GetVariableByName("scale") as CVector).GetVariableByName("numFrames") as CUInt16).val;
                    for (uint idx = 0; idx < scaleNumFrames; idx++)
                    {
                        keyFrame = idx;
                        //keyFrame += numFrames;
                        currkeyframe.Add(keyFrame);
                        var vec = new CVector3D();
                        vec.Read(br, compression);
                        Vector3Df scale = new Vector3Df(vec.x.val, vec.y.val, vec.z.val);
                        currscale.Add(scale);
                    }
                    scalesKeyframes.Add(currkeyframe);
                    scales.Add(currscale);
                }
            }


            bones.Clear();


            Vector3Df current;
            for (int i = 0; i < orientations.Count; i++)
            {
                Bone bone = new Bone();
                bone.index = i;
                //bone.BoneName = i.ToString();
                bone.BoneName = bufferSkel.names[i];
                bones.Add(bone);

                CVector animBone = currentBones[i];

                CVector positionVar = animBone.GetVariableByName("position") as CVector;
                CFloat dtPos = positionVar.GetVariableByName("dt") as CFloat;
                CUInt32 dataAddrPos = positionVar.GetVariableByName("dataAddr") as CUInt32;
                CUInt32 dataAddrFallbackPos = positionVar.GetVariableByName("dataAddrFallback") as CUInt32;
                CUInt16 numframesPos = positionVar.GetVariableByName("numFrames") as CUInt16;
                if (dtPos != null)
                    bone.position_dt = dtPos.val;
                if (dataAddrPos != null)
                    bone.position_dataAddr = dataAddrPos.val;
                if (dataAddrFallbackPos != null)
                    bone.position_dataAddrFallback = dataAddrFallbackPos.val;
                if (numframesPos != null)
                    bone.position_numFrames = numframesPos.val;

                CVector orientationVar = animBone.GetVariableByName("orientation") as CVector;
                CFloat dtRot = orientationVar.GetVariableByName("dt") as CFloat;
                CUInt32 dataAddrRot = orientationVar.GetVariableByName("dataAddr") as CUInt32;
                CUInt32 dataAddrFallbackRot = orientationVar.GetVariableByName("dataAddrFallback") as CUInt32;
                CUInt16 numframesRot = orientationVar.GetVariableByName("numFrames") as CUInt16;
                if (dtRot != null)
                    bone.rotation_dt = dtRot.val;
                if (dataAddrRot != null)
                    bone.rotation_dataAddr = dataAddrRot.val;
                if (dataAddrFallbackRot != null)
                    bone.rotation_dataAddrFallback = dataAddrFallbackRot.val;
                if (numframesRot != null)
                    bone.rotation_numFrames = numframesRot.val;

                CVector scaleVar = animBone.GetVariableByName("scale") as CVector;
                CFloat dtScale = scaleVar.GetVariableByName("dt") as CFloat;
                CUInt32 dataAddrScale = scaleVar.GetVariableByName("dataAddr") as CUInt32;
                CUInt32 dataAddrFallbackScale = scaleVar.GetVariableByName("dataAddrFallback") as CUInt32;
                CUInt16 numframesScale = scaleVar.GetVariableByName("numFrames") as CUInt16;
                if (dtScale != null)
                    bone.scale_dt = dtScale.val;
                if (dataAddrScale != null)
                    bone.scale_dataAddr = dataAddrScale.val;
                if (dataAddrFallbackScale != null)
                    bone.scale_dataAddrFallback = dataAddrFallbackScale.val;
                if (numframesScale != null)
                    bone.scale_numFrames = numframesScale.val;

                for (int j = 0; j < positions[i].Count; j++)
                {
                    bone.positionFrames.Add(new Vector(positions[i][j].X, positions[i][j].Y, positions[i][j].Z));
                }

                for (int j = 0; j < orientations[i].Count; j++)
                {
                    current = orientationsEuler[i][j];
                    bone.rotationFrames.Add(orientations[i][j]);
                }

                for (int j = 0; j < scales[i].Count; j++)
                {
                    bone.scaleFrames.Add(new Vector(scales[i][j].X, scales[i][j].Y, scales[i][j].Z));
                }
            }
            AnimationBuffer.bones = bones;
            return AnimationBuffer;
        }

        public void SaveJson(string filename)
        {
            ReadSelectedAnimation();
            //open file stream
            using (StreamWriter file = File.CreateText(filename))
            {

                JsonConverter[] converters = { new CVariableConverter(animFile) };
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new CVariableConverter(animFile));
                //serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Formatting = Formatting.Indented;
                //serialize object directly into file stream
                serializer.Serialize(file, SkeletalAnimationSet.animations[0]);
            }
        }

        public void SaveSet(string filename)
        {
            //open file stream
            using (StreamWriter file = File.CreateText(filename))
            {
                JsonConverter[] converters = { new CVariableConverter(animFile) };
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new CVariableConverter(animFile));
                //serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Formatting = Formatting.Indented;
                //serialize object directly into file stream
                serializer.Serialize(file, SkeletalAnimationSet);
            }
        }

        //public class CVariableConverter : JsonConverter
        //{
        //    public override bool CanConvert(Type objectType)
        //    {
        //        return (objectType == typeof(List<CVariable>));
        //    }

        //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        //    {
        //        return null;
        //    }

        //    public override bool CanWrite
        //    {
        //        get { return true; }
        //    }

        //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //    {
        //        var varlist = (IList<CVariable>)value;

        //        //var jArray = new JArray();

        //        writer.WriteStartArray();
        //        foreach (var v in varlist)
        //        {
        //            v.SerializeToJson(writer);
        //            //jArray.Add(GetObject(v));
        //            //jArray.Add(v);
        //        }

        //        writer.WriteEndArray();
        //        //jArray.WriteTo(writer);

        //        //JToken t = JToken.FromObject(value);

        //        //t.WriteTo(writer);
        //        //throw new NotImplementedException();
        //    }

        //    protected static JObject GetObject(CVariable entryVector)
        //    {
        //        List<CVariable> animEvent = new List<CVariable>();
        //        List<CVariable> subList = new List<CVariable>();
        //        foreach (var v in entryVector.GetEditableVariables())
        //        {
        //            animEvent.Add(v as CVariable);
        //            if (v.GetEditableVariables() != null)
        //            {
        //            }
        //        }

        //        return JObject.FromObject(new { animEvent });
        //    }
        //}
    }
}
