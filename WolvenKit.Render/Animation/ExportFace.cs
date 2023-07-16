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
using WolvenKit.Render.Animation;

namespace WolvenKit.Render
{

    public class ExportFace
    {

        public MimicPose exportData = new MimicPose();

        public CMimicFace MimicFace = new CMimicFace();

        private CR2WFile w3FaceFile;
        public CR2WFile W3FaceFile
        {
            get { return w3FaceFile; }
            set { w3FaceFile = value; }
        }

        public ExportFace()
        {
        }

        
        Dictionary<string, uint> dict = new Dictionary<string, uint>();
        public void LoadData(CR2WFile facFile)
        {
            CommonData cdata = new CommonData();
            Rig exportMimicSkeleton = new Rig(cdata);
            Rig exportFloatTrackSkeleton = new Rig(cdata);
            exportMimicSkeleton.LoadData(facFile, 1);
            exportFloatTrackSkeleton.LoadData(facFile, 2);
            MimicFace.mimicSkeleton = exportMimicSkeleton.meshSkeleton;
            MimicFace.floatTrackSkeleton = exportFloatTrackSkeleton.meshSkeleton;

            W3FaceFile = facFile;
            //int count = Read(br, 0);
            var chunkMimicFace = W3FaceFile.chunks[0];
            var mimicSkeleton = W3FaceFile.chunks[1];
            var floatTrackSkeleton = W3FaceFile.chunks[2];

            var mimicSkeletonBones = mimicSkeleton.GetVariableByName("bones") as CArray;
            uint nbBones = (uint)mimicSkeletonBones.array.Count;
            foreach (CVector bone in mimicSkeletonBones)
            {
                var boneName = bone.variables.GetVariableByName("nameAsCName") as CName;
                //meshSkeleton.names.Add(boneName.Value);
            }

            //convert mapping into bone array
            var mimicMapping = (chunkMimicFace.GetVariableByName("mapping") as CArray).array;
            //ie. mimicMapping[0] = "uv_center_slide2"

            //give each mapping a name
            var tracks = (floatTrackSkeleton.GetVariableByName("tracks") as CArray).array;


            var mimicPoses = (chunkMimicFace.GetVariableByName("mimicPoses") as CArray).array;
            //save poses into

            for (int i = 0; i < mimicPoses.Count(); i++)
            {
                MimicPose pose = new MimicPose();
                string trackanme = ((tracks[i] as CVector).GetVariableByName("nameAsCName") as CName).Value;
                pose.name = trackanme;
                pose.numFrames = 1;
                var mimicbones = (mimicPoses[i] as CArray).array;
                List<Bone> exportBones = new List<Bone>();
                for (int j = 0; j < mimicbones.Count(); j++)
                {
                    Bone myBone = new Bone();
                    //finde the bone name using mapping
                    int map = (mimicMapping[j] as CInt32).val;
                    if (map < 0)
                    {
                        continue;
                    }
                    var BoneName = (mimicSkeletonBones.array[map] as CVector).GetVariableByName("nameAsCName") as CName;

                    myBone.BoneName = BoneName.Value; // find the mapping
                    float x =  ((mimicbones[j] as CEngineQsTransform).x as CFloat).val;
                    float y = ((mimicbones[j] as CEngineQsTransform).y as CFloat).val;
                    float z = ((mimicbones[j] as CEngineQsTransform).z as CFloat).val;
                    float pitch = ((mimicbones[j] as CEngineQsTransform).pitch as CFloat).val;
                    float yaw = ((mimicbones[j] as CEngineQsTransform).yaw as CFloat).val;
                    float roll = ((mimicbones[j] as CEngineQsTransform).roll as CFloat).val;
                    float w = ((mimicbones[j] as CEngineQsTransform).w as CFloat).val;
                    float scale_x = ((mimicbones[j] as CEngineQsTransform).scale_x as CFloat).val;
                    float scale_y = ((mimicbones[j] as CEngineQsTransform).scale_y as CFloat).val;
                    float scale_z = ((mimicbones[j] as CEngineQsTransform).scale_z as CFloat).val;
                    myBone.positionFrames.Add(new Vector(x, y, z));
                    myBone.rotationFrames.Add(new Quaternion(pitch, yaw, roll, w));
                    myBone.scaleFrames.Add(new Vector(scale_x, scale_y, scale_z));
                    exportBones.Add(myBone);

                }
                pose.bones = exportBones;
                MimicFace.mimicPoses.Add(pose);
            }
            //SaveJson(@"D:\w3.modding\FACE_ANIMATION\face_anims_test_01.json");
        }

        public void SaveJson(string filename)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Error = (serializer, err) =>
            {
                err.ErrorContext.Handled = true;
            };

            var jsonResolver = new IgnorableSerializerContractResolver();
            // ignore single property
            jsonResolver.Ignore(typeof(Matrix));
            jsonResolver.Ignore(typeof(Vector3Df), "SphericalCoordinateAngles");
            jsonResolver.Ignore(typeof(Vector3Df), "HorizontalAngle");
            jsonResolver.Ignore(typeof(Vector3Df), "LengthSQ");
            jsonResolver.Ignore(typeof(Vector3Df), "Length");

            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.ContractResolver = jsonResolver;
            //open file stream
            using (StreamWriter file = File.CreateText(filename))
            {
                string meshSkeletonJson = JsonConvert.SerializeObject(MimicFace, Formatting.Indented, settings);
                file.Write(meshSkeletonJson);
            }
        }
        
    }

}