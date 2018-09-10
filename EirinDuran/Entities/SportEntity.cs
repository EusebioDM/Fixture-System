﻿using EirinDuran.Domain.Fixture;
using EirinDuran.Entities.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.Entities
{
    public class SportEntity
    {
        public string Name { get; set; }
        public IEnumerable<TeamEntity> Teams { get; set; }
        private SportMapper mapper;

        public SportEntity()
        {
            mapper = new SportMapper();
        }

        public SportEntity(Sport sport) : this()
        {
            mapper.Update(sport, this);
        }

        public void UpdateWith(Sport sport)
        {
            mapper.Update(sport,this);
        }

        public Sport ToModel()
        {
            return mapper.Map(this);
        }

        public override bool Equals(object obj)
        {
            var entity = obj as SportEntity;
            return entity != null &&
                   Name == entity.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
