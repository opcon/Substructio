using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFont;

namespace Substructio.Core
{
    public class FontLibrary
    {
        List<QFont> _fontCollection;

        public FontLibrary()
        {
            _fontCollection = new List<QFont>();
        }

    }

    public class GameFont
    {
        public QFont Font { get; private set; }

        public override string ToString()
        {
            return Font.ToString();
        }
    }
}
