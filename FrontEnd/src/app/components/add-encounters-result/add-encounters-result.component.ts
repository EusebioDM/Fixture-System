import { Component, OnInit } from '@angular/core';
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

  hasResults = '';
  results: Array<TeamResult>;
  error: string;
  encounter: Encounter;
  teams: Array<string>;
  positions: Array<string>;
  positionsResults: Array<string>;

  ngOnInit() {
    this.teams = this.encounter.teamIds;
    this.results = this.encounter.results;
    this.showResults();
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
  }

  private showResults() {
    this.results.forEach(result => {
      this.hasResults += 'Equipo: ' + result.teamId + ' Resultado: ' + result.result + ' - ';
    });
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
