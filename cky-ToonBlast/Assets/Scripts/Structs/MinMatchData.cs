namespace cky.Structs
{
    [System.Serializable]
    public struct MinMatchData
    {
        public int minToMacth;
        public int minMacthToRocket;
        public int minMacthToBomb;

        public MinMatchData(int minToMacth, int minMacthToRocket, int minMacthToBomb)
        {
            this.minToMacth = minToMacth;
            this.minMacthToRocket = minMacthToRocket;
            this.minMacthToBomb = minMacthToBomb;
        }
    }
}