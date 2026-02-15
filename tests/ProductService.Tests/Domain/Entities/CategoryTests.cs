using System;
using FluentAssertions;
using ProductService.Domain.Entities;
using Xunit;

namespace ProductService.Tests.Domain.Entities
{
    public class CategoryTests
    {
        [Fact]
        public void Create_ShouldCreateCategory_WhenParametersAreValid()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "Test Category";
            var description = "Test Description";

            // Act
            var category = Category.Create(id, name, description);

            // Assert
            category.Should().NotBeNull();
            category.Id.Should().Be(id);
            category.Name.Should().Be(name);
            category.Description.Should().Be(description);
            category.IsActive.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_ShouldThrowArgumentException_WhenNameIsInvalid(string name)
        {
            // Act
            Action act = () => Category.Create(Guid.NewGuid(), name, "Test Description");

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_ShouldThrowArgumentException_WhenDescriptionIsInvalid(string description)
        {
            // Act
            Action act = () => Category.Create(Guid.NewGuid(), "Test Name", description);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Edit_ShouldUpdateNameAndDescription()
        {
            // Arrange
            var category = Category.Create(Guid.NewGuid(), "Original Name", "Original Description");
            var newName = "Edited Name";
            var newDescription = "Edited Description";

            // Act
            category = category.Edit(newName, newDescription);

            // Assert
            category.Name.Should().Be(newName);
            category.Description.Should().Be(newDescription);
        }

        [Fact]
        public void EditStatus_ShouldUpdateIsActive()
        {
            // Arrange
            var category = Category.Create(Guid.NewGuid(), "Test Name", "Test Description");

            // Act
            category = category.EditStatus(false);

            // Assert
            category.IsActive.Should().BeFalse();
        }
    }
}