using ECommerce.Modules.Items.Application.Commands.Images;
using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Queries.Images;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Api.Controllers
{
    internal class ImagesController : BaseController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public ImagesController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("{imageId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<PhysicalFileResult> Get(Guid imageId)
        {
            var imageSrc = await _queryDispatcher.QueryAsync(new GetImage(imageId));
            var extension = imageSrc.Extension.Split('.')[1];
            return PhysicalFile(imageSrc.ImagePath, $"image/{extension}");
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<string>>> AddImagesAsync([FromForm] IList<IFormFile> files)
        {
            var ids = await _commandDispatcher.SendAsync<IEnumerable<string>>(new CreateImages(files));
            var urls = new List<string>();
            var scheme = Request.Scheme;
            var baseUrl = $"{scheme}://{Request.Host}{Request.Path}";

            foreach (var id in ids)
            {
                urls.Add($"{baseUrl}/{id}");
            }

            return urls;
        }
    }
}
