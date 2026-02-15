
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

namespace ProductService.Tests.Application.Products.Commands.ImagesManagement
{
    public class AddRelatedImagesToProductCommandHandlerTests : TestFixtureBase<AddRelatedImagesToProductCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldAddRelatedImages_WhenProductExistsAndImagesAreNew()
        {
            // Arrange
            var product = Context.Products.First();
            var newImages = new List<ImageDto>
            {
                new ImageDto("http://example.com/image1.jpg", "Image 1"),
                new ImageDto("http://example.com/image2.jpg", "Image 2")
            };
            var command = new AddRelatedImagesToProductCommand(product.Id, newImages);
            var handler = new AddRelatedImagesToProductCommandHandler(ProductRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedProduct = await ProductRepository.GetByIdAsync(product.Id, CancellationToken.None, true);
            updatedProduct.Should().NotBeNull();
            updatedProduct.RelatedImages.Should().Contain(img => img.Url == "http://example.com/image1.jpg");
            updatedProduct.RelatedImages.Should().Contain(img => img.Url == "http://example.com/image2.jpg");
        }

        [Fact]
        public async Task Handle_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new AddRelatedImagesToProductCommand(Guid.NewGuid(), new List<ImageDto>());
            var handler = new AddRelatedImagesToProductCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ProductNotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenImageAlreadyExists()
        {
            // Arrange
            var product = Context.Products.First(p => p.RelatedImages.Any());
            var existingImage = product.RelatedImages.First();
            var imagesToAdd = new List<ImageDto>
            {
                new ImageDto(existingImage.Url, "A new alt text")
            };
            var command = new AddRelatedImagesToProductCommand(product.Id, imagesToAdd);
            var handler = new AddRelatedImagesToProductCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenDuplicateImagesInRequest()
        {
            // Arrange
            var product = Context.Products.First();
            var imagesToAdd = new List<ImageDto>
            {
                new ImageDto("http://example.com/duplicate.jpg", "Image 1"),
                new ImageDto("http://example.com/duplicate.jpg", "Image 2")
            };
            var command = new AddRelatedImagesToProductCommand(product.Id, imagesToAdd);
            var handler = new AddRelatedImagesToProductCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
