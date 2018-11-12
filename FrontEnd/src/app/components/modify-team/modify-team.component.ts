import { Component, OnInit, Inject, Optional, Input } from '@angular/core';
import { TeamsService } from 'src/app/services/teams.service';
import { Team } from 'src/app/classes/team';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TeamsComponent } from '../teams/teams.component';
import { InstantErrorStateMatcher } from 'src/app/shared/instant-error-state-matcher';

@Component({
  selector: 'app-modify-team',
  templateUrl: './modify-team.component.html',
  styleUrls: ['./modify-team.component.css']
})
export class ModifyTeamComponent implements OnInit {

  constructor(
    private teamsServices: TeamsService,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<TeamsComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: Team
  ) { }

  addTeamForm: FormGroup;

  @Input() teamId: string;

  matcher = new InstantErrorStateMatcher();

  name: string;
  sportName: string;
  logo: string;

  ngOnInit() {
    this.loadData();
  }

  modifyTeamForm() {
    this.addTeamForm = this.formBuilder.group({
      name: ['',
        Validators.required
      ]
    });
  }

  private loadData() {
    if (this.data) {
      this.name = this.data.name;
      this.sportName = this.data.sportName;
      this.logo = this.data.logo;
    }
  }

  submit() {
    this.teamsServices.updateTeam(new Team(this.name, this.sportName, this.logo)).subscribe();
  }
}
