using System;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Julesabr.GitBump {
    public partial interface IVersion {
        // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
        public class Factory {
            [Pure]
            public virtual IVersion Create(string value) {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(value), BlankStringError);

                Match match = Regex.Match(value, RegexPattern);
                if (!match.Success)
                    throw new ArgumentException(string.Format(InvalidStringFormatError, value));

                IVersion result;

                string[] revisions = value.Split(Separator);
                if (revisions.Length == 5)
                    result = new Version(ushort.Parse(revisions[0]), ushort.Parse(revisions[1]),
                        ushort.Parse(revisions[2]), revisions[3], ushort.Parse(revisions[4]));
                else
                    result = new Version(ushort.Parse(revisions[0]), ushort.Parse(revisions[1]),
                        ushort.Parse(revisions[2]));

                return result;
            }
        }
    }
}
