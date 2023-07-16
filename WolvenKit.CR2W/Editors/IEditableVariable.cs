﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using WolvenKit.CR2W.Types;

namespace WolvenKit.CR2W.Editors
{
    public interface IEditableVariable
    {
        string Name { get; set; }
        string Type { get; set; }
        CR2WFile CR2WOwner { get; }
        Control GetEditor();
        List<IEditableVariable> GetEditableVariables();
        bool CanRemoveVariable(IEditableVariable child);
        CVariable CreateDefaultVariable();
        bool CanAddVariable(IEditableVariable newvar);
        void AddVariable(CVariable var);
        void RemoveVariable(IEditableVariable child);
        void SerializeToXml(XmlWriter xw);
        void SerializeToJson(JsonWriter xw);
    }
}