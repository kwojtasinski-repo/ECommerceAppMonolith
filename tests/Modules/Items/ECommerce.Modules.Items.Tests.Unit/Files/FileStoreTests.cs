using ECommerce.Modules.Items.Application.Files.Interfaces;
using ECommerce.Modules.Items.Infrastructure.Files.Exceptions;
using ECommerce.Modules.Items.Infrastructure.Files.Implementations;
using NSubstitute;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Items.Tests.Unit
{
    public class FileStoreTests
    {
        [Fact]
        public async Task given_invalid_file_should_throw_an_exception()
        {
            var path = "./test";
            var expectedException = new FileNotFoundException();

            var exception = await Record.ExceptionAsync(() => _fileStore.WriteFileAsync(null, path));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<FileNotFoundException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        [Fact]
        public async Task given_invalid_files_should_throw_an_exception()
        {
            var path = "./test";
            var expectedException = new FilesNotFoundException();

            var exception = await Record.ExceptionAsync(() => _fileStore.WriteFilesAsync(null, path));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<FilesNotFoundException>();
            exception.Message.ShouldBe(expectedException.Message);
        }

        private readonly IFileStore _fileStore;
        private readonly IFileWrapper _fileWrapper;
        private readonly IDirectoryWrapper _directoryWrapper;

        public FileStoreTests()
        {
            _fileWrapper = Substitute.For<IFileWrapper>();
            _directoryWrapper = Substitute.For<IDirectoryWrapper>();
            _fileStore = new FileStore(_fileWrapper, _directoryWrapper);
        }
    }
}