// See https://aka.ms/new-console-template for more information

using System.Text;

namespace WatchMe
{
    class FWMainClass
    {
        private static FileLogger? _fileLogger;
        static void Main(string[] args)
        {
            FileLogger fileLogger = new(DateTime.Now, EventEnum.UNDEFINED, args[0]);
            _fileLogger = fileLogger;
            using var watcher = new FileSystemWatcher(args[0]);

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            watcher.Filter = "*.*";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            DateTime now = DateTime.Now;
            _fileLogger.WriteToFile(now, EventEnum.CREATED, _fileLogger.FullPath);
            string value = $"{now}, Created: {e.FullPath}";
            Console.WriteLine(value);
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            DateTime now = DateTime.Now;
            _fileLogger.WriteToFile(now, EventEnum.CHANGED, _fileLogger.FullPath);
            Console.WriteLine($"{now}, Changed: {e.FullPath}");
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {
            DateTime now = DateTime.Now;
            _fileLogger.WriteToFile(now, EventEnum.DELETED, _fileLogger.FullPath);
            string value = $"{now}, Deleted: {e.FullPath}";
            Console.WriteLine(value);
        }

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            DateTime now = DateTime.Now;
            _fileLogger.WriteToFile(now, EventEnum.RENAMED, _fileLogger.FullPath);
            StringBuilder stringBuilder = new();
            
            string line = $"{now}, Renamed:";
            stringBuilder.AppendLine(line);
            Console.WriteLine(line);

            line = $"    Old: {e.OldFullPath}";
            stringBuilder.AppendLine(line);
            Console.WriteLine(line);

            line = $"    New: {e.FullPath}";
            stringBuilder.AppendLine(line);
            Console.WriteLine(line);
            _fileLogger.WriteToFile(now, EventEnum.RENAMED, _fileLogger.FullPath, stringBuilder.ToString());
        }

        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"{DateTime.Now}, Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}