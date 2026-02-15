using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProductService.Application.Products.Commands;
using ProductService.Application.DTOs;
using ProductService.Domain.Exceptions;
using Xunit;

namespace ProductService.Tests.Application.Products.Commands
{
    public class CreateProductCommandHandlerTests : TestFixtureBase<CreateProductCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldCreateProduct_WhenRequestIsValid()
        {
            // Arrange
            var category = Context.Categories.First();
            var tag = Context.Tags.First();
            var command = new CreateProductCommand(
                category.Id,
                "New Product",
                "New Description",
                100,
                "USD",
                10,
                new ImageDto("http://example.com/main.jpg", "Main image"),
                10,
                DateTime.UtcNow.AddDays(10),
                new List<ImageDto> { new ImageDto("http://example.com/related.jpg", "Related image") },
                new List<FeatureDto> { new FeatureDto("Color", "Blue") },
                new List<Guid> { tag.Id }
            );
            var handler = new CreateProductCommandHandler(ProductRepository, CategoryRepository, TagRepository, Logger);

            // Act
            var productId = await handler.Handle(command, CancellationToken.None);

            // Assert
            var product = await ProductRepository.GetByIdAsync(productId, CancellationToken.None, true);
            product.Should().NotBeNull();
            product.Name.Should().Be(command.Name);
        }

        [Fact]
        public async Task Handle_ShouldThrowDuplicateProductNameException_WhenProductNameExists()
        {
            // Arrange
            var existingProduct = Context.Products.First();
            var category = Context.Categories.First();
            var command = new CreateProductCommand(
                category.Id,
                existingProduct.Name, // Existing name
                "New Description",
                100,
                "USD",
                10,
                new ImageDto("http://example.com/main.jpg", "Main image"),
                10,
                DateTime.UtcNow.AddDays(10),
                null, null, null
            );
            var handler = new CreateProductCommandHandler(ProductRepository, CategoryRepository, TagRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicateProductNameException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowCategoryNotFoundException_WhenCategoryDoesNotExist()
        {
            // Arrange
            var command = new CreateProductCommand(
                Guid.NewGuid(), // Non-existent category
                "New Product",
                "New Description",
                100,
                "USD",
                10,
                new ImageDto("http://example.com/main.jpg", "Main image"),
                10,
                DateTime.UtcNow.AddDays(10),
                null, null, null
            );
            var handler = new CreateProductCommandHandler(ProductRepository, CategoryRepository, TagRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<CategoryNotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowTagsNotFoundException_WhenTagsDoNotExist()
        {
            // Arrange
            var category = Context.Categories.First();
            var command = new CreateProductCommand(
                category.Id,
                "New Product",
                "New Description",
                100,
                "USD",
                10,
                new ImageDto("http://example.com/main.jpg", "Main image"),
                10,
                DateTime.UtcNow.AddDays(10),
                null, null, new List<Guid> { Guid.NewGuid() } // Non-existent tag
            );
            var handler = new CreateProductCommandHandler(ProductRepository, CategoryRepository, TagRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<TagsNotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowProductBuildException_WhenNameIsEmpty()
        {
            // Arrange
            var category = Context.Categories.First();
            var command = new CreateProductCommand(
                category.Id,
                "", // Empty name
                "New Description",
                100,
                "USD",
                10,
                new ImageDto("http://example.com/main.jpg", "Main image"),
                10,
                DateTime.UtcNow.AddDays(10),
                null, null, null
            );
            var handler = new CreateProductCommandHandler(ProductRepository, CategoryRepository, TagRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ProductBuildException>();
        }
    }
}