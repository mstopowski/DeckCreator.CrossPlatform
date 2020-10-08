namespace DeckCreator.Common
{
    public class DeckCreatorCommonPlugin : Rhino.PlugIns.PlugIn
	{
        public static DeckCreatorCommonPlugin Instance { get; private set; }

        public DeckCreatorCommonPlugin()
		{
            Instance = this;
        }
    }
}
