﻿namespace Exebite.DtoModels
{
    public class CustomerDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public int? LocationId { get; set; }

        public string GoogleUserId { get; set; }

        public int? RoleId { get; set; }
    }
}
