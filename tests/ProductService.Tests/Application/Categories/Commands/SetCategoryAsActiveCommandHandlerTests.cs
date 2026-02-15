using Microsoft.Extensions.Logging;
using Moq;
using ProductService.Application.Features.Categories.Commands.SetCategoryAsActive;
using ProductService.Domain.CategoryManagement;

namespace ProductService.Tests.Application.Categories.Commands;

public class SetCategoryAsActiveCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<SetCategoryAsActiveCommandHandler>> _loggerMock;
    private readonly SetCategoryAsActiveCommandHandler _handler;

    public SetCategoryAsActiveCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<SetCategoryAsActiveCommandHandler>>();
        _handler = new SetCategoryAsActiveCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenCategoryExists()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new SetCategoryAsActiveCommand(categoryId);
        var category = Category.Create(categoryId, "Test Category", "Test Description").Value;

        var categoryRepositoryMock = new Mock<IRepository<Category>>();
        categoryRepositoryMock.Setup(r => r.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _unitOfWorkMock.Setup(u => u.GetRepository<Category>()).Returns(categoryRepositoryMock.Object);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(category.IsActive);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new SetCategoryAsActiveCommand(categoryId);

        var categoryRepositoryMock = new Mock<IRepository<Category>>();
        categoryRepositoryMock.Setup(r => r.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category)null);

        _unitOfWorkMock.Setup(u => u.GetRepository<Category>()).Returns(categoryRepositoryMock.Object);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
