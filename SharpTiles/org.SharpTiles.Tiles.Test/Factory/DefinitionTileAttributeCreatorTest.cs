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
 */using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using org.SharpTiles.Tiles.Configuration;
using org.SharpTiles.Tiles.Factory;
using org.SharpTiles.Tiles.Test.Configuration;
using org.SharpTiles.Tiles.Tile;

namespace org.SharpTiles.Tiles.Test.Factory
{
    [TestFixture]
    public class DefinitionTileAttributeCreatorTest
    {
        [Test]
        public void CreatorShouldApplyWhenAttributeTileTypeIsSetToDefinition()
        {
            var entry = new XmlAttributeEntry
                            {
                                Type = TileType.Definition.ToString()
                            };
            Assert.That(new DefinitionTileAttributeCreator().Applies(entry));
        }

        [Test]
        public void CreatorShouldAssembleTileAttributeWithEmbeddedDefinitionTile()
        {
            var factory = new TilesFactory(new MockConfiguration());

            var entry = new XmlAttributeEntry
                            {
                                Name = "name",
                                Value = "definition",
                                Type = TileType.Definition.ToString()
                            };
            TileAttribute tile = new DefinitionTileAttributeCreator().Create(entry, factory);
            Assert.That(tile, Is.Not.Null);
            Assert.That(tile.Name, Is.EqualTo("name"));
            Assert.That(tile.Value, Is.Not.Null);
            Assert.That(tile.Value.GetType(), Is.EqualTo(typeof (TileReference)));
            Assert.That(((TileReference) tile.Value).Name, Is.EqualTo("definition"));
        }

        [Test]
        public void CreatorShouldNotApplyWhenAttributeTileTypeIsNotSet()
        {
            var entry = new XmlAttributeEntry
                            {
                                Type = null
                            };
            Assert.That(new DefinitionTileAttributeCreator().Applies(entry), Is.False);
        }

        [Test]
        public void CreatorShouldNotApplyWhenAttributeTileTypeIsSetToOtherValueThanDefinition()
        {
            var entry = new XmlAttributeEntry
                            {
                                Type = TileType.String.ToString()
                            };
            Assert.That(new DefinitionTileAttributeCreator().Applies(entry), Is.False);
        }
    }
}
