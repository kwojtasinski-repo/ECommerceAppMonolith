using ECommerce.Modules.Items.Application.Commands.Images;
using ECommerce.Modules.Items.Application.Commands.Images.Handlers;
using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Application.Files.Interfaces;
using ECommerce.Modules.Items.Application.Policies.Image;
using ECommerce.Modules.Items.Application.Services;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Events;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Kernel;
using ECommerce.Shared.Abstractions.Messagging;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Items.Tests.Unit.Commands.Images
{
    public class AddImagesCommandHandlerTests
    {
        [Fact]
        public async Task given_null_files_should_throw_an_exception()
        {
            var expectedException = new ImagesCannotBeEmptyException();

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(new CreateImages(null)));

            exception.ShouldNotBeNull();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_invalid_files_should_throw_an_exception() 
        {
            var iFormFiles = new List<IFormFile>();
            var iFormFile = Substitute.For<IFormFile>();
            iFormFile.Length.Returns(1000000);
            iFormFile.FileName.Returns("test.bmp");
            _saveFilePolicy.GetAllowedSize().Returns(10);
            _fileStore.GetFileExtension("test.bmp").Returns(".bmp");
            _saveFilePolicy.GetAllowedImagesExtensions().Returns(new[] { ".jpg" });
            iFormFiles.Add(iFormFile);
            var command = new CreateImages(iFormFiles);
            var expectedException = new InvalidFilesException("");

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidFilesException>();
            exception.Message.ShouldContain("is too big");
            exception.Message.ShouldContain(".bmp is not allowed");
        }

        [Fact]
        public async Task given_valid_files_should_add()
        {
            var iFormFiles = new List<IFormFile>();
            var iFormFile = Substitute.For<IFormFile>();
            iFormFile.Length.Returns(1);
            iFormFile.FileName.Returns("test.bmp");
            _fileStore.ReplaceInvalidChars("test.bmp").Returns("test.bmp");
            iFormFile.Name.Returns("test.bmp");
            var path = "./path";
            _saveFilePolicy.GetFileDirectory().Returns(path);
            _fileStore.WriteFileAsync(iFormFile, _saveFilePolicy.GetFileDirectory()).Returns($"{path}/test.bmp");
            _saveFilePolicy.GetAllowedSize().Returns(10);
            _fileStore.GetFileExtension("test.bmp").Returns(".bmp");
            _saveFilePolicy.GetAllowedImagesExtensions().Returns(new[] { ".bmp" });
            iFormFiles.Add(iFormFile);
            var command = new CreateImages(iFormFiles);

            var ids = await _handler.HandleAsync(command);

            await _imageRepository.Received(1).AddAsync(Arg.Any<Image>());
            ids.ShouldNotBeNull();
            ids.Count().ShouldBeGreaterThan(0);
            _eventMapper.MapAll(Arg.Any<IEnumerable<IDomainEvent>>()).Received(1);
            await _messageBroker.Received(1).PublishAsync(Arg.Any<IMessage[]>());
        }

        private readonly CreateImagesHandler _handler;
        private readonly IImageRepository _imageRepository;
        private readonly IFileStore _fileStore;
        private readonly ISaveFilePolicy _saveFilePolicy;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public AddImagesCommandHandlerTests()
        {
            _imageRepository = Substitute.For<IImageRepository>();
            _fileStore = Substitute.For<IFileStore>();
            _saveFilePolicy = Substitute.For<ISaveFilePolicy>();
            _messageBroker = Substitute.For<IMessageBroker>();
            _eventMapper = Substitute.For<IEventMapper>();
            _handler = new CreateImagesHandler(_imageRepository, _fileStore, _saveFilePolicy, _messageBroker, _eventMapper);
        }
    }
}
