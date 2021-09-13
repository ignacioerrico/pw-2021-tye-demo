namespace Todo.Core.Dto
{
    public class StatsDto
    {
        public int AllTimeTotal { get; set; }
        public double AllTimePercentageDone { get; set; }
        public double AllTimePercentageProcrastinated { get; set; }

        public int FutureTasksTotal { get; set; }
        public double FutureTasksPercentageDone { get; set; }
        public double FutureTasksPercentageProcrastinated { get; set; }
    }
}
