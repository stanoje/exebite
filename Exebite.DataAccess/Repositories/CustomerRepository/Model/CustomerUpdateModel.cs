﻿namespace Exebite.DataAccess.Repositories
{
    public class CustomerUpdateModel
    {
        public string Name { get; set; }

        public decimal Balance { get; set; }

        public int LocationId { get; set; }

        public string GoogleUserId { get; set; }

        public int RoleId { get; set; }
    }
}