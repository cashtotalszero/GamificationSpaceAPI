using UnityEngine;

namespace HTMLEngine.Unity3D
{
    public static class HtmlGUI
    {
        public static float lastCompilerTookSeconds;
        public static float lastDrawTookSeconds;

        public static void Label(Rect rect, string htmlText)
        {
            if (string.IsNullOrEmpty(htmlText)) return;
            var f = Time.realtimeSinceStartup;
            using (var compiler = HtEngine.GetCompiler())
            {
                compiler.Compile(htmlText, (int) rect.width);
                lastCompilerTookSeconds = Time.realtimeSinceStartup - f;
                f = Time.realtimeSinceStartup;
                GUI.BeginGroup(rect);
                compiler.Draw(Time.deltaTime);
                GUI.EndGroup();
                lastDrawTookSeconds = Time.realtimeSinceStartup - f;
            }
        }
    }
}
