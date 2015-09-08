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
 namespace org.SharpTiles.Expressions
{
    public class GreaterThanParser : TupleExpressionParser<GreaterThan>
    {
        private static string GREATTHAN_ADDITIONAL = "gt";
        private static string GREATTHAN_SIGN = ">";

        public override ExpressionOperatorSign DistinctToken
        {
            get { return new ExpressionOperatorSign(GREATTHAN_SIGN, false); }
        }

        public override ExpressionOperatorSign AdditionalToken
        {
            get { return new ExpressionOperatorSign(GREATTHAN_ADDITIONAL, true); }
        }


        public override GreaterThan Create(Expression lhs)
        {
            return new GreaterThan(lhs);
        }
    }
}
