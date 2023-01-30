namespace cky.Structs
{
    [System.Serializable]
    public struct GridData
    {
        public int i;
        public int j;

        public GridData(int i, int j)
        {
            this.i = i;
            this.j = j;
        }
    }
}