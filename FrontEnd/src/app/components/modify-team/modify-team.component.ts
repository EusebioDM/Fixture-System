import { Component, OnInit, Inject, Optional, Input } from '@angular/core';
import { TeamsService } from 'src/app/services/teams.service';
import { Team } from 'src/app/classes/team';
import { MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-modify-team',
  templateUrl: './modify-team.component.html',
  styleUrls: ['./modify-team.component.css']
})
export class ModifyTeamComponent implements OnInit {

  constructor(
    private teamsServices: TeamsService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: Team
  ) { }

  @Input() teamId: string;

  name: string;
  sportName: string;
  logo: string;

  ngOnInit() {
    this.loadData();
  }

  private loadData() {
    if (this.data) {
      this.name = this.data.name;
      this.sportName = this.data.sportName;
      this.logo = this.data.logo;
    }
  }
}
