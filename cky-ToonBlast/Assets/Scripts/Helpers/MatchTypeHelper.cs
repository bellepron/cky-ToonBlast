using cky.Enums;

namespace cky.Helpers
{
    public static class MatchTypeHelper
    {
        public static MatchType SetMatchType(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.None:
                    return MatchType.None;

                case ItemType.GreenCube:
                    return MatchType.Green;

                case ItemType.YellowCube:
                    return MatchType.Yellow;

                case ItemType.BlueCube:
                    return MatchType.Blue;

                case ItemType.RedCube:
                    return MatchType.Red;


                case ItemType.VerticalRocket:
                    return MatchType.Special;

                case ItemType.HorizontalRocket:
                    return MatchType.Special;

                case ItemType.Bomb:
                    return MatchType.Special;



                default:
                    return MatchType.None;
            }
        }
    }
}