namespace Julesabr.IO {
    internal class File : IText {
        private readonly string path;

        internal File(string path) {
            this.path = path;
        }

        public void Write(string content) {
            System.IO.File.WriteAllText(path, content);
        }
    }
}
