namespace BehaiviourTree
{
    public abstract class Node
    {
        public NodeStatus Status = NodeStatus.Uninitialised;

        protected BlackBoard blackboard;
        private readonly Timer interuptionTimer = new(.3f);

        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }

        public NodeStatus Tick()
        {
            if (Status == NodeStatus.Uninitialised || interuptionTimer.IsFinished)
            {
                Status = NodeStatus.Running;
                OnEnter();
            }

            interuptionTimer.Restart();

            OnUpdate();

            NodeStatus returnStatus = Status;

            if (Status == NodeStatus.Success || Status == NodeStatus.Failed)
            {
                Reset();
            }

            return returnStatus;
        }

        public void Reset()
        {
            Status = NodeStatus.Uninitialised;
            OnExit();
        }

        public virtual void SetupBlackboard(BlackBoard blackboard)
        {
            this.blackboard = blackboard;
        }
    }
}
