namespace Julesabr.IO {
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class FileFactory {
        public virtual IText Create(string path) {
            return new File(path);
        }
    }
}
