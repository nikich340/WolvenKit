using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolvenKit.CR2W;
using WolvenKit.CR2W.Types;
using WolvenKit.CR2W.Types.Utils;

namespace WolvenKit.Render.Animation
{
    public class JsonConverters
    {
    }
    public class BufferConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IAnimationBuffer));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            JObject jo = JObject.Load(reader);
            JToken firstFramesExists = jo.SelectToken("firstFrames");
            if (firstFramesExists != null)
            {
                return jo.ToObject<CAnimationBufferMultipart>(serializer);
            }
            else
            {
                return jo.ToObject<CAnimationBufferBitwiseCompressed>(serializer);
            }
            //if (jo["FooBarBuzz"].Value<string>() == "A")
            //    return jo.ToObject<CAnimationBufferMultipart>(serializer);

            //if (jo["FooBarBuzz"].Value<string>() == "B")
            //    return jo.ToObject<CAnimationBufferBitwiseCompressed>(serializer);

            return null;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class IMotionExtractionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IMotionExtraction));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            JObject jo = JObject.Load(reader);
            if (jo == null)
            {
                Console.WriteLine("cake");
            }
            JToken testToken = jo.SelectToken("deltaTimes");
            if (testToken != null)
            {
                return jo.ToObject<CLineMotionExtraction2>(serializer);
            }
            else
            {
                return jo.ToObject<CLineMotionExtraction>(serializer);
            }
        }
        public override bool CanWrite
        {
            get { return false; }
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class CVariableConverter : JsonConverter
    {
        private CR2WFile w2AnimFile;

        public CVariableConverter(CR2WFile w2AnimFile)
        {
            this.w2AnimFile = w2AnimFile;
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(CVariable)) || (objectType == typeof(List<CVariable>)) || (objectType == typeof(CArray) || objectType == typeof(CBufferUInt32<CVectorWrapper>) || objectType == typeof(CTagList));
            //return objectType == typeof(List<CVariable>) || objectType == typeof(CArray);
            //return objectType == typeof(CArray);
            //return (objectType == typeof(CVariable));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType.Name.Contains("List"))
            {
                List<CVariable> cake = new List<CVariable>();
                JArray jArray = JArray.Load(reader);
                foreach (JObject item in jArray.Children<JObject>())
                {
                    //var jObject = JObject.Load(item);
                    cake.Add(GetCVariable(item, w2AnimFile.chunks[0]));
                }
                return cake;
            }
            else if (objectType.Name.Contains("CBufferUInt32"))
            {
                var jObject = JObject.Load(reader);
                CBufferUInt32<CVectorWrapper> animevents = new CBufferUInt32<CVectorWrapper>(w2AnimFile, _ => new CVectorWrapper(_));
                JToken Content;
                jObject.TryGetValue("Content", StringComparison.InvariantCultureIgnoreCase, out Content);
                if (Content != null)
                {
                    foreach (JObject child in Content.Children<JObject>())
                    {
                        JToken Content2;
                        child.TryGetValue("Content", StringComparison.InvariantCultureIgnoreCase, out Content2);
                        foreach (JObject child2 in Content2.Children<JObject>())
                        {
                            CVectorWrapper cake = new CVectorWrapper(w2AnimFile);
                            cake.variable = GetCVariable(child2, w2AnimFile.chunks[0]);
                            animevents.AddVariable(cake);
                        }
                    }
                    return animevents;
                }
                else
                {
                    return null;
                }
                
            }
            else
            {
                var jObject = JObject.Load(reader);
                return GetCVariable(jObject, w2AnimFile.chunks[0]);
            }
        }

        //walk any arrays
        //the action function will recursivly take care of any objects whne it finds sub "Content"
        static void WalkNode(JToken node, Action<JObject> action)
        {

            if (JToken.EqualityComparer.Equals(node.Parent, node.Root))
            {
                return;
            }
            if (node.Type == JTokenType.Object)
            {
                action((JObject)node);

                //foreach (JProperty child in node.Children<JProperty>())
                //{
                //    WalkNode(child.Value, action);
                //}
            }
            else if (node.Type == JTokenType.Array)
            {
                foreach (JToken child in node.Children())
                {
                    WalkNode(child, action);
                }
            }
        }

        protected static CVariable GetCVariable(JObject jObject, CR2WExportWrapper currentChunk)
        {
            CR2WFile w2AnimFile = currentChunk.cr2w;
            JToken jToken;
            jObject.TryGetValue("Type", StringComparison.InvariantCultureIgnoreCase, out jToken);
            string type = jToken.ToString();

            if (type == "CExtAnimCutsceneResetClothAndDangleEvent")
            {

            }

            JToken nameToken;
            jObject.TryGetValue("Name", StringComparison.InvariantCultureIgnoreCase, out nameToken);
            string name = "";
            if (nameToken != null)
                name = nameToken.ToString();

            JToken Content;
            jObject.TryGetValue("Content", StringComparison.InvariantCultureIgnoreCase, out Content);

            JToken RootValue;
            jObject.TryGetValue("Value", StringComparison.InvariantCultureIgnoreCase, out RootValue);


            CVariable newvar = addvar(type, name, w2AnimFile, false);

            if (Content != null)
                WalkNode(Content, n =>
                {
                    JToken Content2;
                    n.TryGetValue("Content", StringComparison.InvariantCultureIgnoreCase, out Content2);
                    if (Content2 != null)
                    {
                        newvar.AddVariable(GetCVariable(n as JObject, currentChunk));
                        return;
                    }

                    JToken Reference;
                    n.TryGetValue("Reference", StringComparison.InvariantCultureIgnoreCase, out Reference);
                    if (Reference != null)
                    {
                        JToken ReferenceType = Reference["Type"];
                        JToken ReferenceContent;
                        (Reference as JObject).TryGetValue("Content", StringComparison.InvariantCultureIgnoreCase, out ReferenceContent);
                        JToken ReferenceName;
                        n.TryGetValue("Name", StringComparison.InvariantCultureIgnoreCase, out ReferenceName);

                        JToken PtrType;
                        n.TryGetValue("Type", StringComparison.InvariantCultureIgnoreCase, out PtrType);

                        CR2WExportWrapper CFXDefinition;
                        if (ReferenceType.ToString() == "CFXSimpleSpawner")
                        {
                            CFXDefinition = w2AnimFile.CreateChunk(ReferenceType.ToString(), currentChunk);
                            CFXDefinition.data = GetCVariable(Reference as JObject, CFXDefinition);
                        }
                        else
                        {
                            CFXDefinition = w2AnimFile.CreateChunk(ReferenceType.ToString(), currentChunk);
                            CFXDefinition.data = GetCVariable(Reference as JObject, CFXDefinition);
                        }

                        //CVariable cake = GetCVariable(Reference as JObject, w2AnimFile);

                        //foreach (CVariable item in cake.GetEditableVariables())
                        //{
                        //    CFXDefinition.data.AddVariable(item);
                        //}


                        //foreach (JObject child in ReferenceContent.Children<JObject>())
                        //{
                        //    CFXDefinition.data.AddVariable(GetCVariable(child, w2AnimFile));
                        //    //JToken Content2;
                        //    //child.TryGetValue("Content", StringComparison.InvariantCultureIgnoreCase, out Content2);
                        //    //foreach (JObject child2 in Content2.Children<JObject>())
                        //    //{
                        //    //    CVectorWrapper cake = new CVectorWrapper(w2AnimFile);
                        //    //    cake.variable = GetCVariable(child2, w2AnimFile);
                        //    //    animevents.AddVariable(cake);
                        //    //}
                        //}
                        //CFXDefinition.AddVariable(GetCVariable(Reference as JObject, CFXDefinition.cr2w));
                        string ptr_name_final = "ptr:" + ReferenceType.ToString();
                        if (PtrType != null && PtrType.ToString() != "")
                        {
                            ptr_name_final = PtrType.ToString();
                        }

                        CPtr newPointer = addvar(ptr_name_final, ReferenceName.ToString(), w2AnimFile, false).SetValue(CFXDefinition) as CPtr;

                        
                        //CFXDefinition.ParentPtr = newPointer;
                        newvar.AddVariable(newPointer);
                        return;
                    }

                    JToken Type = n["Type"];
                    JToken Name = n["Name"];
                    JToken Value = n["Value"];
                    JToken Val = n["val"];
                    JArray Array = (JArray)n["Array"];
                    string typeFinal = Type?.ToString() ?? null;
                    string nameFinal = Name?.ToString() ?? null;
                    var value = Value?.ToString();
                    var val = Val?.ToString();


                    if (typeFinal == "" && nameFinal == "count")
                    {
                        typeFinal = "Int32";
                    }
                    else if (nameFinal == "buffer")
                    {
                        //newvar = new CCompressedBuffer<CBufferUInt16<CFloat>>(cr2w, _ => new CBufferUInt16<CFloat>(_, x => new CFloat(x))) { Name = "buffer" };
                    }
                    else if (type == "")
                    {
                        //newvar = CR2WTypeManager.Get().GetByName("Float", "", cr2w, false) as CFloat;
                    }


                    CVariable animPointerArr = addvar(typeFinal, nameFinal, w2AnimFile, false);

                    if (value != null)
                    {
                        animPointerArr.SetValue(value);
                    }
                    else
                    {
                        switch (typeFinal)
                        {
                            case "Uint8":
                                animPointerArr.SetValue(byte.Parse(val));
                                break;
                            case "Int32":
                                animPointerArr.SetValue(int.Parse(val));
                                break;
                            case "Float":
                                animPointerArr.SetValue(float.Parse(val));
                                break;
                            case "Bool":
                                animPointerArr.SetValue(bool.Parse(val));
                                break;
                            case "StringAnsi":
                                //CStringAnsi newName = new CStringAnsi(w2AnimFile).SetValue(Name + "\0") as CStringAnsi;
                                (animPointerArr as CStringAnsi).SetValue(val);
                                break;
                            case "String":
                                (animPointerArr as CString).SetValue(val);
                                break;
                            default:
                                if (typeFinal.Contains("array:2,0,ptr"))
                                {
                                    foreach (JObject ptrItem in Array)
                                    {
                                        JToken ReferencePtr;
                                        ptrItem.TryGetValue("Reference", StringComparison.InvariantCultureIgnoreCase, out ReferencePtr);
                                        if (ReferencePtr != null)
                                        {
                                            JToken ReferenceType = ReferencePtr["Type"];
                                            JToken ReferenceContent;
                                            (ReferencePtr as JObject).TryGetValue("Content", StringComparison.InvariantCultureIgnoreCase, out ReferenceContent);
                                            CR2WExportWrapper NewChunk = w2AnimFile.CreateChunk(ReferenceType.ToString(), currentChunk);
                                            if (NewChunk.Type.Contains("CFXTrackGroup"))
                                            { 

                                            }
                                            if (NewChunk.Type.Contains("CFXTrackItem"))
                                            {
                                                List<List<CFloat>> bufferfloats = new List<List<CFloat>>();
                                                CFXTrackItem temp = GetCVariable(ReferencePtr as JObject, NewChunk) as CFXTrackItem;
                                                //JToken myCount;
                                                //(ReferenceContent as JObject).TryGetValue("count", StringComparison.InvariantCultureIgnoreCase, out myCount);
                                                //

                                                CFXTrackItem track = NewChunk.data as CFXTrackItem;
                                                //special function for CFXTrackItemParticles
                                                foreach (JObject child in ReferenceContent.Children<JObject>())
                                                {
                                                    JToken outName;
                                                    child.TryGetValue("Name", StringComparison.InvariantCultureIgnoreCase, out outName);
                                                    if (outName.ToString() == "count")
                                                    {
                                                        track.count.val = int.Parse(child["val"].ToString());
                                                    }
                                                    else if (outName.ToString() == "buffer")
                                                    {
                                                        JToken bufferContent;
                                                        child.TryGetValue("Content", out bufferContent);
                                                        foreach (var item in (bufferContent as JArray).Children())
                                                        {
                                                            List<CFloat> bufferfloat = new List<CFloat>();
                                                            JToken bufferArray;
                                                            (item as JObject).TryGetValue("Array", out bufferArray);
                                                            foreach (JObject item2 in (bufferArray as JArray).Children())
                                                            {
                                                                CFloat number = addvar("Float", "", w2AnimFile, false) as CFloat;
                                                                float cake = float.Parse(item2["val"].ToString());
                                                                number.SetValue(cake);
                                                                bufferfloat.Add(number);
                                                            }
                                                            bufferfloats.Add(bufferfloat);
                                                        }
                                                        //JToken bufferArray;
                                                        //(bufferContent as JObject).TryGetValue("Array", out bufferArray);
                                                    }
                                                    else if (outName.ToString() == "unk")
                                                    {
                                                        track.unk.val = 1;
                                                    }
                                                    else if (outName.ToString() == "buffername")
                                                    {
                                                        track.buffername = temp.GetVariableByName("buffername") as CName;
                                                    }
                                                    else
                                                    {
                                                        track.AddVariable(temp.GetVariableByName(outName.ToString()));
                                                    }
                                                }

                                                
                                                track.buffer.ReadJson("cake", 1, track.count.val, bufferfloats);
                                                NewChunk.data = track;
                                            }
                                            else
                                            {
                                                NewChunk.data = GetCVariable(ReferencePtr as JObject, NewChunk);
                                            }
                                            //CVariable cake = GetCVariable(ReferencePtr as JObject, w2AnimFile);

                                            //foreach (CVariable item in cake.GetEditableVariables())
                                            //{
                                            //    NewChunk.data.AddVariable(item);
                                            //}
                                            //NewChunk.ParentPtr = animPointerArr.cr2w as CR2WExportWrapper;
                                            CPtr ffinal = addvar("ptr:" + ReferenceType.ToString(), "", w2AnimFile, false).SetValue(NewChunk) as CPtr;
                                            animPointerArr.AddVariable(ffinal);
                                        }
                                    }
                                }
                                else if (typeFinal.Contains("array"))
                                {
                                    foreach (var item in Array)
                                    {
                                        //TODO THIS MIGHT BE A NORMAL STRING? not StringAnsi
                                        JObject itemAsObj = (item as JObject);
                                        JToken itemType = null;
                                        JToken Content3 = null;
                                        if (itemAsObj != null)
                                        {
                                            itemAsObj.TryGetValue("Type", StringComparison.InvariantCultureIgnoreCase, out itemType);
                                            itemAsObj.TryGetValue("Content", StringComparison.InvariantCultureIgnoreCase, out Content3);
                                        }
                                        if (Content3 != null || itemType != null)
                                        {
                                            animPointerArr.AddVariable(GetCVariable(item as JObject, currentChunk));
                                        }
                                        else
                                        {
                                            CVariable animArr = addvar("StringAnsi", "", w2AnimFile, false);
                                            animArr.SetValue(item.ToString());
                                            animPointerArr.AddVariable(animArr);
                                        }

                                    }
                                }
                                else if (typeFinal.StartsWith("handle:"))
                                {
                                    (animPointerArr as CHandle).DepotPath = n["DepotPath"]?.ToString();
                                    (animPointerArr as CHandle).ClassName = n["ClassName"]?.ToString();
                                    (animPointerArr as CHandle).Name = n["Name"]?.ToString();
                                    (animPointerArr as CHandle).Type = n["Type"]?.ToString();
                                    if (n["Flags"] != null)
                                        (animPointerArr as CHandle).Flags = ushort.Parse(n["Flags"]?.ToString());
                                }
                                else if (typeFinal.StartsWith("soft:"))
                                {
                                    (animPointerArr as CSoft).DepotPath = n["DepotPath"]?.ToString();
                                    (animPointerArr as CSoft).ClassName = n["ClassName"]?.ToString();
                                    if (n["Flags"] != null)
                                        (animPointerArr as CSoft).Flags = ushort.Parse(n["Flags"]?.ToString());
                                }
                                else
                                {
                                    if (val != null)
                                        try
                                        {
                                            animPointerArr.SetValue(int.Parse(val));
                                        }
                                        catch (Exception)
                                        {
                                            animPointerArr.SetValue(val);
                                        }
                                }
                                break;
                        }
                    }
                    newvar.AddVariable(animPointerArr);
                });



            if (type == "StringAnsi")
            {
                JToken Name = jObject["Name"];
                JToken Val = jObject["val"];
                string nameFinal = Name?.ToString() ?? null;
                var val = Val?.ToString();
                (newvar as CStringAnsi).SetValue(val);
                (newvar as CStringAnsi).Name = nameFinal;
                return newvar;
            }
            else
            {
                if (RootValue != null)
                {

                    newvar.SetValue(RootValue.ToString());

                }
            }
            return newvar;
        }

        private static CVariable addvar(string type, string name, CR2WFile cr2w, bool v)
        {
            CVariable newvar = CR2WTypeManager.Get().GetByName(type, name, cr2w, false);
            if (newvar == null)
            {
                if (name == "buffer")
                {
                    newvar = new CCompressedBuffer<CBufferUInt16<CFloat>>(cr2w, _ => new CBufferUInt16<CFloat>(_, x => new CFloat(x))) { Name = "buffer" };
                }
                else
                {
                    throw new Exception("Nope addvar error");
                }
                
            }
            newvar.Name = name;
            newvar.Type = type;
            return newvar;
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value.GetType() == typeof(List<CVariable>))
            {
                writer.WriteStartArray();
                foreach (var v in (IList<CVariable>)value)
                    v.SerializeToJson(writer);
                writer.WriteEndArray();
            }
            else
            {
                (value as CVariable).SerializeToJson(writer);
            }
        }
    }

}
