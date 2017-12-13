using Blue.Updater;
using UnityEditor;

// by @Bullrich

namespace Blue.Menu.Updater
{
    public class ConsoleUpdater : PluginUpdater
    {
        private const string
            USERNAME = "Bullrich",
            REPONAME = "Unity-Console",
            CUREENT_VERSION = "1.2";

        [MenuItem("Window/Blue/Console/Search for updates")]
        public static void SearchUpdate()
        {
            SearchForUpdate(CUREENT_VERSION, USERNAME, REPONAME);
        }
    }
}