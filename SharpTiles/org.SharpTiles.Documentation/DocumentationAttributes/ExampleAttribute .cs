/*
 * SharpTiles, R.Z. Slijp(2008), www.sharptiles.org
 *
 * This file is part of SharpTiles.
 * 
 * SharpTiles is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SharpTiles is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with SharpTiles.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MarkdownDeep;
using org.SharpTiles.Common;

namespace org.SharpTiles.Documentation.DocumentationAttributes
{
    [DataContract]
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class ExampleAttribute : Attribute
    {
        public ExampleAttribute(string value, string description = null)
        {
            Value = value;
            Description = description;
        }

        [DataMember]
        public string Value { get; private set; }
        [DataMember]
        public string Description { get; set; }

        public string Html => AsCodeHtml(Value);
        public string DescriptionHtml => AsDescriptionHtml(Description);


        public static bool Harvest(Type type)
        {
            var example = HarvestTags(type).FirstOrDefault();
            if (example == null) return false;
            return true;
        }

        public static ExampleAttribute[] HarvestTags(Type type)
        {
            return
                type.GetCustomAttributes(typeof (ExampleAttribute), false)
                    .Cast<ExampleAttribute>()
                    .ToArray();
        }

        public static string AsCodeHtml(string code) => StringUtils.EscapeXml(code)
            .Replace("\n", "<br/>")
            .Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;")
            .Replace("    ", "&nbsp;&nbsp;&nbsp;&nbsp;");

        public static string AsDescriptionHtml(string description)
            => new Markdown {ExtraMode = true}.Transform(description);
    }
}
