namespace System
{
    using Character;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor.Callbacks;
#endif

    public static class Globals
    {
        public static Player player;
        public static SoundManager soundManager;
        public static CameraManager camera;
        public static StatsTextManager statsTextManager;
        public static ObjectiveManager objectiveManager;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init()
        {
            AssignReferences();
        }

#if UNITY_EDITOR
        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            if (Application.isPlaying)
            {
                AssignReferences();
            }
        }
#endif

        private static void AssignReferences()
        {
            var playerObj = GameObject.Find("Player");
            if (playerObj != null) player = playerObj.GetComponent<Player>();

            var gmObj = GameObject.Find("GameManager");
            if (gmObj != null)
            {
                soundManager = gmObj.GetComponent<SoundManager>();
                camera = gmObj.GetComponent<CameraManager>();
                statsTextManager = gmObj.GetComponent<StatsTextManager>();
                objectiveManager = gmObj.GetComponent<ObjectiveManager>();
            }
        }
    }
}