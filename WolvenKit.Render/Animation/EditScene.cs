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
using Newtonsoft.Json.Linq;

namespace WolvenKit.Render.Animation
{
    class EditScene
    {
        private CR2WFile cr2wFile;
        public CR2WFile CR2WFile
        {
            get { return cr2wFile; }
            set { cr2wFile = value; }
        }
        internal void LoadData(CR2WFile file)
        {
            CR2WFile = file;

            foreach (var chunk in CR2WFile.chunks)
            {
                if (chunk.Type == "CStorySceneSection")
                {
                    chunk.Type = "CStorySceneCutsceneSection";
                    chunk.data.Type = "CStorySceneCutsceneSection";
                    CHandle handle = (animVar("handle:CCutsceneTemplate", "cutscene", chunk.cr2w) as CHandle);
                    handle.DepotPath = @"animations\cutscenes\additional\cs_generic_teleport\cs_generic_teleport_no_fx.w2cutscene";
                    handle.ClassName = @"CCutsceneTemplate";
                    chunk.data.AddVariable(handle);
                    CTagList tags = animVar("TagList", "point", chunk.cr2w) as CTagList;
                    tags.AddVariable(animVar("CName", "", chunk.cr2w).SetValue("yennpoint"));
                    chunk.data.AddVariable(tags);

                    CArray sceneElements = (chunk.GetVariableByName("sceneElements") as CArray);
                    foreach (var v in sceneElements.GetEditableVariables())
                    {
                        (v as CPtr).Reference.Type = "CStorySceneCutscenePlayer";
                        (v as CPtr).Reference.data.Type = "CStorySceneCutscenePlayer";
                        //sceneElements.RemoveVariable(v);
                    }

                    //CR2WExportWrapper CStorySceneCutscenePlayer = CR2WFile.CreateChunk("CStorySceneCutscenePlayer");
                    //CStorySceneCutscenePlayer.AddVariable(animVar("String", "elementID", CStorySceneCutscenePlayer.cr2w).SetValue("CutscenePlayer_5"));
                    //sceneElements.AddVariable((animVar("ptr:CStorySceneEventInfo", "", CStorySceneCutscenePlayer.cr2w) as CPtr).SetValue(CStorySceneCutscenePlayer));

                    //
                    break;
                }
                if (chunk.Type == "CStorySceneCutsceneSection")
                {
                    //chunk.SetType("CStorySceneCutsceneSection");
                }
            }
        }

        public CVariable animVar(string type, string name, CR2WFile file)
        {
            CVariable newvar = CR2WTypeManager.Get().GetByName(type, name, file, false);
            if (newvar == null)
                throw new Exception("Nope");
            newvar.Name = name;
            newvar.Type = type;
            return newvar;
        }

        internal void saveToFile(string FileName)
        {
            try
            {
                using (var mem = new MemoryStream())
                {
                    using (var writer = new BinaryWriter(mem))
                    {
                        CR2WFile.Write(writer);
                        CR2WFile.Write(writer);//need to run Write twice or there are issues with CNames?
                        mem.Seek(0, SeekOrigin.Begin);

                        using (var fs = new FileStream(FileName, FileMode.Create, FileAccess.Write))
                        {
                            mem.WriteTo(fs);
                            fs.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to save the file(s)! They are probably in use.\n" + e.ToString());
            }
        }
    }
}
