using System;
using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.DocObjects;

namespace DeckCreator.Common.Methods
{
    public class CreateDeckClass
    {
        public String Name { get; set; }
        public int Height { get; set; }

        public CreateDeckClass() { }

        public CreateDeckClass(String name, int height)
        {
            Name = name.ToUpper();
            Height = height;
        }

        public void CreateDeck()
        {
            String deckName = "DECKS";
            RhinoDoc doc = RhinoDoc.ActiveDoc;
            Layer layerBackUp = doc.Layers.CurrentLayer;

            if (doc.Layers.FindName(deckName) == null)
            {
                Layer allDecksLayer = new Layer
                {
                    Name = deckName
                };

                doc.Layers.Add(allDecksLayer);
            }

            if (doc.Layers.FindName(Height.ToString() + " " + Name) != null)
            {
                Rhino.UI.Dialogs.ShowMessage("That layer already exists.\r\nChange name or height.", "Warning");
                return;
            }

            Layer deckLayer = new Layer
            {
                Name = Height.ToString() + " " + Name,
                ParentLayerId = doc.Layers.FindIndex(doc.Layers.FindName(deckName).Index).Id
            };

            try
            {
                RhinoObject[] rhobjs = doc.Objects.FindByLayer("Shell");
                Brep[] breps = new Brep[rhobjs.Length];

                for (int i = 0; i < rhobjs.Length; i++)
                {
                    breps[i] = Brep.TryConvertBrep(rhobjs[i].Geometry);
                }

                Brep[] joinedBreps = Brep.JoinBreps(breps, 1);
                if (joinedBreps.Length > 1)
                {
                    // do something
                }

                Brep brep = joinedBreps[0];

                double bigNumber = 1000000;

                Plane plane = new Plane(new Point3d(-bigNumber, -bigNumber, Height),
                                      new Point3d(-bigNumber, bigNumber, Height),
                                      new Point3d(bigNumber, bigNumber, Height));

                if (Intersection.BrepPlane(brep, plane, 1, out Curve[] curves, out Point3d[] point3Ds))
                {
                    doc.Layers.Add(deckLayer);

                    doc.Layers.SetCurrentLayerIndex(doc.Layers.FindName(deckLayer.Name).Index, true);

                    Brep[] arrayBrep = Brep.CreatePlanarBreps(Curve.JoinCurves(curves), 1);

                    foreach (Brep item in arrayBrep)
                    {
                        doc.Objects.AddBrep(item);
                    }

                    doc.Views.Redraw();

                    // adding params to layer
                    try
                    {
                        doc.Layers.FindName(deckLayer.Name).UserData.Remove(doc.Layers.FindName(deckLayer.Name).UserData.Find(typeof(DeckData)));
                    }
                    catch (Exception ex)
                    {
                        RhinoApp.WriteLine(ex.ToString());
                    }

                    doc.Layers.FindName(deckLayer.Name).UserData.Add(new DeckData(Name, Height));
                }
            }
            catch (Exception ex)
            {
                RhinoApp.WriteLine(ex.ToString());
            }

            // Restoring previous layer and locking frameline layer
            doc.Layers.SetCurrentLayerIndex(layerBackUp.Index, true);
            if (doc.Layers.FindName(deckLayer.Name) != null)
            {
                doc.Layers.FindName(deckLayer.Name).IsLocked = true;
                //doc.Layers.FindIndex(doc.Layers.FindName(deckLayer.Name).Index).IsLocked = true;
            }
            doc.Layers.FindName(deckName).IsLocked = true;
            //doc.Layers.FindIndex(doc.Layers.FindName(deckName).Index).IsLocked = true;
        }

        public void Act()
        {
            RhinoApp.WriteLine($"Name: {Name}");
            RhinoApp.WriteLine($"Height: {Height}");
        }
    }
}
