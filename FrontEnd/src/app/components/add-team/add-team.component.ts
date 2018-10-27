import { Component, OnInit } from '@angular/core';
import { TeamsService } from '../../services/teams.service';
import { SportsService } from '../../services/sports.service';
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
  logo: string;

  selectedFile;

  sports: Array<Sport>;

  constructor(private teamsService: TeamsService, private sportsService: SportsService) { }

  ngOnInit() {
    this.sportsService.getSports().subscribe(
      ((data: Array<Sport>) => this.result(data)),
      ((error: any) => console.log(error))
    );
  }

  private result(data: Array<Sport>): void {
    this.sports = data;
    console.log(this.sports);
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

  public submit() {
    const team = new Team(this.name, this.sportName, this.logo);
    this.teamsService.addTeam(team).subscribe();
  }
}
