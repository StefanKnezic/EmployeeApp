namespace EmployeeApp.Models
{
    public class TaskModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set;}

        public EmployeeModel Employee { get; set; }

        public bool TaskCompleted { get; set; } = false;

    }
}
