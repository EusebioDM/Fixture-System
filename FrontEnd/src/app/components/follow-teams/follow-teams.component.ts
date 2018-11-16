import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort, MatPaginator, MatTableDataSource } from '@angular/material';
import { Team } from '../../classes/team';
import { TeamsService } from '../../services/teams.service';

@Component({
  selector: 'app-follow-teams',
  templateUrl: './follow-teams.component.html',
  styleUrls: ['./follow-teams.component.css']
})
export class FollowTeamsComponent implements OnInit {

  constructor(
    private teamsService: TeamsService
  ) { }

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  displayedColumns: string[] = ['teamName', 'sportName', 'actionFollow'];
  dataSource;
  searchKey: string;
  temas: Array<Team>;

  ngOnInit() {
    this.getData();
  }

  private getData() {
    this.teamsService.getTeams().subscribe(
      ((data: Array<Team>) => this.result(data)),
      ((error: any) => console.log(error))
    );
  }

  private result(data: Array<Team>): void {
    this.temas = data;
    this.loadTableDataSource();
  }

  private loadTableDataSource() {
    this.dataSource = new MatTableDataSource<Team>(this.temas);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  onSearchClear() {
    this.searchKey = '';
    this.applyFilter();
  }

  applyFilter() {
    this.dataSource.filter = this.searchKey.trim().toLowerCase();
  }

  onFollow(team: Team) {
    this.teamsService.addFollowedTeamToLoggedUser(team.name + '_' + team.sportName).subscribe();
  }
}
