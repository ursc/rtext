using System;

namespace rtext
{
    public class FindReplaceData
    {
        private bool isCaseSensitive;
        public bool IsCaseSensitive { get { return isCaseSensitive; } }

        private bool isRegex;
        public bool IsRegex { get { return isRegex; } }

        private string findWhat;
        public string FindWhat { get { return findWhat; } }

        private string replaceWith;
        public string ReplaceWith { get { return replaceWith; } }

        private bool replace;
        public bool Replace { get { return replace; } }

        public string Filename;
        public string Result;

        public FindReplaceData(bool isCaseSensitive, bool isRegex, string findWhat, string replaceWith, bool replace)
        {
            this.isCaseSensitive = isCaseSensitive;
            this.isRegex = isRegex;
            this.findWhat = findWhat;
            this.replaceWith = replaceWith;
            this.replace = replace;
        }
    }
}
