

using MyTaskPlanner.Domain.Models.Models;
using TaskPlanner.DataAccess.Abstractions;

namespace MyTaskPlanner.Domain.Logic
{
    public class SimpleTaskPlanner
    {

        public SimpleTaskPlanner(IWorkItemsRepository workRepository)
        {
            WorkRepository = workRepository;
        }

        public IWorkItemsRepository WorkRepository { get; }

        public WorkItem[] CreatePlan()
        {
            var items = WorkRepository.GetAll().ToList();

            items.Sort(CompareWorkItems);
            return items.ToArray();
        }

        private int CompareWorkItems(WorkItem firstItem, WorkItem secondItem)
        {
            // Спершу порівнюємо за пріоритетом (спадання).
            int priorityComparison = secondItem.Priority.CompareTo(firstItem.Priority);

            // Якщо пріоритети різні, повертаємо результат порівняння пріоритетів.
            if (priorityComparison != 0)
            {
                return priorityComparison;
            }

            // Якщо пріоритети однакові, порівнюємо за DueDate (зростання).
            int dueDateComparison = firstItem.DueDate.CompareTo(secondItem.DueDate);

            // Якщо дати різні, повертаємо результат порівняння дат.
            if (dueDateComparison != 0)
            {
                return dueDateComparison;
            }

            // Якщо дати однакові, порівнюємо за назвою (алфавітний порядок).
            return firstItem.Title.CompareTo(secondItem.Title);
        }
    }
}