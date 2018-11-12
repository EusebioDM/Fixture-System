import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Sport } from '../../classes/sport';
import { SportsService } from '../../services/sports.service';
import { TeamsService } from '../../services/teams.service';
import { Team } from '../../classes/team';
import { MatTableDataSource } from '@angular/material';

@Component({
  selector: 'app-add-encounter',
  templateUrl: './add-encounter.component.html',
  styleUrls: ['./add-encounter.component.css']
})
export class AddEncounterComponent implements OnInit {

  constructor(
    private sportsService: SportsService,
    private teamsService: TeamsService,
    private formBuilder: FormBuilder,
  ) { }

  displayedColumns: string[] = ['check', 'team'];
  dataSource;

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
    this.teamsService.getTeams().subscribe(
      ((data: Array<Team>) => {
        this.teams = data;
        this.dataSource = new MatTableDataSource<Team>(this.teams);
      }),
      ((error: any) => console.log(error))
    );
  }

  createAddEncounterForm() {
    this.addEncounterForm = this.formBuilder.group({
      encounterDate: ['',
        Validators.required
      ],
      sport: ['',
        Validators.required
      ],
    });
  }

  get encounterDate() {
    return this.addEncounterForm.get('encounterDate');
  }

  get sport() {
    return this.addEncounterForm.get('sport');
  }
}
