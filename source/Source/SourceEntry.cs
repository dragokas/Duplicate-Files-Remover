using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DuplicatesFinder
{
    internal class SourceEntry : IEquatable<SourceEntry>
    {
        internal string Path { get; set; } = String.Empty;
        internal bool IsProtected { get; set; } = false;
        internal bool IsDirectory { get; set; } = false;

        internal SourceEntry() { }
        internal SourceEntry(string path)
        {
            this.Path = path;
        }

        bool IEquatable<SourceEntry>.Equals(SourceEntry? other)
            => other is null ? false : StringComparer.OrdinalIgnoreCase.Equals(this.Path, other.Path);
        public override bool Equals(object? other)
            => Equals(other as SourceEntry);
        public override int GetHashCode()
            => StringComparer.OrdinalIgnoreCase.GetHashCode(this.Path);
    }

    internal static class HashSetExtensions
    {
        public static bool Contains(this HashSet<SourceEntry> set, string path)
            => set.Contains(new SourceEntry { Path = path });

        public static bool TryGetValue(this HashSet<SourceEntry> set,
            string path, [MaybeNullWhen(false)] out SourceEntry entry)
        {
            return set.TryGetValue(new SourceEntry { Path = path }, out entry);
        }
    }
}
