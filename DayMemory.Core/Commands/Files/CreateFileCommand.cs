using DayMemory.Core.Models.Personal;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace DayMemory.Core.Commands.Files
{
    public class CreateFileCommand : IRequest<string>
    {
        public IFormFile? FormFile { get; set; }
        
        public string? UserId { get; set; }

        public string? FileId { get; set; }

        public FileType FileType { get; set; }
        
        public int Height { get; set; }

        public int Width { get; set; }
    }
}
