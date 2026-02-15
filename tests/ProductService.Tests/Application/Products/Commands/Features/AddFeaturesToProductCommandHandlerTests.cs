using ProductService.Application.DTOs;

namespace ProductService.Tests.Application.Products.Commands.Features
{
    public class AddFeaturesToProductCommandHandlerTests : TestFixtureBase<AddFeaturesToProductCommandHandler>
    {
        [Fact]
        public async Task Handle_ShouldAddFeatures_WhenProductExistsAndFeaturesAreNew()
        {
            // Arrange
            var product = Context.Products.First();
            var newFeatures = new List<FeatureDto>
            {
                new FeatureDto("NewFeature1", "Value1"),
                new FeatureDto("NewFeature2", "Value2")
            };
            var command = new AddFeaturesToProductCommand(product.Id, newFeatures);
            var handler = new AddFeaturesToProductCommandHandler(ProductRepository, Logger);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updatedProduct = await ProductRepository.GetByIdAsync(product.Id, CancellationToken.None, true);
            updatedProduct.Should().NotBeNull();
            updatedProduct.Features.Should().Contain(f => f.Name == "NewFeature1");
            updatedProduct.Features.Should().Contain(f => f.Name == "NewFeature2");
        }

        [Fact]
        public async Task Handle_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new AddFeaturesToProductCommand(Guid.NewGuid(), new List<FeatureDto>());
            var handler = new AddFeaturesToProductCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ProductNotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenFeatureAlreadyExists()
        {
            // Arrange
            var product = Context.Products.First(p => p.Features.Any());
            var existingFeature = product.Features.First();
            var featuresToAdd = new List<FeatureDto>
            {
                new FeatureDto(existingFeature.Name, "NewValue")
            };
            var command = new AddFeaturesToProductCommand(product.Id, featuresToAdd);
            var handler = new AddFeaturesToProductCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenDuplicateFeaturesInRequest()
        {
            // Arrange
            var product = Context.Products.First();
            var featuresToAdd = new List<FeatureDto>
            {
                new FeatureDto("DuplicateName", "Value1"),
                new FeatureDto("DuplicateName", "Value2")
            };
            var command = new AddFeaturesToProductCommand(product.Id, featuresToAdd);
            var handler = new AddFeaturesToProductCommandHandler(ProductRepository, Logger);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
