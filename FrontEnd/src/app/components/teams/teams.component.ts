import { Component, OnInit, ViewChild } from '@angular/core';
import { Team } from 'src/app/classes/team';
import { MatPaginator, MatTableDataSource } from '@angular/material';
import { TeamsService } from 'src/app/services/teams.service';

@Component({
  selector: 'app-teams',
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.css']
})
export class TeamsComponent implements OnInit {

  constructor(private teamsService: TeamsService) { }

  @ViewChild(MatPaginator) paginator: MatPaginator;
  teams: Array<Team>;

  displayedColumns: string[] = ['name', 'sportName', 'logo'];
  dataSource;

  ngOnInit() {
    this.teamsService.getTeams().subscribe(
      ((data: Array<Team>) => this.result(data)),
      ((error: any) => console.log(error))
    );

    this.dataSource = new MatTableDataSource<Team>(this.teams);
    this.dataSource.paginator = this.paginator;
  }

  private result(data: Array<Team>): void {
    this.teams = data;
    console.log(this.teams);
  }
}
