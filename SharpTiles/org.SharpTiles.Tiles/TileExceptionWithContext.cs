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
using org.SharpTiles.Common;

namespace org.SharpTiles.Tiles
{
    public class TileExceptionWithContext : ExceptionWithContext
    {
        public TileExceptionWithContext(string msg)
            : base(msg)
        {
        }

        public static TileExceptionWithContext ErrorInTile(string path, ExceptionWithContext exception)
        {
            String msg = String.Format("Error in tile {0}:{1}", path, exception.Message);
            return MakePartial(new TileExceptionWithContext(msg)).Decorate(exception.Context).KeepHttpErrorCode(exception);
        }

        public static Exception ErrorInTile(TileException te, ParseContext context)
        {
            return MakePartial(new TileExceptionWithContext(te.Message)).Decorate(context).KeepHttpErrorCode(te);
        }

        private TileExceptionWithContext KeepHttpErrorCode(IHaveHttpErrorCode httpErrorCode)
        {
            if (httpErrorCode != null && httpErrorCode.HttpErrorCode.HasValue)
                HttpErrorCode = httpErrorCode.HttpErrorCode;
            return this;
        }

        public TileExceptionWithContext AddErrorCodeIfNull(int code)
        {
            if (!HttpErrorCode.HasValue)
                HttpErrorCode = code;
            return this;
        }


    }
}