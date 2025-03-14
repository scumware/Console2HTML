using System;
using System.Collections.Generic;
using System.Text;

namespace Console2HTML
{
    public class HtmlGenerator
    {
        private readonly Dictionary<ConsoleColor, string> m_styles;
        private readonly ConsoleColor m_defaultColor;

        public HtmlGenerator()
        {
           m_styles = new Dictionary<ConsoleColor, string>();
           m_defaultColor = ConsoleColor.Gray; // Цвет по умолчанию
           m_styles[m_defaultColor] = "defaultColor";
        }

        public string GenerateHtmlFromArray(CharWithColor[,] array)
        {
            var columns = array.GetLength(1);
            var rows = array.GetLength(0);

            GenerateStylesAndDetermineBoundaries(array, rows, columns, out var maxRow, out var minRow);

            var cssBuilder = GenerateCSSStyles();

            return GenerateHtml(array, cssBuilder, minRow, maxRow, columns);
        }

        private string GenerateHtml(CharWithColor[,] array, StringBuilder cssBuilder, int minRow, int maxRow, int columns)
        {
            // Генерация HTML
            var htmlBuilder = new StringBuilder();
            htmlBuilder.AppendLine("<html>");
            htmlBuilder.AppendLine("<head>");
            htmlBuilder.Append(cssBuilder);
            htmlBuilder.AppendLine("</head>");
            htmlBuilder.AppendLine("<body>");
            htmlBuilder.AppendLine("<pre>");

            // Второй проход: формируем HTML с учетом стилей
            for (int row = minRow; row <= maxRow; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    var charWithColor = array[row, col];
                    var chr = charWithColor.Char;
                    if (chr == '\0')
                    {
                        break; // Игнорируем оставшиеся символы в строке
                    }

                    if (chr == ' ')
                    {
                        htmlBuilder.Append(' ');
                    }
                    else
                    {
                        // Определяем стиль для текущего символа
                        string styleClass = m_styles.ContainsKey(charWithColor.Color) ? m_styles[charWithColor.Color] : m_styles[m_defaultColor];

                        // Группируем символы одного цвета
                        if (col > 0 && array[row, col - 1].Color == charWithColor.Color && array[row, col - 1].Char != ' ')
                        {
                            var escString = EscapeHtmlCharacters(chr, out var escaped);
                            if (escaped)
                                htmlBuilder.Append(escString);
                            else
                                htmlBuilder.Append(chr);
                        }
                        else
                        {
                            htmlBuilder.Append($"<span class='{styleClass}'>{chr}");
                        }

                        // Закрываем тег, если следующий символ другого цвета или пробел
                        if (col < array.GetLength(1) - 1 && (array[row, col + 1].Color != charWithColor.Color || array[row, col + 1].Char == ' '))
                        {
                            htmlBuilder.Append("</span>");
                        }
                    }
                }

                htmlBuilder.AppendLine();
            }

            htmlBuilder.AppendLine("</pre>");
            htmlBuilder.AppendLine("</body>");
            htmlBuilder.AppendLine("</html>");

            return htmlBuilder.ToString();
        }

        private StringBuilder GenerateCSSStyles()
        {
            var cssBuilder = new StringBuilder();
            cssBuilder.AppendLine("<style>");
            cssBuilder.AppendLine("body { background-color: black; }");
            cssBuilder.AppendLine(".defaultColor { color: #808080; font-family: monospace; }"); // Стиль по умолчанию
            foreach (var style in m_styles)
            {
                if (style.Key != m_defaultColor)
                {
                    cssBuilder.AppendLine($".{style.Value} {{ color: {ConsoleColorToHex(style.Key)}; font-family: monospace; }}");
                }
            }

            cssBuilder.AppendLine("</style>");
            return cssBuilder;
        }

        private void GenerateStylesAndDetermineBoundaries(CharWithColor[,] array, int rows, int columns, out int maxRow, out int minRow)
        {
            minRow = 0;
            maxRow = 0;
            for (int row = 0; row < rows; row++)
            {
                bool isEmptyRow = true;
                for (int col = 0; col < columns-1; col++)
                {
                    var charWithColor = array[row, col];
                    if (charWithColor.Char != '\0' && charWithColor.Char != ' ')
                    {
                        isEmptyRow = false;
                        if (!m_styles.ContainsKey(charWithColor.Color))
                        {
                            m_styles[charWithColor.Color] = $"color{charWithColor.Color}";
                        }
                    }
                }
                if (!isEmptyRow)
                {
                    maxRow = row;
                    if (minRow == 0)
                        minRow = row;
                }
            }
        }

        static string EscapeHtmlCharacters(char chr, out bool escaped)
        {
            escaped = true;
            switch (chr)
            {
                case '<':
                    return "&lt;";
                case '>':
                    return "&gt;";
                case '&':
                    return "&amp;";
                case '\"':
                    return "&quot;";
                case '\'':
                    return "&apos;";
                    
                default:
                    escaped = false;
                    return "";
            }
        }

        static string ConsoleColorToHex(ConsoleColor color)
        {
            // Преобразуем ConsoleColor в HEX-код цвета
            switch (color)
            {
                case ConsoleColor.Black: return "#000000";
                case ConsoleColor.DarkBlue: return "#00008B";
                case ConsoleColor.DarkGreen: return "#006400";
                case ConsoleColor.DarkCyan: return "#008B8B";
                case ConsoleColor.DarkRed: return "#8B0000";
                case ConsoleColor.DarkMagenta: return "#8B008B";
                case ConsoleColor.DarkYellow: return "#808000";
                case ConsoleColor.Gray: return "#808080";
                case ConsoleColor.DarkGray: return "#A9A9A9";
                case ConsoleColor.Blue: return "#0000FF";
                case ConsoleColor.Green: return "#00FF00";
                case ConsoleColor.Cyan: return "#00FFFF";
                case ConsoleColor.Red: return "#FF0000";
                case ConsoleColor.Magenta: return "#FF00FF";
                case ConsoleColor.Yellow: return "#FFFF00";
                case ConsoleColor.White: return "#FFFFFF";
                default: return "#000000";
            }
        }
    }
}