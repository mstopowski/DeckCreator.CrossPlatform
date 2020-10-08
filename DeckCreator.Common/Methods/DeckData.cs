using System;
using Rhino;
using Rhino.FileIO;
using Rhino.Collections;
using Rhino.DocObjects.Custom;
using System.Runtime.InteropServices;

namespace DeckCreator.Common.Methods
{
    [Guid("bd166663-dafb-40da-b3d4-641cb006ad87")]
    public class DeckData : UserData
    {
        public String Name { get; set; }
        public int Height { get; set; }

        public DeckData() { }

        public DeckData(String name, int height)
        {
            Name = name;
            Height = height;
        }

        public override bool ShouldWrite
        {
            get
            {
                if (Height > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public override string Description
        {
            get { return "Deck parameters:"; }
        }

        protected override void OnDuplicate(UserData source)
        {
            if (source is DeckData src)
            {
                Height = src.Height;
                Name = src.Name;
            }
        }

        protected override bool Read(BinaryArchiveReader archive)
        {
            ArchivableDictionary dict = archive.ReadDictionary();
            if (dict.ContainsKey("Name"))
            {
                RhinoApp.WriteLine("Reading in progress...");
                Name = (String)dict["Name"];
                Height = (int)dict["Height"];
            }
            return true;
        }
        protected override bool Write(BinaryArchiveWriter archive)
        {
            var dict = new ArchivableDictionary(1, "Parameters");
            dict.Set("Name", Name);
            dict.Set("Height", Height);

            archive.WriteDictionary(dict);
            return true;
        }
    }
}
