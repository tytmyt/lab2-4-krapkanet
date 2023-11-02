using Moq;
using MyTaskPlanner.Domain.Logic;
using MyTaskPlanner.Domain.Models.Enums;
using MyTaskPlanner.Domain.Models.Models;
using TaskPlanner.DataAccess.Abstractions;

namespace TaskPlanner.Domain.Logic.Tests;

public class SimpleTaskPlannerTests
{
    WorkItem task1 = new WorkItem
    {
        Id = Guid.NewGuid(),
        CreationDate = DateTime.Now,
        DueDate = DateTime.Parse("2023-10-15"),
        Priority = Priority.High,
        Complexity = Complexity.Days,
        Title = "Complete Project Proposal",
        Description = "Prepare and finalize the project proposal document.",
        IsCompleted = false
    };

    WorkItem task2 = new WorkItem
    {
        Id = Guid.NewGuid(),
        CreationDate = DateTime.Now,
        DueDate = DateTime.Parse("2023-09-30"),
        Priority = Priority.Medium,
        Complexity = Complexity.Hours,
        Title = "Review Meeting Notes",
        Description = "Go through the notes from the team meeting and extract action items.",
        IsCompleted = true
    };

    WorkItem task3 = new WorkItem
    {
        Id = Guid.NewGuid(),
        CreationDate = DateTime.Now,
        DueDate = DateTime.Parse("2023-11-05"),
        Priority = Priority.Low,
        Complexity = Complexity.Minutes,
        Title = "Update Software",
        Description = "Implement the latest updates and fixes in the software.",
        IsCompleted = false
    };

    [Fact]
    public void SortsTasksCorrectly()
    {
        // Arrange
        var mockRepository = new Mock<IWorkItemsRepository>();

        mockRepository.Setup(repo => repo.GetAll())
            .Returns(new WorkItem[] { task1, task2, task3 });

        var planner = new SimpleTaskPlanner(mockRepository.Object);

        // Act
        WorkItem[] plan = planner.CreatePlan();

        // Assert
        Assert.Equal(new WorkItem[] { task1, task2, task3 }, plan);
    }

    [Fact]
    public void IncludesOnlyUncompletedTasks()
    {
        // Arrange
        var mockRepository = new Mock<IWorkItemsRepository>();
        mockRepository.Setup(repo => repo.GetAll())
        .Returns(new WorkItem[] { task1, task2, task3 });

        var planner = new SimpleTaskPlanner(mockRepository.Object);

        // Act
        WorkItem[] plan = planner.CreatePlan();

        // Assert
        Assert.True(!plan.Contains(task2));
    }

    [Fact]
    public void DoesNotIncludeCompletedTasks()
    {
        // Arrange
        var mockRepository = new Mock<IWorkItemsRepository>();
        mockRepository.Setup(repo => repo.GetAll())
            .Returns(new WorkItem[] { task1, task2, task3 });

        var planner = new SimpleTaskPlanner(mockRepository.Object);

        // Act
        WorkItem[] plan = planner.CreatePlan();

        // Assert
        Assert.True(!plan.Contains(task2));
    }
}
