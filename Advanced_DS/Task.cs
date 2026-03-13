namespace DataStructureStudy
{
    public partial class Task : IComparable<Task>
    {
        public Task(string description, int priorty)
        {
            Description = description;
            Priorty = priorty;
        }

        public string Description { get; private set; }
        public int Priorty { get; private set; }


        static private int _counter = 0;
        public override string ToString() => $"\n{++_counter}) {Description} , Priorty({Priorty})";

        public int CompareTo(Task? other) => this.Priorty.CompareTo(other?.Priorty);
    }
}
