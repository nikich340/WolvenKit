﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolvenKit.Common.Model;

namespace WolvenKit.Common
{
    public class WitcherTreeNode
    {
        public WitcherTreeNode()
        {
            Directories = new Dictionary<string, WitcherTreeNode>();
            Files = new Dictionary<string, List<IWitcherFile>>();
            Name = "";
        }

        public WitcherTreeNode(EBundleType bundleType)
        {
            Directories = new Dictionary<string, WitcherTreeNode>();
            Files = new Dictionary<string, List<IWitcherFile>>();
            Name = "";
        }

        public EBundleType BundleType
        {
            get
            {
                var parts = FullPath.Split(Path.DirectorySeparatorChar);
                string bundlename = parts.First();
                if (parts.Count() > 1)
                {
                    bundlename = parts.Skip(1).First();
                }
                EBundleType type = EBundleType.Bundle;
                Enum.TryParse<EBundleType>(bundlename, out type);
                return type;
            }
        }

        public string FullPath
        {
            get
            {
                var path = "";
                var current = this;
                while (true)
                {
                    path = Path.Combine(current.Name, path);
                    current = current.Parent;
                    if (current == null)
                        break;
                }

                return path ?? "";
            }
        }

        public string Name { get; set; }
        public WitcherTreeNode Parent { get; set; }
        public Dictionary<string, WitcherTreeNode> Directories { get; set; }
        public Dictionary<string, List<IWitcherFile>> Files { get; set; }
    }
}
