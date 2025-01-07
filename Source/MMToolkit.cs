using Verse;

namespace MindMattersInterface
{
    public static class MMToolkit
    {
        private static readonly HashSet<string> GripeMessages = new();
        private static int GripeKeyCounter = 0;

        

        public static void DebugLog(string message)
        {
            if (!MindMattersAPI.EnableLogging) return; // Respect the logging setting

            Log.Message($"[MindMattersInterface] {message}");
        }
        
        public static void DebugWarn(string message)
        {
            if (!MindMattersAPI.EnableLogging) return; // Respect the logging setting

            Log.Warning($"[MindMattersInterface] {message}");
        }

        public static void GripeOnce(string message)
        {
            if (!MindMattersAPI.EnableLogging) return; // Respect the logging setting

            int gripeKey = GetGripeKey(message);
            Log.WarningOnce($"[MindMattersInterface] {message}", gripeKey);
        }

        private static int GetGripeKey(string message)
        {
            // Generate or retrieve a unique key for the message
            if (!GripeMessages.Contains(message))
            {
                GripeMessages.Add(message);
                GripeKeyCounter++;
            }

            return message.GetHashCode();
        }
    }
}