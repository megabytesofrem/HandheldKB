using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HandheldKB.Util
{
    public class DimColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush brush)
            {
                // Extract the Color from the SolidColorBrush
                Color originalColor = brush.Color;

                // Convert to HSL
                double h, s, l;
                ColorToHsl(originalColor, out h, out s, out l);

                // Adjust the lightness while preserving saturation and hue
                l *= 0.5; // Adjust the multiplier as needed

                // Convert back to RGB
                Color dimmedColor = HslToColor(h, s, l);

                // Return a new SolidColorBrush with the adjusted color
                return new SolidColorBrush(dimmedColor);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static void ColorToHsl(Color color, out double h, out double s, out double l)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));

            h = 0;
            s = 0;
            l = (max + min) / 2;

            if (max != min)
            {
                s = l < 0.5 ? (max - min) / (max + min) : (max - min) / (2.0 - max - min);

                if (max == r)
                {
                    h = (g - b) / (max - min) + (g < b ? 6 : 0);
                }
                else if (max == g)
                {
                    h = (b - r) / (max - min) + 2;
                }
                else if (max == b)
                {
                    h = (r - g) / (max - min) + 4;
                }

                h /= 6;
            }
        }

        private static Color HslToColor(double h, double s, double l)
        {
            double r, g, b;

            if (s == 0)
            {
                r = g = b = l;
            }
            else
            {
                double q = l < 0.5 ? l * (1 + s) : l + s - l * s;
                double p = 2 * l - q;

                r = HueToRgb(p, q, h + 1.0 / 3);
                g = HueToRgb(p, q, h);
                b = HueToRgb(p, q, h - 1.0 / 3);
            }

            return Color.FromRgb((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }

        private static double HueToRgb(double p, double q, double t)
        {
            if (t < 0) t += 1;
            if (t > 1) t -= 1;

            if (t < 1.0 / 6) return p + (q - p) * 6 * t;
            if (t < 1.0 / 2) return q;
            if (t < 2.0 / 3) return p + (q - p) * (2.0 / 3 - t) * 6;

            return p;
        }
    }
}