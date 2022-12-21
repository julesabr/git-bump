namespace Julesabr.IO {
    public class FileFactory {
        public virtual IText Create(string path) {
            return new File(path);
        }
    }
}
