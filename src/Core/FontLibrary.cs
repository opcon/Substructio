using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickFont;
using QuickFont.Configuration;
using System.Collections.ObjectModel;
using OpenTK;

namespace Substructio.Core
{
    public class FontLibrary : IDisposable
    {
        List<GameFont> _fontCollection;
        ReadOnlyCollection<GameFont> FontCollection { get { return _fontCollection.AsReadOnly(); } }

        public FontLibrary()
        {
            _fontCollection = new List<GameFont>();
        }

        public void AddFont(GameFont font)
        {
            if (!_fontCollection.Contains(font))
                _fontCollection.Add(font);
        }

        public void RemoveFont(GameFont font)
        {
            if (_fontCollection.Contains(font))
                _fontCollection.Remove(font);
        }

        public GameFont GetFirstOrDefault(GameFontType fontType)
        {
            return GetFirstOrDefault(GameFont.GetLookupString(fontType));
        }
        public GameFont GetFirstOrDefault(string fontType)
        {
            return _fontCollection.FirstOrDefault(gf => gf.FontType.Equals(fontType, StringComparison.OrdinalIgnoreCase)) ??
                _fontCollection.First(gf => gf.FontType.Equals(GameFont.DEFAULT));
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var gf in _fontCollection)
                    {
                        if (gf != null)
                        {
                            gf.Dispose();
                        }
                    }
                    _fontCollection.Clear();
                    _fontCollection = null;
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
    }

    public class GameFont : IDisposable
    {
        public const string HEADING = "heading";
        public const string BODY = "body";
        public const string DEFAULT = "default";
        public const string MENU = "menu";

        public QFont Font { get; set; }
        public string FontType { get; private set; }
        public Vector2 OptimalScreenDimensions { get; private set; }
        public int MaxLineHeight { get { return Font.MaxLineHeight; } }

        public GameFont(QFont font, string fontType, Vector2 screenDimensions)
        {
            Font = font;
            FontType = fontType;
            OptimalScreenDimensions = screenDimensions;
        }

        public GameFont(QFont font, GameFontType fontType, Vector2 screenDimensions)
        {
            Font = font;
            OptimalScreenDimensions = screenDimensions;
            FontType = GetLookupString(fontType);
        }

        public bool NeedsResizing(int currentWidth, int currentHeight)
        {
            return !(System.Math.Abs(OptimalScreenDimensions.X - currentWidth) < float.Epsilon
                    && System.Math.Abs(OptimalScreenDimensions.Y - currentHeight) < float.Epsilon);
        }

        public override string ToString()
        {
            return Font.ToString();
        }

        public static string GetLookupString(GameFontType fontType)
        {
            switch (fontType)
            {
                case GameFontType.Heading:
                    return HEADING;
                case GameFontType.Body:
                    return BODY;
                case GameFontType.Menu:
                    return MENU;
                case GameFontType.Default:
                default:
                    return DEFAULT;
            }
        }

        public void Dispose()
        {
            if (Font != null)
            {
                Font.Dispose();
                Font = null;
            }
        }
    }

    public enum GameFontType
    {
        Heading,
        Body,
        Default,
        Menu,
    }
}
