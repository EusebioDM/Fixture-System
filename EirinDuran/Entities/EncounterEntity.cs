﻿using EirinDuran.Domain.Fixture;
using EirinDuran.Entities.Mappers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EirinDuran.Entities
{
    public class EncounterEntity
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public SportEntity Sport { get; set; }
        public IEnumerable<TeamEntity> Teams { get; set; }
        private EncounterMapper mapper;

        public EncounterEntity()
        {
            mapper = new EncounterMapper();
        }

        public EncounterEntity(Encounter encounter) : this()
        {
            mapper.Update(encounter, this);
        }

        public void UpdateWith(Encounter encounter)
        {
            mapper.Update(encounter, this);
        }

        public Encounter ToModel()
        {
            return mapper.Map(this);
        }
    }
}
