﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Xml;
using WolvenKit.CR2W.Editors;

namespace WolvenKit.CR2W.Types
{
    [DataContract(Namespace = "")]
    public class CVector : CVariable
    {
        public List<CVariable> variables = new List<CVariable>();

        public CVector(CR2WFile cr2w)
            : base(cr2w)
        {
        }

        public override void Read(BinaryReader file, uint size)
        {
            var zero = file.ReadByte();

            // quests\minor_quests\skellige\mq2008_lured_into_drowners.w2phase
            // in a CVariant for class "@SItem"
            // ... okay CDPR, is that a joke or what?
            if (zero == 1)
            {
                int joke = file.ReadInt32();
            }

            while (true)
            {
                var var = cr2w.ReadVariable(file);
                if (var == null)
                    break;
                AddVariable(var);
            }
        }

        public override void Write(BinaryWriter file)
        {
            file.Write((byte) 0);
            for (var i = 0; i < variables.Count; i++)
            {
                CR2WFile.WriteVariable(file, variables[i]);
            }
            file.Write((ushort) 0);
        }

        public override CVariable Create(CR2WFile cr2w)
        {
            return new CVector(cr2w);
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var obj = (CVector) base.Copy(context);
            foreach (var item in variables)
            {
                if (context.ShouldCopy(item))
                {
                    obj.AddVariable(item.Copy(context));
                }
            }
            return obj;
        }

        public override Control GetEditor()
        {
            return null;
        }

        public override List<IEditableVariable> GetEditableVariables()
        {
            return variables.Cast<IEditableVariable>().ToList();
        }

        public override bool CanRemoveVariable(IEditableVariable child)
        {
            if (child is CVariable)
            {
                var v = (CVariable) child;
                return variables.Contains(v);
            }

            return false;
        }

        public override bool CanAddVariable(IEditableVariable newvar)
        {
            return newvar == null || newvar is CVariable;
        }

        public override void AddVariable(CVariable var)
        {
            variables.Add(var);
            if (var != null)
                var.ParentVariable = this;
        }

        public override void RemoveVariable(IEditableVariable child)
        {
            if (child is CVariable)
            {
                var v = (CVariable) child;
                variables.Remove(v);
                v.ParentVariable = null;
            }
        }

        public override string ToString()
        {
            return "";
        }

        public override void SerializeToXml(XmlWriter xw)
        {
            DataContractSerializer ser = new DataContractSerializer(this.GetType());
            using (var ms = new MemoryStream())
            {
                ser.WriteStartObject(xw, this);
                ser.WriteObjectContent(xw, this);

                if (GetEditableVariables() != null)
                {
                    xw.WriteStartElement("variables");
                    foreach (var v in GetEditableVariables())
                    {
                        v.SerializeToXml(xw);
                    }
                    xw.WriteEndElement();
                }
                ser.WriteEndObject(xw);
            }
        }
    }
}