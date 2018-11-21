import { Component, OnInit, Inject } from '@angular/core';
import { Encounter } from '../../classes/encounter';
import { TeamResult } from 'src/app/classes/team-result';
import { EncountersService } from 'src/app/services/encounters.service';
import { EncountersComponent } from '../encounters/encounters.component';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-add-encounters-result',
  templateUrl: './add-encounters-result.component.html',
  styleUrls: ['./add-encounters-result.component.css']
})
export class AddEncountersResultComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<EncountersComponent>,
    private encountersService: EncountersService
  ) { }

  isRadio: boolean;
  hasResults = '';
  results: Array<TeamResult>;
  error: string;
  encounter: Encounter;
  teams: Array<string>;
  positions: Array<string>;

  ngOnInit() {
    this.hasResults = '';
    this.teams = this.encounter.teamIds;
    this.results = this.encounter.results;
    this.showResults();
    this.loadPositions();
  }

  loadPositions() {
    this.positions = new Array<string>();
    for (let i = 1; i <= this.teams.length; i++) {
      this.positions.push(i + '');
    }
  }

  getTeamResult(result: string, teamId: string) {
    const isit = this.encounter.results.findIndex(i => i.teamId === teamId);
    if (isit === -1) {
      this.encounter.results.push(new TeamResult(teamId, result));
    } else {
      this.encounter.results.splice(isit, 1);
      this.encounter.results.push(new TeamResult(teamId, result));
    }
  }

  private cleanResultsArray() {
    const len = this.encounter.results.length;
    this.encounter.results.splice(0, len);
  }

  private showResults() {
    this.hasResults = '';
    if (this.results.length === 2) {
      if (this.results[0].result == '1' && this.results[1].result == '1') {
        this.hasResults = 'Empate';
      } else if (this.results[0].result == '1' && this.results[1].result != '1') {
        this.hasResults += 'Equipo ganador: ' + this.results[0].teamId;
      } else if (this.results[0].result == '2' && this.results[1].result == '2') {
        this.hasResults = 'Empate';
      } else {
        this.hasResults += 'Equipo ganador: ' + this.results[1].teamId;
      }
    } else {
      this.results.forEach(result => {
        this.hasResults += 'Equipo: ' + result.teamId + ' Resultado: ' + result.result + ' - ';
      });
    }
  }

  onSubmit() {
    this.encountersService.updateEncounter(this.encounter).subscribe(
      (() => {
        this.dialogRef.close(true);
      }),
      (error) => this.error = error
    );
  }
}
