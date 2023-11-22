namespace BehaiviourTree
{
    public abstract class CompositeNode : Node
    {
        protected readonly Node[] childNodes;

        public CompositeNode(params Node[] childNodes)
        {
            this.childNodes = childNodes;
        }
    }
}

