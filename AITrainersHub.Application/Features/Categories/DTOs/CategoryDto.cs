using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITrainersHub.Application.Features.Categories.DTOs
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for a category, containing its unique identifier, name, and an optional description. Create, Update
    /// </summary>
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
