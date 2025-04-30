namespace EF4Unity.Scripts.Core
{
    public static class Startup
    {
        private static bool _isInitialized;

        public static void SetupSQLCipher()
        {
            if (_isInitialized)
                return;
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlcipher());
            _isInitialized = true;
        }
        
        public static void SetupSQLite()
        {
            if (_isInitialized)
                return;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN // ---- Windows
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX // -- MacOS
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_sqlite3());
#elif UNITY_ANDROID // ----------------------------- Android
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
#elif UNITY_IOS // --------------------------------- iOS
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_sqlite3());
#else
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
#endif
            _isInitialized = true;
        }
    }
}