import { Component, OnInit, Inject, Optional, Input } from '@angular/core';
import { TeamsService } from 'src/app/services/teams.service';
import { Team } from 'src/app/classes/team';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { TeamsComponent } from '../teams/teams.component';

@Component({
  selector: 'app-modify-team',
  templateUrl: './modify-team.component.html',
  styleUrls: ['./modify-team.component.css']
})
export class ModifyTeamComponent implements OnInit {

  constructor(
    private teamsServices: TeamsService,
    public dialogRef: MatDialogRef<TeamsComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: Team
  ) { }

  @Input() teamId: string;

  selectedFile;
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

  onFileSelected(event) {
    this.selectedFile = event.target.files[0];
    if (this.selectedFile) {
      const reader = new FileReader();

      reader.onload = this.handleReaderLoaded.bind(this);
      reader.readAsBinaryString(this.selectedFile);
    }
  }

  handleReaderLoaded(e) {
    this.logo = btoa(e.target.result);
  }

  submit() {
    const team = new Team(this.name, this.sportName, this.logo);
    this.teamsServices.updateTeam(team).subscribe(
      (() => {
        this.dialogRef.close(team);
      })
    );
  }
}
