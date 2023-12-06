namespace Advent_Of_Code_day_5
{
    class Mapping
    {
        public long DestinationStart { get; set; }
        public long SourceStart { get; set; }
        public long Length { get; set; }
    }

    class CategoryMap
    {
        public List<Mapping> Mappings { get; set; }
    }

    class Almanac
    {
        public List<long> Seeds { get; set; }
        public Dictionary<string, CategoryMap> Maps { get; set; }
    }
}
