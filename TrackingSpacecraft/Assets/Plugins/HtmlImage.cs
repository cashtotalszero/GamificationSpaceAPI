using System;
using UnityEngine;

namespace HTMLEngine.Unity3D
{
    /// <summary>
    /// Provides image for use with HTMLEngine. Implements abstract class.
    /// </summary>
    public class HtmlImage : HtImage
    {
        /// <summary>
        /// Is special kind of image? (shows time)
        /// </summary>
        private readonly bool isTime;
        /// <summary>
        /// GUIStyle for time
        /// </summary>
        private readonly GUIStyle timeStyle;
        /// <summary>
        /// Loaded texture
        /// </summary>
        public readonly Texture2D Texture;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">src attribute from img tag</param>
        public HtmlImage(string source)
        {
            if ("#time".Equals(source, StringComparison.InvariantCultureIgnoreCase))
            {
                isTime = true;
                timeStyle = new GUIStyle();
                timeStyle.font = Resources.Load("fonts/code16") as Font;
                timeStyle.normal.textColor = Color.white;
                timeStyle.alignment = TextAnchor.MiddleCenter;
            }
            else
            {
                this.Texture = Resources.Load(source, typeof (Texture2D)) as Texture2D;
                if (this.Texture == null)
                {
                    Debug.LogError("Could not load html image from " + source);
                }
            }
        }

        /// <summary>
        /// Returns width of image
        /// </summary>
        public override int Width { get
        {
            if (isTime) return 120;
            return this.Texture == null ? 1 : this.Texture.width;
        } }

        /// <summary>
        /// Returns height of image
        /// </summary>
        public override int Height { get
        {
            if (isTime) return 20;
            return this.Texture == null ? 1 : this.Texture.height;
        } }

        /// <summary>
        /// Draw method
        /// </summary>
        /// <param name="rect">Where to draw</param>
        /// <param name="color">Color to use (ignored for now)</param>
        public override void Draw(HtRect rect, HtColor color)
        {
            if (isTime)
            {
                var now = DateTime.Now;
                timeStyle.Draw(new Rect(rect.X, rect.Y, rect.Width, rect.Height), 
                string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",now.Hour,now.Minute,now.Second,now.Millisecond),
                               false, false, false, false);
            }
            else if (this.Texture != null)
            {
                GUI.DrawTexture(new Rect(rect.X, rect.Y, rect.Width, rect.Height), this.Texture);
            }
        }
    }

}