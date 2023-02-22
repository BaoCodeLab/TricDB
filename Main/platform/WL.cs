namespace Main.platform
{
    public class WL
    {
        private static int wlusernum = 0;
        public int add()
        {
            wlusernum++;
            return wlusernum;
        }

        public int reduce()
        {
            wlusernum--;
            return wlusernum;
        }
        public int get()
        {
            return wlusernum;
        }
    }
}
