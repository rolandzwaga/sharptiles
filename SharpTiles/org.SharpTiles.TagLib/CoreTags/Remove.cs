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
 using System.ComponentModel;
using org.SharpTiles.Common;

namespace org.SharpTiles.Tags.CoreTags
{
    [Category("GeneralPurpose"), HasExample]
    public class Remove : BaseCoreTag, ITag
    {
        public static readonly string NAME = "remove";


        [Required]
        public ITagAttribute Var { get; set; }

        [EnumProperyType(typeof(VariableScope))]
        [TagDefaultValue(VariableScope.Page)]
        public ITagAttribute Scope { get; set; }

        #region ITag Members

        public string TagName
        {
            get { return NAME; }
        }

        public string Evaluate(TagModel model)
        {
            string var = GetAsString(Var, model);
            VariableScope scope = GetAutoValueAs<VariableScope>("Scope", model).Value;
            model[scope + "." + var] = null;
            return null;
        }

        public TagBodyMode TagBodyMode
        {
            get { return TagBodyMode.None; }
        }

        #endregion
    }
}
