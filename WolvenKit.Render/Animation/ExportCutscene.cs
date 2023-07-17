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

namespace WolvenKit.Render.Animation
{
    public class ExportCutscene : ExportAnimation
    {
        CR2WFile cR2WFile;
        CCutsceneTemplate CutsceneTemplate = new CCutsceneTemplate();
        public static List<KeyValuePair<string, Rig>> RigNames = new List<KeyValuePair<string, Rig>>();

        public void LoadCutsceneData(CR2WFile cutsceneFile, BundleManager manager)
        {
            EntityGenerator generator = new EntityGenerator();
            cR2WFile = cutsceneFile;
            if (cutsceneFile != null)
                foreach (var chunk in cutsceneFile.chunks)
                {
                    //var exeDir = Path.GetDirectoryName(MainController.Get().Configuration.ExecutablePath);

                    if (chunk.Type == "CCutsceneTemplate")
                    {
                        //get actor info
                        List<CVariable> actorsDef = (chunk.GetVariableByName("actorsDef") as CArray).array;
                        var animevents = (chunk.data as CR2W.Types.CCutsceneTemplate).animevents;
                        CutsceneTemplate.SCutsceneActorDefs = (chunk.GetVariableByName("actorsDef") as CArray);
                        CutsceneTemplate.animevents = animevents;
                        CutsceneTemplate.effects = chunk.GetVariableByName("effects");
                        CutsceneTemplate.burnedAudioTrackName = chunk.GetVariableByName("burnedAudioTrackName") as CStringAnsi;
                        CutsceneTemplate.entToHideTags = chunk.GetVariableByName("entToHideTags") as CArray;
                        CutsceneTemplate.lastLevelLoaded = chunk.GetVariableByName("lastLevelLoaded") as CString;
                        CutsceneTemplate.point = chunk.GetVariableByName("point") as CTagList;
                        CutsceneTemplate.requiredSfxTag = chunk.GetVariableByName("requiredSfxTag") as CName;
                        CutsceneTemplate.fadeBefore = chunk.GetVariableByName("fadeBefore") as CFloat;
                        CutsceneTemplate.fadeAfter = chunk.GetVariableByName("fadeAfter") as CFloat;
                        CutsceneTemplate.cameraBlendInTime = chunk.GetVariableByName("cameraBlendInTime") as CFloat;
                        CutsceneTemplate.cameraBlendOutTime = chunk.GetVariableByName("cameraBlendOutTime") as CFloat;
                        CutsceneTemplate.blackscreenWhenLoading = chunk.GetVariableByName("blackscreenWhenLoading") as CBool;
                        CutsceneTemplate.checkActorsPosition = chunk.GetVariableByName("checkActorsPosition") as CBool;
                        CutsceneTemplate.streamable = chunk.GetVariableByName("streamable") as CBool;
                        CutsceneTemplate.usedInFiles = chunk.GetVariableByName("usedInFiles") as CArray;
                        CutsceneTemplate.resourcesToPreloadManuallyPaths = chunk.GetVariableByName("resourcesToPreloadManuallyPaths") as CArray;
                        CutsceneTemplate.reverbName = chunk.GetVariableByName("reverbName") as CString;
                        CutsceneTemplate.banksDependency = chunk.GetVariableByName("banksDependency") as CArray;

                        for (int i = 0; i < actorsDef.Count(); i++)
                        {
                            CVector actor = (actorsDef[i] as CVector);
                            actor.Name = "SCutsceneActorDef";
                            actor.Type = "SCutsceneActorDef";
                            SCutsceneActorDef sceneActor = new SCutsceneActorDef();
                            sceneActor.name = actor.GetVariableByName("name").ToString();
                            sceneActor.type = actor.GetVariableByName("type").ToString();
                            if (actor.GetVariableByName("voiceTag") != null) sceneActor.voiceTag = actor.GetVariableByName("voiceTag").ToString();
                            if (actor.GetVariableByName("tag") != null)
                            {
                                foreach (var tag in (actor.GetVariableByName("tag") as CTagList).tags)
                                {
                                    sceneActor.tag.Add(tag.ToString());
                                }
                            }
                            sceneActor.template = actor.GetVariableByName("template").ToString().Split(' ')[1];

                            //TODO get geralt properly
                            if (sceneActor.template == @"gameplay\templates\characters\player\player.w2ent" ||
                                sceneActor.template == @"characters\player_entities\geralt\geralt_player.w2ent")
                            {
                                sceneActor.template = @"characters\npc_entities\main_npc\lambert.w2ent";
                            }

                            generator.LoadManager(manager);
                            generator.readCEntityTemplate(sceneActor.template, true);
                            sceneActor.rig = generator.entJson.animation_rig_object;

                            for (int j = 0; sceneActor.face_rig == null && j < generator.entJson.appearances.Count; j += 1)
                            {
                                foreach (ModelEnt template in generator.entJson.appearances[j].includedTemplates)
                                {
                                    if (template.animation_face_object != null)
                                    {
                                        sceneActor.useMimic = true;
                                        sceneActor.face_rig = template.animation_face_object.MimicFace.floatTrackSkeleton;
                                    }
                                }
                            }
                            //sceneActor.templateData = generator;
                            CutsceneTemplate.SCutsceneActorDefsNat.Add(sceneActor);
                            //IF ACTOR USES MIMIC YOU MUST ADD ACTOR:FACE geralt:face skeleton based on geralt mimic face
                        }



                        //use actor skeletons to load animation data for export
                        foreach (CPtr animationPtr in (chunk.GetVariableByName("animations") as CArray).array)
                        {
                            var setEntry = animationPtr.Reference;
                            var animation = (setEntry.GetVariableByName("animation") as CPtr).Reference;
                            string anim_part_name = (animation.GetVariableByName("name") as CName).Value;
                            string[] splitName = anim_part_name.Split(':');
                            CSkeleton skel = new CSkeleton();
                            foreach (var actor in CutsceneTemplate.SCutsceneActorDefsNat)
                            {
                                if (actor.name == splitName[0] && splitName[1] != "face")
                                {
                                    skel = actor.rig.meshSkeleton;
                                    break;
                                }
                                if (actor.name == splitName[0] && splitName[1] == "face")
                                {
                                    skel = actor.face_rig;
                                    //Console.WriteLine("found the face for ", splitName[0]);
                                    break;
                                }
                            }
                            CutsceneTemplate.animations.Add(LoadEntry(setEntry, skel));
                        }
                    }
                }
            
        }


        public new void SaveJson(string filename)
        {
            //open file stream
            using (StreamWriter file = File.CreateText(filename))
            {
                JsonConverter[] converters = { new CVariableConverter(cR2WFile) };
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new CVariableConverter(cR2WFile));
                serializer.Formatting = Formatting.Indented;
                //serialize object directly into file stream
                serializer.Serialize(file, CutsceneTemplate);
            }
        }
    }


}