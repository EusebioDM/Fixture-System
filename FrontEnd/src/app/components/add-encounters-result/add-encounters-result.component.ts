import { Component, OnInit } from '@angular/core';
import { Encounter } from '../../classes/encounter';
import { FormControl } from '@angular/forms';
import { TeamResult } from 'src/app/classes/team-result';
import { EncountersService } from 'src/app/services/encounters.service';

@Component({
  selector: 'app-add-encounters-result',
  templateUrl: './add-encounters-result.component.html',
  styleUrls: ['./add-encounters-result.component.css']
})
export class AddEncountersResultComponent implements OnInit {

  constructor(private encountersService: EncountersService) { }

  encounter: Encounter;
  teams: Array<string>;
  positions: Array<string>;
  positionsResults: Array<string>;
  addEncounterResultsForm;

  ngOnInit() {
    this.teams = this.encounter.teamIds;
    this.addEncounterResultsForm = new FormControl();
    this.loadPositions();
  }

  loadPositions() {
    this.positions = new Array<string>();
    this.positionsResults = new Array<string>();
    for (let i = 1; i <= this.teams.length; i++) {
      this.positions.push(i + '');
    }
  }

  getTeamResult(result: string, teamId: string) {
    this.encounter.results.push(new TeamResult(teamId, result));
    this.encountersService.updateEncounter(this.encounter);
  }
}
