namespace BlockBlast
{
    public class BlockLayout
    {
        class L_Shape
        {
            public static readonly bool[,] shape = new bool[,]
            {
                { true, false },
                { true, false },
                { true, true }
            };
        }
    

        class T_Shape
        {
            public static readonly bool[,] shape = new bool[,]
            {
                { true, true, true },
                { false, true, false },
                { false, true, false }
            };
        }

        class i_Shape
        {
            public static readonly bool[,] shape = new bool[,]
            {
                { true },
                { true },
                { true },
                { true }
            };
        }

        class O_Shape
        {
            public static readonly bool[,] shape = new bool[,]
            {
                { true, true },
                { true, true }
            };
        }

        class S_Shape
        {
            public static readonly bool[,] shape = new bool[,]
            {
                { false, true, true },
                { true, true, false }
            };
        }

    
    }
}
