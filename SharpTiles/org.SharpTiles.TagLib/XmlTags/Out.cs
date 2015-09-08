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
using System.ComponentModel;
using System.Reflection;
using System.Xml.XPath;
using org.SharpTiles.Common;
using org.SharpTiles.Tags.CoreTags;
using TypeConverter=org.SharpTiles.Common.TypeConverter;

namespace org.SharpTiles.Tags.XmlTags
{
    [Category("XMLActions"), HasExample]
    public class Out : BaseCoreTag, ITag
    {
        public const string NAME = "out";
        public static readonly PropertyInfo info = typeof (XPathNavigator).GetProperty("Value");
        
        [Required]
        public ITagAttribute Source { get; set; }

        [Required]
        public ITagAttribute Select { get; set; }

        [TagDefaultValue(true)]
        public ITagAttribute EscapeXml { get; set; }

        #region ITag Members

        public string TagName
        {
            get { return NAME; }
        }

        public string Evaluate(TagModel model)
        {
            try
            {
                XPathNodeIterator result = XmlHelper.GetAndEvaluate(Source, Select, model);
                var escapeXml = GetAutoValueAsBool("EscapeXml", model);
                string resultStr = result != null ? CollectionUtils.ToString(result, info) : String.Empty;
                return escapeXml ? StringUtils.EscapeXml(resultStr) : resultStr;
            } catch (XPathException XPe)
            {
                throw TagException.IllegalXPath(XPe).Decorate(Context);
            }
        }


        public TagBodyMode TagBodyMode
        {
            get { return TagBodyMode.None; }
        }

        #endregion
    }
}
