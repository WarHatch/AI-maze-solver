namespace Labirinth
{
    struct Move 
    {
        public readonly Point to;
        public readonly int rule;

        public Move(Point to, int rule)
        {
            this.to = to;
            this.rule = rule;
        }
    }
}
