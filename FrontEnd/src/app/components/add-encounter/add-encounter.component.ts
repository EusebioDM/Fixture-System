import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Sport } from '../../classes/sport';
import { SportsService } from '../../services/sports.service';
import { TeamsService } from '../../services/teams.service';
import { Team } from '../../classes/team';
import { EncountersService } from '../../services/encounters.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { EncountersComponent } from '../encounters/encounters.component';

@Component({
  selector: 'app-add-encounter',
  templateUrl: './add-encounter.component.html',
  styleUrls: ['./add-encounter.component.css']
})
export class AddEncounterComponent implements OnInit {

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: AddEncounterComponent,
    private sportsService: SportsService,
    private teamsService: TeamsService,
    private encountersService: EncountersService,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<EncountersComponent>
  ) { }

  error: string;
  sports: Array<Sport>;
  teams: Array<Team>;
  addEncounterForm: FormGroup;

  ngOnInit() {
    this.getSportsData();
    this.createAddEncounterForm();
  }

  getSportsData(): any {
    this.sportsService.getSports().subscribe(
      ((data: Array<Sport>) => { this.sports = data; }),
      ((error: any) => console.log(error))
    );
  }

  getTeamsData(sportName: string) {
    debugger;
    this.sportsService.getTeamsBySport(sportName).subscribe(
      ((data: Array<Team>) => {
        this.teams = data;
      }),
      ((error: any) => console.log(error))
    );
/*
    this.teamsService.getTeams().subscribe(
      ((data: Array<Team>) => {
        this.teams = data;
      }),
      ((error: any) => console.log(error))
    );
    */
  }

  createAddEncounterForm() {
    this.addEncounterForm = this.formBuilder.group({
      dateTime: ['',
        Validators.required
      ],
      sportName: ['',
        Validators.required
      ],
      teamIds: ['',
        Validators.required]
    });
  }

  get dateTime() {
    return this.addEncounterForm.get('dateTime');
  }

  get sportName() {
    return this.addEncounterForm.get('sportName');
  }

  get teamIds() {
    return this.addEncounterForm.get('teamIds');
  }

  public getTeamId(teamName: string, sportName: string): string {
    return teamName + '_' + sportName;
  }

  public submit() {
    const encounter = this.addEncounterForm.value;
    this.encountersService.addEncounter(encounter).subscribe(
      (() => {
        this.dialogRef.close(encounter);
      }),
      (error) => this.error = error
    );
  }
}
