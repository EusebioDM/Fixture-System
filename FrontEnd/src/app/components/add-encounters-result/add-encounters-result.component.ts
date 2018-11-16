import { Component, OnInit } from '@angular/core';
import { Encounter } from '../../classes/encounter';
import { FormControl } from '@angular/forms';
import { debug } from 'util';

@Component({
  selector: 'app-add-encounters-result',
  templateUrl: './add-encounters-result.component.html',
  styleUrls: ['./add-encounters-result.component.css']
})
export class AddEncountersResultComponent implements OnInit {

  constructor() { }

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

  getTeamResult(result: any, teamId: string) {
    debugger;
    console.log('Resultado: ' + result + ' para el equipo ' + teamId);
  }
}
