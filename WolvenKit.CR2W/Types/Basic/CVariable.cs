using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;

using WolvenKit.CR2W.Editors;
using WolvenKit.Utils;

namespace WolvenKit.CR2W.Types
{
    [DataContract(Namespace = "")]
    public abstract class CVariable : IEditableVariable
    {
        [NonSerialized]
        public CR2WFile cr2w;
        public Guid InternalGuid;

        public CVariable(CR2WFile cr2w)
        {
            this.cr2w = cr2w;
            InternalGuid = Guid.NewGuid();
        }


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ushort typeId
        {
            get { return (ushort)cr2w.GetStringIndex(Type, true); }
            set { Type = cr2w.names[value].Str; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ushort nameId
        {
            get { return (ushort)cr2w.GetStringIndex(Name, true); }
            set { Name = cr2w.names[value].Str; }
        }

        public CVariable ParentVariable { get; set; }

        public string FullName
        {
            get
            {
                var name = Name;
                var c = ParentVariable;
                while (c != null)
                {
                    name = c.Name + "/" + name;
                    c = c.ParentVariable;
                }
                return name;
            }
        }

        public CR2WFile CR2WOwner => cr2w;

        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Type { get; set; }



        public virtual Control GetEditor()
        {
            return null;
        }

        public virtual List<IEditableVariable> GetEditableVariables()
        {
            return null;
        }

        public virtual bool CanRemoveVariable(IEditableVariable child)
        {
            return false;
        }

        public virtual bool CanAddVariable(IEditableVariable newvar)
        {
            return false;
        }

        public virtual void RemoveVariable(IEditableVariable child)
        {
        }

        public virtual void AddVariable(CVariable var)
        {
        }

        public abstract void Read(BinaryReader file, uint size);
        public abstract void Write(BinaryWriter file);
        public abstract CVariable Create(CR2WFile cr2w);

        public virtual CVariable SetValue(object val)
        {
            return this;
        }


        public virtual CVariable Copy(CR2WCopyAction context)
        {
            var var = Create(context.DestinationFile);
            var.Type = Type;
            var.Name = Name;
            return var;
        }

        public virtual CVariable CreateDefaultVariable()
        {
            return null;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = Type == null ? hash : hash * 29 + Type.GetHashCode();
                hash = Name == null ? hash : hash * 29 + Name.GetHashCode();
                hash = FullName == null ? hash : hash * 29 + FullName.GetHashCode();
                hash = hash * 29 + ToString().GetHashCode();
                var evars = GetEditableVariables();
                if (evars != null)
                {
                    foreach (var item in evars)
                    {
                        hash = hash * 29 + item.GetHashCode();
                    }
                }
                return hash;
            }
        }

        #region serialization
        //vl: I leave it commented here for it's rareness
        /*
        private static IEnumerable<Type> _variableTypes;

        private static IEnumerable<Type> GetKnownVariableTypes()
        {
            if (_variableTypes == null)
            {
                _variableTypes = Assembly.GetExecutingAssembly()
                                        .GetTypes()
                                        .Where(t => typeof(CVariable).IsAssignableFrom(t))
                                        .ToList();
            }
            return _variableTypes;
        }
        */

        public virtual void SerializeToXml(XmlWriter xw)
        {
            DataContractSerializer ser = new DataContractSerializer(this.GetType());
            using (var ms = new MemoryStream())
            {
                ser.WriteStartObject(xw, this);
                ser.WriteObjectContent(xw, this);


                if (GetEditableVariables() != null)
                {
                    foreach (var v in GetEditableVariables())
                    {
                        v.SerializeToXml(xw);
                    }
                }
                ser.WriteEndObject(xw);
            }
        }


        public virtual void SerializeToJson(JsonWriter writer)
        {
            //try
            //{
            //    if (this.Type != null && !this.Type.Contains("array"))
            //    {
            //        JObject t = JObject.FromObject(this);
            //        t.WriteTo(writer);
            //    }
            //    else
            //    {
            //        JArray t = JArray.FromObject(this);
            //        t.WriteTo(writer);
            //    }
            //}
            //catch (Exception)
            //{
            //    Console.WriteLine("cake");
            //}
            //JObject t = JObject.FromObject(this);
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(this.GetType());

            //JToken t = JToken.FromObject("cake");

            //t.WriteTo(writer);
            JToken t = GetObject(this);
            t.WriteTo(writer);
            //t.Add(t);
            //writer.WriteStartObject();
            //writer.WritePropertyName("cake");
            //writer.WriteValue("cake");
            //if (GetEditableVariables() != null)
            //{
            //    foreach (var v in GetEditableVariables())
            //    {
            //        v.SerializeToJson(writer);
            //    }
            //}
            //writer.WriteEndObject();
            //t.WriteTo(writer);
        }
        /// <summary>
        /// Transfers bytes array to hex string like 0x00AADD..., TODO: build reverse function
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string HexStr(byte[] p)
        {
            char[] c = new char[p.Length * 2 + 2];
            byte b;
            c[0] = '0'; c[1] = 'x';

            for (int y = 0, x = 2; y < p.Length; ++y, ++x)
            {
                b = ((byte)(p[y] >> 4));
                c[x] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                b = ((byte)(p[y] & 0xF));
                c[++x] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }
            return new string(c);
        }
        #endregion
        protected static JObject GetObject(CVariable entryVector)
        {
            List<object> Content = new List<object>();
            if (entryVector.GetEditableVariables() != null)
            {
                foreach (var Value in entryVector.GetEditableVariables())
                {
                    switch (Value.GetType().Name)
                    {
                        case "CPtr":
                            object Reference = GetObject((Value as CPtr).Reference.data);
                            //CVariable Reference = (Value as CPtr).Reference.data;
                            Content.Add(new { Value.Type, Value.Name, Reference });
                            break;
                        default:
                            if (Value.Name != null && Value.Name.Contains("boneNames"))
                            {

                            }
                            if (Value.Type != null && !Value.Type.Contains("array"))
                            {
                                if (Value.GetEditableVariables() != null)
                                {
                                    object Content2 = GetObject(Value as CVariable);
                                    Content.Add(Content2);
                                }
                                else
                                {
                                    //var fields = Value.GetType().GetProperties();
                                    //foreach (var prop in fields)
                                    //{
                                    //    Console.WriteLine("{0}={1}", prop.Name, prop.GetValue(Value, null));
                                    //}
                                    ////dynamic EO = new ExpandoObject();
                                    ////foreach (int i = 0; i < fields.Length; i++)
                                    ////{
                                    ////    AddProperty(EO, "Language", "lang" + i);
                                    ////    Console.Write(EO.Language);
                                    ////}
                                    ////Content.Add(new { Value.Type, Value });
                                    //if (Value.Name == "soundEventName")
                                    //{

                                    //}
                                    Content.Add(Value);
                                }

                            }
                            else
                            {
                                if (Value.GetType().Name.Contains("CBufferUInt16"))
                                {
                                    List<object> Array = new List<object>();
                                    foreach (var item in Value.GetEditableVariables())
                                    {
                                        Array.Add(item);
                                    }
                                    Content.Add(new {Type = "array:2,0,CInt32", Value.Name, Array });
                                    }
                                // if it is a pointer should write Reference for deserilisation?
                                else if (Value.GetType().Name == "CPtr")
                                {
                                    object ReferencePtr = GetObject((Value as CPtr).Reference.data);
                                    //CVariable Reference = (Value as CPtr).Reference.data;
                                    Content.Add(new { Value.Type, Value.Name, Reference = ReferencePtr });
                                }
                                else if (Value.GetType().Name == "CArray")
                                {
                                    List<object> Array = new List<object>();
                                    //Content.Add(GetObject(Value as CVariable));
                                    foreach (var item in (Value as CArray).array)
                                    {
                                        Type cake = item.GetType();
                                        if (item.GetType().Name == "CPtr")
                                        {
                                            try
                                            {
                                                object ReferencePtr = GetObject((item as CPtr).Reference.data);
                                                //CVariable Reference = (item as CPtr).Reference.data;
                                                Array.Add(new { item.Type, item.Name, Reference = ReferencePtr });

                                                //object Content2 = GetObject((item as CPtr).Reference.data);
                                                //Array.Add(Content2);
                                            }
                                            catch (Exception)
                                            {
                                                throw new Exception("Cake1");
                                            }
                                        }
                                        else if (item.GetType().Name == "CName")
                                        {
                                            Array.Add(item);
                                        }
                                        else if (item.Type == null && item.Name == null)
                                        {
                                            try
                                            {
                                                Type thistype = item.GetType();

                                                var valProp = thistype.GetProperty("val");
                                                if (valProp != null)
                                                    Array.Add(valProp.GetValue(item));

                                                var ValueProp = thistype.GetProperty("Value");
                                                if (ValueProp != null)
                                                    Array.Add(ValueProp.GetValue(item));

                                            }
                                            catch (Exception)
                                            {
                                                throw new Exception("Cake2");
                                            }
                                        }
                                        else
                                        {
                                            //throw new Exception("Cake3");
                                            Array.Add(GetObject(item as CVariable));
                                        }
                                    }
                                    Content.Add(new { Value.Type, Value.Name, Array });
                                }
                                else
                                {
                                    if (Value.GetEditableVariables() != null)
                                    {
                                        object Content2 = GetObject(Value as CVariable);
                                        Content.Add(Content2);
                                    }
                                    else
                                    {
                                        Content.Add(Value);
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            else
            {
                Type thistype = entryVector.GetType();

                var valProp = thistype.GetProperty("val");
                if (valProp != null)
                    Content.Add(valProp.GetValue(entryVector));

                var ValueProp = thistype.GetProperty("Value");
                if (ValueProp != null)
                    Content.Add(ValueProp.GetValue(entryVector));
            }

            return JObject.FromObject(new { entryVector.Type, entryVector.Name, Content });
        }
    }

}