using System;
using Rhino.Commands;
using Rhino.UI;

namespace DeckCreator.Common.Commands
{
    public class DeckCreatorCommonCommand : Command
    {
        private Views.DeckCreatorEtoModelessForm Form { get; set; }

        public override string EnglishName => "DeckCreator";

        protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
        {
            if (null == Form)
            {
                Form = new Views.DeckCreatorEtoModelessForm{ Owner = RhinoEtoApp.MainWindow };
                Form.RestorePosition();
                Form.Closed += OnFormClosed;
                Form.Show();
            }
            return Result.Success;
        }

        private void OnFormClosed(object sender, EventArgs e)
        {
            Form.SavePosition();
            Form.Dispose();
            Form = null;
        }
    }
}