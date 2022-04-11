using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolvenKit.App;
using WolvenKit.Bundles;
using WolvenKit.Common;
using WolvenKit.Common.Services;
using WolvenKit.Common.Wcc;
using WolvenKit.CR2W;
using WolvenKit.CR2W.Types;

namespace WolvenKit.Render.Animation
{
    public class EntityGenerator
    {
        //public WccHelper WccHelper { get; set; }
        public LoggerService Logger { get; set; }
        public EntFile entJson { get; set; }
        public BundleManager manager { get; private set; }
        public string depo { get; private set; }
        public WccLite wccHelper { get; private set; }
        public string entityFilename { get; private set; }

        public EntityGenerator()
        {
            Logger = new LoggerService();
            depo = @"D:\Witcher_uncooked_clean\raw_ent_TEST\";
            //WccHelper = new WccHelper(wccpath, Logger);
        }

        //public async Task SaveEntAsync(string filename)
        public void SaveEntAsync(string filename = null)
        {
            //private bool AddToMod(WitcherListViewItem in frmMain
            //extract and convert all the relevent files to the raw directory
            //The will be fbx files, converted fbx files, w2rig.json etc.
            //save the ent.json at top level for importing into maya

            if (entJson.MovingPhysicalAgentComponent.skeleton != null)
            {
                ExportRig(entJson.MovingPhysicalAgentComponent.skeleton);
            }
            else
            {
                entJson.MovingPhysicalAgentComponent.skeleton = "characters\\base_entities\\man_base\\man_base.w2rig";
                ExportRig(entJson.MovingPhysicalAgentComponent.skeleton);
            }

            foreach (EntityAppearance appearance in entJson.appearances)
            {
                foreach (ModelEnt template in appearance.includedTemplates)
                    foreach (var chunk in template.chunks)
                        switch (chunk.type)
                        {
                            //case "CMeshComponent": if(!entJson.fbx_list.Contains((chunk as CMeshComponent).mesh)) entJson.fbx_list.Add((chunk as CMeshComponent).mesh); break;
                            //case "CMeshComponent": await ExportMeshAsync((chunk as CMeshComponent).mesh); break;
                            case "CMimicComponent": ExportFace((chunk as CMimicComponent).mimicFace); break;
                            case "CAnimDangleBufferComponent": ExportRig((chunk as CAnimDangleBufferComponent).skeleton); break;
                            case "CAnimDangleConstraint_Dyng": ExportRig((chunk as CAnimDangleConstraint_Dyng).dyng, 1); break;
                            case "CAnimDangleConstraint_Breast": ExportRig((chunk as CAnimDangleConstraint_Breast).skeleton); break;
                            case "CAnimDangleConstraint_Collar": ExportRig((chunk as CAnimDangleConstraint_Collar).skeleton); break;
                            case "CAnimDangleConstraint_Dress": ExportRig((chunk as CAnimDangleConstraint_Dress).skeleton); break;
                            case "CAnimDangleConstraint_Hood": ExportRig((chunk as CAnimDangleConstraint_Hood).skeleton); break;
                            case "CAnimDangleConstraint_Hinge": ExportRig((chunk as CAnimDangleConstraint_Hinge).skeleton); break;
                            case "CAnimDangleConstraint_Pusher": ExportRig((chunk as CAnimDangleConstraint_Pusher).skeleton); break;
                        }
            }

            //if (entJson.includedTemplates != null && entJson.includedTemplates.Count() > 0)
            //{

            //} else {
            //    foreach (var chunk in entJson.staticMeshes.chunks)
            //        if (chunk.type == "CStaticMeshComponent")
            //            ExportMeshAsync((chunk as CStaticMeshComponent).mesh);
            //}

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Error = (serializer, err) =>
            {
                err.ErrorContext.Handled = true;
            };
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.NullValueHandling = NullValueHandling.Ignore;

            //Save a copy of the entity to provided filename
            if (filename != null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
                using (StreamWriter file = File.CreateText(filename))
                {
                    string json = JsonConvert.SerializeObject(entJson, Formatting.Indented, settings);
                    file.Write(json);
                }
            }

            //save the entity in the depo
            string saveFile = depo + entityFilename + ".json";
            Directory.CreateDirectory(Path.GetDirectoryName(saveFile));
            using (StreamWriter file = File.CreateText(saveFile))
            {
                string json = JsonConvert.SerializeObject(entJson, Formatting.Indented, settings);
                file.Write(json);
            }
        }

        async Task ExportMeshAsync(string mesh)
        {
            string importwdir = "E:/w3.modding/modkit/r4data"; // MainController.DepotDir ??
            string outfile = depo + Path.ChangeExtension(mesh, ".fbx");
            string wccPath = "E:/w3.modding/modkit/bin/x64/wcc_lite.exe";
            string args1 = "export -depot=\"{0}\" -file=\"{1}\" -out=\"{2}\" fbx=2016";
            string args = string.Format(args1, importwdir, mesh, outfile);
            try
            {
                var export = new Wcc_lite.export()
                {
                    File = mesh,
                    Out = outfile,
                    Depot = importwdir,
                    Fbx = "2016",
                };
                await Task.Run(() => wccHelper.RunCommand(export));
                //using (Process process = new Process())
                //{
                //    process.StartInfo.FileName = wccPath;
                //    process.StartInfo.WorkingDirectory = Path.GetDirectoryName(wccPath);
                //    process.StartInfo.Arguments = args;
                //    process.StartInfo.UseShellExecute = false;
                //    process.StartInfo.RedirectStandardOutput = true;
                //    process.StartInfo.CreateNoWindow = true;
                //    process.Start();
                //    Console.WriteLine(process.StandardOutput.ReadToEnd());
                //    //process.BeginOutputReadLine();
                //    process.WaitForExit();
                //}

                using (Process process = new Process())
                {
                    string BLENDER_276 = @"F:\witcher3\blender-2.76-windows64\blender.exe";
                    string BLEND_PATH = @"F:\witcher3\blender-2.76-windows64\SCRIPTS\working.blend";
                    string IMPORT_SCRIPT = @"F:\witcher3\blender-2.76-windows64\SCRIPTS\import_FBX.py";
                    string MODEL_PATH = outfile;

                    string blender_args = "{0} --background --python {1} -- {2}";
                    string args_blend = string.Format(blender_args, BLEND_PATH, IMPORT_SCRIPT, MODEL_PATH);
                    process.StartInfo.FileName = BLENDER_276;
                    process.StartInfo.WorkingDirectory = Path.GetDirectoryName(wccPath);
                    process.StartInfo.Arguments = args_blend;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    Console.WriteLine(process.StandardOutput.ReadToEnd());
                    //process.BeginOutputReadLine();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString() + "\n", Logtype.Error);
            }

            //find mesh in the uncooked depot
            //use fbx command
            //convert it with blender
            //use the generated ent file to load all models and skeletons
            //throw new NotImplementedException();
        }

        private void ExportRig(string skeleton, int sindex = 0)
        {
            Rig exportRig = GetRig(skeleton, sindex);

            string saveFile = depo + skeleton + ".json";
            if (File.Exists(saveFile))
            {
                //File.Delete(saveFile);
                Console.WriteLine(saveFile + " exists!");
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(saveFile));
                exportRig.SaveRig(saveFile);
            }
        }

        private Rig GetRig(string templateFilename, int sindex)
        {
            var archives = manager.FileList.Where(x => x.Name == templateFilename).Select(y => new KeyValuePair<string, IWitcherFile>(y.Bundle.FileName, y));
            BundleItem meshEntityData = archives.FirstOrDefault().Value as BundleItem;
            MemoryStream ms = new MemoryStream();
            meshEntityData.Extract(ms);
            byte[] dataRig;
            dataRig = ms.ToArray();
            using (MemoryStream ms2 = new MemoryStream(dataRig))
            using (BinaryReader br2 = new BinaryReader(ms2))
            {

                CR2WFile entity = new CR2WFile(br2);
                CommonData cdata = new CommonData();
                Rig exportRig = new Rig(cdata);
                exportRig.LoadData(entity, sindex);
                return exportRig;
            }
        }

        private void ExportFace(string faceFilename)
        {
            ExportFace exportRig = GetFace(faceFilename);

            string saveFile = depo + faceFilename + ".json";
            if (File.Exists(saveFile))
            {
                //File.Delete(saveFile);
                Console.WriteLine(saveFile + " exists!");
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(saveFile));
                exportRig.SaveJson(saveFile);
            }
        }
        private ExportFace GetFace(string templateFilename)
        {
            var archives = manager.FileList.Where(x => x.Name == templateFilename).Select(y => new KeyValuePair<string, IWitcherFile>(y.Bundle.FileName, y));
            BundleItem meshEntityData = archives.FirstOrDefault().Value as BundleItem;
            MemoryStream ms = new MemoryStream();
            meshEntityData.Extract(ms);
            byte[] dataRig;
            dataRig = ms.ToArray();
            using (MemoryStream ms2 = new MemoryStream(dataRig))
            using (BinaryReader br2 = new BinaryReader(ms2))
            {

                CR2WFile entity = new CR2WFile(br2);
                ExportFace face = new ExportFace();
                face.LoadData(entity);
                return face;
            }
        }

        public void readCEntityTemplate(string templateFilename, bool fromBundle = false)
        {
            entityFilename = templateFilename;
            byte[] dataRig = new byte[0];
            if (fromBundle)
            {
                //check the local mod before using bundle manager thing
                if (manager.Items.Any(x => x.Value.Any(y => y.Name == templateFilename)))
                {
                    var archives = manager.FileList.Where(x => x.Name == templateFilename).Select(y => new KeyValuePair<string, IWitcherFile>(y.Bundle.FileName, y));
                    BundleItem meshEntityData = archives.FirstOrDefault().Value as BundleItem;
                    MemoryStream ms = new MemoryStream();
                    meshEntityData.Extract(ms);
                    dataRig = ms.ToArray();
                }
            }
            else
            {
                dataRig = File.ReadAllBytes(templateFilename);
            }

            using (MemoryStream ms = new MemoryStream(dataRig))
            using (BinaryReader br = new BinaryReader(ms))
            {
                bool hasCMovingPhysicalAgentComponent = false;
                entJson = new EntFile();
                entJson.name = Path.GetFileNameWithoutExtension(templateFilename);//"shani_name";
                CR2WFile entity = new CR2WFile(br);
                ModelEnt new_mesh = new ModelEnt("staticMeshes", "staticMeshes");
                if (entity != null)
                    foreach (var chunk in entity.chunks)
                    {
                        if (chunk.Type == "CEntityTemplate" && chunk.GetVariableByName("appearances") != null)
                        {
                            List<CVariable> appearances = (chunk.GetVariableByName("appearances") as CArray).array;
                            int appSelect = 0;

                            foreach (CVector appearance in appearances)
                            {
                                string name = appearance.GetVariableByName("name").ToString();
                                EntityAppearance currentApp = new EntityAppearance();
                                currentApp.name = name;
                                //currentApp.includedTemplates
                                entJson.appearances.Add(currentApp);
                                if (appearance.GetVariableByName("includedTemplates") != null)
                                {
                                    List<CVariable> includedTemplates = (appearance.GetVariableByName("includedTemplates") as CArray).array;
                                    foreach (CHandle entryTemplate in includedTemplates)
                                    {
                                        var entry = entryTemplate.ToString().Split(' ')[1];
                                        currentApp.includedTemplates.Add(ReadMeshCEntityTemplate(entry));
                                    }
                                }
                                else
                                {
                                    //some "invisible" appearances have no entities attached
                                    //throw new Exception("Entity has no includedTemplates");
                                    //GetFace(@"characters\models\geralt\head\model\h_01_mg__geralt.w3fac");
                                }
                            }
                        }
                        else if (chunk.Type == "CEntity") //entity is 
                        {
                            foreach (CHandle staticChunkPtr in (chunk.GetVariableByName("components") as CArray).array)
                            {
                                CR2WExportWrapper staticChunk = staticChunkPtr.Reference;
                                if (staticChunk.Type == "CStaticMeshComponent")
                                {
                                    string mesh = (staticChunk.GetVariableByName("mesh") as CHandle).ToString().Split(' ')[1];
                                    new_mesh.chunks.Add(new CStaticMeshComponent(mesh));
                                    new_mesh.chunks.Last().refChunk = staticChunk.cr2w;
                                    new_mesh.chunks.Last().type = staticChunk.Type;
                                    new_mesh.chunks.Last().chunkIndex = staticChunk.ChunkIndex;
                                }
                                if (staticChunk.Type == "CMeshComponent")
                                {
                                    string mesh = (staticChunk.GetVariableByName("mesh") as CHandle).ToString().Split(' ')[1];
                                    new_mesh.chunks.Add(new CMeshComponent(mesh));
                                    new_mesh.chunks.Last().refChunk = staticChunk.cr2w;
                                    new_mesh.chunks.Last().type = staticChunk.Type;
                                    new_mesh.chunks.Last().chunkIndex = staticChunk.ChunkIndex;
                                }
                                if (staticChunk.Type == "CAnimatedComponent")
                                {
                                    string name = (staticChunk.GetVariableByName("name") as CString).val;
                                    string skeleton = (staticChunk.GetVariableByName("skeleton") as CHandle).ToString().Split(' ')[1];
                                    new_mesh.chunks.Add(new CAnimatedComponent(name, skeleton));
                                    new_mesh.chunks.Last().refChunk = staticChunk.cr2w;
                                    new_mesh.chunks.Last().type = staticChunk.Type;
                                    new_mesh.chunks.Last().chunkIndex = staticChunk.ChunkIndex;
                                }

                            }
                        }
                        else if(chunk.Type == "CHardAttachment")
                        {
                            if (chunk.GetVariableByName("parentSlot") != null)
                            {
                                int parent = (chunk.GetVariableByName("parent") as CPtr).Reference.ChunkIndex;
                                int child = (chunk.GetVariableByName("child") as CPtr).Reference.ChunkIndex;
                                int parentSlot = (chunk.GetVariableByName("parentSlot") as CPtr).Reference.ChunkIndex;
                                string parentSlotName = chunk.GetVariableByName("parentSlotName").ToString();
                                new_mesh.chunks.Add(new CHardAttachment(parent, child, parentSlot, parentSlotName));
                                new_mesh.chunks.Last().refChunk = chunk.cr2w;
                                new_mesh.chunks.Last().type = chunk.Type;
                                new_mesh.chunks.Last().chunkIndex = chunk.ChunkIndex;
                            }
                        }
                        else if(chunk.Type == "CSkeletonBoneSlot")
                        {
                            uint boneIndex = (chunk.GetVariableByName("boneIndex") as CUInt32).val;
                            new_mesh.chunks.Add(new CSkeletonBoneSlot(boneIndex));
                            new_mesh.chunks.Last().refChunk = chunk.cr2w;
                            new_mesh.chunks.Last().type = chunk.Type;
                            new_mesh.chunks.Last().chunkIndex = chunk.ChunkIndex;
                        }
                        else if(chunk.Type == "CMovingPhysicalAgentComponent" && chunk.GetVariableByName("skeleton") != null)
                        {
                            string animation_rig = (chunk.GetVariableByName("skeleton") as CHandle).ToString().Split(' ')[1];
                            entJson.MovingPhysicalAgentComponent.skeleton = animation_rig;
                            entJson.animation_rig_object = GetRig(animation_rig, 0);
                            hasCMovingPhysicalAgentComponent = true;
                        }
                        else if(chunk.Type == "CAnimatedComponent" && !hasCMovingPhysicalAgentComponent) {
                            string animation_rig = (chunk.GetVariableByName("skeleton") as CHandle).ToString().Split(' ')[1];
                            entJson.MovingPhysicalAgentComponent.skeleton = animation_rig;
                            entJson.animation_rig_object = GetRig(animation_rig, 0);
                        }
                    }
                entJson.staticMeshes = new_mesh;
            }


            //get the skeleton in CEntityTemplate->CNewNPC->CMovingPhysicalAgentComponent

            //get the w2ents in CEntityTemplate->appearances

            //get the w3fac in the head attached, it will be under CEntityTemplate->CEntity->CMimicComponent->mimicFace

            //some mesh entities have CEntityTemplate->CEntity->CAnimDangleBufferComponent->skeleton similar to how mimic faces
            //are attached. This skeleton mush be attached to the base CMovingPhysicalAgentComponent skeleton that drives animation
            //or the animations will not move some bones. NOT ALL HAVE THIS SOME MESHES ARE ATTACHED DIRECTLY TO ANIMATION SKELLY
            //WITH NO "HINGE"

            //CMovingPhysicalAgentComponent skeleton is the daddy that drives everything.

            //There are additional dynging skeletons that are for dynamic objects that are not direclty being animated
            //They are like hinge skeletons but do not have bones that are animated by the CMovingPhysicalAgentComponent skeleton 

            //These are the three main animsets for characters
            //They are all defined under CEntityTemplate.
            //You need to add your animset to these or create a new animset to have it show up in the game
            //reference CEntityTemplate->CAnimAnimsetsParam for list of usable animation sets?
            //reference CEntityTemplate->CAnimAnimsetsParam for list of usable dialog animation sets?
            //reference CEntityTemplate->CAnimMimicParam for list of usable mimic animations
        }

        public void LoadManager(BundleManager manage)
        {
            this.manager = manage;
        }

        public void LoadWccHelper(WccLite wccHelper)
        {
            this.wccHelper = wccHelper;
        }

        public ModelEnt ReadMeshCEntityTemplate(string templateFilename)
        {
            ModelEnt new_mesh = new ModelEnt(templateFilename, templateFilename.Split('\\').Last().Split('_')[0]);
            //check the depo before using bundle manager thing
            if (manager.Items.Any(x => x.Value.Any(y => y.Name == templateFilename)))
            {
                var archives = manager.FileList.Where(x => x.Name == templateFilename).Select(y => new KeyValuePair<string, IWitcherFile>(y.Bundle.FileName, y));
                BundleItem meshEntityData = archives.FirstOrDefault().Value as BundleItem;
                MemoryStream ms = new MemoryStream();
                meshEntityData.Extract(ms);
                byte[] dataRig;
                dataRig = ms.ToArray();
                using (MemoryStream ms2 = new MemoryStream(dataRig))
                using (BinaryReader br2 = new BinaryReader(ms2))
                {

                    CR2WFile entity = new CR2WFile(br2);

                    if (entity != null)
                        foreach (var chunk in entity.chunks)
                        {
                            if (chunk.Type == "CMeshComponent")
                            {
                                string mesh = (chunk.GetVariableByName("mesh") as CHandle).ToString().Split(' ')[1];
                                new_mesh.chunks.Add(new CMeshComponent(mesh));
                                new_mesh.chunks.Last().refChunk = chunk.cr2w;
                                new_mesh.chunks.Last().type = chunk.Type;
                                new_mesh.chunks.Last().chunkIndex = chunk.ChunkIndex;
                            }
                            if (chunk.Type == "CMorphedMeshComponent")
                            {
                                string morphTarget = (chunk.GetVariableByName("morphTarget") as CHandle).ToString().Split(' ')[1];
                                string morphSource = (chunk.GetVariableByName("morphSource") as CHandle).ToString().Split(' ')[1];
                                string morphComponentId = (chunk.GetVariableByName("morphComponentId") as CName).Value;
                                new_mesh.chunks.Add(new CMorphedMeshComponent(morphTarget, morphSource, morphComponentId));
                                new_mesh.chunks.Last().refChunk = chunk.cr2w;
                                new_mesh.chunks.Last().type = chunk.Type;
                                new_mesh.chunks.Last().chunkIndex = chunk.ChunkIndex;
                            }
                            if (chunk.Type == "CMimicComponent")
                            {
                                string name = (chunk.GetVariableByName("name") as CString).val;
                                string mimicFace = (chunk.GetVariableByName("mimicFace") as CHandle).ToString().Split(' ')[1];
                                new_mesh.chunks.Add(new CMimicComponent(name, mimicFace));
                                new_mesh.chunks.Last().refChunk = chunk.cr2w;
                                new_mesh.chunks.Last().type = chunk.Type;
                                new_mesh.chunks.Last().chunkIndex = chunk.ChunkIndex;
                                new_mesh.animation_face_object = GetFace(mimicFace);
                            }
                            if (chunk.Type == "CMeshSkinningAttachment")
                            {
                                int parent = (chunk.GetVariableByName("parent") as CPtr).Reference.ChunkIndex;
                                int child = (chunk.GetVariableByName("child") as CPtr).Reference.ChunkIndex;
                                new_mesh.chunks.Add(new CMeshSkinningAttachment(parent, child));
                                new_mesh.chunks.Last().refChunk = chunk.cr2w;
                                new_mesh.chunks.Last().type = chunk.Type;
                                new_mesh.chunks.Last().chunkIndex = chunk.ChunkIndex;
                            }
                            if (chunk.Type == "CAnimatedAttachment")
                            {
                                int parent = (chunk.GetVariableByName("parent") as CPtr).Reference.ChunkIndex;
                                int child = (chunk.GetVariableByName("child") as CPtr).Reference.ChunkIndex;
                                new_mesh.chunks.Add(new CAnimatedAttachment(parent, child));
                                new_mesh.chunks.Last().refChunk = chunk.cr2w;
                                new_mesh.chunks.Last().type = chunk.Type;
                                new_mesh.chunks.Last().chunkIndex = chunk.ChunkIndex;
                            }
                            if (chunk.Type == "CAnimDangleBufferComponent")
                            {
                                string name = (chunk.GetVariableByName("name") as CString).val;
                                string skeleton = (chunk.GetVariableByName("skeleton") as CHandle).ToString().Split(' ')[1];
                                new_mesh.chunks.Add(new CAnimDangleBufferComponent(name, skeleton));
                                new_mesh.chunks.Last().refChunk = chunk.cr2w;
                                new_mesh.chunks.Last().type = chunk.Type;
                                new_mesh.chunks.Last().chunkIndex = chunk.ChunkIndex;
                            }
                            if (chunk.Type == "CAnimDangleComponent")
                            {
                                string name = (chunk.GetVariableByName("name") as CString).val;
                                int constraint = (chunk.GetVariableByName("constraint") as CPtr).Reference.ChunkIndex;
                                new_mesh.chunks.Add(new CAnimDangleComponent(name, constraint));
                                new_mesh.chunks.Last().refChunk = chunk.cr2w;
                                new_mesh.chunks.Last().type = chunk.Type;
                                new_mesh.chunks.Last().chunkIndex = chunk.ChunkIndex;
                            }
                            if (chunk.Type == "CAnimDangleConstraint_Dyng")
                            {
                                string dyng = (chunk.GetVariableByName("dyng") as CHandle).ToString().Split(' ')[1];
                                new_mesh.chunks.Add(new CAnimDangleConstraint_Dyng(dyng));
                                new_mesh.chunks.Last().refChunk = chunk.cr2w;
                                new_mesh.chunks.Last().type = chunk.Type;
                                new_mesh.chunks.Last().chunkIndex = chunk.ChunkIndex;
                            }
                            if (chunk.Type == "CAnimDangleConstraint_Breast")
                            {
                                string skeleton = (chunk.GetVariableByName("skeleton") as CHandle).ToString().Split(' ')[1];
                                new_mesh.chunks.Add(new CAnimDangleConstraint_Breast(skeleton));
                                new_mesh.chunks.Last().refChunk = chunk.cr2w;
                                new_mesh.chunks.Last().type = chunk.Type;
                                new_mesh.chunks.Last().chunkIndex = chunk.ChunkIndex;
                            }
                            if (chunk.Type == "CAnimDangleConstraint_Collar")
                            {
                                string skeleton = (chunk.GetVariableByName("skeleton") as CHandle).ToString().Split(' ')[1];
                                new_mesh.chunks.Add(new CAnimDangleConstraint_Collar(skeleton));
                                new_mesh.chunks.Last().refChunk = chunk.cr2w;
                                new_mesh.chunks.Last().type = chunk.Type;
                                new_mesh.chunks.Last().chunkIndex = chunk.ChunkIndex;
                            }
                            if (chunk.Type == "CAnimDangleConstraint_Dress")
                            {
                                string skeleton = (chunk.GetVariableByName("skeleton") as CHandle).ToString().Split(' ')[1];
                                new_mesh.chunks.Add(new CAnimDangleConstraint_Dress(skeleton));
                                new_mesh.chunks.Last().refChunk = chunk.cr2w;
                                new_mesh.chunks.Last().type = chunk.Type;
                                new_mesh.chunks.Last().chunkIndex = chunk.ChunkIndex;
                            }
                            if (chunk.Type == "CAnimDangleConstraint_Hood")
                            {
                                string skeleton = (chunk.GetVariableByName("skeleton") as CHandle).ToString().Split(' ')[1];
                                new_mesh.chunks.Add(new CAnimDangleConstraint_Hood(skeleton));
                                new_mesh.chunks.Last().refChunk = chunk.cr2w;
                                new_mesh.chunks.Last().type = chunk.Type;
                                new_mesh.chunks.Last().chunkIndex = chunk.ChunkIndex;
                            }
                            if (chunk.Type == "CAnimDangleConstraint_Hinge")
                            {
                                string skeleton = (chunk.GetVariableByName("skeleton") as CHandle).ToString().Split(' ')[1];
                                new_mesh.chunks.Add(new CAnimDangleConstraint_Hinge(skeleton));
                                new_mesh.chunks.Last().refChunk = chunk.cr2w;
                                new_mesh.chunks.Last().type = chunk.Type;
                                new_mesh.chunks.Last().chunkIndex = chunk.ChunkIndex;
                            }
                            if (chunk.Type == "CAnimDangleConstraint_Pusher")
                            {
                                string skeleton = (chunk.GetVariableByName("skeleton") as CHandle).ToString().Split(' ')[1];
                                new_mesh.chunks.Add(new CAnimDangleConstraint_Pusher(skeleton));
                                new_mesh.chunks.Last().refChunk = chunk.cr2w;
                                new_mesh.chunks.Last().type = chunk.Type;
                                new_mesh.chunks.Last().chunkIndex = chunk.ChunkIndex;
                            }
                        }

                }
            }
            return new_mesh;
        }
    }
    public class EntFile
    {
        public string name;
        //public List<string> fbx_list = new List<string>();
        public CMovingPhysicalAgentComponent MovingPhysicalAgentComponent = new CMovingPhysicalAgentComponent();
        public List<EntityAppearance> appearances = new List<EntityAppearance>();
        public ModelEnt staticMeshes;
        [JsonIgnore]
        public Rig animation_rig_object;
    }

    public class CMovingPhysicalAgentComponent
    {
        public string name;
        public string skeleton;
    }

    public class EntityAppearance
    {
        public string name;
        public List<ModelEnt> includedTemplates = new List<ModelEnt>();
    }

    public class ModelEnt
    {

        public List<JsonChunk> chunks = new List<JsonChunk>();
        [JsonIgnore]
        public ExportFace animation_face_object;

        public ModelEnt(string templateFilename, string ns)
        {
            this.templateFilename = templateFilename;
            this.ns = ns;
        }

        public string templateFilename { get; private set; }
        public string ns { get; private set; }
    }

    public class JsonChunk
    {
        public int chunkIndex { get; set; }
        public string type { get; set; }
        [JsonIgnore]
        public CR2WFile refChunk;
    }

    public class CMeshComponent : JsonChunk
    {
        public CMeshComponent(string mesh)
        {
            this.mesh = mesh;
        }

        public string mesh { get; private set; }
    }

    public class CMorphedMeshComponent : JsonChunk
    {
        public CMorphedMeshComponent(string morphTarget, string morphSource, string morphComponentId)
        {
            this.morphTarget = morphTarget;
            this.morphSource = morphSource;
            //this.morphControlTextures = morphSource;
            this.morphComponentId = morphComponentId;
        }

        public string morphTarget { get; private set; }
        public string morphSource { get; private set; }
        //public string morphControlTextures { get; private set; }
        public string morphComponentId { get; private set; }
    }

    public class CStaticMeshComponent : JsonChunk
    {
        public CStaticMeshComponent(string mesh)
        {
            this.mesh = mesh;
        }

        public string mesh { get; private set; }
    }
    
    public class CMimicComponent : JsonChunk
    {

        public CMimicComponent(string name, string mimicFace)
        {
            this.name = name;
            this.mimicFace = mimicFace;
        }

        public string name { get; private set; }
        public string mimicFace { get; private set; }
    }

    public class CAnimDangleBufferComponent : JsonChunk
    {
        public CAnimDangleBufferComponent(string name, string skeleton)
        {
            this.name = name;
            this.skeleton = skeleton;
        }
        public string name { get; private set; }
        public string skeleton { get; private set; }
    }
    public class CAnimatedComponent : JsonChunk
    {
        public CAnimatedComponent(string name, string skeleton)
        {
            this.name = name;
            this.skeleton = skeleton;
        }
        public string name { get; private set; }
        public string skeleton { get; private set; }
    }

    public class CAnimDangleComponent : JsonChunk
    {
        public CAnimDangleComponent(string name, int constraint)
        {
            this.name = name;
            this.constraint = constraint;
        }
        public string name { get; private set; }
        public int constraint { get; private set; }
    }

    public class CAnimDangleConstraint_Dyng : JsonChunk
    {
        public CAnimDangleConstraint_Dyng(string dyng)
        {
            this.dyng = dyng;
        }
        public string dyng { get; private set; }
    }

    //This describes any additional skeletons the entity has other than the animation skeleton
    public class SkinningAttachment : JsonChunk
    {
        public SkinningAttachment(int parent, int child)
        {
            this.parent = parent;
            this.child = child;
        }

        public int parent { get; private set; }
        public int child { get; private set; }
    }
    public class CHardAttachment : SkinningAttachment
    {
        public CHardAttachment(int parent, int child, int parentSlot, string parentSlotName) : base(parent, child)
        {
            this.parentSlotName = parentSlotName;
            this.parentSlot = parentSlot;
        }
        public string parentSlotName { get; private set; }
        public int parentSlot { get; private set; }
    }

    public class CSkeletonBoneSlot : JsonChunk
    {
        public CSkeletonBoneSlot(uint boneIndex)
        {
            this.boneIndex = boneIndex;
        }

        public uint boneIndex { get; private set; }
    }
    
    public class CMeshSkinningAttachment : SkinningAttachment
    {
        public CMeshSkinningAttachment(int parent, int child) : base(parent, child)
        {
        }

    }

    public class CAnimatedAttachment : SkinningAttachment
    {
        public CAnimatedAttachment(int parent, int child) : base(parent, child)
        {
        }
    }

    public class CAnimDangleConstraint_Breast : JsonChunk
    {
        public CAnimDangleConstraint_Breast(string skeleton)
        {
            this.skeleton = skeleton;
        }
        public string skeleton { get; private set; }
    }
    public class CAnimDangleConstraint_Collar : JsonChunk
    {
        public CAnimDangleConstraint_Collar(string skeleton)
        {
            this.skeleton = skeleton;
        }
        public string skeleton { get; private set; }
    }
    public class CAnimDangleConstraint_Pusher : JsonChunk
    {
        public CAnimDangleConstraint_Pusher(string skeleton)
        {
            this.skeleton = skeleton;
        }
        public string skeleton { get; private set; }
    }
    public class CAnimDangleConstraint_Hinge : JsonChunk
    {
        public CAnimDangleConstraint_Hinge(string skeleton)
        {
            this.skeleton = skeleton;
        }
        public string skeleton { get; private set; }
    }
    public class CAnimDangleConstraint_Hood : JsonChunk
    {
        public CAnimDangleConstraint_Hood(string skeleton)
        {
            this.skeleton = skeleton;
        }
        public string skeleton { get; private set; }
    }
    public class CAnimDangleConstraint_Dress : JsonChunk
    {
        public CAnimDangleConstraint_Dress(string skeleton)
        {
            this.skeleton = skeleton;
        }
        public string skeleton { get; private set; }
    }
}
