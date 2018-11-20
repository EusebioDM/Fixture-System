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
    this.checkIfIsRadio();
    this.results = this.encounter.results;
    this.showResults();
    this.loadPositions();
  }

  private checkIfIsRadio() {
    if (this.teams.length === 2) {
      this.isRadio = true;
    } else {
      this.isRadio = false;
    }
  }

  loadPositions() {
    this.positions = new Array<string>();
    for (let i = 1; i <= this.teams.length; i++) {
      this.positions.push(i + '');
    }
  }

  getTeamResult(result: string, teamId: string) {
    const isit = this.encounter.results.findIndex(i => i.teamId === teamId);
    if (this.isRadio) {
      const looser = this.encounter.results.find(i => i.teamId !== teamId);
      this.cleanResultsArray();
      this.encounter.results.push(new TeamResult(teamId, '1'));
      this.encounter.results.push(new TeamResult(looser.teamId, '2'));
    } else {
      if (isit === -1) {
        this.encounter.results.push(new TeamResult(teamId, result));
      } else {
        this.encounter.results.splice(isit, 1);
        this.encounter.results.push(new TeamResult(teamId, result));
      }
    }
  }

  private cleanResultsArray() {
    const len = this.encounter.results.length;
    this.encounter.results.splice(0, len);
  }

  private showResults() {
    if (this.results.length === 2) {
      this.hasResults = '';
      this.results.forEach(result => {
        if (result.result == '1') {
          this.hasResults += 'Equipo ganador: ' + result.teamId;
        }
      });
    } else {
      this.hasResults = '';
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
