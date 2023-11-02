using MyTaskPlanner.Domain.Models.Enums;

namespace MyTaskPlanner.Domain.Models.Models
{
    public class WorkItem : ICloneable
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Complexity Complexity { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }

        public override string ToString() => $"Do {Title?.ToLower()}: due {DueDate:d}, {Enum.GetName(Priority)} priority";

        public object Clone() => new WorkItem()
        {
            Id = Id,
            CreationDate = CreationDate,
            DueDate = DueDate,
            Priority = Priority,
            Complexity = Complexity,
            Title = Title,
            Description = Description,
            IsCompleted = IsCompleted,
        };
    }
}