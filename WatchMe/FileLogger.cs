namespace WatchMe
{
    class FileLogger
    {
        public DateTime Date { get; set; }
        public EventEnum Event { get; set; }
        public string? FullPath { get; set; }
        public string? FileName { get; private set; }
        private StreamWriter? _file;

        public FileLogger()
        {
            Date = DateTime.Now;
            Event = EventEnum.UNDEFINED;
            FullPath = null;
            FileName = null;
            _file = null;
        }

        public FileLogger(DateTime date, EventEnum aEvent, string fullPath)
        {
            Date = date;
            Event = aEvent;
            FullPath = fullPath;
            FileName = Date + "_" + new string(FullPath.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-').ToArray());
            _file = new(FileName, append: true);
            WriteToFile(Date, Event, FullPath);
        }

        public async void WriteToFile(DateTime date, EventEnum aEvent, string fullPath)
        {
            string preparedLineToWrite = date + ", " + aEvent + ": " + fullPath;
            await _file.WriteLineAsync(preparedLineToWrite);
        }

        public async void WriteToFile(DateTime date, EventEnum aEvent, string fullPath, string description)
        {
            string preparedLineToWrite = date + ", " + aEvent + ": " + fullPath;
            preparedLineToWrite += "\n" + description;
            await _file.WriteLineAsync(preparedLineToWrite);
        }
    }
}
