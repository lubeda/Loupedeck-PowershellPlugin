namespace Loupedeck.PowershellPlugin
{
    
        using System;
        using System.Drawing;
        using System.Drawing.Drawing2D;
        using System.Drawing.Imaging;
        using System.Globalization;
        using System.IO;

        public static class PowershellDrawingHelper
        {
        public static GraphicsPath RoundedRect(Rectangle bounds, Int32 radius)
        {
            var diameter = radius * 2;
            var size = new Size(diameter, diameter);
            var arc = new Rectangle(bounds.Location, size);
            var path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc
            path.AddArc(arc, 180, 90);

            // top right arc
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }   
            
            public static BitmapBuilder LoadBitmapBuilder
                (BitmapImage image, String text = null, BitmapColor? textColor = null)
            {
                var builder = new BitmapBuilder(80, 80);

                builder.Clear(BitmapColor.Black);
                builder.DrawImage(image);
                // builder.FillRectangle(0, 0, 90, 90, new BitmapColor(0, 0, 0, 100));

                return text is null ? builder : builder.AddTextOutlined(text, textColor: textColor);
            }

            public static BitmapBuilder AddTextOutlined(this BitmapBuilder builder, String text,
                BitmapColor? outlineColor = null,
                BitmapColor? textColor = null, Int32 fontSize = 12)
            {
                // TODO: Make it outline
                builder.DrawText(text, 0, -30, 80, 80, textColor, fontSize, 0, 0);
                return builder;
            }

        
            public static BitmapImage DrawPercent(PluginImageSize imageSize, BitmapColor backgroundColor, BitmapColor foregroundColor, Single currentValue)
            {
                var dim = imageSize.GetDimension();
                var percentage = currentValue;
                var width = (Int32)(dim * percentage / 100.0);

                var builder = new BitmapBuilder(dim, dim);
                builder.Clear(BitmapColor.Black);

                builder.Translate(dim / 4, 0);
                builder.DrawRectangle(0, 2, dim / 2, dim - 2, backgroundColor);
                builder.FillRectangle(0, dim-2, dim / 2, -width, backgroundColor);
                builder.ResetMatrix();
                builder.DrawText(currentValue.ToString(CultureInfo.CurrentCulture), foregroundColor);
                return builder.ToImage();
            }

            public static Int32 GetDimension(this PluginImageSize size) => size == PluginImageSize.Width60 ? 50 : 80;
        }
    }
    