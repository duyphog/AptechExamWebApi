using System;
namespace api.DTOs
{
    public class CategoryDTO
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Descriptions { get; set; } = null;
    }
}
