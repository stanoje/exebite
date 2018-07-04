﻿namespace Exebite.DomainModel
{
    public class CustomerAliasQueryModel : QueryBase
    {
        public CustomerAliasQueryModel()
        {
        }

        public CustomerAliasQueryModel(int page, int size)
            : base(page, size)
        {
        }

        public int? Id { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}";
        }
    }
}