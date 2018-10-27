import { Component, OnInit } from '@angular/core';
import { TeamsService } from '../../services/teams.service';
import { Sport } from '../../classes/sport';
import { Team } from 'src/app/classes/team';

@Component({
  selector: 'app-add-team',
  templateUrl: './add-team.component.html',
  styleUrls: ['./add-team.component.css']
})
export class AddTeamComponent implements OnInit {

  name: string;
  sportName: string;
  logoArray = [];
  logo: string;
  selectedFile;

  sports: Sport[] = [
    { name: 'SportTest1' },
    { name: 'HardCode2' },
    { name: 'HardCode3' }
  ];

  constructor(private teamsService: TeamsService) { }

  ngOnInit() {
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
    // this.logoArray.push('data:image/png;base64,' + btoa(e.target.result));
    this.logo = btoa(e.target.result);
  }

  public submit() {
    const team = new Team(this.name, this.sportName, this.logo);
    this.teamsService.addTeam(team).subscribe();
  }
}
